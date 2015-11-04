using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class AgitatorHouseRelation
    {
        public int Id { get; set; }

        [Display(Name = "Агитатор")]
        public int? AgitatorId { get; set; }

        [Display(Name = "Дом")]
        public int? HouseId { get; set; }
    }
}
