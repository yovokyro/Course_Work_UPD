using System.Threading.Tasks;

namespace UPDController
{
    public interface ISocket
    {
        bool IsReceive { get; }
        SocketTypes Type { get; }

        void SendMessageAsync(string message);
        void ReceiveMessageAsync();

        void StopReceive();
        void StartReceive();

        void ClearInstance();

        string GetInfo();

        string GetAddress();
    }
}
