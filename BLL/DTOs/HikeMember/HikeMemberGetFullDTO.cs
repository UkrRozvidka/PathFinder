using BLL.DTOs.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.HikeMember
{
    public class HikeMemberGetFullDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long HikeId { get; set; }
        public string HikeName { get; set;}
        public List<PointGetDTO> Points { get; set; } = new List<PointGetDTO>();
    }
}
