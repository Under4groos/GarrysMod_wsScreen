using Avalonia.Controls;
using Control.Model;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using wsScreen.Model;

namespace wsScreen.Views;

public partial class MainView : UserControl
{
    private UdpClient Client = new UdpClient()
    {
        EnableBroadcast = true
    };

    public MainView()
    {
        InitializeComponent();
        grid.PointerMoved += Grid_PointerMoved;
        grid.SizeChanged += Grid_SizeChanged;
    }

    private void Grid_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        Global.WindowSize = e.NewSize;
    }

    private void Grid_PointerMoved(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        //System.Collections.Generic.IReadOnlyList<Avalonia.Input.PointerPoint> points = e.GetIntermediatePoints(grid);

        //foreach (Avalonia.Input.PointerPoint item in points)
        //{
        //    Debug.WriteLine(item.Position);
        //    __send(string data)

        //}


        Avalonia.Input.PointerPoint point = e.GetCurrentPoint(grid);
        int x = ((int)Global.WindowSize.Width / 2) - (int)point.Position.X;
        int y = ((int)Global.WindowSize.Height / 2) - (int)point.Position.Y;


        __send($"{x}:{y}");
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
