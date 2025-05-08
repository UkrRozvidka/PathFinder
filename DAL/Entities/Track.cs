using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Track : BaseEntity
    {
        public long HikeId { get; set; }
        public Hike Hike { get; set; }
        public string FileName { get; set; }
        public byte[] GpxFile { get; set; }
    }
}
