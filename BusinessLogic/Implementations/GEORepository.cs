using BusinessLogic.Interfaces;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Implementations
{
    public class GEORepository : IGEORepository
    {
        private EFDbContext context;

        public GEORepository(EFDbContext context)
        {
            this.context = context;
        }
        
        public IEnumerable<LatLng> LatLngGetAll()
        {
            return context.LatLngs;
        }

        public LatLng LatLngGet(int Id)
        {
            return context.LatLngs.Find(Id);
        }

        public void LatLngSave(LatLng obj)
        {
            if (obj.Id == 0)
                context.Entry(obj).State = System.Data.Entity.EntityState.Added;
            else
                context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }

        public void LatLngDelete(int Id)
        {
            var obj = LatLngGet(Id);
            context.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
        }

        public IEnumerable<Marker> MarkerGetAll()
        {
            return context.Markers;
        }

        public Marker MarkerGet(int Id)
        {
            return context.Markers.Find(Id);
        }

        public void MarkerSave(Marker obj)
        {
            if (obj.Id == 0)
                context.Entry(obj).State = System.Data.Entity.EntityState.Added;
            else
                context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }

        public void MarkerDelete(int Id)
        {
            var obj = MarkerGet(Id);
            context.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
        }

        public IEnumerable<Polygon> PolygonGetAll()
        {
            return context.Polygons;
        }

        public Polygon PolygonGet(int Id)
        {
            return context.Polygons.Find(Id);
        }

        public void PolygonSave(Polygon obj)
        {
            if (obj.Id == 0)
                context.Entry(obj).State = System.Data.Entity.EntityState.Added;
            else
                context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }

        public void PolygonDelete(int Id)
        {
            var obj = PolygonGet(Id);
            context.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
        }

        public IEnumerable<PolygonLatLngRelation> PolygonLatLngRelationGetAll()
        {
            return context.PolygonLatLngRelations;
        }

        public PolygonLatLngRelation PolygonLatLngRelationGet(int Id)
        {
            return context.PolygonLatLngRelations.Find(Id);
        }

        public void PolygonLatLngRelationSave(PolygonLatLngRelation obj)
        {
            if (obj.Id == 0)
                context.Entry(obj).State = System.Data.Entity.EntityState.Added;
            else
                context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }

        public void PolygonLatLngRelationDelete(int Id)
        {
            var obj = PolygonLatLngRelationGet(Id);
            context.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
        }
    }
}
