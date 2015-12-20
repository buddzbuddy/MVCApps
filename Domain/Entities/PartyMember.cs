using Domain.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    /// <summary>
    /// Класс "Член партии"
    /// </summary>
    public class PartyMember : EntityBase
    {
        [Display(Name = "Физ. лицо")]
        public int? PersonId { get; set; }

        [Display(Name = "Дата вступления")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{yyyy'-'MM'-'0:dd}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EntryDate { get; set; }

        [Display(Name = "Позиция в партийной структуре")]
        public string StructurePosition { get; set; }
    }
}
