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

    public interface IUdpStream : IDisposable
    {

    }
}
