using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class EFDbContext : DbContext
    {
        public EFDbContext()
        {

        }
        public EFDbContext(string connectionString)
        {
            Database.Connection.ConnectionString = connectionString;
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<District> Districts { get; set; }
        public DbSet<Locality> Localities { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<MunicipalityHouseRelation> MunicipalityHouseRelations { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<Precinct> Precincts { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<PersonPartyRelation> PersonPartyRelations { get; set; }
    }
}
