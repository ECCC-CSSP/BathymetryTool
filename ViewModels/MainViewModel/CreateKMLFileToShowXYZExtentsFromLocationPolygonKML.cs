using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MainViewModels;

public partial class MainViewModel
{
    public void CreateKMLFileToShowXYZExtentsFromLocationPolygonKML()
    {
        FileInfo fi = new FileInfo($"{StartDir}\\kml\\Location Polygons.kml");

        if (!fi.Exists)
        {
            Log = $"File does not exist [{fi.FullName}]";

            return;
        }

        List<LatLngFileCount> LatLngFileCountList = GetLatLngFileCountListForXYZ();

        XDocument doc = XDocument.Load(fi.FullName);

        if (doc.Root != null)
        {

            foreach (XElement e1 in doc.Root.Descendants().Where(c => c.Name.ToString().EndsWith("Placemark")))
            {
                string DirectoryName = string.Empty;

                List<Coord> poly = new List<Coord>();

                XElement? elem_name = e1.Descendants().Where(c => c.Name.ToString().EndsWith("name")).FirstOrDefault();
                if (elem_name != null)
                {
                    DirectoryName = elem_name.Value;
                    DirectoryInfo di = new DirectoryInfo($"{StartDir}\\sub_xyz\\{DirectoryName}");
                    if (!di.Exists)
                    {
                        try
                        {
                            di.Create();
                        }
                        catch (Exception ex)
                        {
                            return;
                        }
                    }
                }

                if (DirectoryName == string.Empty)
                {
                    continue;
                }

                XElement? elem_Coord = e1.Descendants().Where(c => c.Name.ToString().EndsWith("coordinates")).FirstOrDefault();
                if (elem_Coord != null)
                {
                    string CoordText = elem_Coord.Value;
                    List<string> CoordListText = CoordText.Split(" ", StringSplitOptions.None).ToList();
                    foreach (string CoordText2 in CoordListText)
                    {
                        List<string> CoordListText2 = CoordText2.Trim().Split(",").ToList();
                        if (CoordListText2.Count < 3)
                        {
                            continue;
                        }
                        float lat = float.Parse(CoordListText2[1]);
                        float lng = float.Parse(CoordListText2[0]);

                        poly.Add(new Coord() { Lat = lat, Lng = lng });
                    }

                    StringBuilder sb = new StringBuilder();

                    CreateTopKMLPart(sb, "_XYZExtent");

                    DirectoryInfo diKML = new DirectoryInfo($"{StartDir}\\sub_xyz\\{DirectoryName}\\kml\\");
                    if (!diKML.Exists)
                    {
                        try
                        {
                            diKML.Create();
                        }
                        catch (Exception ex)
                        {
                            return;
                        }
                    }

                    sb.AppendLine($"	<Folder>");
                    sb.AppendLine($"		<name>Location Polygon</name>");

                    sb.AppendLine($"		<Placemark>");
                    sb.AppendLine($"		<name>Location Polygon</name>");
                    sb.AppendLine($"			<styleUrl>#m_ylw-pushpin</styleUrl>");
                    sb.AppendLine($"			<Polygon>");
                    sb.AppendLine($"				<outerBoundaryIs>");
                    sb.AppendLine($"					<LinearRing>");
                    sb.AppendLine($"						<coordinates>");
                    foreach (Coord coord in poly)
                    {
                        sb.Append($"{coord.Lng.ToString("F6")},{coord.Lat.ToString("F6")},0 ");
                    }
                    sb.AppendLine("");
                    sb.AppendLine($"						</coordinates>");
                    sb.AppendLine($"					</LinearRing>");
                    sb.AppendLine($"				</outerBoundaryIs>");
                    sb.AppendLine($"			</Polygon>");
                    sb.AppendLine($"		</Placemark>");

                    sb.AppendLine($"	</Folder>");


                    List<LatLngFileCount> latLngFileCountKMLList = GetLatLngFileCountListForKML(diKML);

                    foreach (LatLngFileCount latLngFileCount in LatLngFileCountList)
                    {
                        FileInfo fiNew = new FileInfo($"{StartDir}\\sub_xyz\\{latLngFileCount.FileName}");
                        if (fiNew.Exists)
                        {
                            continue;
                        }

                        List<Coord> fileCoordList = new List<Coord>()
                        {
                            new Coord() { Lat = latLngFileCount.Lat1, Lng = latLngFileCount.Lng1 },
                            new Coord() { Lat = latLngFileCount.Lat2, Lng = latLngFileCount.Lng1 },
                            new Coord() { Lat = latLngFileCount.Lat1, Lng = latLngFileCount.Lng2 },
                            new Coord() { Lat = latLngFileCount.Lat2, Lng = latLngFileCount.Lng2 }
                        };

                        foreach (Coord fileCoord in fileCoordList)
                        {
                            if (CoordInPolygon(poly, fileCoord))
                            {
                                LatLngFileCount latLngFileCountKMLToUse = new LatLngFileCount()
                                {
                                    Lat1 = 0,
                                    Lat2 = 0,
                                    Lng1 = 0,
                                    Lng2 = 0,
                                    ValCount = 0,
                                    FileName = "No KML File Found"
                                };

                                if (latLngFileCountKMLList.Count > 0)
                                {
                                    string startFileNameXYZ = latLngFileCount.FileName.Substring(0, latLngFileCount.FileName.LastIndexOf("_"));
                                    foreach (LatLngFileCount latLngFileCountKML in latLngFileCountKMLList)
                                    {
                                        string startFileNameKML = latLngFileCountKML.FileName.Substring(0, latLngFileCountKML.FileName.LastIndexOf("_"));
                                        if (startFileNameXYZ == startFileNameKML)
                                        {
                                            latLngFileCountKMLToUse = latLngFileCountKML;
                                            break;
                                        }
                                    }

                                    sb.AppendLine($"	<Folder>");
                                    sb.AppendLine($"		<name>{latLngFileCountKMLToUse.FileName}</name>");
                                    sb.AppendLine($"		<Placemark>");
                                    sb.AppendLine($"			<name>{latLngFileCount.Lat1.ToString("F1")} {latLngFileCount.Lng1.ToString("F1")} Count:{latLngFileCount.ValCount}</name>");
                                    sb.AppendLine($"			<styleUrl>#m_ylw-pushpin</styleUrl>");
                                    sb.AppendLine($"			<Point>");
                                    sb.AppendLine($"				<coordinates>{latLngFileCount.Lng1},{latLngFileCount.Lat1},0</coordinates>");
                                    sb.AppendLine($"			</Point>");
                                    sb.AppendLine($"		</Placemark>");

                                    sb.AppendLine($"		<Placemark>");
                                    sb.AppendLine($"			<name>{latLngFileCountKMLToUse.FileName}</name>");
                                    sb.AppendLine($"			<styleUrl>#m_ylw-pushpin</styleUrl>");
                                    sb.AppendLine($"			<Polygon>");
                                    sb.AppendLine($"				<outerBoundaryIs>");
                                    sb.AppendLine($"					<LinearRing>");
                                    sb.AppendLine($"						<coordinates>");
                                    sb.AppendLine($"							{latLngFileCount.Lng1},{latLngFileCount.Lat1},0 {latLngFileCount.Lng2},{latLngFileCount.Lat1},0 {latLngFileCount.Lng2},{latLngFileCount.Lat2},0 {latLngFileCount.Lng1},{latLngFileCount.Lat2},0 {latLngFileCount.Lng1},{latLngFileCount.Lat1},0 ");
                                    sb.AppendLine($"						</coordinates>");
                                    sb.AppendLine($"					</LinearRing>");
                                    sb.AppendLine($"				</outerBoundaryIs>");
                                    sb.AppendLine($"			</Polygon>");
                                    sb.AppendLine($"		</Placemark>");

                                    sb.AppendLine($"	    <Folder>");
                                    sb.AppendLine($"		    	<name>Data</name>");
                                    sb.AppendLine($"		    	<visibility>0</visibility>");
                                    sb.AppendLine($"		    <NetworkLink>");
                                    sb.AppendLine($"		    	<name>{latLngFileCountKMLToUse.FileName}</name>");
                                    sb.AppendLine($"		    	<visibility>0</visibility>");
                                    sb.AppendLine($"		    	<Link>");
                                    sb.AppendLine($"                    <href>{latLngFileCountKMLToUse.FileName}</href>");
                                    sb.AppendLine($"                </Link>");
                                    sb.AppendLine($"		    </NetworkLink>");
                                    sb.AppendLine($"	    </Folder>");

                                    sb.AppendLine($"	</Folder>");
                                }
                                else
                                {
                                    sb.AppendLine($"	<Folder>");
                                    sb.AppendLine($"		<name>{latLngFileCount.FileName}</name>");
                                    sb.AppendLine($"		<Placemark>");
                                    sb.AppendLine($"			<name>{latLngFileCount.Lat1.ToString("F1")} {latLngFileCount.Lng1.ToString("F1")} Count:{latLngFileCount.ValCount}</name>");
                                    sb.AppendLine($"			<styleUrl>#m_ylw-pushpin</styleUrl>");
                                    sb.AppendLine($"			<Point>");
                                    sb.AppendLine($"				<coordinates>{latLngFileCount.Lng1},{latLngFileCount.Lat1},0</coordinates>");
                                    sb.AppendLine($"			</Point>");
                                    sb.AppendLine($"		</Placemark>");

                                    sb.AppendLine($"		<Placemark>");
                                    sb.AppendLine($"			<name>{latLngFileCount.FileName}</name>");
                                    sb.AppendLine($"			<styleUrl>#m_ylw-pushpin</styleUrl>");
                                    sb.AppendLine($"			<Polygon>");
                                    sb.AppendLine($"				<outerBoundaryIs>");
                                    sb.AppendLine($"					<LinearRing>");
                                    sb.AppendLine($"						<coordinates>");
                                    sb.AppendLine($"							{latLngFileCount.Lng1},{latLngFileCount.Lat1},0 {latLngFileCount.Lng2},{latLngFileCount.Lat1},0 {latLngFileCount.Lng2},{latLngFileCount.Lat2},0 {latLngFileCount.Lng1},{latLngFileCount.Lat2},0 {latLngFileCount.Lng1},{latLngFileCount.Lat1},0 ");
                                    sb.AppendLine($"						</coordinates>");
                                    sb.AppendLine($"					</LinearRing>");
                                    sb.AppendLine($"				</outerBoundaryIs>");
                                    sb.AppendLine($"			</Polygon>");
                                    sb.AppendLine($"		</Placemark>");

                                    sb.AppendLine($"	    <Folder>");
                                    sb.AppendLine($"		    	<name>No Data</name>");
                                    sb.AppendLine($"		    	<visibility>0</visibility>");
                                    sb.AppendLine($"	    </Folder>");

                                    sb.AppendLine($"	</Folder>");
                                }

                                break;
                            }
                        }
                    }

                    sb.AppendLine($"</Document>");
                    sb.AppendLine($"</kml>");



                    FileInfo fiKML = new FileInfo($"{StartDir}\\sub_xyz\\{DirectoryName}\\kml\\_XYZExtent.kml");
                    if (fiKML.Exists)
                    {
                        fiKML.Delete();
                    }

                    using (StreamWriter sw = fiKML.CreateText())
                    {
                        sw.Write(sb.ToString());
                    }
                }
            }
        }
    }
}