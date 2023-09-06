namespace MainViewModels;

public partial class MainViewModel
{
    public bool CoordInPolygon(List<Coord> poly, Coord pnt)
    {
        int i, j;
        int nvert = poly.Count;
        bool InPoly = false;
        for (i = 0, j = nvert - 1; i < nvert; j = i++)
        {
            if (((poly[i].Lat > pnt.Lat) != (poly[j].Lat > pnt.Lat)) &&
             (pnt.Lng < (poly[j].Lng - poly[i].Lng) * (pnt.Lat - poly[i].Lat) / (poly[j].Lat - poly[i].Lat) + poly[i].Lng))
                InPoly = !InPoly;
        }
        return InPoly;
    }

}
