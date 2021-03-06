﻿using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace huypq.LogViewerControl
{
    public class UdpServer : IServer
    {
        private bool _isRunning;
        public bool IsRunning { get { return _isRunning; } }
        public Action<string> ReadCompleted { get; set; }
        public Action Started { get; set; }
        public Action Stopped { get; set; }

        UdpClient udpClient;
        public async Task Start()
        {
            udpClient = new UdpClient(11000);
            _isRunning = true;

            Started?.Invoke();

            while (_isRunning)
            {
                try
                {                    
                    var result = await udpClient.ReceiveAsync();
                    
                    var text = Encoding.ASCII.GetString(result.Buffer);

                    ReadCompleted?.Invoke(text);
                }
                catch { }
            }

            Stopped?.Invoke();
        }

        public void Stop()
        {
            if (udpClient != null)
            {
                udpClient.Close();
                udpClient = null;
            }
            _isRunning = false;
        }
    }
}
