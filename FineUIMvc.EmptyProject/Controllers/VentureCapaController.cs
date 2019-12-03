using FineUIMvc.EmptyProject.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FineUIMvc.EmptyProject.Controllers
{
    public class VentureCapaController : BaseController
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

            ViewBag.RecordCount = db.VentureCapa.Count();
            return View(PagingHelper<VentureCapa>.GetPagedDataTable(0, 20, ViewBag.RecordCount, db.VentureCapa.ToList()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string[] Grid1_fields, string deportment)
        {
            List<VentureCapa> pmList;

            pmList = db.VentureCapa.ToList();

            if (!string.IsNullOrEmpty(deportment))
            {
                pmList = pmList.Where(p => p.FAB_NAME == deportment).ToList();
            }

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = pmList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<VentureCapa>.GetPagedDataTable(0, 20, recordCount, pmList);
            grid1.DataSource(dataSource, Grid1_fields);

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult btnSubmit_Click(string[] Grid1_fields, JArray Grid1_modifiedData, int pageIndex)
        {
            foreach (JObject mergedRow in Grid1_modifiedData)
            {
                string status = mergedRow.Value<string>("status");
                int rowIndex = mergedRow.Value<int>("index");
                JObject values = mergedRow.Value<JObject>("values");

                if (status == "modified")
                {
                    int id = mergedRow.Value<int>("id");

                    VentureCapa pm = db.VentureCapa.Where(p => p.ID == id).FirstOrDefault();

                    string FAB_NAME = values.Value<string>("FAB_NAME");
                    string VENTURENAME = values.Value<string>("VENTURENAME");
                    string OPERATIONNAME = values.Value<string>("OPERATIONNAME");
                    double? PROCUT_QUOTA = values.Value<double?>("PROCUT_QUOTA");
                    double? PROCUT_QUOTA_WEEK = values.Value<double?>("PROCUT_QUOTA_WEEK");
                    double? PROCUT_QUOTA_MONTH = values.Value<double?>("PROCUT_QUOTA_MONTH");

                    if (FAB_NAME != null)
                        pm.FAB_NAME = FAB_NAME;
                    if (VENTURENAME != null)
                        pm.VENTURENAME = VENTURENAME;
                    if (OPERATIONNAME != null)
                        pm.OPERATIONNAME = OPERATIONNAME;
                    if (PROCUT_QUOTA != null)
                        pm.PROCUT_QUOTA = PROCUT_QUOTA;
                    if (PROCUT_QUOTA_WEEK != null)
                        pm.PROCUT_QUOTA_WEEK = PROCUT_QUOTA_WEEK;
                    if (PROCUT_QUOTA_MONTH != null)
                        pm.PROCUT_QUOTA_MONTH = PROCUT_QUOTA_MONTH;

                    db.SaveChanges();
                }
                else if (status == "newadded")
                {
                    VentureCapa pm = new VentureCapa();

                    string FAB_NAME = values.Value<string>("FAB_NAME");
                    string VENTURENAME = values.Value<string>("VENTURENAME");
                    string OPERATIONNAME = values.Value<string>("OPERATIONNAME");
                    double? PROCUT_QUOTA = values.Value<double?>("PROCUT_QUOTA");
                    double? PROCUT_QUOTA_WEEK = values.Value<double?>("PROCUT_QUOTA_WEEK");
                    double? PROCUT_QUOTA_MONTH = values.Value<double?>("PROCUT_QUOTA_MONTH");


                    if (FAB_NAME != null)
                        pm.FAB_NAME = FAB_NAME;
                    if (VENTURENAME != null)
                        pm.VENTURENAME = VENTURENAME;
                    if (OPERATIONNAME != null)
                        pm.OPERATIONNAME = OPERATIONNAME;
                    if (PROCUT_QUOTA != null)
                        pm.PROCUT_QUOTA = PROCUT_QUOTA;
                    if (PROCUT_QUOTA_WEEK != null)
                        pm.PROCUT_QUOTA_WEEK = PROCUT_QUOTA_WEEK;
                    if (PROCUT_QUOTA_MONTH != null)
                        pm.PROCUT_QUOTA_MONTH = PROCUT_QUOTA_MONTH;

                    db.VentureCapa.Add(pm);
                    db.SaveChanges();
                }
                else if (status == "deleted")
                {
                    int id = mergedRow.Value<int>("id");

                    VentureCapa pm = db.VentureCapa.Where(p => p.ID == id).FirstOrDefault();

                    db.VentureCapa.Remove(pm);

                    db.SaveChanges();
                }
            }

            List<VentureCapa> pmList;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            pmList = db.VentureCapa.ToList();

            var dataSource = PagingHelper<VentureCapa>.GetPagedDataTable(pageIndex, 20, pmList.Count(), pmList);
            UIHelper.Grid("Grid1").DataSource(dataSource, Grid1_fields);
            Alert.Show("操作成功！");

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_PageIndexChanged(string[] Grid1_fields, int Grid1_pageIndex, string deportment)
        {
            List<VentureCapa> pmList;

            pmList = db.VentureCapa.ToList();

            if (!string.IsNullOrEmpty(deportment))
            {
                pmList = pmList.Where(p => p.FAB_NAME == deportment).ToList();
            }

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = pmList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<VentureCapa>.GetPagedDataTable(Grid1_pageIndex, 20, recordCount, pmList);
            grid1.DataSource(dataSource, Grid1_fields);

            return UIHelper.Result();
        }
    }
}