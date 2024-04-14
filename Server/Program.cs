using Newtonsoft.Json;
using Server.Model;
using sv_Control;
using System.Net;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string data_cursor = "sad";

WebApplication app = builder.Build();

ThreadUdpClient threadUdpClient = new ThreadUdpClient();
StringBuilder stringBuilder = new StringBuilder();
threadUdpClient.EvRequestData += (IPAddress adress, string data) =>
{

    JsonObj_Data json = JsonConvert.DeserializeObject<JsonObj_Data>(data);



    stringBuilder.AppendLine(json.ScreenSize);
    stringBuilder.AppendLine(json.PositionCenter);
    stringBuilder.AppendLine(json.Position);

    foreach (KeyValuePair<string, int> item in json.Buttons)
    {
        stringBuilder.Append($"{item.Key}:{item.Value},");
    }


    try
    {
        File.WriteAllText(@"E:\Steam\steamapps\common\GarrysMod\garrysmod\data\e2files\test.txt", stringBuilder.ToString());
        Console.WriteLine(stringBuilder.ToString());
        stringBuilder.Clear();
    }
    catch (Exception)
    {

    }

};
threadUdpClient.ToListen();
app.Map("/", () =>
{
    return data_cursor;
});

app.Map("/GetCursors", () =>
{
    return data_cursor;
});
app.Run();
