﻿using Domain;
using Domain.Entities;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;

namespace BusinessLogic
{
    public class DataManager
    {
        private IDistrictRepository districtRepository;
        private IMunicipalityRepository municipalityRepository;
        private IPersonRepository personRepository;
        private ILocalityRepository localityRepository;
        private IStreetRepository streetRepository;
        private IHouseRepository houseRepository;
        private INationalityRepository nationalityRepository;
        private IEducationRepository educationRepository;
        private IOrganizationRepository organizationRepository;
        private IMunicipalityHouseRelationRepository municipalityHouseRelationRepository;
        private IRegistrationRepository registrationRepository;
        private IPartyRepository partyRepository;
        private IPrecinctRepository precinctRepository;
        private IUserRepository userRepository;
        private IWorkerRepository workerRepository;
        private IPersonPartyRelationRepository personPartyRelationRepository;
        public DataManager(IDistrictRepository districtRepository,
            ILocalityRepository localityRepository,
            IStreetRepository streetRepository,
            IHouseRepository houseRepository,
            IMunicipalityRepository municipalityRepository,
            IPersonRepository personRepository,
            INationalityRepository nationalityRepository,
            IEducationRepository educationRepository,
            IOrganizationRepository organizationRepository,
            IMunicipalityHouseRelationRepository municipalityHouseRelationRepository,
            IRegistrationRepository registrationRepository,
            IPartyRepository partyRepository,
            IPrecinctRepository precinctRepository,
            IUserRepository userRepository,
            IWorkerRepository workerRepository,
            IPersonPartyRelationRepository personPartyRelationRepository)
        {
            this.districtRepository = districtRepository;
            this.municipalityRepository = municipalityRepository;
            this.personRepository = personRepository;
            this.localityRepository = localityRepository;
            this.nationalityRepository = nationalityRepository;
            this.streetRepository = streetRepository;
            this.houseRepository = houseRepository;
            this.educationRepository = educationRepository;
            this.organizationRepository = organizationRepository;
            this.municipalityHouseRelationRepository = municipalityHouseRelationRepository;
            this.registrationRepository = registrationRepository;
            this.partyRepository = partyRepository;
            this.precinctRepository = precinctRepository;
            this.userRepository = userRepository;
            this.workerRepository = workerRepository;
            this.personPartyRelationRepository = personPartyRelationRepository;
        }

        public IDistrictRepository Districts { get { return districtRepository; } }
        public ILocalityRepository Localities { get { return localityRepository; } }
        public IStreetRepository Streets { get { return streetRepository; } }
        public IHouseRepository Houses { get { return houseRepository; } }
        public IMunicipalityRepository Municipalities { get { return municipalityRepository; } }
        public IPersonRepository Persons { get { return personRepository; } }
        public INationalityRepository Nationalities { get { return nationalityRepository; } }
        public IEducationRepository Educations { get { return educationRepository; } }
        public IOrganizationRepository Organizations { get { return organizationRepository; } }
        public IMunicipalityHouseRelationRepository MunicipalityHouseRelations { get { return municipalityHouseRelationRepository; } }
        public IRegistrationRepository Registrations { get { return registrationRepository; } }
        public IPartyRepository Parties { get { return partyRepository; } }
        public IPrecinctRepository Precincts { get { return precinctRepository; } }
        public IUserRepository Users { get { return userRepository; } }
        public IWorkerRepository Workers { get { return workerRepository; } }
        public IPersonPartyRelationRepository PersonPartyRelations { get { return personPartyRelationRepository; } }
    }
}
