using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MinerGame.UPD
{
    public class Server
    {
        private IPAddress _localAddress = IPAddress.Parse("127.0.0.1");
        private int _port = 8888;
        private Socket _sender;
        private Socket _receiver;

        public Server()
        {
            _sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _receiver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            Task.Run(ReceiveMessageAsync);
        }

        public Server(IPAddress address, int port)
        {
            _localAddress = address;
            _port = port;

            _sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _receiver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            Task.Run(ReceiveMessageAsync);
        }

        async Task SendMessageAsync()
        {
            while (true)
            {
                string message = "test";
                if (string.IsNullOrWhiteSpace(message)) break;
                byte[] data = Encoding.UTF8.GetBytes(message);
                await _sender.SendToAsync(data, new IPEndPoint(_localAddress, _port));
            }
        }

        async Task ReceiveMessageAsync()
        {
            byte[] data = new byte[1024];

            _receiver.Bind(new IPEndPoint(_localAddress, _port));
            while (true)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.SetBuffer(data, 0, data.Length);
                args.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                if (_receiver.ReceiveFromAsync(args))
                {
                    var message = Encoding.UTF8.GetString(data, 0, args.BytesTransferred);
                    Console.WriteLine(message);
                }
            }
        }
    }
}
