﻿namespace BathymetryToolApp;

public partial class BathymetryTool
{
    public partial class LatLngFile
    {
        public float Lat1 { get; set; }
        public float Lat2 { get; set; }
        public float Lng1 { get; set; }
        public float Lng2 { get; set; }
        public int ValCount { get; set; }
        public string FileName { get; set; } = "";
    }
}
