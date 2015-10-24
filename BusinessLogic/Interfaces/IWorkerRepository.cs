using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IWorkerRepository
    {
        IEnumerable<Worker> GetAll();
        Worker Get(int Id);
        void Save(Worker obj);
        void Delete(int Id);
    }
}
