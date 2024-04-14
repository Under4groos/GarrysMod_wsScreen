using lib_json;
using Newtonsoft.Json;
using sv_Control;
using System.Net;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
List<JsonButtons> buttons = new List<JsonButtons>();
string FileJsonButtons = Path.GetFullPath("buttons.json");


WebApplication app = builder.Build();

ThreadUdpClient threadUdpClient = new ThreadUdpClient();
StringBuilder stringBuilder = new StringBuilder();
threadUdpClient.EvRequestData += (IPAddress adress, string data) =>
{
    if (data == "getbuttons")
    {
        if (!File.Exists(FileJsonButtons))
        {
            buttons = new List<JsonButtons>()
            {
                new JsonButtons()
                {
                    Name = "Test 1",
                    Command = ""


                },
                new JsonButtons()
                {
                    Name = "Test 2",
                    Command = ""
                }
            };

            File.WriteAllText(FileJsonButtons, JsonConvert.SerializeObject(buttons, Formatting.Indented));
        }
        else
        {
            buttons = JsonConvert.DeserializeObject<List<JsonButtons>>(File.ReadAllText(FileJsonButtons));
        }


        threadUdpClient.Send(JsonConvert.SerializeObject(buttons));
        return;
    }

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

});

app.Map("/GetCursors", () =>
{

});
app.Run();
