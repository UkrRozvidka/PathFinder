using Infrasturcture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Hike : BaseEntity
    {
        public string Name { get; set; }
        public long AdminId { get; set; }
        public User Admin { get; set; }
        public GeoPoint Start { get; set; }
        public GeoPoint End { get; set; } 
        public int MaxDistanceKm { get; set; } 
        public ICollection<Track> Tracks { get; set; }
        public ICollection<HikeMember> HikeMembers { get; set; }
    }
}
