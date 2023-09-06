namespace MainViewModels;

public partial class MainViewModel
{
    private string GetColorStyleID(double ColVal, List<ColorVal> ColorValList)
    {
        string ColValStr = "";
        ColorVal lowVal = (from cv in ColorValList where cv.Value <= ColVal orderby cv.Value descending select cv).First();
        ColorVal highVal = (from cv in ColorValList where cv.Value >= ColVal orderby cv.Value select cv).First();

        if ((ColVal - lowVal.Value) < (highVal.Value - ColVal))
        {
            ColValStr = "C_" + lowVal.Value.ToString().Replace(".", "_");
        }
        else
        {
            ColValStr = "C_" + highVal.Value.ToString().Replace(".", "_");
        }
        return ColValStr;
    }

}
