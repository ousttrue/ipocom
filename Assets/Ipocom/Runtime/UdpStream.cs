using System;
using System.Net.Sockets;
using System.Net;
using UnityEngine.Profiling;

namespace Ipocom
{
    public class UdpState
    {
        bool m_canceled = false;
        public bool IsCanceled => m_canceled;
        public void Cancel()
        {
            m_canceled = true;
        }

        public Action<byte[]> OnReceive;
        public Action<Exception> OnError;
    }

    public class UdpStream : IDisposable
    {
        UdpClient m_udp;

        public UdpStream(int port, UdpState state)
        {
            try
            {
                var e = new IPEndPoint(IPAddress.Any, port);
                m_udp = new UdpClient(e);
                BeginRead(state);
            }
            catch (Exception ex)
            {
                state.OnError(ex);
            }
        }

        public void Dispose()
        {
            this.m_udp.Close();
            this.m_udp = null;
        }

        void BeginRead(UdpState state)
        {
            if (state.IsCanceled)
            {
                return;
            }

            AsyncCallback callback = (IAsyncResult ar) =>
            {
                Profiler.BeginSample("BeginReceive AsyncCallback");
                var udp = m_udp;
                if (udp == null)
                {
                    // disposed
                    return;
                }
                var s = (UdpState)ar.AsyncState;
                var e = default(IPEndPoint);
                try
                {
                    var bytes = udp.EndReceive(ar, ref e);
                    if (bytes == null)
                    {
                        s.OnError(new ArgumentNullException());
                        return;
                    }
                    s.OnReceive(bytes);
                    // next
                    BeginRead(state);
                }
                catch (Exception ex)
                {
                    s.OnError(ex);
                }
                Profiler.EndSample();
            };

            {
                var udp = m_udp;
                if (udp == null)
                {
                    // disposed
                    return;
                }
                udp.BeginReceive(callback, state);
            }
        }
    }
}
