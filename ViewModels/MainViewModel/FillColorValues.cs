namespace MainViewModels;

public partial class MainViewModel
{
    private List<ColorVal> FillColorValues()
    {
        List<ColorVal> ColorValList = new List<ColorVal>();
        ColorValList.Add(new ColorVal() { Value = -100000, ColorHexStr = "ffffffff" });
        ColorValList.Add(new ColorVal() { Value = 0, ColorHexStr = "ffffffff" });
        ColorValList.Add(new ColorVal() { Value = 0.1, ColorHexStr = "ff0000ff" });
        ColorValList.Add(new ColorVal() { Value = 0.3, ColorHexStr = "ff0033ff" });
        ColorValList.Add(new ColorVal() { Value = 0.5, ColorHexStr = "ff0066ff" });
        ColorValList.Add(new ColorVal() { Value = 0.8, ColorHexStr = "ff0099ff" });
        ColorValList.Add(new ColorVal() { Value = 1, ColorHexStr = "ff00ccff" });
        ColorValList.Add(new ColorVal() { Value = 2, ColorHexStr = "ff00ffff" });
        ColorValList.Add(new ColorVal() { Value = 3, ColorHexStr = "ff00ffcc" });
        ColorValList.Add(new ColorVal() { Value = 5, ColorHexStr = "ff00ff99" });
        ColorValList.Add(new ColorVal() { Value = 7, ColorHexStr = "ff00ff66" });
        ColorValList.Add(new ColorVal() { Value = 10, ColorHexStr = "ff00ff33" });
        ColorValList.Add(new ColorVal() { Value = 12, ColorHexStr = "ff00ff00" });
        ColorValList.Add(new ColorVal() { Value = 15, ColorHexStr = "ff00cc00" });
        ColorValList.Add(new ColorVal() { Value = 20, ColorHexStr = "ff009900" });
        ColorValList.Add(new ColorVal() { Value = 30, ColorHexStr = "ffff0000" });
        ColorValList.Add(new ColorVal() { Value = 45, ColorHexStr = "ffff0033" });
        ColorValList.Add(new ColorVal() { Value = 70, ColorHexStr = "ffff0066" });
        ColorValList.Add(new ColorVal() { Value = 100, ColorHexStr = "ffff0099" });
        ColorValList.Add(new ColorVal() { Value = 140, ColorHexStr = "ffff00cc" });
        ColorValList.Add(new ColorVal() { Value = 200, ColorHexStr = "ffff00ff" });
        ColorValList.Add(new ColorVal() { Value = 250, ColorHexStr = "ffcc00ff" });
        ColorValList.Add(new ColorVal() { Value = 400, ColorHexStr = "ff9900ff" });
        ColorValList.Add(new ColorVal() { Value = 600, ColorHexStr = "ff6600ff" });
        ColorValList.Add(new ColorVal() { Value = 900, ColorHexStr = "ff3300ff" });
        ColorValList.Add(new ColorVal() { Value = 1400, ColorHexStr = "ff0000ff" });
        ColorValList.Add(new ColorVal() { Value = 2000, ColorHexStr = "ffcccccc" });
        ColorValList.Add(new ColorVal() { Value = 3000, ColorHexStr = "ff999999" });
        ColorValList.Add(new ColorVal() { Value = 5000, ColorHexStr = "ff666666" });
        ColorValList.Add(new ColorVal() { Value = 7500, ColorHexStr = "ff333333" });
        ColorValList.Add(new ColorVal() { Value = 10000, ColorHexStr = "ff000000" });
        return ColorValList;
    }
}

internal class FillColorValues
{
}
