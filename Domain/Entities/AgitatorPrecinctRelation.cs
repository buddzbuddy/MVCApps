using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class AgitatorPrecinctRelation
    {
        public int Id { get; set; }

        [Display(Name = "Агитатор")]
        public int? AgitatorId { get; set; }

        [Display(Name = "УИК")]
        public int? PrecinctId { get; set; }
    }
}
