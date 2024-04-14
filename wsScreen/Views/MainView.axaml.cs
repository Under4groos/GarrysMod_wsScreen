using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using Control.Model;
using lib_json;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
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
        but_refresh.Click += But_refresh_Click;
    }

    private void But_refresh_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        __send("getbuttons");

        Client.ReadStringAsync().ContinueWith((Task<string> data) =>
        {

            Dispatcher.UIThread.Invoke(() =>
            {
                List<JsonButtons> buttons = JsonConvert.DeserializeObject<List<JsonButtons>>(data.Result);

                WrapPanelList.Children.Clear();

                foreach (JsonButtons item in buttons)
                {
                    ToggleButton toggleButton = new ToggleButton()
                    {
                        Content = item.Name,
                        Tag = item.Command.ToString(),
                        IsChecked = item.IsChecked
                    };
                    toggleButton.Click += ToggleButton_Click;

                    WrapPanelList.Children.Add(toggleButton);




                }


            });
        });

    }

    private void ToggleButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string data___ = string.Empty;
        if (sender is ToggleButton toggleButton)
        {
            data___ = toggleButton.Tag as string;
        }




        if (jsonObj_Data.Buttons.ContainsKey(data___))
        {
            jsonObj_Data.Buttons[data___] = (bool)(sender as ToggleButton).IsChecked ? 1 : 0;
        }
        else
        {
            if (sender is ToggleButton toggleButtond)
            {
                jsonObj_Data.Buttons.Add(data___, (bool)toggleButtond.IsChecked ? 1 : 0);
            }


        }
        SendJsonData();
    }

    private void MainView_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

        But_refresh_Click(null, null);

    }

    private void Grid_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        Global.WindowSize = e.NewSize;
        jsonObj_Data.ScreenSize = $"{e.NewSize.Width},{e.NewSize.Height}";
    }

    private void Grid_PointerMoved(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        Avalonia.Input.PointerPoint point = e.GetCurrentPoint(grid);


        position.Text = $"Pos: {(int)point.Position.X},{(int)point.Position.Y}";

        jsonObj_Data.Position = $"{(int)point.Position.X},{(int)point.Position.Y}";


        int x = ((int)Global.WindowSize.Width / 2) - (int)point.Position.X;
        int y = ((int)Global.WindowSize.Height / 2) - (int)point.Position.Y;

        localposition.Text = $"{x},{y}";
        localposition.Margin = new Avalonia.Thickness(
           point.Position.X,
            point.Position.Y,
            0, 0
            );

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
