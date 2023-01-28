using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ipocom
{
    public class IpocomReceiver : MonoBehaviour
    {
        public UnityEngine.Events.UnityEvent<SonyMotionFormat.SkeletonMessage> m_onSkeleton;
        public UnityEngine.Events.UnityEvent<SonyMotionFormat.FrameMessage> m_onFrame;
        public int m_port = 12351;
        IDisposable m_udp;

        /// <summary>
        /// UDP 受信スレッドからメインスレッドデータを渡すのに使う
        /// </summary>
        ConcurrentQueue<object> m_queue = new ConcurrentQueue<object>();
        int m_skeletonCount = 0;
        int m_frameCount = 0;
        uint m_frameNumber = 0;
        uint m_maxFrame = 0;
        List<float> m_receiveTimes = new List<float>();
        float m_frameTime = 0;
        const int MAX_TIMES = 60;
        float m_frameRate = 0;
        int m_lastTime = 0;
        float m_maxTime = 0;

        void OnEnable()
        {
            Debug.Log($"OnEnable: start udp: {m_port}");
            m_udp = new UdpSocketStream(m_port, new UdpState
            {
                OnError = ex => Debug.LogException(ex),
                OnReceive = OnReceiveOnThread,
            });
        }

        void OnDisable()
        {
            m_udp.Dispose();
            m_udp = null;
        }

        /// <summary>
        /// UDP 受信スレッドで動作する
        /// </summary>
        /// <param name="message"></param>
        void OnReceiveOnThread(ArraySegment<byte> message)
        {
            var parsed = SonyMotionFormat.Parser.Parse(message);
            if (parsed != null)
            {
                m_queue.Enqueue(parsed);
            }
        }

        void Update()
        {
            while (m_queue.TryDequeue(out object data))
            {
                switch (data)
                {
                    case SonyMotionFormat.SkeletonMessage skeleton:
                        ++m_skeletonCount;
                        Debug.Log($"{skeleton}");
                        m_onSkeleton.Invoke(skeleton);
                        break;

                    case SonyMotionFormat.FrameMessage frame:
                        ++m_frameCount;
                        if (m_receiveTimes.Count > 0 && m_frameTime > frame.fram.Time)
                        {
                            // Rewind
                            Debug.Log("rewind");
                            m_maxTime = Mathf.Max(m_maxTime, m_frameTime);
                            m_maxFrame = Math.Max(m_maxFrame, m_frameNumber);
                            m_receiveTimes.Clear();
                        }
                        m_frameNumber = frame.fram.FrameNumber;
                        m_frameTime = frame.fram.Time;
                        var time = UnityEngine.Time.time;
                        m_receiveTimes.Add(time);
                        if (m_receiveTimes.Count > MAX_TIMES)
                        {
                            m_receiveTimes.RemoveRange(0, m_receiveTimes.Count - MAX_TIMES);
                        }
                        m_onFrame.Invoke(frame);
                        break;

                    default:
                        throw new ArgumentException("unknown");
                }
            }
        }

        void OnGUI()
        {
            // skeleton count
            GUILayout.Label($"received skeleton: {m_skeletonCount}");
            GUILayout.Label($"received frame: {m_frameCount}");
            if (m_frameTime < m_maxTime)
            {
                GUILayout.Label($"frame time: {m_frameTime:.00}/{m_maxTime:.00}");
            }
            else
            {
                GUILayout.Label($"frame time: {m_frameTime:.00}");
            }
            if (m_frameNumber < m_maxFrame)
            {
                GUILayout.Label($"frame no: {m_frameNumber}/{m_maxFrame}");
            }
            else
            {
                GUILayout.Label($"frame no: {m_frameNumber}");
            }
            // frame rate
            if (m_receiveTimes.Count == MAX_TIMES)
            {
                var current = (int)Time.time;
                if (current != m_lastTime)
                {
                    m_lastTime = current;
                    float delta = m_receiveTimes.Last() - m_receiveTimes.First();
                    // for (int i = 1; i < m_frameTimes.Count; ++i)
                    // {
                    //     delta += m_frameTimes[i] - m_frameTimes[i - 1];
                    // }
                    delta /= (m_receiveTimes.Count - 1);
                    m_frameRate = 1 / delta;
                }
                GUILayout.Label($"fps: {m_frameRate}");
            }
        }
    }
}
