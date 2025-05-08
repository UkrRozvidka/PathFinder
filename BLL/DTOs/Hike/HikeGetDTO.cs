using Infrasturcture;

namespace BLL.DTOs.Hike
{
    public class HikeGetDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public long AdminId { get; set; }
        public int MaxDistanceKm { get; set; }
        public GeoPoint StartPoint { get; set; }
        public GeoPoint EndPoint { get; set; }
    }
}
