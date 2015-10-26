﻿using BusinessLogic.Implementations;
using BusinessLogic.Interfaces;
using Domain;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VoterManager
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBinds();
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBinds()
        {
            ninjectKernel.Bind<EFDbContext>().ToSelf()
                .WithConstructorArgument("connectionString",
                System.Configuration.ConfigurationManager.ConnectionStrings[1].ConnectionString);
            ninjectKernel.Bind<IDistrictRepository>().To<DistrictRepository>();
            ninjectKernel.Bind<ILocalityRepository>().To<LocalityRepository>();
            ninjectKernel.Bind<IStreetRepository>().To<StreetRepository>();
            ninjectKernel.Bind<IHouseRepository>().To<HouseRepository>();
            ninjectKernel.Bind<IMunicipalityRepository>().To<MunicipalityRepository>();
            ninjectKernel.Bind<IPersonRepository>().To<PersonRepository>();
            ninjectKernel.Bind<INationalityRepository>().To<NationalityRepository>();
            ninjectKernel.Bind<IEducationRepository>().To<EducationRepository>();
            ninjectKernel.Bind<IOrganizationRepository>().To<OrganizationRepository>();
            ninjectKernel.Bind<IMunicipalityHouseRelationRepository>().To<MunicipalityHouseRelationRepository>();
            ninjectKernel.Bind<IRegistrationRepository>().To<RegistrationRepository>();
            ninjectKernel.Bind<IPartyRepository>().To<PartyRepository>();
            ninjectKernel.Bind<IPrecinctRepository>().To<PrecinctRepository>();
            ninjectKernel.Bind<IUserRepository>().To<UserRepository>();
            ninjectKernel.Bind<IWorkerRepository>().To<WorkerRepository>();
            ninjectKernel.Bind<IPersonPartyRelationRepository>().To<PersonPartyRelationRepository>();
        }
    }
}