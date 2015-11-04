using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class CandidatePrecinctRelation
    {
        public int Id { get; set; }

        [Display(Name = "Кандидат")]
        public int? CandidateId { get; set; }

        [Display(Name = "УИК")]
        public int? PrecinctId { get; set; }
    }
}
