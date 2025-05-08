using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class HikeMember : BaseEntity
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public long HikeId { get; set; }
        public Hike Hike { get; set; }
        public ICollection<Point> Points { get; set; }
    }
}
