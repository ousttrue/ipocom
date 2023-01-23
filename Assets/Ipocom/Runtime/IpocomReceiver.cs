using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Ipocom
{
    public class IpocomReceiver : MonoBehaviour
    {
        public UnityEngine.Events.UnityEvent<SkeletonMessage> m_onSkeleton;
        public UnityEngine.Events.UnityEvent<FrameMessage> m_onFrame;
        public int m_port = 12351;
        UdpStream m_udp;

        /// <summary>
        /// UDP 受信スレッドからメインスレッドデータを渡すのに使う
        /// </summary>
        ConcurrentQueue<object> m_queue = new ConcurrentQueue<object>();

        void OnEnable()
        {
            Debug.Log($"OnEnable: start udp: {m_port}");
            m_udp = new UdpStream(m_port, new UdpState
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
        void OnReceiveOnThread(byte[] message)
        {
            var parsed = SmfParser.Parse(message);
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
                    case SkeletonMessage skeleton:
                        Debug.Log($"{skeleton}");
                        m_onSkeleton.Invoke(skeleton);
                        break;

                    case FrameMessage frame:
                        m_onFrame.Invoke(frame);
                        break;

                    default:
                        throw new ArgumentException("unknown");
                }
            }
        }
    }
}