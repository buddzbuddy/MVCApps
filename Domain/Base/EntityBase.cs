using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Base
{
    public partial class EntityBase
    {
        public int Id { get; set; }
        private DateTime? CreationTimeDef = DateTime.Now;
        public DateTime? CreationTime { get { return CreationTimeDef; } set { CreationTimeDef = value; } }
        private bool? HasConstraintDef = false;
        public bool? HasConstraint { get { return HasConstraintDef; } set { HasConstraintDef = false; } }
    }
}
