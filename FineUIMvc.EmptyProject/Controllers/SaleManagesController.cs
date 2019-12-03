using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FineUIMvc.EmptyProject.Models;
using Newtonsoft.Json.Linq;
using FineUIMvc.EmptyProject.Models.Model;
using System.IO;
using OfficeOpenXml;
using SAPLib;

namespace FineUIMvc.EmptyProject.Controllers
{
    public class SaleManagesController : BaseController
    {
        private MoJuDataEntities db = new MoJuDataEntities();
        private SAPReader sapreader = new SAPReader();

        // GET: ProductManages
        public ActionResult Index()
        {
            ViewBag.RecordCount = db.SaleManage.Count();
            return View(PagingHelper<SaleManage>.GetPagedDataTable(0, 20, db.SaleManage.Count(), db.SaleManage.ToList()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string[] Grid1_fields, DateTime? yearMonth)
        {
            List<SaleManage> smList;

            if (yearMonth != null)
                smList = db.SaleManage.Where(p => p.PLAN_DATE.Year == yearMonth.Value.Year && p.PLAN_DATE.Month == yearMonth.Value.Month).ToList();
            else
                smList = db.SaleManage.ToList();

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = smList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<SaleManage>.GetPagedDataTable(0, 20, recordCount, smList);
            grid1.DataSource(dataSource, Grid1_fields);

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Compute(string[] Grid1_fields, DateTime? yearMonth)
        {
            List<SaleManage> smList;
            List<ProductManage> pmList;

            if (yearMonth != null)
            {
                smList = db.SaleManage.Where(p => p.PLAN_DATE.Year == yearMonth.Value.Year && p.PLAN_DATE.Month == yearMonth.Value.Month).ToList();
                pmList = db.ProductManage.Where(p => p.PLAN_DATE.Value.Year == yearMonth.Value.Year && p.PLAN_DATE.Value.Month == yearMonth.Value.Month).ToList();

                foreach(var sm in smList)
                {
                    sm.SALE_PLAN = pmList.Where(p => p.FAB_NAME == sm.RSPO_DEPT).Sum(p => p.OUTPUT_VALUE);
                    sm.SALE_FINISH = Convert.ToDouble(sapreader.GetProductWeightCount(GetFactoryCode(sm.RSPO_DEPT), DateTime.Now).ToString("0.00"));
                }

                db.SaveChanges();
            }    
            else
            {
                smList = db.SaleManage.ToList();
            }
                

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = smList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<SaleManage>.GetPagedDataTable(0, 20, recordCount, smList);
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

                    SaleManage sm = db.SaleManage.Where(p => p.ID == id).FirstOrDefault();

                    string RSPO_DEPT = values.Value<string>("RSPO_DEPT");
                    double? MANAGE_PLAN = values.Value<double?>("MANAGE_PLAN");
                    double? SALE_PLAN = values.Value<double?>("SALE_PLAN");
                    double? SALE_FINISH = values.Value<double?>("SALE_FINISH");
                    DateTime? PLAN_DATE = values.Value<DateTime?>("PLAN_DATE");
                    string REMARK = values.Value<string>("REMARK");
                    string Company = values.Value<string>("Company");
                    int? SellYear = values.Value<int>("SellYear");

                    if (RSPO_DEPT != null)
                        sm.RSPO_DEPT = RSPO_DEPT;
                    if (MANAGE_PLAN != null)
                        sm.MANAGE_PLAN = (double)MANAGE_PLAN;
                    if (SALE_PLAN != null)
                        sm.SALE_PLAN = SALE_PLAN;
                    if (SALE_FINISH != null)
                        sm.SALE_FINISH = SALE_FINISH;
                    if (PLAN_DATE != null)
                        sm.PLAN_DATE = (DateTime)PLAN_DATE;
                    if (REMARK != null)
                        sm.REMARK = REMARK;
                    if (Company != null)
                        sm.Company = Company;
                    if (SellYear != null)
                        sm.SellYear = (int)SellYear;

                    db.SaveChanges();
                }
                else if (status == "newadded")
                {
                    SaleManage sm = new SaleManage();

                    string RSPO_DEPT = values.Value<string>("RSPO_DEPT");
                    double? MANAGE_PLAN = values.Value<double?>("MANAGE_PLAN");
                    double? SALE_PLAN = values.Value<double?>("SALE_PLAN");
                    double? SALE_FINISH = values.Value<double?>("SALE_FINISH");
                    string PLAN_DATE = values.Value<string>("PLAN_DATE");
                    string REMARK = values.Value<string>("REMARK");
                    string Company = values.Value<string>("Company");
                    int? SellYear = values.Value<int>("SellYear");

                    if (RSPO_DEPT != null)
                        sm.RSPO_DEPT = RSPO_DEPT;
                    if (MANAGE_PLAN != null)
                        sm.MANAGE_PLAN = (double)MANAGE_PLAN;
                    if (SALE_PLAN != null)
                        sm.SALE_PLAN = SALE_PLAN;
                    if (SALE_FINISH != null)
                        sm.SALE_FINISH = SALE_FINISH;
                    if (PLAN_DATE != null)
                        sm.PLAN_DATE = Convert.ToDateTime(PLAN_DATE);
                    if (REMARK != null)
                        sm.REMARK = REMARK;
                    if (Company != null)
                        sm.Company = Company;
                    if (SellYear != null)
                        sm.SellYear = (int)SellYear;


                    db.SaleManage.Add(sm);
                    db.SaveChanges();
                }
                else if (status == "deleted")
                {
                    int id = mergedRow.Value<int>("id");

                    SaleManage sm = db.SaleManage.Where(p => p.ID == id).FirstOrDefault();

                    db.SaleManage.Remove(sm);

                    db.SaveChanges();
                }
            }

            List<SaleManage> smList;

            if (yearMonth != null)
                smList = db.SaleManage.Where(p => p.PLAN_DATE.Year == yearMonth.Value.Year && p.PLAN_DATE.Month == yearMonth.Value.Month).ToList();
            else
                smList = db.SaleManage.ToList();

            var dataSource = PagingHelper<SaleManage>.GetPagedDataTable(pageIndex, 20, smList.Count(), smList);
            UIHelper.Grid("Grid1").DataSource(dataSource, Grid1_fields);
            Alert.Show("操作成功！");

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_PageIndexChanged(string[] Grid1_fields, int Grid1_pageIndex, DateTime? yearMonth)
        {
            List<SaleManage> smList;

            if (yearMonth != null)
                smList = db.SaleManage.Where(p => p.PLAN_DATE.Year == yearMonth.Value.Year && p.PLAN_DATE.Month == yearMonth.Value.Month).ToList();
            else
                smList = db.SaleManage.ToList();

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = smList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<SaleManage>.GetPagedDataTable(Grid1_pageIndex, 20, recordCount, smList);
            grid1.DataSource(dataSource, Grid1_fields);

            return UIHelper.Result();
        }

        public ActionResult SellReport()
        {
            List<SellReport> prList = new List<SellReport>();

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            List<SaleManage> smList = db.SaleManage.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month).ToList();
            List<SaleManage> smListAll = db.SaleManage.ToList();



            SellReport pr2 = new SellReport();
            pr2.Month = month;
            pr2.Deportment = "精工工厂";
            pr2.YearlyPlan = 0;
            if (smList.Where(p=>p.RSPO_DEPT == "精工工厂" && p.MANAGE_PLAN != null).Any())
                pr2.YearlyPlan = (double)smList.Where(p => p.RSPO_DEPT == "精工工厂" && p.MANAGE_PLAN != null).FirstOrDefault().MANAGE_PLAN;
            pr2.Complete = (double)smListAll.Where(p => p.PLAN_DATE.Year == year && p.RSPO_DEPT == "精工工厂").Sum(p => p.SALE_FINISH);
            pr2.FinishingRate = Convert.ToDouble((pr2.Complete / pr2.YearlyPlan * 100.0).ToString("0"));
            pr2.MonthHourPlan = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "精工工厂").Sum(p => p.SALE_PLAN);
            pr2.MonthHourComplete = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "精工工厂").Sum(p => p.SALE_FINISH);
            pr2.MonthFinishingRate = Convert.ToDouble((pr2.MonthHourComplete / pr2.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr2);

            SellReport pr3 = new SellReport();
            pr3.Month = month;
            pr3.Deportment = "加工工厂";
            pr3.YearlyPlan = 0;
            if (smList.Where(p => p.RSPO_DEPT == "加工工厂" && p.MANAGE_PLAN != null).Any())
                pr3.YearlyPlan = (double)smList.Where(p => p.RSPO_DEPT == "加工工厂" && p.MANAGE_PLAN != null).FirstOrDefault().MANAGE_PLAN;
            pr3.Complete = (double)smListAll.Where(p => p.PLAN_DATE.Year == year && p.RSPO_DEPT == "加工工厂").Sum(p => p.SALE_FINISH);
            pr3.FinishingRate = Convert.ToDouble((pr3.Complete / pr3.YearlyPlan * 100.0).ToString("0"));
            pr3.MonthHourPlan = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "加工工厂").Sum(p => p.SALE_PLAN);
            pr3.MonthHourComplete = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "加工工厂").Sum(p => p.SALE_FINISH);
            pr3.MonthFinishingRate = Convert.ToDouble((pr3.MonthHourComplete / pr3.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr3);

            SellReport pr4 = new SellReport();
            pr4.Month = month;
            pr4.Deportment = "模具工厂";           
            pr4.YearlyPlan = 0;
            if (smList.Where(p => p.RSPO_DEPT == "模具工厂" && p.MANAGE_PLAN != null).Any())
                pr4.YearlyPlan = (double)smList.Where(p => p.RSPO_DEPT == "模具工厂" && p.MANAGE_PLAN != null).FirstOrDefault().MANAGE_PLAN;
            pr4.Complete = (double)smListAll.Where(p => p.PLAN_DATE.Year == year && p.RSPO_DEPT == "模具工厂").Sum(p => p.SALE_FINISH);
            pr4.FinishingRate = Convert.ToDouble((pr4.Complete / pr4.YearlyPlan * 100.0).ToString("0")) ;
            pr4.MonthHourPlan = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "模具工厂").Sum(p => p.SALE_PLAN);
            pr4.MonthHourComplete = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "模具工厂").Sum(p => p.SALE_FINISH);
            pr4.MonthFinishingRate = Convert.ToDouble((pr4.MonthHourComplete / pr4.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr4);

            SellReport pr5 = new SellReport();
            pr5.Month = month;
            pr5.Deportment = "智能设备工厂";
            pr5.YearlyPlan = 0;
            if (smList.Where(p => p.RSPO_DEPT == "智能设备工厂" && p.MANAGE_PLAN != null).Any())
                pr5.YearlyPlan = (double)smList.Where(p => p.RSPO_DEPT == "智能设备工厂" && p.MANAGE_PLAN != null).FirstOrDefault().MANAGE_PLAN;
            pr5.Complete = (double)smListAll.Where(p => p.PLAN_DATE.Year == year && p.RSPO_DEPT == "智能设备工厂").Sum(p => p.SALE_FINISH);
            pr5.FinishingRate = Convert.ToDouble((pr5.Complete / pr5.YearlyPlan * 100.0).ToString("0"));
            pr5.MonthHourPlan = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "智能设备工厂").Sum(p => p.SALE_PLAN);
            pr5.MonthHourComplete = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "智能设备工厂").Sum(p => p.SALE_FINISH);
            pr5.MonthFinishingRate = Convert.ToDouble((pr5.MonthHourComplete / pr5.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr5);

            SellReport pr6 = new SellReport();
            pr6.Month = month;
            pr6.Deportment = "智能机器";
            pr6.YearlyPlan = 0;
            if (smList.Where(p => p.RSPO_DEPT == "智能机器" && p.MANAGE_PLAN != null).Any())
                pr6.YearlyPlan = (double)smList.Where(p => p.RSPO_DEPT == "智能机器" && p.MANAGE_PLAN != null).FirstOrDefault().MANAGE_PLAN;
            pr6.Complete = (double)smListAll.Where(p => p.PLAN_DATE.Year == year && p.RSPO_DEPT == "智能机器").Sum(p => p.SALE_FINISH);
            pr6.FinishingRate = Convert.ToDouble((pr6.Complete / pr6.YearlyPlan * 100.0).ToString("0"));
            pr6.MonthHourPlan = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "智能机器").Sum(p => p.SALE_PLAN);
            pr6.MonthHourComplete = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "智能机器").Sum(p => p.SALE_FINISH);
            pr6.MonthFinishingRate = Convert.ToDouble((pr6.MonthHourComplete / pr6.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr6);

            SellReport pr1 = new SellReport();
            pr1.Month = month;
            pr1.Deportment = "制造事业部";
            pr1.YearlyPlan = pr2.YearlyPlan + pr3.YearlyPlan + pr4.YearlyPlan+ pr5.YearlyPlan+ pr6.YearlyPlan;
            pr1.Complete = Convert.ToDouble((pr2.Complete + pr3.Complete + pr4.Complete + pr5.Complete + pr6.Complete).ToString("0.00"));
            pr1.FinishingRate = Convert.ToDouble((pr1.Complete / pr1.YearlyPlan * 100.0).ToString("0"));
            pr1.MonthHourPlan = pr2.MonthHourPlan + pr3.MonthHourPlan + pr4.MonthHourPlan + pr5.MonthHourPlan + pr6.MonthHourPlan;
            pr1.MonthHourComplete = pr2.MonthHourComplete + pr3.MonthHourComplete + pr4.MonthHourComplete + pr5.MonthHourComplete + pr6.MonthHourComplete;
            pr1.MonthFinishingRate = Convert.ToDouble((pr1.MonthHourComplete / pr1.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr1);

            return View(prList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SellReport(string[] Grid1_fields, DateTime? yearMonth)
        {
            List<SellReport> prList = new List<SellReport>();

            int year = yearMonth.Value.Year;
            int month = yearMonth.Value.Month;

            List<SaleManage> smList = db.SaleManage.Where(p => p.PLAN_DATE.Year == yearMonth.Value.Year && p.PLAN_DATE.Month == yearMonth.Value.Month).ToList();
            List<SaleManage> smListAll = db.SaleManage.ToList();

            SellReport pr2 = new SellReport();
            pr2.Month = month;
            pr2.Deportment = "精工工厂";
            pr2.YearlyPlan = 0;
            if (smList.Where(p => p.RSPO_DEPT == "精工工厂" && p.MANAGE_PLAN != null).Any())
                pr2.YearlyPlan = (double)smList.Where(p => p.RSPO_DEPT == "精工工厂" && p.MANAGE_PLAN != null).FirstOrDefault().MANAGE_PLAN;
            pr2.Complete = (double)smListAll.Where(p => p.PLAN_DATE.Year == year && p.RSPO_DEPT == "精工工厂").Sum(p => p.SALE_FINISH);
            pr2.FinishingRate = Convert.ToDouble((pr2.Complete / pr2.YearlyPlan * 100.0).ToString("0"));
            pr2.MonthHourPlan = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "精工工厂").Sum(p => p.SALE_PLAN);
            pr2.MonthHourComplete = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "精工工厂").Sum(p => p.SALE_FINISH);
            pr2.MonthFinishingRate = Convert.ToDouble((pr2.MonthHourComplete / pr2.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr2);

            SellReport pr3 = new SellReport();
            pr3.Month = month;
            pr3.Deportment = "加工工厂";
            pr3.YearlyPlan = 0;
            if (smList.Where(p => p.RSPO_DEPT == "加工工厂" && p.MANAGE_PLAN != null).Any())
                pr3.YearlyPlan = (double)smList.Where(p => p.RSPO_DEPT == "加工工厂" && p.MANAGE_PLAN != null).FirstOrDefault().MANAGE_PLAN;
            pr3.Complete = (double)smListAll.Where(p => p.PLAN_DATE.Year == year && p.RSPO_DEPT == "加工工厂").Sum(p => p.SALE_FINISH);
            pr3.FinishingRate = Convert.ToDouble((pr3.Complete / pr3.YearlyPlan * 100.0).ToString("0"));
            pr3.MonthHourPlan = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "加工工厂").Sum(p => p.SALE_PLAN);
            pr3.MonthHourComplete = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "加工工厂").Sum(p => p.SALE_FINISH);
            pr3.MonthFinishingRate = Convert.ToDouble((pr3.MonthHourComplete / pr3.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr3);

            SellReport pr4 = new SellReport();
            pr4.Month = month;
            pr4.Deportment = "模具工厂";
            pr4.YearlyPlan = 0;
            if (smList.Where(p => p.RSPO_DEPT == "模具工厂" && p.MANAGE_PLAN != null).Any())
                pr4.YearlyPlan = (double)smList.Where(p => p.RSPO_DEPT == "模具工厂" && p.MANAGE_PLAN != null).FirstOrDefault().MANAGE_PLAN;
            pr4.Complete = (double)smListAll.Where(p => p.PLAN_DATE.Year == year && p.RSPO_DEPT == "模具工厂").Sum(p => p.SALE_FINISH);
            pr4.FinishingRate = Convert.ToDouble((pr4.Complete / pr4.YearlyPlan * 100.0).ToString("0"));
            pr4.MonthHourPlan = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "模具工厂").Sum(p => p.SALE_PLAN);
            pr4.MonthHourComplete = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "模具工厂").Sum(p => p.SALE_FINISH);
            pr4.MonthFinishingRate = Convert.ToDouble((pr4.MonthHourComplete / pr4.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr4);

            SellReport pr5 = new SellReport();
            pr5.Month = month;
            pr5.Deportment = "智能设备工厂";
            pr5.YearlyPlan = 0;
            if (smList.Where(p => p.RSPO_DEPT == "智能设备工厂" && p.MANAGE_PLAN != null).Any())
                pr5.YearlyPlan = (double)smList.Where(p => p.RSPO_DEPT == "智能设备工厂" && p.MANAGE_PLAN != null).FirstOrDefault().MANAGE_PLAN;
            pr5.Complete = (double)smListAll.Where(p => p.PLAN_DATE.Year == year && p.RSPO_DEPT == "智能设备工厂").Sum(p => p.SALE_FINISH);
            pr5.FinishingRate = Convert.ToDouble((pr5.Complete / pr5.YearlyPlan * 100.0).ToString("0"));
            pr5.MonthHourPlan = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "智能设备工厂").Sum(p => p.SALE_PLAN);
            pr5.MonthHourComplete = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "智能设备工厂").Sum(p => p.SALE_FINISH);
            pr5.MonthFinishingRate = Convert.ToDouble((pr5.MonthHourComplete / pr5.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr5);

            SellReport pr6 = new SellReport();
            pr6.Month = month;
            pr6.Deportment = "智能机器";
            pr6.YearlyPlan = 0;
            if (smList.Where(p => p.RSPO_DEPT == "智能机器" && p.MANAGE_PLAN != null).Any())
                pr6.YearlyPlan = (double)smList.Where(p => p.RSPO_DEPT == "智能机器" && p.MANAGE_PLAN != null).FirstOrDefault().MANAGE_PLAN;
            pr6.Complete = (double)smListAll.Where(p => p.PLAN_DATE.Year == year && p.RSPO_DEPT == "智能机器").Sum(p => p.SALE_FINISH);
            pr6.FinishingRate = Convert.ToDouble((pr6.Complete / pr6.YearlyPlan * 100.0).ToString("0"));
            pr6.MonthHourPlan = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "智能机器").Sum(p => p.SALE_PLAN);
            pr6.MonthHourComplete = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "智能机器").Sum(p => p.SALE_FINISH);
            pr6.MonthFinishingRate = Convert.ToDouble((pr6.MonthHourComplete / pr6.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr6);

            SellReport pr1 = new SellReport();
            pr1.Month = month;
            pr1.Deportment = "制造事业部";
            pr1.YearlyPlan = pr2.YearlyPlan + pr3.YearlyPlan + pr4.YearlyPlan + pr5.YearlyPlan + pr6.YearlyPlan;
            pr1.Complete = Convert.ToDouble((pr2.Complete + pr3.Complete + pr4.Complete + pr5.Complete + pr6.Complete).ToString("0.00"));
            pr1.FinishingRate = Convert.ToDouble((pr1.Complete / pr1.YearlyPlan * 100.0).ToString("0"));
            pr1.MonthHourPlan = pr2.MonthHourPlan + pr3.MonthHourPlan + pr4.MonthHourPlan + pr5.MonthHourPlan + pr6.MonthHourPlan;
            pr1.MonthHourComplete = pr2.MonthHourComplete + pr3.MonthHourComplete + pr4.MonthHourComplete + pr5.MonthHourComplete + pr6.MonthHourComplete;
            pr1.MonthFinishingRate = Convert.ToDouble((pr1.MonthHourComplete / pr1.MonthHourPlan * 100.0).ToString("0"));
            prList.Add(pr1);

            var grid1 = UIHelper.Grid("Grid1");

            var dataSource = prList;
            grid1.DataSource(dataSource, Grid1_fields);

            return UIHelper.Result();
        }

        public ActionResult MonthAnalyze()
        {
            List<MonthAnalyze> prList = new List<MonthAnalyze>();

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            List<SaleManage> smList = db.SaleManage.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month).ToList();

            MonthAnalyze pr4 = new MonthAnalyze();
            pr4.Content = "年/月计划";
            pr4.ZZSYB = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT != "制造事业部").Sum(p => p.MANAGE_PLAN) / 12;
            pr4.JGGC = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "精工工厂").Sum(p => p.MANAGE_PLAN) / 12;
            pr4.JAGGC = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "加工工厂").Sum(p => p.MANAGE_PLAN) / 12;
            pr4.MJGC = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "模具工厂").Sum(p => p.MANAGE_PLAN) / 12;
            pr4.ZNSBGC = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "智能设备工厂").Sum(p => p.MANAGE_PLAN) / 12;
            pr4.ZNJQ = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "智能机器").Sum(p => p.MANAGE_PLAN) / 12;
            prList.Add(pr4);

            MonthAnalyze pr1 = new MonthAnalyze();
            pr1.Content = "月计划";
            pr1.ZZSYB = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT != "制造事业部").Sum(p => p.SALE_PLAN); 
            pr1.JGGC = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "精工工厂").Sum(p => p.SALE_PLAN); 
            pr1.JAGGC = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "加工工厂").Sum(p => p.SALE_PLAN); 
            pr1.MJGC = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "模具工厂").Sum(p => p.SALE_PLAN); 
            pr1.ZNSBGC = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "智能设备工厂").Sum(p => p.SALE_PLAN); 
            pr1.ZNJQ = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "智能机器").Sum(p => p.SALE_PLAN); 
            prList.Add(pr1);

            MonthAnalyze pr2 = new MonthAnalyze();
            pr2.Content = "月实际";
            pr2.ZZSYB = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT != "制造事业部").Sum(p => p.SALE_FINISH);
            pr2.JGGC = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "精工工厂").Sum(p => p.SALE_FINISH);
            pr2.JAGGC = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "加工工厂").Sum(p => p.SALE_FINISH);
            pr2.MJGC = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "模具工厂").Sum(p => p.SALE_FINISH);
            pr2.ZNSBGC = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "智能设备工厂").Sum(p => p.SALE_FINISH);
            pr2.ZNJQ = (double)smList.Where(p => p.PLAN_DATE.Year == year && p.PLAN_DATE.Month == month && p.RSPO_DEPT == "智能机器").Sum(p => p.SALE_FINISH);
            prList.Add(pr2);

            MonthAnalyze pr3 = new MonthAnalyze();
            pr3.Content = "月完成率";
            pr3.ZZSYB = Convert.ToDouble((pr2.ZZSYB / pr1.ZZSYB * 100.0).ToString("0"));
            pr3.JGGC = Convert.ToDouble((pr2.JGGC / pr1.JGGC * 100.0).ToString("0")); 
            pr3.JAGGC = Convert.ToDouble((pr2.JAGGC / pr1.JAGGC * 100.0).ToString("0")); 
            pr3.MJGC = Convert.ToDouble((pr2.MJGC / pr1.MJGC * 100.0).ToString("0.00")); 
            pr3.ZNSBGC = Convert.ToDouble((pr2.ZNSBGC / pr1.ZNSBGC * 100.0).ToString("0")); 
            pr3.ZNJQ = Convert.ToDouble((pr2.ZNJQ / pr1.ZNJQ * 100.0).ToString("0"));
            prList.Add(pr3);

            foreach(var pr in prList)
            {
                if (double.IsNaN(pr.JAGGC))
                    pr.JAGGC = 0;
                if (double.IsNaN(pr.JGGC))
                    pr.JGGC = 0;
                if (double.IsNaN(pr.MJGC))
                    pr.MJGC = 0;
                if (double.IsNaN(pr.ZNJQ))
                    pr.ZNJQ = 0;
                if (double.IsNaN(pr.ZNSBGC))
                    pr.ZNSBGC = 0;
                if (double.IsNaN(pr.ZZSYB))
                    pr.ZZSYB = 0;
            }

            return View(prList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MonthAnalyze(string[] Grid1_fields, DateTime? yearMonth,DateTime? toYearMonth)
        {
            List<MonthAnalyze> prList = new List<MonthAnalyze>();

            int year = yearMonth.Value.Year;
            int month = yearMonth.Value.Month;

            int toYear = toYearMonth.Value.Year;
            int toMonth = toYearMonth.Value.Month;

            List<SaleManage> smList = db.SaleManage.Where(p => p.PLAN_DATE.Year >= yearMonth.Value.Year && p.PLAN_DATE.Month >= yearMonth.Value.Month 
            && p.PLAN_DATE.Year <= toYear && p.PLAN_DATE.Month <= toMonth).ToList();

            MonthAnalyze pr4 = new MonthAnalyze();
            pr4.Content = "年/月计划";
            pr4.ZZSYB = (double)smList.Where(p => p.PLAN_DATE.Year >= yearMonth.Value.Year && p.PLAN_DATE.Month >= yearMonth.Value.Month
            && p.PLAN_DATE.Year <= toYear && p.PLAN_DATE.Month <= toMonth && p.RSPO_DEPT != "制造事业部").Sum(p => p.MANAGE_PLAN) / 12;
            pr4.JGGC = (double)smList.Where(p => p.PLAN_DATE.Year >= yearMonth.Value.Year && p.PLAN_DATE.Month >= yearMonth.Value.Month
            && p.PLAN_DATE.Year <= toYear && p.PLAN_DATE.Month <= toMonth && p.RSPO_DEPT == "精工工厂").Sum(p => p.MANAGE_PLAN) / 12;
            pr4.JAGGC = (double)smList.Where(p => p.PLAN_DATE.Year >= yearMonth.Value.Year && p.PLAN_DATE.Month >= yearMonth.Value.Month
            && p.PLAN_DATE.Year <= toYear && p.PLAN_DATE.Month <= toMonth && p.RSPO_DEPT == "加工工厂").Sum(p => p.MANAGE_PLAN) / 12;
            pr4.MJGC = (double)smList.Where(p => p.PLAN_DATE.Year >= yearMonth.Value.Year && p.PLAN_DATE.Month >= yearMonth.Value.Month
            && p.PLAN_DATE.Year <= toYear && p.PLAN_DATE.Month <= toMonth && p.RSPO_DEPT == "模具工厂").Sum(p => p.MANAGE_PLAN) / 12;
            pr4.ZNSBGC = (double)smList.Where(p => p.PLAN_DATE.Year >= yearMonth.Value.Year && p.PLAN_DATE.Month >= yearMonth.Value.Month
            && p.PLAN_DATE.Year <= toYear && p.PLAN_DATE.Month <= toMonth && p.RSPO_DEPT == "智能设备工厂").Sum(p => p.MANAGE_PLAN) / 12;
            pr4.ZNJQ = (double)smList.Where(p => p.PLAN_DATE.Year >= yearMonth.Value.Year && p.PLAN_DATE.Month >= yearMonth.Value.Month
            && p.PLAN_DATE.Year <= toYear && p.PLAN_DATE.Month <= toMonth && p.RSPO_DEPT == "智能机器").Sum(p => p.MANAGE_PLAN) / 12;
            prList.Add(pr4);

            MonthAnalyze pr1 = new MonthAnalyze();
            pr1.Content = "月计划";
            pr1.ZZSYB = (double)smList.Where(p => p.PLAN_DATE.Year >= yearMonth.Value.Year && p.PLAN_DATE.Month >= yearMonth.Value.Month
            && p.PLAN_DATE.Year <= toYear && p.PLAN_DATE.Month <= toMonth && p.RSPO_DEPT != "制造事业部").Sum(p => p.SALE_PLAN);
            pr1.JGGC = (double)smList.Where(p => p.PLAN_DATE.Year >= yearMonth.Value.Year && p.PLAN_DATE.Month >= yearMonth.Value.Month
            && p.PLAN_DATE.Year <= toYear && p.PLAN_DATE.Month <= toMonth && p.RSPO_DEPT == "精工工厂").Sum(p => p.SALE_PLAN);
            pr1.JAGGC = (double)smList.Where(p => p.PLAN_DATE.Year >= yearMonth.Value.Year && p.PLAN_DATE.Month >= yearMonth.Value.Month
            && p.PLAN_DATE.Year <= toYear && p.PLAN_DATE.Month <= toMonth && p.RSPO_DEPT == "加工工厂").Sum(p => p.SALE_PLAN);
            pr1.MJGC = (double)smList.Where(p => p.PLAN_DATE.Year >= yearMonth.Value.Year && p.PLAN_DATE.Month >= yearMonth.Value.Month
            && p.PLAN_DATE.Year <= toYear && p.PLAN_DATE.Month <= toMonth && p.RSPO_DEPT == "模具工厂").Sum(p => p.SALE_PLAN);
            pr1.ZNSBGC = (double)smList.Where(p => p.PLAN_DATE.Year >= yearMonth.Value.Year && p.PLAN_DATE.Month >= yearMonth.Value.Month
            && p.PLAN_DATE.Year <= toYear && p.PLAN_DATE.Month <= toMonth && p.RSPO_DEPT == "智能设备工厂").Sum(p => p.SALE_PLAN);
            pr1.ZNJQ = (double)smList.Where(p => p.PLAN_DATE.Year >= yearMonth.Value.Year && p.PLAN_DATE.Month >= yearMonth.Value.Month
            && p.PLAN_DATE.Year <= toYear && p.PLAN_DATE.Month <= toMonth && p.RSPO_DEPT == "智能机器").Sum(p => p.SALE_PLAN);
            prList.Add(pr1);

            MonthAnalyze pr2 = new MonthAnalyze();
            pr2.Content = "月实际";
            pr2.ZZSYB = (double)smList.Where(p => p.PLAN_DATE.Year >= yearMonth.Value.Year && p.PLAN_DATE.Month >= yearMonth.Value.Month
            && p.PLAN_DATE.Year <= toYear && p.PLAN_DATE.Month <= toMonth && p.RSPO_DEPT != "制造事业部").Sum(p => p.SALE_FINISH);
            pr2.JGGC = (double)smList.Where(p => p.PLAN_DATE.Year >= yearMonth.Value.Year && p.PLAN_DATE.Month >= yearMonth.Value.Month
            && p.PLAN_DATE.Year <= toYear && p.PLAN_DATE.Month <= toMonth && p.RSPO_DEPT == "精工工厂").Sum(p => p.SALE_FINISH);
            pr2.JAGGC = (double)smList.Where(p => p.PLAN_DATE.Year >= yearMonth.Value.Year && p.PLAN_DATE.Month >= yearMonth.Value.Month
            && p.PLAN_DATE.Year <= toYear && p.PLAN_DATE.Month <= toMonth && p.RSPO_DEPT == "加工工厂").Sum(p => p.SALE_FINISH);
            pr2.MJGC = (double)smList.Where(p => p.PLAN_DATE.Year >= yearMonth.Value.Year && p.PLAN_DATE.Month >= yearMonth.Value.Month
            && p.PLAN_DATE.Year <= toYear && p.PLAN_DATE.Month <= toMonth && p.RSPO_DEPT == "模具工厂").Sum(p => p.SALE_FINISH);
            pr2.ZNSBGC = (double)smList.Where(p => p.PLAN_DATE.Year >= yearMonth.Value.Year && p.PLAN_DATE.Month >= yearMonth.Value.Month
            && p.PLAN_DATE.Year <= toYear && p.PLAN_DATE.Month <= toMonth && p.RSPO_DEPT == "智能设备工厂").Sum(p => p.SALE_FINISH);
            pr2.ZNJQ = (double)smList.Where(p => p.PLAN_DATE.Year >= yearMonth.Value.Year && p.PLAN_DATE.Month >= yearMonth.Value.Month
            && p.PLAN_DATE.Year <= toYear && p.PLAN_DATE.Month <= toMonth && p.RSPO_DEPT == "智能机器").Sum(p => p.SALE_FINISH);
            prList.Add(pr2);

            MonthAnalyze pr3 = new MonthAnalyze();
            pr3.Content = "月完成率";
            pr3.ZZSYB = Convert.ToDouble((pr2.ZZSYB / pr1.ZZSYB * 100.0).ToString("0"));
            pr3.JGGC = Convert.ToDouble((pr2.JGGC / pr1.JGGC * 100.0).ToString("0"));
            pr3.JAGGC = Convert.ToDouble((pr2.JAGGC / pr1.JAGGC * 100.0).ToString("0"));
            pr3.MJGC = Convert.ToDouble((pr2.MJGC / pr1.MJGC * 100.0).ToString("0"));
            pr3.ZNSBGC = Convert.ToDouble((pr2.ZNSBGC / pr1.ZNSBGC * 100.0).ToString("0"));
            pr3.ZNJQ = Convert.ToDouble((pr2.ZNJQ / pr1.ZNJQ * 100.0).ToString("0"));
            prList.Add(pr3);

            var grid1 = UIHelper.Grid("Grid1");

            var dataSource = prList;
            grid1.DataSource(dataSource, Grid1_fields);

            return UIHelper.Result();
        }

        public ActionResult YearPlan()
        {
            ViewBag.RecordCount = db.YearPlan.Count();
            return View(db.YearPlan.ToList());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult btnSubmit_YearPlan_Click(string[] Grid1_fields, JArray Grid1_modifiedData, int pageIndex)
        {
            foreach (JObject mergedRow in Grid1_modifiedData)
            {
                string status = mergedRow.Value<string>("status");
                int rowIndex = mergedRow.Value<int>("index");
                JObject values = mergedRow.Value<JObject>("values");

                if (status == "modified")
                {
                    int id = mergedRow.Value<int>("id");

                    YearPlan yp = db.YearPlan.Where(p => p.ID == id).FirstOrDefault();

                    string Factory = values.Value<string>("Factory");
                    int? Year = values.Value<int?>("Year");
                    double? SellYearPlan = values.Value<double?>("SellYearPlan");
                    double? YearPlan1 = values.Value<double?>("YearPlan1");

                    if (Factory != null)
                        yp.Factory = Factory;
                    if (Year != null)
                        yp.Year = (int)Year;
                    if (SellYearPlan != null)
                        yp.SellYearPlan = (double)SellYearPlan;
                    if (YearPlan1 != null)
                        yp.YearPlan1 = (double)YearPlan1;

                    db.SaveChanges();
                }
                else if (status == "newadded")
                {
                    YearPlan yp = new YearPlan();

                    string Factory = values.Value<string>("Factory");
                    int? Year = values.Value<int?>("Year");
                    double? SellYearPlan = values.Value<double?>("SellYearPlan");
                    double? YearPlan1 = values.Value<double?>("YearPlan1");

                    if (Factory != null)
                        yp.Factory = Factory;
                    if (Year != null)
                        yp.Year = (int)Year;
                    if (SellYearPlan != null)
                        yp.SellYearPlan = (double)SellYearPlan;
                    if (YearPlan1 != null)
                        yp.YearPlan1 = (double)YearPlan1;


                    db.YearPlan.Add(yp);
                    db.SaveChanges();
                }
                else if (status == "deleted")
                {
                    int id = mergedRow.Value<int>("id");

                    YearPlan yp = db.YearPlan.Where(p => p.ID == id).FirstOrDefault();

                    db.YearPlan.Remove(yp);

                    db.SaveChanges();
                }
            }

            List<YearPlan> ypList;

            ypList = db.YearPlan.ToList();

            var dataSource = PagingHelper<YearPlan>.GetPagedDataTable(pageIndex, 20, ypList.Count(), ypList);
            UIHelper.Grid("Grid1").DataSource(dataSource, Grid1_fields);
            Alert.Show("操作成功！");

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_YearPlan_PageIndexChanged(string[] Grid1_fields, int Grid1_pageIndex)
        {
            List<YearPlan> ypList;

            ypList = db.YearPlan.ToList();

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = ypList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<YearPlan>.GetPagedDataTable(Grid1_pageIndex, 20, recordCount, ypList);
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

                    List<SaleManage> smList = ConvertToProductManage(beanList);

                    db.SaleManage.AddRange(smList);

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
            switch (FactoryName)
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

        public List<SaleManage> ConvertToProductManage(List<List<object>> beanList)
        {
            List<SaleManage> pList = new List<SaleManage>();

            foreach (var bean in beanList)
            {
                try
                {

                    SaleManage p = new SaleManage();

                    if (bean[0] != null)
                        p.PLAN_DATE = (DateTime)bean[0];
                    else
                        continue;

                    if (bean[1] != null)
                        p.MANAGE_PLAN = (double)bean[1];
                    else
                        continue;

                    if (bean[2] != null)
                        p.SALE_PLAN = (double)bean[2];
                    else
                        continue;

                    if (bean[3] != null)
                        p.SALE_FINISH = (double)bean[3];
                    else
                        continue;

                    if (bean[4] != null)
                        p.RSPO_DEPT = (string)bean[4];
                    else
                        continue;

                    if (bean[5] != null)
                        p.REMARK = (string)bean[5];
                    else
                        continue;

                    if (bean[6] != null)
                        p.Company = (string)bean[6];
                    else
                        continue;

                    if (bean[7] != null && bean[7].GetType() == typeof(double))
                        p.SellYear = Convert.ToInt32((double)bean[7]);
                    else
                        continue;

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
    }
}
