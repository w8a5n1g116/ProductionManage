using FineUIMvc.EmptyProject.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FineUIMvc.EmptyProject.Controllers
{
    public class ProductQuaController : BaseController
    {
        private MoJuDataEntities db = new MoJuDataEntities();
        // GET: ProductQua
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

            ViewBag.RecordCount = db.ProductQua.Where(p => p.DATE.Value.Year == year && p.DATE.Value.Month == month).Count();
            return View(PagingHelper<ProductQua>.GetPagedDataTable(0, 20, ViewBag.RecordCount, db.ProductQua.Where(p => p.DATE.Value.Year == year && p.DATE.Value.Month == month).ToList()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string[] Grid1_fields, DateTime? yearMonth, string deportment)
        {
            List<ProductQua> pmList;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (yearMonth != null)
                pmList = db.ProductQua.Where(p => p.DATE.Value.Year == yearMonth.Value.Year && p.DATE.Value.Month == yearMonth.Value.Month).ToList();
            else
                pmList = db.ProductQua.Where(p => p.DATE.Value.Year == year && p.DATE.Value.Month == month).ToList();

            if (!string.IsNullOrEmpty(deportment))
            {
                pmList = pmList.Where(p => p.FAB_NAME == deportment).ToList();
            }

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = pmList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<ProductQua>.GetPagedDataTable(0, 20, recordCount, pmList);
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

                    ProductQua pm = db.ProductQua.Where(p => p.ID == id).FirstOrDefault();

                    string FAB_NAME = values.Value<string>("FAB_NAME");
                    string VENTURENAME = values.Value<string>("VENTURENAME");
                    string OPERATION_NAME = values.Value<string>("OPERATION_NAME");
                    int? TOTALQTY = values.Value<int?>("TOTALQTY");
                    int? QUAQTY = values.Value<int?>("QUAQTY");                   
                    DateTime? DATE = values.Value<DateTime?>("DATE");
                    double? RATE = values.Value<double?>("RATE");

                    if (FAB_NAME != null)
                        pm.FAB_NAME = FAB_NAME;
                    if (VENTURENAME != null)
                        pm.VENTURENAME = VENTURENAME;
                    if (OPERATION_NAME != null)
                        pm.OPERATION_NAME = OPERATION_NAME;
                    if (TOTALQTY != null)
                        pm.TOTALQTY = TOTALQTY;
                    if (QUAQTY != null)
                        pm.QUAQTY = QUAQTY;
                    if (DATE != null)
                        pm.DATE = DATE;
                    if (RATE != null)
                        pm.RATE = RATE;

                    db.SaveChanges();
                }
                else if (status == "newadded")
                {
                    ProductQua pm = new ProductQua();

                    string FAB_NAME = values.Value<string>("FAB_NAME");
                    string VENTURENAME = values.Value<string>("VENTURENAME");
                    string OPERATION_NAME = values.Value<string>("OPERATION_NAME");
                    int? TOTALQTY = values.Value<int?>("TOTALQTY");
                    int? QUAQTY = values.Value<int?>("QUAQTY");
                    string DATE = values.Value<string>("DATE");
                    double? RATE = values.Value<double?>("RATE");


                    if (FAB_NAME != null)
                        pm.FAB_NAME = FAB_NAME;
                    if (VENTURENAME != null)
                        pm.VENTURENAME = VENTURENAME;
                    if (OPERATION_NAME != null)
                        pm.OPERATION_NAME = OPERATION_NAME;
                    if (TOTALQTY != null)
                        pm.TOTALQTY = TOTALQTY;
                    if (QUAQTY != null)
                        pm.QUAQTY = QUAQTY;
                    if (RATE != null)
                        pm.RATE = RATE;
                    if (!string.IsNullOrEmpty(DATE))
                        pm.DATE = Convert.ToDateTime(DATE);

                    db.ProductQua.Add(pm);
                    db.SaveChanges();
                }
                else if (status == "deleted")
                {
                    int id = mergedRow.Value<int>("id");

                    ProductQua pm = db.ProductQua.Where(p => p.ID == id).FirstOrDefault();

                    db.ProductQua.Remove(pm);

                    db.SaveChanges();
                }
            }

            List<ProductQua> pmList;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (yearMonth != null)
                pmList = db.ProductQua.Where(p => p.DATE.Value.Year == yearMonth.Value.Year && p.DATE.Value.Month == yearMonth.Value.Month).ToList();
            else
                pmList = db.ProductQua.Where(p => p.DATE.Value.Year == year && p.DATE.Value.Month == month).ToList();

            var dataSource = PagingHelper<ProductQua>.GetPagedDataTable(pageIndex, 20, pmList.Count(), pmList);
            UIHelper.Grid("Grid1").DataSource(dataSource, Grid1_fields);
            Alert.Show("操作成功！");

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_PageIndexChanged(string[] Grid1_fields, int Grid1_pageIndex, DateTime? yearMonth, string deportment)
        {
            List<ProductQua> pmList;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (yearMonth != null)
                pmList = db.ProductQua.Where(p => p.DATE.Value.Year == yearMonth.Value.Year && p.DATE.Value.Month == yearMonth.Value.Month).ToList();
            else
                pmList = db.ProductQua.Where(p => p.DATE.Value.Year == year && p.DATE.Value.Month == month).ToList();

            if (!string.IsNullOrEmpty(deportment))
            {
                pmList = pmList.Where(p => p.FAB_NAME == deportment).ToList();
            }

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = pmList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<ProductQua>.GetPagedDataTable(Grid1_pageIndex, 20, recordCount, pmList);
            grid1.DataSource(dataSource, Grid1_fields);

            return UIHelper.Result();
        }
    }
}