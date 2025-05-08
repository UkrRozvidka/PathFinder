using Infrasturcture;

namespace BLL.DTOs.Hike
{
    public class HikeCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public int MaxDistanceKm { get; set; }
        public GeoPoint StartPoint { get; set; }
        public GeoPoint EndPoint { get; set; } 
    }
}
