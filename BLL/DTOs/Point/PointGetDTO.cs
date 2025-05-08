using Infrasturcture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.Point
{
    public class PointGetDTO
    {
        public long Id { get; set; }
        public GeoPoint GeoPoint { get; set; }
        public int Priority { get; set; }
        public long HikeMemberId { get; set; }
    }
}
