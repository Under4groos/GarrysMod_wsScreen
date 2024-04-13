using Avalonia.Controls;
using System.Diagnostics;

namespace wsScreen.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        grid.PointerMoved += Grid_PointerMoved;
    }

    private void Grid_PointerMoved(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        System.Collections.Generic.IReadOnlyList<Avalonia.Input.PointerPoint> points = e.GetIntermediatePoints(grid);

        foreach (Avalonia.Input.PointerPoint item in points)
        {
            Debug.WriteLine(item.Position);
        }


    }


}
