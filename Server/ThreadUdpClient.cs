using System.Net;
using System.Net.Sockets;
using System.Text;

namespace sv_Control
{
    public class ThreadUdpClient : IDisposable
    {
        private UdpClient Server = new UdpClient(8882);
        private IPEndPoint ClientEp = new IPEndPoint(IPAddress.Any, 0);
        public Action<IPAddress, string> EvRequestData;
        public void Send(string data)
        {
            byte[] ResponseData = Encoding.ASCII.GetBytes(data);
            Server.Send(ResponseData, ResponseData.Length, ClientEp);
        }
        public void ToListen()
        {
            new Thread(() =>
            {



                while (true)
                {

                    byte[] ClientRequestData = Server.Receive(ref ClientEp);
                    string ClientRequest = Encoding.ASCII.GetString(ClientRequestData);
                    // Console.WriteLine($"IP:{ClientEp.Address.ToString()}\n  -> {ClientRequest}");
                    if (EvRequestData != null)
                        EvRequestData(ClientEp.Address, ClientRequest);


                }
            }).Start();
        }
        public void Dispose()
        {
            Server.Dispose();
        }
    }
}
