using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine.Profiling;

namespace Ipocom
{
    public class UdpClientStream : IDisposable
    {
        UdpClient m_udp;

        public UdpClientStream(int port, UdpState state)
        {
            try
            {
                var ep = new IPEndPoint(IPAddress.Any, port);
                m_udp = new UdpClient(ep);
                BeginRead(state);
            }
            catch (Exception ex)
            {
                state.OnError(ex);
            }
        }

        public void Dispose()
        {
            var udp = m_udp;
            this.m_udp = null;
            if (udp != null)
            {
                udp.Close();
            }
        }

        void BeginRead(UdpState state)
        {
            if (state.IsCanceled)
            {
                return;
            }

            AsyncCallback callback = (IAsyncResult ar) =>
            {
                Profiler.BeginSample("ClientStream AsyncCallback");
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
                    s.OnReceive(new ArraySegment<byte>(bytes));
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
