using sv_Control;
using System.Net;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string data_cursor = "sad";

WebApplication app = builder.Build();

ThreadUdpClient threadUdpClient = new ThreadUdpClient();
threadUdpClient.EvRequestData += (IPAddress adress, string data) =>
{
    data_cursor = data;
    try
    {
        File.WriteAllText(@"E:\Steam\steamapps\common\GarrysMod\garrysmod\data\e2files\test.txt", data);
    }
    catch (Exception)
    {

    }

};
threadUdpClient.ToListen();


app.Map("/GetCursors", () =>
{
    return data_cursor;
});
app.Run();
