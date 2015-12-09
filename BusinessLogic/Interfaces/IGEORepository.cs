using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IGEORepository
    {
        IEnumerable<LatLng> LatLngGetAll();
        LatLng LatLngGet(int Id);
        void LatLngSave(LatLng obj);
        void LatLngDelete(int Id);
        IEnumerable<Marker> MarkerGetAll();
        Marker MarkerGet(int Id);
        void MarkerSave(Marker obj);
        void MarkerDelete(int Id);
        IEnumerable<Polygon> PolygonGetAll();
        Polygon PolygonGet(int Id);
        void PolygonSave(Polygon obj);
        void PolygonDelete(int Id);
        IEnumerable<PolygonLatLngRelation> PolygonLatLngRelationGetAll();
        PolygonLatLngRelation PolygonLatLngRelationGet(int Id);
        void PolygonLatLngRelationSave(PolygonLatLngRelation obj);
        void PolygonLatLngRelationDelete(int Id);
    }
}
