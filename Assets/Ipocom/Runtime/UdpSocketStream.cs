using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine.Profiling;

namespace Ipocom
{
    public class UdpSocketStream : IDisposable
    {
        Socket m_socket;
        EndPoint m_senderRemote;
        byte[] m_readBuffer = new byte[2048];

        public UdpSocketStream(int port, UdpState state)
        {
            try
            {
                var ep = new IPEndPoint(IPAddress.Any, port);
                m_socket = new Socket(ep.Address.AddressFamily,
                    SocketType.Dgram,
                    ProtocolType.Udp);

                // Creates an IPEndPoint to capture the identity of the sending host.
                var sender = new IPEndPoint(IPAddress.Any, 0);
                m_senderRemote = (EndPoint)sender;

                // Binding is required with ReceiveFrom calls.
                m_socket.Bind(ep);

                BeginRead(state);
            }
            catch (Exception ex)
            {
                state.OnError(ex);
            }
        }

        public void Dispose()
        {
            var socket = m_socket;
            m_socket = null;
            if (socket != null)
            {
                socket.Close();
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
                Profiler.BeginSample("SocketStream AsyncCallback");
                var socket = m_socket;
                if (socket == null)
                {
                    // disposed
                    return;
                }
                var s = (UdpState)ar.AsyncState;
                try
                {
                    var readSize = socket.EndReceiveFrom(ar, ref m_senderRemote);
                    if (readSize == 0)
                    {
                        s.OnError(new ArgumentNullException());
                        return;
                    }
                    s.OnReceive(new ArraySegment<byte>(m_readBuffer, 0, readSize));
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
                var socket = m_socket;
                if (socket == null)
                {
                    // disposed
                    return;
                }
                socket.BeginReceiveFrom(m_readBuffer, 0, m_readBuffer.Length, default, ref m_senderRemote, callback, state);
            }
        }
    }
}
