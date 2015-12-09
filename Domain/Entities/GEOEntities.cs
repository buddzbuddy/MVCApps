using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{

    public class LatLng
    {
        public int Id { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class Marker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? LatLngId { get; set; }
    }

    public class Polygon
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PolygonLatLngRelation
    {
        public int Id { get; set; }
        public int? LatLngId { get; set; }
        public int? PolygonId { get; set; }
    }
}
