using System.ComponentModel.DataAnnotations;
using Domain.Base;
namespace Domain.Entities
{
    /// <summary>
    /// Класс "Избиратель"
    /// </summary>
    public class Voter : EntityBase
    {
        //public int Id { get; set; }

        [Display(Name = "Физ. лицо")]
        public int? PersonId { get; set; }

        [Display(Name = "Сдал биометрические данные?")]
        public bool GaveBiometricData { get; set; }
    }
}
