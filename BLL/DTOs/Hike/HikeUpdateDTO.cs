using Infrasturcture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.Hike
{
    public class HikeUpdateDTO
    {
        public string Name { get; set; } = string.Empty;
        public int MaxDistanceKm { get; set; }
        public GeoPoint StartPoint { get; set; }
        public GeoPoint EndPoint { get; set; }
    }
}
