using eCQM2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCQM2.Models
{
    [Table("References")]
    public class Reference
    {
        [Key]
        public int ReferenceId { get; set; }
        public string reference { get; set; }
        public int eSMeasureId { get; set; }

        public virtual eMeasure eMeasure { get; set; }
    }
}