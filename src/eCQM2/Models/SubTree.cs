using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCQM2.Models
{
    public class SubTree
    {
        public int subTreeId { get; set; }
        public int clauseId { get; set; }
        public virtual Clause Clause { get; set; }
    }
}
