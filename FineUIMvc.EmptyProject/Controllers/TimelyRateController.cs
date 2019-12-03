using FineUIMvc.EmptyProject.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FineUIMvc.EmptyProject.Controllers
{
    public class TimelyRateController : BaseController
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

            ViewBag.RecordCount = db.TimelyRate.Where(p => p.PlanDate.Value.Year == year && p.PlanDate.Value.Month == month).Count();
            return View(PagingHelper<TimelyRate>.GetPagedDataTable(0, 20, ViewBag.RecordCount, db.TimelyRate.Where(p => p.PlanDate.Value.Year == year && p.PlanDate.Value.Month == month).ToList()));
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string[] Grid1_fields, DateTime? yearMonth, string deportment)
        {
            List<TimelyRate> pmList;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (yearMonth != null)
                pmList = db.TimelyRate.Where(p => p.PlanDate.Value.Year == yearMonth.Value.Year && p.PlanDate.Value.Month == yearMonth.Value.Month).ToList();
            else
                pmList = db.TimelyRate.Where(p => p.PlanDate.Value.Year == year && p.PlanDate.Value.Month == month).ToList();

            if (!string.IsNullOrEmpty(deportment))
            {
                pmList = pmList.Where(p => p.FAB_NAME == deportment).ToList();
            }

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = pmList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<TimelyRate>.GetPagedDataTable(0, 20, recordCount, pmList);
            grid1.DataSource(dataSource, Grid1_fields);

            return UIHelper.Result();
        }

        public ActionResult Check()
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

            ViewBag.RecordCount = db.TimelyRate.Where(p => p.PlanDate.Value.Year == year && p.PlanDate.Value.Month == month).Count();
            return View(PagingHelper<TimelyRate>.GetPagedDataTable(0, 20, ViewBag.RecordCount, db.TimelyRate.Where(p => p.PlanDate.Value.Year == year && p.PlanDate.Value.Month == month).ToList()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Check(string[] Grid1_fields, DateTime? yearMonth, string deportment)
        {
            List<TimelyRate> pmList;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (yearMonth != null)
                pmList = db.TimelyRate.Where(p => p.PlanDate.Value.Year == yearMonth.Value.Year && p.PlanDate.Value.Month == yearMonth.Value.Month).ToList();
            else
                pmList = db.TimelyRate.Where(p => p.PlanDate.Value.Year == year && p.PlanDate.Value.Month == month).ToList();

            if (!string.IsNullOrEmpty(deportment))
            {
                pmList = pmList.Where(p => p.FAB_NAME == deportment).ToList();
            }

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = pmList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<TimelyRate>.GetPagedDataTable(0, 20, recordCount, pmList);
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

                    TimelyRate pm = db.TimelyRate.Where(p => p.ID == id).FirstOrDefault();

                    string FAB_NAME = values.Value<string>("FAB_NAME");
                    double? Week1 = values.Value<double?>("Week1");
                    double? Week2 = values.Value<double?>("Week2");
                    double? Week3 = values.Value<double?>("Week3");
                    double? Week4 = values.Value<double?>("Week4");
                    double? Month1 = values.Value<double?>("Month1");
                    double? RealPlan = values.Value<double?>("RealPlan");
                    DateTime? PlanDate = values.Value<DateTime?>("PlanDate");

                    if (FAB_NAME != null)
                        pm.FAB_NAME = FAB_NAME;
                    if (Week1 != null)
                        pm.Week1 = Week1;
                    if (Week2 != null)
                        pm.Week2 = Week2;
                    if (Week3 != null)
                        pm.Week3 = Week3;
                    if (Week4 != null)
                        pm.Week4 = Week4;
                    if (Month1 != null)
                        pm.Month1 = Month1;
                    if (RealPlan != null)
                        pm.RealPlan = RealPlan;
                    if (PlanDate != null)
                        pm.PlanDate = PlanDate;

                    db.SaveChanges();
                }
                else if (status == "newadded")
                {
                    TimelyRate pm = new TimelyRate();

                    string FAB_NAME = values.Value<string>("FAB_NAME");
                    double? Week1 = values.Value<double?>("Week1");
                    double? Week2 = values.Value<double?>("Week2");
                    double? Week3 = values.Value<double?>("Week3");
                    double? Week4 = values.Value<double?>("Week4");
                    double? Month1 = values.Value<double?>("Month1");
                    double? RealPlan = values.Value<double?>("RealPlan");
                    string PlanDate = values.Value<string>("PlanDate");


                    if (FAB_NAME != null)
                        pm.FAB_NAME = FAB_NAME;
                    if (Week1 != null)
                        pm.Week1 = Week1;
                    if (Week2 != null)
                        pm.Week2 = Week2;
                    if (Week3 != null)
                        pm.Week3 = Week3;
                    if (Week4 != null)
                        pm.Week4 = Week4;
                    if (Month1 != null)
                        pm.Month1 = Month1;
                    if (RealPlan != null)
                        pm.RealPlan = RealPlan;
                    if (!string.IsNullOrEmpty(PlanDate))
                        pm.PlanDate = Convert.ToDateTime(PlanDate);

                    db.TimelyRate.Add(pm);
                    db.SaveChanges();
                }
                else if (status == "deleted")
                {
                    int id = mergedRow.Value<int>("id");

                    TimelyRate pm = db.TimelyRate.Where(p => p.ID == id).FirstOrDefault();

                    db.TimelyRate.Remove(pm);

                    db.SaveChanges();
                }
            }

            List<TimelyRate> pmList;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (yearMonth != null)
                pmList = db.TimelyRate.Where(p => p.PlanDate.Value.Year == yearMonth.Value.Year && p.PlanDate.Value.Month == yearMonth.Value.Month).ToList();
            else
                pmList = db.TimelyRate.Where(p => p.PlanDate.Value.Year == year && p.PlanDate.Value.Month == month).ToList();

            var dataSource = PagingHelper<TimelyRate>.GetPagedDataTable(pageIndex, 20, pmList.Count(), pmList);
            UIHelper.Grid("Grid1").DataSource(dataSource, Grid1_fields);
            Alert.Show("操作成功！");

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_PageIndexChanged(string[] Grid1_fields, int Grid1_pageIndex, DateTime? yearMonth, string deportment)
        {
            List<TimelyRate> pmList;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (yearMonth != null)
                pmList = db.TimelyRate.Where(p => p.PlanDate.Value.Year == yearMonth.Value.Year && p.PlanDate.Value.Month == yearMonth.Value.Month).ToList();
            else
                pmList = db.TimelyRate.Where(p => p.PlanDate.Value.Year == year && p.PlanDate.Value.Month == month).ToList();

            if (!string.IsNullOrEmpty(deportment))
            {
                pmList = pmList.Where(p => p.FAB_NAME == deportment).ToList();
            }

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = pmList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<TimelyRate>.GetPagedDataTable(Grid1_pageIndex, 20, recordCount, pmList);
            grid1.DataSource(dataSource, Grid1_fields);

            return UIHelper.Result();
        }
    }
}