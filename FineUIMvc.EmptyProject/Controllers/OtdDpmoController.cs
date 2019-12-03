using FineUIMvc.EmptyProject.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FineUIMvc.EmptyProject.Controllers
{
    public class OtdDpmoController : BaseController
    {
        private MoJuDataEntities db = new MoJuDataEntities();

        public ActionResult Index()
        {
            List<ListItemExtension> lieList = new List<ListItemExtension>();
            List<string> deportments = new List<string>() { "精工工厂", "加工工厂", "模具工厂", "智能设备工厂", "智能机器" };
            foreach (var fanName in deportments)
            {
                ListItem item = new ListItem();
                item.Text = fanName;
                item.Value = fanName;
                ListItemExtension lie = new ListItemExtension(item);
                lieList.Add(lie);
            }

            ViewBag.Deportment = lieList.ToArray();

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            ViewBag.RecordCount = db.OtdDpmo.Where(p => p.Date.Value.Year == year && p.Date.Value.Month == month).Count();
            return View(PagingHelper<OtdDpmo>.GetPagedDataTable(0, 20, ViewBag.RecordCount, db.OtdDpmo.Where(p => p.Date.Value.Year == year && p.Date.Value.Month == month).ToList()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string[] Grid1_fields, DateTime? yearMonth, string deportment)
        {
            List<OtdDpmo> pmList;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (yearMonth != null)
                pmList = db.OtdDpmo.Where(p => p.Date.Value.Year == yearMonth.Value.Year && p.Date.Value.Month == yearMonth.Value.Month).ToList();
            else
                pmList = db.OtdDpmo.Where(p => p.Date.Value.Year == year && p.Date.Value.Month == month).ToList();

            if (!string.IsNullOrEmpty(deportment))
            {
                pmList = pmList.Where(p => p.FAB_NAME == deportment).ToList();
            }

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = pmList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<OtdDpmo>.GetPagedDataTable(0, 20, recordCount, pmList);
            grid1.DataSource(dataSource, Grid1_fields);

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult btnSubmit_Click(string[] Grid1_fields, JArray Grid1_modifiedData, int pageIndex, DateTime? yearMonth)
        {
            foreach (JObject mergedRow in Grid1_modifiedData)
            {
                string status = mergedRow.Value<string>("status");
                int rowIndex = mergedRow.Value<int>("index");
                JObject values = mergedRow.Value<JObject>("values");

                if (status == "modified")
                {
                    int id = mergedRow.Value<int>("id");

                    OtdDpmo pm = db.OtdDpmo.Where(p => p.ID == id).FirstOrDefault();

                    string FAB_NAME = values.Value<string>("FAB_NAME");
                    double? OTD = values.Value<double?>("OTD");
                    double? OTDComplete = values.Value<double?>("OTDComplete");
                    double? DPMO = values.Value<double?>("DPMO");
                    double? DPMOCompletefloat = values.Value<double?>("DPMOCompletefloat");
                    double? Ncr = values.Value<double?>("Ncr");
                    double? HourYearComplete = values.Value<double?>("HourYearComplete");
                    double? HourMonthPlan = values.Value<double?>("HourMonthPlan");
                    double? HourMonthComplete = values.Value<double?>("HourMonthComplete");
                    double? JGGSPlan = values.Value<double?>("JGGSPlan");
                    double? JGGSComplete = values.Value<double?>("JGGSComplete");
                    double? CJGDWPlan = values.Value<double?>("CJGDWPlan");
                    double? CJGDWComplete = values.Value<double?>("CJGDWComplete");
                    double? JJGDWPlan = values.Value<double?>("JJGDWPlan");
                    double? JJGDWComplete = values.Value<double?>("JJGDWComplete");
                    double? Odds = values.Value<double?>("Odds");
                    double? OddsComplete = values.Value<double?>("OddsComplete");
                    DateTime? Date = values.Value<DateTime?>("Date");

                    if (FAB_NAME != null)
                        pm.FAB_NAME = FAB_NAME;
                    if (OTD != null)
                        pm.OTD = OTD;
                    if (OTDComplete != null)
                        pm.OTDComplete = OTDComplete;
                    if (DPMO != null)
                        pm.DPMO = DPMO;
                    if (DPMOCompletefloat != null)
                        pm.DPMOCompletefloat = DPMOCompletefloat;
                    if (Ncr != null)
                        pm.Ncr = Ncr;
                    if (HourYearComplete != null)
                        pm.HourYearComplete = HourYearComplete;
                    if (HourMonthPlan != null)
                        pm.HourMonthPlan = HourMonthPlan;
                    if (HourMonthComplete != null)
                        pm.HourMonthComplete = HourMonthComplete;
                    if (JGGSPlan != null)
                        pm.JGGSPlan = JGGSPlan;
                    if (JGGSComplete != null)
                        pm.JGGSComplete = JGGSComplete;
                    if (CJGDWPlan != null)
                        pm.CJGDWPlan = CJGDWPlan;
                    if (CJGDWComplete != null)
                        pm.CJGDWComplete = CJGDWComplete;
                    if (JJGDWPlan != null)
                        pm.JJGDWPlan = JJGDWPlan;
                    if (JJGDWComplete != null)
                        pm.JJGDWComplete = JJGDWComplete;
                    if (Odds != null)
                        pm.Odds = Odds;
                    if (OddsComplete != null)
                        pm.OddsComplete = OddsComplete;
                    if (Date != null)
                        pm.Date = Date;

                    db.SaveChanges();
                }
                else if (status == "newadded")
                {
                    OtdDpmo pm = new OtdDpmo();

                    string FAB_NAME = values.Value<string>("FAB_NAME");
                    double? OTD = values.Value<double?>("OTD");
                    double? OTDComplete = values.Value<double?>("OTDComplete");
                    double? DPMO = values.Value<double?>("DPMO");
                    double? DPMOCompletefloat = values.Value<double?>("DPMOCompletefloat");
                    double? Ncr = values.Value<double?>("Ncr");
                    double? HourYearComplete = values.Value<double?>("HourYearComplete");
                    double? HourMonthPlan = values.Value<double?>("HourMonthPlan");
                    double? HourMonthComplete = values.Value<double?>("HourMonthComplete");
                    double? JGGSPlan = values.Value<double?>("JGGSPlan");
                    double? JGGSComplete = values.Value<double?>("JGGSComplete");
                    double? CJGDWPlan = values.Value<double?>("CJGDWPlan");
                    double? CJGDWComplete = values.Value<double?>("CJGDWComplete");
                    double? JJGDWPlan = values.Value<double?>("JJGDWPlan");
                    double? JJGDWComplete = values.Value<double?>("JJGDWComplete");
                    double? Odds = values.Value<double?>("Odds");
                    double? OddsComplete = values.Value<double?>("OddsComplete");
                    string Date = values.Value<string>("Date");


                    if (FAB_NAME != null)
                        pm.FAB_NAME = FAB_NAME;
                    if (OTD != null)
                        pm.OTD = OTD;
                    if (OTDComplete != null)
                        pm.OTDComplete = OTDComplete;
                    if (DPMO != null)
                        pm.DPMO = DPMO;
                    if (DPMOCompletefloat != null)
                        pm.DPMOCompletefloat = DPMOCompletefloat;
                    if (Ncr != null)
                        pm.Ncr = Ncr;
                    if (HourYearComplete != null)
                        pm.HourYearComplete = HourYearComplete;
                    if (HourMonthPlan != null)
                        pm.HourMonthPlan = HourMonthPlan;
                    if (HourMonthComplete != null)
                        pm.HourMonthComplete = HourMonthComplete;
                    if (JGGSPlan != null)
                        pm.JGGSPlan = JGGSPlan;
                    if (JGGSComplete != null)
                        pm.JGGSComplete = JGGSComplete;
                    if (CJGDWPlan != null)
                        pm.CJGDWPlan = CJGDWPlan;
                    if (CJGDWComplete != null)
                        pm.CJGDWComplete = CJGDWComplete;
                    if (JJGDWPlan != null)
                        pm.JJGDWPlan = JJGDWPlan;
                    if (JJGDWComplete != null)
                        pm.JJGDWComplete = JJGDWComplete;
                    if (Odds != null)
                        pm.Odds = Odds;
                    if (OddsComplete != null)
                        pm.OddsComplete = OddsComplete;
                    if (!string.IsNullOrEmpty(Date))
                        pm.Date = Convert.ToDateTime(Date);

                    db.OtdDpmo.Add(pm);
                    db.SaveChanges();
                }
                else if (status == "deleted")
                {
                    int id = mergedRow.Value<int>("id");

                    OtdDpmo pm = db.OtdDpmo.Where(p => p.ID == id).FirstOrDefault();

                    db.OtdDpmo.Remove(pm);

                    db.SaveChanges();
                }
            }

            List<OtdDpmo> pmList;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (yearMonth != null)
                pmList = db.OtdDpmo.Where(p => p.Date.Value.Year == yearMonth.Value.Year && p.Date.Value.Month == yearMonth.Value.Month).ToList();
            else
                pmList = db.OtdDpmo.Where(p => p.Date.Value.Year == year && p.Date.Value.Month == month).ToList();

            var dataSource = PagingHelper<OtdDpmo>.GetPagedDataTable(pageIndex, 20, pmList.Count(), pmList);
            UIHelper.Grid("Grid1").DataSource(dataSource, Grid1_fields);
            Alert.Show("操作成功！");

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_PageIndexChanged(string[] Grid1_fields, int Grid1_pageIndex, DateTime? yearMonth, string deportment)
        {
            List<OtdDpmo> pmList;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (yearMonth != null)
                pmList = db.OtdDpmo.Where(p => p.Date.Value.Year == yearMonth.Value.Year && p.Date.Value.Month == yearMonth.Value.Month).ToList();
            else
                pmList = db.OtdDpmo.Where(p => p.Date.Value.Year == year && p.Date.Value.Month == month).ToList();

            if (!string.IsNullOrEmpty(deportment))
            {
                pmList = pmList.Where(p => p.FAB_NAME == deportment).ToList();
            }

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = pmList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<OtdDpmo>.GetPagedDataTable(Grid1_pageIndex, 20, recordCount, pmList);
            grid1.DataSource(dataSource, Grid1_fields);

            return UIHelper.Result();
        }
    }
}