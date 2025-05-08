using Infrasturcture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Point : BaseEntity
    {
        public GeoPoint GeoPoint { get; set; }
        public int Priority { get; set; }
        public long HikeMemberId { get; set; }
        public HikeMember HikeMember { get; set; }
    }
}
