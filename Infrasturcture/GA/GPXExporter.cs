using System.Globalization;
using System.Text;

namespace Infrasturcture.GA;

public static class GpxExporter
{
    public static byte[] ExportPolylineToGpxBytes(string encodedPolyline)
    {
        
        var coordinates = Decode(encodedPolyline);

        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<gpx version=\"1.1\" creator=\"PathFinder\" xmlns=\"http://www.topografix.com/GPX/1/1\">");
        sb.AppendLine("  <trk><name>Optimized Route</name><trkseg>");

        foreach (var coord in coordinates)
        {
            string lat = coord.GeoPoint.Lat.ToString(CultureInfo.InvariantCulture);
            string lon = coord.GeoPoint.Lon.ToString(CultureInfo.InvariantCulture);
            sb.AppendLine($"    <trkpt lat=\"{lat}\" lon=\"{lon}\" />");
        }

        sb.AppendLine("  </trkseg></trk></gpx>");
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    private static List<UserPoint> Decode(string polyline)
    {
        var points = new List<UserPoint>();
        int index = 0, lat = 0, lng = 0;

        while (index < polyline.Length)
        {
            int b, shift = 0, result = 0;
            do
            {
                b = polyline[index++] - 63;
                result |= (b & 0x1F) << shift;
                shift += 5;
            } while (b >= 0x20);
            int dlat = (result & 1) != 0 ? ~(result >> 1) : result >> 1;
            lat += dlat;

            shift = 0;
            result = 0;
            do
            {
                b = polyline[index++] - 63;
                result |= (b & 0x1F) << shift;
                shift += 5;
            } while (b >= 0x20);
            int dlng = (result & 1) != 0 ? ~(result >> 1) : result >> 1;
            lng += dlng;

            points.Add(new UserPoint
            {
                GeoPoint = new GeoPoint
                {
                    Lat = lat / 1e5,
                    Lon = lng / 1e5
                },
                Priority = 0
            });
        }

        return points;
    }
}
