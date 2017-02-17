﻿using eCQM2.Models;
using eQM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCQM2.Models
{
    public class eMeasure
    {
        public int eMeasureId { get; set; }
        public string title { get; set; }
        public int eMeasureNumber { get; set; }
        public string guidance { get; set; }
        public string clinicalRecommendation { get; set; }
        public string rationale { get; set; }

        public virtual ICollection<Reference> References{ get; set; }
    }
}
