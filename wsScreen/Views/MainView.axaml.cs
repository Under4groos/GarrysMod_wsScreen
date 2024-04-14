using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Control.Model;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using wsScreen.Model;

namespace wsScreen.Views;

public partial class MainView : UserControl
{
    private JsonObj_Data jsonObj_Data = new JsonObj_Data();

    private UdpClient Client = new UdpClient()
    {
        EnableBroadcast = true
    };

    public MainView()
    {
        InitializeComponent();
        grid.PointerMoved += Grid_PointerMoved;
        grid.SizeChanged += Grid_SizeChanged;

        this.Loaded += MainView_Loaded;

    }

    private void MainView_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string data___ = "";
        foreach (ToggleButton item in WrapPanelList.Children)
        {
            item.Click += (o, e) =>
            {
                data___ = (o as ToggleButton).Content as string;


                if (jsonObj_Data.Buttons.ContainsKey(data___))
                {
                    jsonObj_Data.Buttons[data___] = (bool)(o as ToggleButton).IsChecked ? 1 : 0;
                }
                else
                {
                    jsonObj_Data.Buttons.Add(data___, (bool)(o as ToggleButton).IsChecked ? 1 : 0);
                }
                SendJsonData();
            };
        }
    }

    private void Grid_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        Global.WindowSize = e.NewSize;
        jsonObj_Data.ScreenSize = $"{e.NewSize.Width},{e.NewSize.Height}";
    }

    private void Grid_PointerMoved(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        Avalonia.Input.PointerPoint point = e.GetCurrentPoint(grid);

        jsonObj_Data.Position = $"{(int)point.Position.X},{(int)point.Position.Y}";


        int x = ((int)Global.WindowSize.Width / 2) - (int)point.Position.X;
        int y = ((int)Global.WindowSize.Height / 2) - (int)point.Position.Y;

        jsonObj_Data.PositionCenter = $"{x},{y}";


        SendJsonData();
    }

    public void SendJsonData()
    {
        __send(JsonConvert.SerializeObject(jsonObj_Data));
    }

    private void __send(string data)
    {
        try
        {
            Client.SendString(data, new IPEndPoint(IPAddress.Broadcast, 8882));

        }
        catch (System.Exception ee)
        {
            Debug.WriteLine(ee.Message);
        }

    }


}
