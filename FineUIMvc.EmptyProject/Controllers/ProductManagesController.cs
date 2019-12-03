using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FineUIMvc.EmptyProject.Models;
using FineUIMvc.EmptyProject.Models.Model;
using Newtonsoft.Json.Linq;
using SAPLib;
using OfficeOpenXml;
using System.IO;
using System.Globalization;

namespace FineUIMvc.EmptyProject.Controllers
{
    public class ProductManagesController : BaseController
    {
        private MoJuDataEntities db = new MoJuDataEntities();
        private SAPReader sapreader = new SAPReader();

        // GET: ProductManages
        public ActionResult Index()
        {
            List<ListItemExtension> lieList = new List<ListItemExtension>();
            List<string> deportments = new List<string>() { "","精工工厂", "加工工厂", "模具工厂", "智能设备工厂", "智能机器" };//db.ProductManage.Select(p => p.FAB_NAME).Distinct().ToList();
            foreach (var fanName in deportments)
            {
                ListItem item = new ListItem();
                item.Text = fanName;
                item.Value = fanName;
                ListItemExtension lie = new ListItemExtension(item);
                lieList.Add(lie);
            }

            ViewBag.Deportment = lieList.ToArray();

            ViewBag.RecordCount = db.ProductManage.Count();
            return View(PagingHelper<ProductManage>.GetPagedDataTable(0, 20, db.ProductManage.Count(), db.ProductManage.ToList()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string[] Grid1_fields,DateTime? yearMonth, string deportment)
        {
            List<ProductManage> pmList;

            if (yearMonth != null)
                pmList = db.ProductManage.Where(p => p.PLAN_DATE.Value.Year == yearMonth.Value.Year && p.PLAN_DATE.Value.Month == yearMonth.Value.Month).ToList();
            else
                pmList = db.ProductManage.ToList();

            if(!string.IsNullOrEmpty(deportment))
            {
                pmList = pmList.Where(p => p.FAB_NAME == deportment).ToList();
            }

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = pmList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<ProductManage>.GetPagedDataTable(0, 20, recordCount, pmList);
            grid1.DataSource(dataSource, Grid1_fields);

            return UIHelper.Result();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult btnSubmit_Click(string[] Grid1_fields, JArray Grid1_modifiedData,int pageIndex, DateTime? yearMonth)
        {
            foreach (JObject mergedRow in Grid1_modifiedData)
            {
                string status = mergedRow.Value<string>("status");
                int rowIndex = mergedRow.Value<int>("index");
                JObject values = mergedRow.Value<JObject>("values");

                if (status == "modified")
                {
                    int id = mergedRow.Value<int>("id");

                    ProductManage pm = db.ProductManage.Where(p => p.ID == id).FirstOrDefault();

                    string FAB_NAME = values.Value<string>("FAB_NAME");
                    string PRODUCT_NAME = values.Value<string>("PRODUCT_NAME");
                    string PRODUCT_ID = values.Value<string>("PRODUCT_ID");
                    string MACH_TYPE = values.Value<string>("MACH_TYPE");
                    string PRODUCT_TYPE = values.Value<string>("PRODUCT_TYPE");
                    string MACH_PRO = values.Value<string>("MACH_PRO");
                    string MATERIAL_ID = values.Value<string>("MATERIAL_ID");
                    double? WEIGHT = values.Value<double?>("WEIGHT");
                    double? PROCUT_QUOTA = values.Value<double?>("PROCUT_QUOTA");
                    double? OUTPUT_VALUE = values.Value<double?>("OUTPUT_VALUE");
                    DateTime? DELIVERY_DATE = values.Value<DateTime?>("DELIVERY_DATE");
                    DateTime? PLAN_START = values.Value<DateTime?>("PLAN_START");
                    DateTime? PLAN_END = values.Value<DateTime?>("PLAN_END");
                    DateTime? PLAN_DATE = values.Value<DateTime?>("PLAN_DATE");
                    DateTime? CREATE_TIME = values.Value<DateTime?>("CREATE_TIME");
                    string CREATE_USER = values.Value<string>("CREATE_USER");
                    string VENTURENAME = values.Value<string>("VENTURENAME");
                    string OPERATIONNAME = values.Value<string>("OPERATIONNAME");

                    if (FAB_NAME != null)
                        pm.FAB_NAME = FAB_NAME;
                    if (PRODUCT_NAME != null)
                        pm.PRODUCT_NAME = PRODUCT_NAME;
                    if (PRODUCT_ID != null)
                        pm.PRODUCT_ID = PRODUCT_ID;
                    if (MACH_TYPE != null)
                        pm.MACH_TYPE = MACH_TYPE;
                    if (PRODUCT_TYPE != null)
                        pm.PRODUCT_TYPE = PRODUCT_TYPE;
                    if (MACH_PRO != null)
                        pm.MACH_PRO = MACH_PRO;
                    if (MATERIAL_ID != null)
                        pm.MATERIAL_ID = MATERIAL_ID;
                    if (WEIGHT != null)
                        pm.WEIGHT = WEIGHT;
                    if (PROCUT_QUOTA != null)
                        pm.PROCUT_QUOTA = PROCUT_QUOTA;
                    if (OUTPUT_VALUE != null)
                        pm.OUTPUT_VALUE = OUTPUT_VALUE;
                    if (DELIVERY_DATE != null)
                        pm.DELIVERY_DATE = DELIVERY_DATE;
                    if (PLAN_START != null)
                        pm.PLAN_START = PLAN_START;
                    if (PLAN_END != null)
                        pm.PLAN_END = PLAN_END;
                    if (PLAN_DATE != null)
                        pm.PLAN_DATE = PLAN_DATE;
                    if (CREATE_TIME != null)
                        pm.CREATE_TIME = CREATE_TIME;
                    if (CREATE_USER != null)
                        pm.CREATE_USER = CREATE_USER;
                    if (VENTURENAME != null)
                        pm.VENTURENAME = VENTURENAME;
                    if (OPERATIONNAME != null)
                        pm.OPERATIONNAME = OPERATIONNAME;

                    db.SaveChanges();
                }
                else if (status == "newadded")
                {
                    ProductManage pm = new ProductManage();

                    string FAB_NAME = values.Value<string>("FAB_NAME");
                    string PRODUCT_NAME = values.Value<string>("PRODUCT_NAME");
                    string PRODUCT_ID = values.Value<string>("PRODUCT_ID");
                    string MACH_TYPE = values.Value<string>("MACH_TYPE");
                    string PRODUCT_TYPE = values.Value<string>("PRODUCT_TYPE");
                    string MACH_PRO = values.Value<string>("MACH_PRO");
                    string MATERIAL_ID = values.Value<string>("MATERIAL_ID");
                    double? WEIGHT = values.Value<double?>("WEIGHT");
                    double? PROCUT_QUOTA = values.Value<double?>("PROCUT_QUOTA");
                    double? OUTPUT_VALUE = values.Value<double?>("OUTPUT_VALUE");
                    string DELIVERY_DATE = values.Value<string>("DELIVERY_DATE");
                    string PLAN_START = values.Value<string>("PLAN_START");
                    string PLAN_END = values.Value<string>("PLAN_END");
                    string PLAN_DATE = values.Value<string>("PLAN_DATE");
                    string CREATE_TIME = values.Value<string>("CREATE_TIME");
                    string CREATE_USER = values.Value<string>("CREATE_USER");
                    string VENTURENAME = values.Value<string>("VENTURENAME");
                    string OPERATIONNAME = values.Value<string>("OPERATIONNAME");

                    if (FAB_NAME != null)
                        pm.FAB_NAME = FAB_NAME;
                    if (PRODUCT_NAME != null)
                        pm.PRODUCT_NAME = PRODUCT_NAME;
                    if (PRODUCT_ID != null)
                        pm.PRODUCT_ID = PRODUCT_ID;
                    if (MACH_TYPE != null)
                        pm.MACH_TYPE = MACH_TYPE;
                    if (PRODUCT_TYPE != null)
                        pm.PRODUCT_TYPE = PRODUCT_TYPE;
                    if (MACH_PRO != null)
                        pm.MACH_PRO = MACH_PRO;
                    if (MATERIAL_ID != null)
                        pm.MATERIAL_ID = MATERIAL_ID;
                    if (WEIGHT != null)
                        pm.WEIGHT = WEIGHT;
                    if (PROCUT_QUOTA != null)
                        pm.PROCUT_QUOTA = PROCUT_QUOTA;
                    if (OUTPUT_VALUE != null)
                        pm.OUTPUT_VALUE = OUTPUT_VALUE;
                    if (!string.IsNullOrEmpty(DELIVERY_DATE))
                        pm.DELIVERY_DATE = Convert.ToDateTime(DELIVERY_DATE);
                    if (!string.IsNullOrEmpty(PLAN_START))
                        pm.PLAN_START = Convert.ToDateTime(PLAN_START);
                    if (!string.IsNullOrEmpty(PLAN_END))
                        pm.PLAN_END = Convert.ToDateTime(PLAN_END);
                    if (!string.IsNullOrEmpty(PLAN_DATE))
                        pm.PLAN_DATE = Convert.ToDateTime(PLAN_DATE);
                    if (!string.IsNullOrEmpty(CREATE_TIME))
                        pm.CREATE_TIME = Convert.ToDateTime(CREATE_TIME);
                    if (CREATE_USER != null)
                        pm.CREATE_USER = CREATE_USER;
                    if (VENTURENAME != null)
                        pm.VENTURENAME = VENTURENAME;
                    if (OPERATIONNAME != null)
                        pm.OPERATIONNAME = OPERATIONNAME;

                    db.ProductManage.Add(pm);
                    db.SaveChanges();
                }
                else if (status == "deleted")
                {
                    int id = mergedRow.Value<int>("id");

                    ProductManage pm = db.ProductManage.Where(p => p.ID == id).FirstOrDefault();

                    db.ProductManage.Remove(pm);

                    db.SaveChanges();
                }
            }

            List<ProductManage> pmList;

            if (yearMonth != null)
                pmList = db.ProductManage.Where(p => p.PLAN_DATE.Value.Year == yearMonth.Value.Year && p.PLAN_DATE.Value.Month == yearMonth.Value.Month).ToList();
            else
                pmList = db.ProductManage.ToList();

            var dataSource = PagingHelper<ProductManage>.GetPagedDataTable(pageIndex, 20, pmList.Count(), pmList);
            UIHelper.Grid("Grid1").DataSource(dataSource, Grid1_fields);
            Alert.Show("操作成功！");

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_PageIndexChanged(string[] Grid1_fields, int Grid1_pageIndex, DateTime? yearMonth,string deportment)
        {
            List<ProductManage> pmList;

            if (yearMonth != null)
                pmList = db.ProductManage.Where(p => p.PLAN_DATE.Value.Year == yearMonth.Value.Year && p.PLAN_DATE.Value.Month == yearMonth.Value.Month).ToList();
            else
                pmList = db.ProductManage.ToList();

            if (!string.IsNullOrEmpty(deportment))
            {
                pmList = pmList.Where(p => p.FAB_NAME == deportment).ToList();
            }

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = pmList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<ProductManage>.GetPagedDataTable(Grid1_pageIndex, 20, recordCount , pmList);
            grid1.DataSource(dataSource, Grid1_fields);

            return UIHelper.Result();
        }


        public ActionResult ProductReport()
        {
            List<ListItemExtension> lieList = new List<ListItemExtension>();

            List<string> deportments = new List<string>() { "精工工厂","加工工厂","模具工厂","智能设备工厂","智能机器" };//db.ProductManage.Select(p => p.FAB_NAME).Distinct().ToList();
            foreach(var fanName in deportments)
            {
                ListItem item = new ListItem();
                item.Text = fanName;
                item.Value = fanName;
                ListItemExtension lie = new ListItemExtension(item);
                lieList.Add(lie);
            }

            ViewBag.Deportment = lieList.ToArray();

            List<ProductionsReport> prList = new List<ProductionsReport>();

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            double GSPlan = 0;
            double GSComplete = 0;
            double CJGPlan = 0;
            double CJGComplete = 0;
            double JGGPlan = 0;
            double JGGComplete = 0;
            double OTD = 0;
            double OTDComplete = 0;
            double DPMO = 0;
            double DPMOComplete = 0;
            //double C__adds = 0;

            foreach(var deportment in deportments)
            {
                List<ProductManage> pmList = db.ProductManage.Where(p => p.FAB_NAME == deportment && p.PLAN_DATE.Value.Year == year && p.PLAN_DATE.Value.Month == month).ToList();             

                OtdDpmo otd = db.OtdDpmo.Where(p => p.FAB_NAME == deportment && p.Date.Value.Year == year && p.Date.Value.Month == month).FirstOrDefault();

                
                ProductionsReport pr5 = new ProductionsReport();
                pr5.Unit = "月";
                pr5.Item = "加工工时";
                pr5.Plan = (double)pmList.Where(p => p.FAB_NAME == deportment).Sum(p => p.PROCUT_QUOTA); //otd != null ? (double)otd.JGGSPlan : 0;
                pr5.Complete = otd != null ? (double)otd.JGGSComplete : 0;//Convert.ToDouble(sapreader.GetProductHourCount(GetFactoryCode(deportment), DateTime.Now).ToString("0.00"));
                pr5.FinishingRate = Convert.ToDouble((pr5.Complete / pr5.Plan * 100.0).ToString("0"));
                prList.Add(pr5);

                GSPlan += pr5.Plan;
                GSComplete += pr5.Complete;
                               

                ProductionsReport pr1 = new ProductionsReport();
                pr1.Unit = "月";
                pr1.Item = "粗加工吨位";
                pr1.Plan = Convert.ToDouble(((double)pmList.Where(p => p.FAB_NAME == deportment && p.MACH_TYPE == "粗加").Sum(p => p.WEIGHT)).ToString("0.00")); //otd != null ? (double)otd.CJGDWPlan : 0;
                pr1.Complete = otd != null ? (double)otd.CJGDWComplete : 0;//Convert.ToDouble(sapreader.GetProductWeightCount(GetFactoryCode(deportment), DateTime.Now).ToString("0.00"));
                pr1.FinishingRate = Convert.ToDouble((pr1.Complete / pr1.Plan * 100.0).ToString("0"));
                prList.Add(pr1);

                CJGPlan += pr1.Plan;
                CJGComplete += pr1.Complete;

                ProductionsReport pr2 = new ProductionsReport();
                pr2.Unit = "月";
                pr2.Item = "精加工吨位";
                pr2.Plan = Convert.ToDouble(((double)pmList.Where(p => p.FAB_NAME == deportment && p.MACH_TYPE == "精加").Sum(p => p.WEIGHT)).ToString("0.00"));//otd != null ? (double)otd.JJGDWPlan : 0;
                pr2.Complete = otd != null ? (double)otd.JJGDWComplete : 0;//Convert.ToDouble(sapreader.GetProductWeightCount(GetFactoryCode(deportment), DateTime.Now).ToString("0.00"));
                pr2.FinishingRate = Convert.ToDouble((pr2.Complete / pr2.Plan * 100.0).ToString("0"));
                prList.Add(pr2);

                JGGPlan += pr2.Plan;
                JGGComplete += pr2.Complete;

                ProductionsReport pr3 = new ProductionsReport();
                pr3.Unit = "月";
                pr3.Item = "OTD";
                pr3.Plan = otd != null ? (double)otd.OTD : 0;
                pr3.Complete = otd != null ? (double)otd.OTDComplete : 0;
                pr3.FinishingRate = Convert.ToDouble((pr3.Complete / pr3.Plan * 100.0).ToString("0"));
                prList.Add(pr3);

                OTD += pr3.Plan;
                OTDComplete = pr3.Complete;

                ProductionsReport pr4 = new ProductionsReport();
                pr4.Unit = "月";
                pr4.Item = "DPMO";
                pr4.Plan = otd != null ? (double)otd.DPMO : 0;
                pr4.Complete = otd != null ? (double)otd.DPMOCompletefloat : 0;
                pr4.FinishingRate = Convert.ToDouble(((pr4.Plan - pr4.Complete) / pr4.Plan * 100.0).ToString("0"));
                prList.Add(pr4);

                DPMO = pr4.Plan;
                DPMOComplete = pr4.Complete;

                ProductionsReport pr6 = new ProductionsReport();
                pr6.Unit = "月";
                pr6.Item = "索赔率";
                pr6.Plan = otd != null ? (double)otd.Odds : 0; ;
                pr6.Complete = otd != null ? (double)otd.OddsComplete : 0; ;
                pr6.FinishingRate = Convert.ToDouble(((pr6.Plan - pr6.Complete) / pr4.Plan * 100.0).ToString("0"));
                prList.Add(pr6);

            }

            ProductionsReport apr5 = new ProductionsReport();
            apr5.Unit = "月";
            apr5.Item = "加工工时";
            apr5.Plan = Convert.ToDouble(GSPlan.ToString("0.00")); ;
            apr5.Complete = Convert.ToDouble(GSComplete.ToString("0.00"));
            apr5.FinishingRate = Convert.ToDouble((apr5.Complete / apr5.Plan * 100.0).ToString("0"));
            prList.Add(apr5);


            ProductionsReport apr1 = new ProductionsReport();
            apr1.Unit = "月";
            apr1.Item = "粗加工吨位";
            apr1.Plan = Convert.ToDouble(CJGPlan.ToString("0.00"));
            apr1.Complete = Convert.ToDouble(CJGComplete.ToString("0.00"));
            apr1.FinishingRate = Convert.ToDouble((apr1.Complete / apr1.Plan * 100.0).ToString("0"));
            prList.Add(apr1);


            ProductionsReport apr2 = new ProductionsReport();
            apr2.Unit = "月";
            apr2.Item = "精加工吨位";
            apr2.Plan = Convert.ToDouble(JGGPlan.ToString("0.00"));
            apr2.Complete = Convert.ToDouble(JGGComplete.ToString("0.00"));
            apr2.FinishingRate = Convert.ToDouble((apr2.Complete / apr2.Plan * 100.0).ToString("0"));
            prList.Add(apr2);

            ProductionsReport apr3 = new ProductionsReport();
            apr3.Unit = "月";
            apr3.Item = "OTD";
            apr3.Plan = OTD;
            apr3.Complete = OTDComplete;
            apr3.FinishingRate = Convert.ToDouble((apr3.Complete / apr3.Plan * 100.0).ToString("0"));
            prList.Add(apr3);

            ProductionsReport apr4 = new ProductionsReport();
            apr4.Unit = "月";
            apr4.Item = "DPMO";
            apr4.Plan = DPMO;
            apr4.Complete = DPMOComplete;
            apr4.FinishingRate = Convert.ToDouble(((apr4.Plan - apr4.Complete) / apr4.Plan * 100.0).ToString("0")); 
            prList.Add(apr4);

            return View(prList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductReport(string[] Grid1_fields, DateTime? yearMonth)
        {
            List<ProductionsReport> prList = new List<ProductionsReport>();

            List<string> deportments = new List<string>() { "精工工厂", "加工工厂", "模具工厂", "智能设备工厂", "智能机器" };

            double GSPlan = 0;
            double GSComplete = 0;
            double CJGPlan = 0;
            double CJGComplete = 0;
            double JGGPlan = 0;
            double JGGComplete = 0;
            double OTD = 0;
            double OTDComplete = 0;
            double DPMO = 0;
            double DPMOComplete = 0;

            foreach (var deportment in deportments)
            {
                List<ProductManage> pmList = db.ProductManage.Where(p => p.FAB_NAME == deportment && p.PLAN_DATE.Value.Year == yearMonth.Value.Year && p.PLAN_DATE.Value.Month == yearMonth.Value.Month).ToList();

                OtdDpmo otd = db.OtdDpmo.Where(p => p.FAB_NAME == deportment && p.Date.Value.Year == yearMonth.Value.Year && p.Date.Value.Month == yearMonth.Value.Month).FirstOrDefault();

                ProductionsReport pr5 = new ProductionsReport();
                pr5.Unit = "月";
                pr5.Item = "加工工时";
                pr5.Plan = (double)pmList.Where(p => p.FAB_NAME == deportment).Sum(p => p.PROCUT_QUOTA); //otd != null ? (double)otd.JGGSPlan : 0;
                pr5.Complete = otd != null ? (double)otd.JGGSComplete : 0;//Convert.ToDouble(sapreader.GetProductHourCount(GetFactoryCode(deportment), DateTime.Now).ToString("0.00"));
                pr5.FinishingRate = Convert.ToDouble((pr5.Complete / pr5.Plan * 100.0).ToString("0"));
                prList.Add(pr5);

                GSPlan += pr5.Plan;
                GSComplete += pr5.Complete;


                ProductionsReport pr1 = new ProductionsReport();
                pr1.Unit = "月";
                pr1.Item = "粗加工吨位";
                pr1.Plan = Convert.ToDouble(((double)pmList.Where(p => p.FAB_NAME == deportment && p.MACH_TYPE == "粗加").Sum(p => p.WEIGHT)).ToString("0.00"));//otd != null ? (double)otd.CJGDWPlan : 0;
                pr1.Complete = otd != null ? (double)otd.CJGDWComplete : 0;//Convert.ToDouble(sapreader.GetProductWeightCount(GetFactoryCode(deportment), DateTime.Now).ToString("0.00"));
                pr1.FinishingRate = Convert.ToDouble((pr1.Complete / pr1.Plan * 100.0).ToString("0"));
                prList.Add(pr1);

                CJGPlan += pr1.Plan;
                CJGComplete += pr1.Complete;

                ProductionsReport pr2 = new ProductionsReport();
                pr2.Unit = "月";
                pr2.Item = "精加工吨位";
                pr2.Plan = Convert.ToDouble(((double)pmList.Where(p => p.FAB_NAME == deportment && p.MACH_TYPE == "精加").Sum(p => p.WEIGHT)).ToString("0.00")); //otd != null ? (double)otd.JJGDWPlan : 0;
                pr2.Complete = otd != null ? (double)otd.JJGDWComplete : 0;//Convert.ToDouble(sapreader.GetProductWeightCount(GetFactoryCode(deportment), DateTime.Now).ToString("0.00"));
                pr2.FinishingRate = Convert.ToDouble((pr2.Complete / pr2.Plan * 100.0).ToString("0"));
                prList.Add(pr2);

                JGGPlan += pr2.Plan;
                JGGComplete += pr2.Complete;

                ProductionsReport pr3 = new ProductionsReport();
                pr3.Unit = "月";
                pr3.Item = "OTD";
                pr3.Plan = otd != null ? (double)otd.OTD : 0;
                pr3.Complete = otd != null ? (double)otd.OTDComplete : 0;
                pr3.FinishingRate = Convert.ToDouble((pr3.Complete / pr3.Plan * 100.0).ToString("0"));
                prList.Add(pr3);

                OTD += pr3.Plan;
                OTDComplete = pr3.Complete;

                ProductionsReport pr4 = new ProductionsReport();
                pr4.Unit = "月";
                pr4.Item = "DPMO";
                pr4.Plan = otd != null ? (double)otd.DPMO : 0;
                pr4.Complete = otd != null ? (double)otd.DPMOCompletefloat : 0;
                pr4.FinishingRate = Convert.ToDouble((pr4.Complete / pr4.Plan * 100.0).ToString("0")); ;
                prList.Add(pr4);

                DPMO = pr4.Plan;
                DPMOComplete = pr4.Complete;

                ProductionsReport pr6 = new ProductionsReport();
                pr6.Unit = "月";
                pr6.Item = "所赔率";
                pr6.Plan = 0;
                pr6.Complete = 0;
                pr6.FinishingRate = otd != null ? (double)otd.Odds : 0;
                prList.Add(pr6);
            }

            ProductionsReport apr5 = new ProductionsReport();
            apr5.Unit = "月";
            apr5.Item = "加工工时";
            apr5.Plan = Convert.ToDouble(GSPlan.ToString("0.00")); ;
            apr5.Complete = Convert.ToDouble(GSComplete.ToString("0.00"));
            apr5.FinishingRate = Convert.ToDouble((apr5.Complete / apr5.Plan * 100.0).ToString("0"));
            prList.Add(apr5);


            ProductionsReport apr1 = new ProductionsReport();
            apr1.Unit = "月";
            apr1.Item = "粗加工吨位";
            apr1.Plan = Convert.ToDouble(CJGPlan.ToString("0.00"));
            apr1.Complete = Convert.ToDouble(CJGComplete.ToString("0.00"));
            apr1.FinishingRate = Convert.ToDouble((apr1.Complete / apr1.Plan * 100.0).ToString("0"));
            prList.Add(apr1);


            ProductionsReport apr2 = new ProductionsReport();
            apr2.Unit = "月";
            apr2.Item = "精加工吨位";
            apr2.Plan = Convert.ToDouble(JGGPlan.ToString("0.00"));
            apr2.Complete = Convert.ToDouble(JGGComplete.ToString("0.00"));
            apr2.FinishingRate = Convert.ToDouble((apr2.Complete / apr2.Plan * 100.0).ToString("0"));
            prList.Add(apr2);

            ProductionsReport apr3 = new ProductionsReport();
            apr3.Unit = "月";
            apr3.Item = "OTD";
            apr3.Plan = OTD;
            apr3.Complete = OTDComplete;
            apr3.FinishingRate = Convert.ToDouble((apr3.Complete / apr3.Plan * 100.0).ToString("0"));
            prList.Add(apr3);

            ProductionsReport apr4 = new ProductionsReport();
            apr4.Unit = "月";
            apr4.Item = "DPMO";
            apr4.Plan = DPMO;
            apr4.Complete = DPMOComplete;
            apr4.FinishingRate = Convert.ToDouble((apr4.Complete / apr4.Plan * 100.0).ToString("0"));
            prList.Add(apr4);

            var grid1 = UIHelper.Grid("Grid1");
            var grid2 = UIHelper.Grid("Grid2");
            var grid3 = UIHelper.Grid("Grid3");
            var grid4 = UIHelper.Grid("Grid4");
            var grid5 = UIHelper.Grid("Grid5");
            var grid6 = UIHelper.Grid("Grid6");

            grid1.DataSource(prList.GetRange(25,5), Grid1_fields);
            grid2.DataSource(prList.GetRange(0, 5), Grid1_fields);
            grid3.DataSource(prList.GetRange(5, 5), Grid1_fields);
            grid4.DataSource(prList.GetRange(10, 5), Grid1_fields);
            grid5.DataSource(prList.GetRange(15, 5), Grid1_fields);
            grid6.DataSource(prList.GetRange(20, 5), Grid1_fields);

            return UIHelper.Result();
        }

        public ActionResult ProductionHoursReport()
        {
            List<ProductionHoursReport> prList = new List<ProductionHoursReport>();

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            List<ProductManage> pmList = db.ProductManage.Where(p => p.PLAN_DATE.Value.Year == year && p.PLAN_DATE.Value.Month == month).ToList();

            List<OtdDpmo> otdList = db.OtdDpmo.Where(p =>  p.Date.Value.Year == year && p.Date.Value.Month == month).ToList();

            OtdDpmo otd2 = otdList.Where(p => p.FAB_NAME == "精工工厂").FirstOrDefault();

            ProductionHoursReport pr2 = new ProductionHoursReport();
            pr2.Month = month;
            pr2.Deportment = "精工工厂";
            pr2.YearlyPlan = 0;
            if (db.YearPlan.Any() && db.YearPlan.Where(p => p.Year == year && p.Factory == "精工工厂").FirstOrDefault() != null)
                pr2.YearlyPlan = db.YearPlan.Where(p => p.Year == year && p.Factory == "精工工厂").FirstOrDefault().YearPlan1;
            pr2.Complete = otd2 != null ? (double)otd2.HourYearComplete : 0;//(double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.FAB_NAME == "精工工厂").Sum(p => p.PROCUT_QUOTA); ;
            pr2.FinishingRate = Convert.ToDouble((pr2.Complete / pr2.YearlyPlan * 100.0).ToString("0.00"));
            pr2.MonthHourPlan = (double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.PLAN_DATE.Value.Month == month && p.FAB_NAME == "精工工厂").Sum(p => p.PROCUT_QUOTA); //otd2 != null ? (double)otd2.HourMonthPlan : 0;
            pr2.MonthHourComplete = otd2 != null ? (double)otd2.HourMonthComplete : 0;//Convert.ToDouble(sapreader.GetProductHourCount(GetFactoryCode("精工工厂"), DateTime.Now).ToString("0.00"));
            pr2.MonthFinishingRate = Convert.ToDouble((pr2.MonthHourComplete / pr2.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr2);

            OtdDpmo otd3 = otdList.Where(p => p.FAB_NAME == "加工工厂").FirstOrDefault();

            ProductionHoursReport pr3 = new ProductionHoursReport();
            pr3.Month = month;
            pr3.Deportment = "加工工厂";
            pr3.YearlyPlan = 0;
            if (db.YearPlan.Any() && db.YearPlan.Where(p => p.Year == year && p.Factory == "加工工厂").FirstOrDefault() != null)
                pr3.YearlyPlan = db.YearPlan.Where(p => p.Year == year && p.Factory == "加工工厂").FirstOrDefault().YearPlan1;
            pr3.Complete = otd3 != null ? (double)otd3.HourYearComplete : 0;//(double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.FAB_NAME == "加工工厂").Sum(p => p.PROCUT_QUOTA);
            pr3.FinishingRate = Convert.ToDouble((pr3.Complete / pr3.YearlyPlan * 100.0).ToString("0.00"));
            pr3.MonthHourPlan = (double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.PLAN_DATE.Value.Month == month && p.FAB_NAME == "加工工厂").Sum(p => p.PROCUT_QUOTA); //otd3 != null ? (double)otd3.HourMonthPlan : 0;
            pr3.MonthHourComplete = otd3 != null ? (double)otd3.HourMonthComplete : 0;//Convert.ToDouble(sapreader.GetProductHourCount(GetFactoryCode("加工工厂"), DateTime.Now).ToString("0.00"));
            pr3.MonthFinishingRate = Convert.ToDouble((pr3.MonthHourComplete / pr3.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr3);

            OtdDpmo otd4 = otdList.Where(p => p.FAB_NAME == "模具工厂").FirstOrDefault();

            ProductionHoursReport pr4 = new ProductionHoursReport();
            pr4.Month = month;
            pr4.Deportment = "模具工厂";
            pr4.YearlyPlan = 0;
            if (db.YearPlan.Any() && db.YearPlan.Where(p => p.Year == year && p.Factory == "模具工厂").FirstOrDefault() != null)
                pr4.YearlyPlan = db.YearPlan.Where(p => p.Year == year && p.Factory == "模具工厂").FirstOrDefault().YearPlan1;
            pr4.Complete = otd4 != null ? (double)otd4.HourYearComplete : 0;//(double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.FAB_NAME == "模具工厂").Sum(p => p.PROCUT_QUOTA); 
            pr4.FinishingRate = Convert.ToDouble((pr4.Complete / pr4.YearlyPlan * 100.0).ToString("0.00"));
            pr4.MonthHourPlan = (double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.PLAN_DATE.Value.Month == month && p.FAB_NAME == "模具工厂").Sum(p => p.PROCUT_QUOTA); //otd4 != null ? (double)otd4.HourMonthPlan : 0;
            pr4.MonthHourComplete = otd4 != null ? (double)otd4.HourMonthComplete : 0;//Convert.ToDouble(sapreader.GetProductHourCount(GetFactoryCode("模具工厂"), DateTime.Now).ToString("0.00"));
            pr4.MonthFinishingRate = Convert.ToDouble((pr4.MonthHourComplete / pr4.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr4);

            OtdDpmo otd5 = otdList.Where(p => p.FAB_NAME == "智能设备工厂").FirstOrDefault();

            ProductionHoursReport pr5 = new ProductionHoursReport();
            pr5.Month = month;
            pr5.Deportment = "智能设备工厂";
            pr5.YearlyPlan = 0;
            if (db.YearPlan.Any() && db.YearPlan.Where(p => p.Year == year && p.Factory == "智能设备工厂").FirstOrDefault() != null)
                pr5.YearlyPlan = db.YearPlan.Where(p => p.Year == year && p.Factory == "智能设备工厂").FirstOrDefault().YearPlan1;
            pr5.Complete = otd5 != null ? (double)otd5.HourYearComplete : 0;//(double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.FAB_NAME == "智能设备工厂").Sum(p => p.PROCUT_QUOTA);
            pr5.FinishingRate = Convert.ToDouble((pr5.Complete / pr5.YearlyPlan * 100.0).ToString("0.00"));
            pr5.MonthHourPlan = (double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.PLAN_DATE.Value.Month == month && p.FAB_NAME == "智能设备工厂").Sum(p => p.PROCUT_QUOTA); //otd5 != null ? (double)otd5.HourMonthPlan : 0;
            pr5.MonthHourComplete = otd5 != null ? (double)otd5.HourMonthComplete : 0;//Convert.ToDouble(sapreader.GetProductHourCount(GetFactoryCode("智能设备工厂"), DateTime.Now).ToString("0.00"));
            pr5.MonthFinishingRate = Convert.ToDouble((pr5.MonthHourComplete / pr5.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr5);

            OtdDpmo otd6 = otdList.Where(p => p.FAB_NAME == "智能机器").FirstOrDefault();

            ProductionHoursReport pr6 = new ProductionHoursReport();
            pr6.Month = month;
            pr6.Deportment = "智能机器";
            pr6.YearlyPlan = 0;
            if (db.YearPlan.Any() && db.YearPlan.Where(p => p.Year == year && p.Factory == "智能机器").FirstOrDefault() != null)
                pr6.YearlyPlan = db.YearPlan.Where(p => p.Year == year && p.Factory == "智能机器").FirstOrDefault().YearPlan1;
            pr6.Complete = otd6 != null ? (double)otd6.HourYearComplete : 0;//(double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.FAB_NAME == "智能机器").Sum(p => p.PROCUT_QUOTA);
            pr6.FinishingRate = Convert.ToDouble((pr6.Complete / pr6.YearlyPlan * 100.0).ToString("0.00"));
            pr6.MonthHourPlan = (double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.PLAN_DATE.Value.Month == month && p.FAB_NAME == "智能机器").Sum(p => p.PROCUT_QUOTA); //otd6 != null ? (double)otd6.HourMonthPlan : 0;
            pr6.MonthHourComplete = otd6 != null ? (double)otd6.HourMonthComplete : 0;//Convert.ToDouble(sapreader.GetProductHourCount(GetFactoryCode("智能机器"), DateTime.Now).ToString("0.00"));
            pr6.MonthFinishingRate = Convert.ToDouble((pr6.MonthHourComplete / pr6.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr6);

            ProductionHoursReport pr1 = new ProductionHoursReport();
            pr1.Month = month;
            pr1.Deportment = "制造事业部";
            pr1.YearlyPlan = pr2.YearlyPlan + pr3.YearlyPlan + pr4.YearlyPlan + pr5.YearlyPlan + pr6.YearlyPlan; ;
            pr1.Complete = pr2.Complete + pr3.Complete + pr4.Complete + pr5.Complete + pr6.Complete;
            pr1.FinishingRate = Convert.ToDouble((pr1.Complete / pr1.YearlyPlan * 100.0).ToString("0.00"));
            pr1.MonthHourPlan = pr2.MonthHourPlan + pr3.MonthHourPlan + pr4.MonthHourPlan + pr5.MonthHourPlan + pr6.MonthHourPlan;
            pr1.MonthHourComplete = pr2.MonthHourComplete + pr3.MonthHourComplete + pr4.MonthHourComplete + pr5.MonthHourComplete + pr6.MonthHourComplete; ;
            pr1.MonthFinishingRate = Convert.ToDouble((pr1.MonthHourComplete / pr1.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr1);

            return View(prList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductionHoursReport(string[] Grid1_fields, DateTime? yearMonth)
        {
            List<ProductionHoursReport> prList = new List<ProductionHoursReport>();

            int year = yearMonth.Value.Year;
            int month = yearMonth.Value.Month;

            List<ProductManage> pmList = db.ProductManage.Where(p => p.PLAN_DATE.Value.Year == yearMonth.Value.Year && p.PLAN_DATE.Value.Month == yearMonth.Value.Month ).ToList();

            List<OtdDpmo> otdList = db.OtdDpmo.Where(p => p.Date.Value.Year == year && p.Date.Value.Month == month).ToList();

            OtdDpmo otd2 = otdList.Where(p => p.FAB_NAME == "精工工厂").FirstOrDefault();

            ProductionHoursReport pr2 = new ProductionHoursReport();
            pr2.Month = month;
            pr2.Deportment = "精工工厂";
            pr2.YearlyPlan = 0;
            if (db.YearPlan.Any() && db.YearPlan.Where(p => p.Year == year && p.Factory == "精工工厂").FirstOrDefault() != null)
                pr2.YearlyPlan = db.YearPlan.Where(p => p.Year == year && p.Factory == "精工工厂").FirstOrDefault().YearPlan1;
            pr2.Complete = otd2 != null ? (double)otd2.HourYearComplete : 0;//(double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.FAB_NAME == "精工工厂").Sum(p => p.PROCUT_QUOTA); ;
            pr2.FinishingRate = Convert.ToDouble((pr2.Complete / pr2.YearlyPlan * 100.0).ToString("0.00"));
            pr2.MonthHourPlan = (double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.PLAN_DATE.Value.Month == month && p.FAB_NAME == "精工工厂").Sum(p => p.PROCUT_QUOTA); ;
            pr2.MonthHourComplete = otd2 != null ? (double)otd2.HourMonthComplete : 0;//Convert.ToDouble(sapreader.GetProductHourCount(GetFactoryCode("精工工厂"), DateTime.Now).ToString("0.00"));
            pr2.MonthFinishingRate = Convert.ToDouble((pr2.MonthHourComplete / pr2.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr2);

            OtdDpmo otd3 = otdList.Where(p => p.FAB_NAME == "加工工厂").FirstOrDefault();

            ProductionHoursReport pr3 = new ProductionHoursReport();
            pr3.Month = month;
            pr3.Deportment = "加工工厂";
            pr3.YearlyPlan = 0;
            if (db.YearPlan.Any() && db.YearPlan.Where(p => p.Year == year && p.Factory == "加工工厂").FirstOrDefault() != null)
                pr3.YearlyPlan = db.YearPlan.Where(p => p.Year == year && p.Factory == "加工工厂").FirstOrDefault().YearPlan1;
            pr3.Complete = otd3 != null ? (double)otd3.HourYearComplete : 0;//(double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.FAB_NAME == "加工工厂").Sum(p => p.PROCUT_QUOTA);
            pr3.FinishingRate = Convert.ToDouble((pr3.Complete / pr3.YearlyPlan * 100.0).ToString("0.00"));
            pr3.MonthHourPlan = (double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.PLAN_DATE.Value.Month == month && p.FAB_NAME == "加工工厂").Sum(p => p.PROCUT_QUOTA); ;
            pr3.MonthHourComplete = otd3 != null ? (double)otd3.HourMonthComplete : 0;//Convert.ToDouble(sapreader.GetProductHourCount(GetFactoryCode("加工工厂"), DateTime.Now).ToString("0.00"));
            pr3.MonthFinishingRate = Convert.ToDouble((pr3.MonthHourComplete / pr3.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr3);

            OtdDpmo otd4 = otdList.Where(p => p.FAB_NAME == "模具工厂").FirstOrDefault();

            ProductionHoursReport pr4 = new ProductionHoursReport();
            pr4.Month = month;
            pr4.Deportment = "模具工厂";
            pr4.YearlyPlan = 0;
            if (db.YearPlan.Any() && db.YearPlan.Where(p => p.Year == year && p.Factory == "模具工厂").FirstOrDefault() != null)
                pr4.YearlyPlan = db.YearPlan.Where(p => p.Year == year && p.Factory == "模具工厂").FirstOrDefault().YearPlan1;
            pr4.Complete = otd4 != null ? (double)otd4.HourYearComplete : 0;//(double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.FAB_NAME == "模具工厂").Sum(p => p.PROCUT_QUOTA); 
            pr4.FinishingRate = Convert.ToDouble((pr4.Complete / pr4.YearlyPlan * 100.0).ToString("0.00"));
            pr4.MonthHourPlan = (double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.PLAN_DATE.Value.Month == month && p.FAB_NAME == "模具工厂").Sum(p => p.PROCUT_QUOTA); ;
            pr4.MonthHourComplete = otd4 != null ? (double)otd4.HourMonthComplete : 0;//Convert.ToDouble(sapreader.GetProductHourCount(GetFactoryCode("模具工厂"), DateTime.Now).ToString("0.00"));
            pr4.MonthFinishingRate = Convert.ToDouble((pr4.MonthHourComplete / pr4.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr4);

            OtdDpmo otd5 = otdList.Where(p => p.FAB_NAME == "智能设备工厂").FirstOrDefault();

            ProductionHoursReport pr5 = new ProductionHoursReport();
            pr5.Month = month;
            pr5.Deportment = "智能设备工厂";
            pr5.YearlyPlan = 0;
            if (db.YearPlan.Any() && db.YearPlan.Where(p => p.Year == year && p.Factory == "智能设备工厂").FirstOrDefault() != null)
                pr5.YearlyPlan = db.YearPlan.Where(p => p.Year == year && p.Factory == "智能设备工厂").FirstOrDefault().YearPlan1;
            pr5.Complete = otd5 != null ? (double)otd5.HourYearComplete : 0;//(double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.FAB_NAME == "智能设备工厂").Sum(p => p.PROCUT_QUOTA);
            pr5.FinishingRate = Convert.ToDouble((pr5.Complete / pr5.YearlyPlan * 100.0).ToString("0.00"));
            pr5.MonthHourPlan = (double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.PLAN_DATE.Value.Month == month && p.FAB_NAME == "智能设备工厂").Sum(p => p.PROCUT_QUOTA); ;
            pr5.MonthHourComplete = otd5 != null ? (double)otd5.HourMonthComplete : 0;//Convert.ToDouble(sapreader.GetProductHourCount(GetFactoryCode("智能设备工厂"), DateTime.Now).ToString("0.00"));
            pr5.MonthFinishingRate = Convert.ToDouble((pr5.MonthHourComplete / pr5.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr5);

            OtdDpmo otd6 = otdList.Where(p => p.FAB_NAME == "智能机器").FirstOrDefault();

            ProductionHoursReport pr6 = new ProductionHoursReport();
            pr6.Month = month;
            pr6.Deportment = "智能机器";
            pr6.YearlyPlan = 0;
            if (db.YearPlan.Any() && db.YearPlan.Where(p => p.Year == year && p.Factory == "智能机器").FirstOrDefault() != null)
                pr6.YearlyPlan = db.YearPlan.Where(p => p.Year == year && p.Factory == "智能机器").FirstOrDefault().YearPlan1;
            pr6.Complete = otd6 != null ? (double)otd6.HourYearComplete : 0;//(double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.FAB_NAME == "智能机器").Sum(p => p.PROCUT_QUOTA);
            pr6.FinishingRate = Convert.ToDouble((pr6.Complete / pr6.YearlyPlan * 100.0).ToString("0.00"));
            pr6.MonthHourPlan = (double)pmList.Where(p => p.PLAN_DATE.Value.Year == year && p.PLAN_DATE.Value.Month == month && p.FAB_NAME == "智能机器").Sum(p => p.PROCUT_QUOTA); ;
            pr6.MonthHourComplete = otd6 != null ? (double)otd6.HourMonthComplete : 0;//Convert.ToDouble(sapreader.GetProductHourCount(GetFactoryCode("智能机器"), DateTime.Now).ToString("0.00"));
            pr6.MonthFinishingRate = Convert.ToDouble((pr6.MonthHourComplete / pr6.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr6);

            ProductionHoursReport pr1 = new ProductionHoursReport();
            pr1.Month = month;
            pr1.Deportment = "制造事业部";
            pr1.YearlyPlan = pr2.YearlyPlan + pr3.YearlyPlan + pr4.YearlyPlan + pr5.YearlyPlan + pr6.YearlyPlan; ;
            pr1.Complete = pr2.Complete + pr3.Complete + pr4.Complete + pr5.Complete + pr6.Complete;
            pr1.FinishingRate = Convert.ToDouble((pr1.Complete / pr1.YearlyPlan * 100.0).ToString("0.00"));
            pr1.MonthHourPlan = pr2.MonthHourPlan + pr3.MonthHourPlan + pr4.MonthHourPlan + pr5.MonthHourPlan + pr6.MonthHourPlan;
            pr1.MonthHourComplete = pr2.MonthHourComplete + pr3.MonthHourComplete + pr4.MonthHourComplete + pr5.MonthHourComplete + pr6.MonthHourComplete; ;
            pr1.MonthFinishingRate = Convert.ToDouble((pr1.MonthHourComplete / pr1.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr1);

            var grid1 = UIHelper.Grid("Grid1");

            var dataSource = prList;
            grid1.DataSource(dataSource, Grid1_fields);

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult fileExcel_FileSelected(HttpPostedFileBase fileExcel, FormCollection values)
        {
            if (fileExcel != null)
            {
                string fileName = fileExcel.FileName;

                if (!(fileName.Contains(".XLSX") || fileName.Contains(".xlsx")))
                {
                    // 清空文件上传组件
                    UIHelper.FileUpload("fileUpload").Reset();

                    ShowNotify("无效的文件类型！");
                }
                else
                {
                    List<List<object>> beanList = ReadExcel(fileExcel.InputStream);

                    List<ProductManage> pList = ConvertToProductManage(beanList);

                    db.ProductManage.AddRange(pList);

                    db.SaveChanges();

                    UIHelper.FileUpload("fileUpload").Reset();
                }
            }

            UIHelper.FileUpload("fileUpload").Reset();

            Alert.Show("导入Excel完成！请刷新页面！\n如未导入数据，请检查Excel对应列单位是否正确");

            return UIHelper.Result();
        }

        string GetFactoryCode(string FactoryName)
        {
            switch(FactoryName)
            {
                case "制造事业部":
                    return "";
                case "精工工厂":
                    return "1300";
                case "加工工厂":
                    return "1410";
                case "模具工厂":
                    return "1420";
                case "智能设备工厂":
                    return "1430";
                case "智能机器":
                    return "2000";
                default:
                    return "";
            }
        }

        public List<List<object>> ReadExcel(Stream fileStream)
        {
            List<List<object>> beanList = new List<List<object>>();

            using (ExcelPackage package = new ExcelPackage(fileStream))
            {
                for (int i = 1; i <= package.Workbook.Worksheets.Count; ++i)
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets[i];
                    for (int m = sheet.Dimension.Start.Row, n = sheet.Dimension.End.Row; m <= n; m++)
                    {                     
                        List<object> objectList = new List<object>();
                        for (int j = sheet.Dimension.Start.Column, k = sheet.Dimension.End.Column; j <= k; j++)
                        {
                            object o = sheet.Cells[m, j].Value;
                            objectList.Add(o);
                        }
                        beanList.Add(objectList);
                    }
                }
            }
            beanList.RemoveAt(0);

            return beanList;
        }

        public List<ProductManage> ConvertToProductManage(List<List<object>> beanList)
        {
            List<ProductManage> pList = new List<ProductManage>();

            foreach(var bean in beanList)
            {
                try
                {
              
                    ProductManage p = new ProductManage();

                    if (bean[0] != null)
                        p.FAB_NAME = (string)bean[0];                 
                    else
                        continue;

                    if (bean[1] != null)
                        p.MACH_TYPE = (string)bean[1];
                    else
                        continue;

                    if (bean[2] != null)
                        p.PRODUCT_TYPE = (string)bean[2];
                    else
                        continue;

                    if (bean[3] != null)
                        p.MACH_PRO = (string)bean[3];
                    else
                        continue;

                    if (bean[4] != null)
                        p.PRODUCT_NAME = (string)bean[4];
                    else
                        continue;

                    try
                    {
                        if (bean[5] != null)
                            p.PRODUCT_ID = (string)bean[5];
                        else
                            continue;
                    }
                    catch
                    {
                        if (bean[5] != null)
                            p.PRODUCT_ID = ((double)bean[5]).ToString();
                        else
                            continue;
                    }

                    if (bean[6] != null)
                        p.WEIGHT = (double)bean[6];
                    else
                        continue;

                    if (bean[7] != null)
                        p.PROCUT_QUOTA = (double)bean[7];
                    else
                        continue;

                    if (bean[8] != null)
                        p.OUTPUT_VALUE = (double)bean[8];
                    else
                        continue;

                    if (bean[9] != null)
                        p.MATERIAL_ID = (string)bean[9];
                    else
                        continue;

                    try
                    {
                        if (bean[10] != null)
                        {
                            string strDate = DateTime.FromOADate(Convert.ToInt32(bean[10])).ToString("d");

                            p.DELIVERY_DATE = Convert.ToDateTime(strDate);
                        }
                        //p.DELIVERY_DATE = Convert.ToDateTime(bean[11]);
                        else
                            continue;
                    }
                    catch
                    {
                        if (bean[10] != null)
                            p.DELIVERY_DATE = (DateTime)bean[10];
                        else
                            continue;
                    }

                    try
                    {
                        

                        if (bean[11] != null)
                        {
                            string strDate = DateTime.FromOADate(Convert.ToInt32(bean[11])).ToString("d");

                            p.DELIVERY_DATE = Convert.ToDateTime(strDate);
                        }
                        //p.PLAN_START = Convert.ToDateTime(bean[12]);
                        else
                            continue;
                    }
                    catch
                    {
                        if (bean[11] != null)
                            p.PLAN_START = (DateTime)bean[11];

                        else
                            continue;
                    }


                    try
                    {
                        

                        if (bean[12] != null)
                        {
                            string strDate = DateTime.FromOADate(Convert.ToInt32(bean[12])).ToString("d");

                            p.DELIVERY_DATE = Convert.ToDateTime(strDate);
                        }
                        //p.PLAN_END = Convert.ToDateTime(bean[13]);
                        else
                            continue;
                    }
                    catch
                    {
                        if (bean[12] != null)
                            p.PLAN_END = (DateTime)bean[12];
                        else
                            continue;
                    }


                        try
                    {
                        
                        if (bean[13] != null)
                        {
                            //string strDate = DateTime.FromOADate(Convert.ToInt32(bean[14])).ToString("d");

                            //p.DELIVERY_DATE = Convert.ToDateTime(strDate);
                            string[] ss = ((string)bean[13]).Split(new char[] { '-' });
                            p.PLAN_DATE = new DateTime(int.Parse(ss[0]), int.Parse(ss[1]), 1);
                        }
                        //p.PLAN_DATE = Convert.ToDateTime((string)bean[14]);
                        else
                            continue;
                    }
                    catch
                    {
                        if (bean[13] != null)
                            p.PLAN_DATE = (DateTime)bean[13];
                        else
                            continue;

                    }

                    if (bean[14] != null)
                        p.VENTURENAME = (string)bean[14];
                    else
                        p.VENTURENAME = "";

                    if (bean[15] != null)
                        p.OPERATIONNAME = (string)bean[15];
                    else
                        p.OPERATIONNAME = "";


                    pList.Add(p);
                }
                catch(Exception e)
                {
                    e.ToString();
                    continue;
                }
            }

            return pList;
        }


        public ActionResult Capa()
        {
            List<ListItemExtension> lieList = new List<ListItemExtension>();
            List<string> deportments = new List<string>() { "精工工厂", "加工工厂", "模具工厂", "智能设备工厂", "智能机器" };//db.ProductManage.Select(p => p.FAB_NAME).Distinct().ToList();
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
            int week = GetWeekOfYear(DateTime.Now);
            DateTime startDay = new DateTime();
            DateTime endDay = new DateTime();

            CalcWeekDay(year, week, out startDay, out endDay);

            List<ProductManage> pmList;

            pmList = db.ProductManage.Where(p => p.PLAN_END.Value.Year == DateTime.Now.Year && p.PLAN_END.Value.Month == DateTime.Now.Month && p.PLAN_END.Value.Day >= startDay.Day && p.PLAN_END.Value.Day <= endDay.Day).ToList();

            List<Capa> cpList = GetCapa(pmList, "精工工厂");

            ViewBag.RecordCount = cpList.Count();
            return View(PagingHelper<Capa>.GetPagedDataTable(0, 20, cpList.Count(), cpList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Capa(string[] Grid1_fields, DateTime? yearMonth, string deportment,string unit)
        {
            List<ProductManage> pmList;

            

            if (unit == "月")
            {
                pmList = db.ProductManage.Where(p => p.PLAN_END.Value.Year == yearMonth.Value.Year && p.PLAN_END.Value.Month == yearMonth.Value.Month).ToList();
            }
            else//月
            {
                int year = yearMonth.Value.Year;
                int week = GetWeekOfYear((DateTime)yearMonth);
                DateTime startDay = new DateTime();
                DateTime endDay = new DateTime();

                CalcWeekDay(year, week, out startDay, out endDay);

                pmList = db.ProductManage.Where(p => p.PLAN_END.Value.Year == yearMonth.Value.Year && p.PLAN_END.Value.Month == yearMonth.Value.Month && p.PLAN_END.Value.Day >= startDay.Day && p.PLAN_END.Value.Day <= endDay.Day).ToList();
            }

            pmList = pmList.Where(p => p.FAB_NAME == deportment).ToList();

            List<Capa> cpList = GetCapa(pmList, deportment);

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = cpList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<Capa>.GetPagedDataTable(0, 20, recordCount, cpList);
            grid1.DataSource(dataSource, Grid1_fields);

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Capa_PageIndexChanged(string[] Grid1_fields, int Grid1_pageIndex, DateTime? yearMonth, string deportment, string unit)
        {
            List<ProductManage> pmList;



            if (unit == "月")
            {
                pmList = db.ProductManage.Where(p => p.PLAN_END.Value.Year == yearMonth.Value.Year && p.PLAN_END.Value.Month == yearMonth.Value.Month).ToList();
            }
            else//月
            {
                int year = yearMonth.Value.Year;
                int week = GetWeekOfYear((DateTime)yearMonth);
                DateTime startDay = new DateTime();
                DateTime endDay = new DateTime();

                CalcWeekDay(year, week, out startDay, out endDay);

                pmList = db.ProductManage.Where(p => p.PLAN_END.Value.Year == yearMonth.Value.Year && p.PLAN_END.Value.Month == yearMonth.Value.Month && p.PLAN_END.Value.Day >= startDay.Day && p.PLAN_END.Value.Day <= endDay.Day).ToList();
            }

            pmList = pmList.Where(p => p.FAB_NAME == deportment).ToList();

            List<Capa> cpList = GetCapa(pmList, deportment);

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = cpList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<Capa>.GetPagedDataTable(Grid1_pageIndex, 20, recordCount, cpList);
            grid1.DataSource(dataSource, Grid1_fields);

            return UIHelper.Result();
        }

        /// <summary>
        /// 获取指定日期，在为一年中为第几周
        /// </summary>
        /// <param name="dt">指定时间</param>
        /// <reutrn>返回第几周</reutrn>
        private static int GetWeekOfYear(DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return weekOfYear;
        }

        public static bool CalcWeekDay(int year, int week, out DateTime first, out DateTime last)
        {
            first = DateTime.MinValue;
            last = DateTime.MinValue;
            //年份超限
            if (year < 1700 || year > 9999) return false;
            //周数错误
            if (week < 1 || week > 53) return false;
            //指定年范围
            DateTime start = new DateTime(year, 1, 1);
            DateTime end = new DateTime(year, 12, 31);
            int startWeekDay = (int)start.DayOfWeek;

            if (week == 1)
            {
                first = start;
                last = start.AddDays(6 - startWeekDay);
            }
            else
            {
                //周的起始日期
                first = start.AddDays((7 - startWeekDay) + (week - 2) * 7);
                last = first.AddDays(6);
                if (last > end)
                {
                    last = end;
                }
            }
            return (first <= end);
        }

        public List<Capa> GetCapa(List<ProductManage> pmList,string department)
        {
            List<Capa> cpList = new List<Capa>();

            //var ProcessList = pmList.Where(p => !string.IsNullOrEmpty(p.OPERATIONNAME) && !string.IsNullOrEmpty(p.VENTURENAME)).Select( p => new ss(p.OPERATIONNAME, p.VENTURENAME));
            var ProcessList = db.VentureCapa.Where(p => p.FAB_NAME == department).ToList();//.Select(p => new ss(p.OPERATIONNAME, p.VENTURENAME));

            //foreach(ss s in ProcessList)
            //{
            //    if (!ssList.Where(p => p.OPERATIONNAME == s.OPERATIONNAME && p.VENTURENAME == s.VENTURENAME).Any())
            //        ssList.Add(s);
            //}

            foreach (var process in ProcessList)
            {
                Capa cp = new Capa();
                cp.Unit = "周";
                cp.Process = process.OPERATIONNAME;
                cp.Venture = process.VENTURENAME;
                //VentureCapa vcp = db.VentureCapa.Where(p => p.VENTURENAME == process.VENTURENAME && p.OPERATIONNAME == process.OPERATIONNAME).FirstOrDefault();
                //if(vcp != null)
                    cp.Plan = (double)pmList.Where(p => p.VENTURENAME == process.VENTURENAME && p.OPERATIONNAME == process.OPERATIONNAME).Sum(p => p.PROCUT_QUOTA);
                cp.Real = (double)process.PROCUT_QUOTA_WEEK; 

                cpList.Add(cp);
            }

            return cpList;
        }

        class ss
        {
            public ss(string s1,string s2)
            {
                this.OPERATIONNAME = s1;
                this.VENTURENAME = s2;
            }
            public string OPERATIONNAME { get; set; }
            public string VENTURENAME { get; set; }
        }
    }
}


