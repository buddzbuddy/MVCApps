using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class WorkerHouseRelation
    {
        public int Id { get; set; }

        [Display(Name = "Работник штаба")]
        public int? WorkerId { get; set; }

        [Display(Name = "Дом")]
        public int? HouseId { get; set; }
    }
}
