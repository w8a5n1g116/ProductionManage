using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FineUIMvc.EmptyProject.Models.Model
{
    public class ProductionHoursReport
    {
        public int Month { get; set; }
        public string Deportment { get; set; }
        public double YearlyPlan  { get; set; }
        public double Complete { get; set; }
        public double FinishingRate { get; set; }
        public double MonthHourPlan { get; set; }
        public double MonthHourComplete { get; set; }
        public double MonthFinishingRate { get; set; }
    }
}