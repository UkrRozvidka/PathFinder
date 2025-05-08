using BLL.DTOs.HikeMember;
using BLL.DTOs.Track;
using BLL.DTOs.User;
using Infrasturcture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.Hike
{
    public class HikeGetFullDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public long AdminId { get; set; }
        public string AdminName {  get; set; }
        public int MaxDistanceKm { get; set; }
        public GeoPoint StartPoint { get; set; }
        public GeoPoint EndPoint { get; set; }
        public List<HikeMemberGetFullDTO> HikeMembers { get; set; } = new List<HikeMemberGetFullDTO>();
        public List<TrackGetDTO> Tracks { get; set; } = new List<TrackGetDTO>();

    }
}
