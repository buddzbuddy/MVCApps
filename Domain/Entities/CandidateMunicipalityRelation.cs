using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class CandidateMunicipalityRelation
    {
        public int Id { get; set; }

        [Display(Name = "Кандидат")]
        public int? CandidateId { get; set; }

        [Display(Name = "МТУ")]
        public int? MunicipalityId { get; set; }
    }
}
