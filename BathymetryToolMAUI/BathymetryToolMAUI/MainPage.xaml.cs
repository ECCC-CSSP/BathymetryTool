using Microsoft.Maui.Storage;

namespace BathymetryToolMAUI;

public partial class MainPage : ContentPage
{
    MainViewModel vm { get; }

    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
    }

    override protected void OnAppearing()
    {
        base.OnAppearing();

        vm.StartDir = Preferences.Get("StartDir", "E:\\Bathymetry\\CHS_High_Res\\Data\\sub_xyz\\");
        vm.FileXYZWithinPolygonFullName = vm.StartDir + "\\" + vm.FileXYZWithinPolygonName;
    }

    override protected void OnDisappearing()
    {
        base.OnDisappearing();

        Preferences.Set("StartDir", vm.StartDir);
    }
}
