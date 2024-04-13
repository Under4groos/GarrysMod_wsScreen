using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Control.Model
{
    public static class s_UdpClient
    {
        public static void SendString(this UdpClient udp, string data, IPEndPoint? endPoint)
        {
            byte[] RequestData = Encoding.UTF8.GetBytes(data);
            udp.Send(RequestData, RequestData.Length, endPoint);

        }
        public static async Task<string> ReadStringAsync(this UdpClient udp)
        {
            UdpReceiveResult ddd_ = await udp.ReceiveAsync();
            return Encoding.UTF8.GetString(ddd_.Buffer);

        }
    }

}
