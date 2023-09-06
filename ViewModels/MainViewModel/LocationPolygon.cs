using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainViewModels;

public class LocationPolygon
{
    public string Location { get; set; } = string.Empty;
    public List<Coord> Polygon { get; set; } = new List<Coord>();
}

