using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoterManager.Models
{
    public class AgitatorViewModel
    {
        public Agitator Agitator { get; set; }

        private Person _person;
        public Person Person { 
            get 
            {
                if (_person == null)
                    return new Person();
                else
                    return _person;
            }
            set
            {
                _person = value;
            }
        }

        private PersonViewModel _personView;
        public PersonViewModel PersonView {
            get
            {
                return _personView != null ? _personView : new PersonViewModel();
            }
            set
            {
                _personView = value;
            }
        }

        public List<AgitatorHouseRelationViewModel> RelatedHouses { get; set; }
        public List<AgitatorPrecinctRelationViewModel> RelatedPrecincts { get; set; }
    }
}