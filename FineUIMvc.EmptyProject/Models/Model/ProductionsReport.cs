using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FineUIMvc.EmptyProject.Models.Model
{
    public class ProductionsReport
    {
        public string Unit { get; set; }
        public string Item { get; set; }
        public double Plan { get; set; }
        public double Complete { get; set; }
        public double FinishingRate { get; set; }
    }
}