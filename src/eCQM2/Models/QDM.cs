using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCQM2.Models
{
    [Table("QDMs")]
    public class QDM
    {
        [Key]
        public int qdmId { get; set; }
        public string qdmTitle { get; set; }
        public string qdmDatatype { get; set; }
        public string valueSet { get; set; }
    }
}
