using Infrasturcture;

namespace BLL.DTOs.Point
{
    public class PointCreateDTO
    {
        public GeoPoint GeoPoint {  get; set; }
        public int Priority { get; set; }
        public long HikeMemberId { get; set; }
    }
}
