using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrasturcture
{
    [Owned]
    public class GeoPoint
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
