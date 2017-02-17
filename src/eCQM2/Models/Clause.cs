using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCQM2.Models
{
    public class Clause
    {
        public int clauseId { get; set; }
        public string clauseName { get; set; }
        public string logicalOperator { get; set; }
        
        public SubTree subTreeName { get; set; }
    }
}
