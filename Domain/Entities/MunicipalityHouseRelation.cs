using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class MunicipalityHouseRelation
    {
        public int Id { get; set; }
        
        [Display(Name = "Дом")]
        public int? HouseId { get; set; }

        [Display(Name = "МТУ")]
        public int? MunicipalityId { get; set; }
    }
}
