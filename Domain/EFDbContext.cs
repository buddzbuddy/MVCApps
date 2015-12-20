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
        public DbSet<VoterPartyRelation> VoterPartyRelations { get; set; }
        public DbSet<Voter> Voters { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<PersonRelation> PersonRelations { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Agitator> Agitators { get; set; }
        public DbSet<WorkerHouseRelation> WorkerHouseRelations { get; set; }
        public DbSet<AgitatorHouseRelation> AgitatorHouseRelations { get; set; }
        public DbSet<AgitatorPrecinctRelation> AgitatorPrecinctRelations { get; set; }
        public DbSet<CandidatePrecinctRelation> CandidatePrecinctRelations { get; set; }
        public DbSet<CandidateMunicipalityRelation> CandidateMunicipalityRelations { get; set; }
        public DbSet<TempPerson> TempPersons { get; set; }
        public DbSet<LatLng> LatLngs { get; set; }
        public DbSet<Marker> Markers { get; set; }
        public DbSet<Polygon> Polygons { get; set; }
        public DbSet<PolygonLatLngRelation> PolygonLatLngRelations { get; set; }
        public DbSet<PartyMember> PartyMembers { get; set; }
        public DbSet<PartySupporter> PartySupporters { get; set; }
    }
}
