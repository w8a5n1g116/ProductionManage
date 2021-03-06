﻿using FineUIMvc.EmptyProject.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FineUIMvc.EmptyProject.Controllers
{
    public class CapCompleteController : BaseController
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

            ViewBag.RecordCount = db.CapCompleteRate.Where(p => p.PlanDate.Value.Year == year && p.PlanDate.Value.Month == month).Count();
            return View(PagingHelper<CapCompleteRate>.GetPagedDataTable(0, 20, ViewBag.RecordCount, db.CapCompleteRate.Where(p => p.PlanDate.Value.Year == year && p.PlanDate.Value.Month == month).ToList()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string[] Grid1_fields, DateTime? yearMonth, string deportment)
        {
            List<CapCompleteRate> pmList;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (yearMonth != null)
                pmList = db.CapCompleteRate.Where(p => p.PlanDate.Value.Year == yearMonth.Value.Year && p.PlanDate.Value.Month == yearMonth.Value.Month).ToList();
            else
                pmList = db.CapCompleteRate.Where(p => p.PlanDate.Value.Year == year && p.PlanDate.Value.Month == month).ToList();

            if (!string.IsNullOrEmpty(deportment))
            {
                pmList = pmList.Where(p => p.FAB_NAME == deportment).ToList();
            }

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = pmList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<CapCompleteRate>.GetPagedDataTable(0, 20, recordCount, pmList);
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

            ViewBag.RecordCount = db.CapCompleteRate.Where(p => p.PlanDate.Value.Year == year && p.PlanDate.Value.Month == month).Count();
            return View(PagingHelper<CapCompleteRate>.GetPagedDataTable(0, 20, ViewBag.RecordCount, db.CapCompleteRate.Where(p => p.PlanDate.Value.Year == year && p.PlanDate.Value.Month == month).ToList()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Check(string[] Grid1_fields, DateTime? yearMonth, string deportment)
        {
            List<CapCompleteRate> pmList;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (yearMonth != null)
                pmList = db.CapCompleteRate.Where(p => p.PlanDate.Value.Year == yearMonth.Value.Year && p.PlanDate.Value.Month == yearMonth.Value.Month).ToList();
            else
                pmList = db.CapCompleteRate.Where(p => p.PlanDate.Value.Year == year && p.PlanDate.Value.Month == month).ToList();

            if (!string.IsNullOrEmpty(deportment))
            {
                pmList = pmList.Where(p => p.FAB_NAME == deportment).ToList();
            }

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = pmList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<CapCompleteRate>.GetPagedDataTable(0, 20, recordCount, pmList);
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

                    CapCompleteRate pm = db.CapCompleteRate.Where(p => p.ID == id).FirstOrDefault();

                    string FAB_NAME = values.Value<string>("FAB_NAME");
                    string Venture = values.Value<string>("Venture");
                    string Operation = values.Value<string>("Operation");
                    double? CompleteRate = values.Value<double?>("CompleteRate");
                    double? PromptnessRate = values.Value<double?>("PromptnessRate");
                    double? PassRate = values.Value<double?>("PassRate");                    
                    DateTime? PlanDate = values.Value<DateTime?>("PlanDate");

                    if (FAB_NAME != null)
                        pm.FAB_NAME = FAB_NAME;
                    if (Venture != null)
                        pm.Venture = Venture;
                    if (Operation != null)
                        pm.Operation = Operation;
                    if (CompleteRate != null)
                        pm.CompleteRate = CompleteRate;
                    if (PromptnessRate != null)
                        pm.PromptnessRate = PromptnessRate;
                    if (PassRate != null)
                        pm.PassRate = PassRate;
                    if (PlanDate != null)
                        pm.PlanDate = PlanDate;

                    db.SaveChanges();
                }
                else if (status == "newadded")
                {
                    CapCompleteRate pm = new CapCompleteRate();

                    string FAB_NAME = values.Value<string>("FAB_NAME");
                    string Venture = values.Value<string>("Venture");
                    string Operation = values.Value<string>("Operation");
                    double? CompleteRate = values.Value<double?>("CompleteRate");
                    double? PromptnessRate = values.Value<double?>("PromptnessRate");
                    double? PassRate = values.Value<double?>("PassRate");
                    string PlanDate = values.Value<string>("PlanDate");


                    if (FAB_NAME != null)
                        pm.FAB_NAME = FAB_NAME;
                    if (Venture != null)
                        pm.Venture = Venture;
                    if (Operation != null)
                        pm.Operation = Operation;
                    if (CompleteRate != null)
                        pm.CompleteRate = CompleteRate;
                    if (PromptnessRate != null)
                        pm.PromptnessRate = PromptnessRate;
                    if (PassRate != null)
                        pm.PassRate = PassRate;
                    if (!string.IsNullOrEmpty(PlanDate))
                        pm.PlanDate = Convert.ToDateTime(PlanDate);

                    db.CapCompleteRate.Add(pm);
                    db.SaveChanges();
                }
                else if (status == "deleted")
                {
                    int id = mergedRow.Value<int>("id");

                    CapCompleteRate pm = db.CapCompleteRate.Where(p => p.ID == id).FirstOrDefault();

                    db.CapCompleteRate.Remove(pm);

                    db.SaveChanges();
                }
            }

            List<CapCompleteRate> pmList;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (yearMonth != null)
                pmList = db.CapCompleteRate.Where(p => p.PlanDate.Value.Year == yearMonth.Value.Year && p.PlanDate.Value.Month == yearMonth.Value.Month).ToList();
            else
                pmList = db.CapCompleteRate.Where(p => p.PlanDate.Value.Year == year && p.PlanDate.Value.Month == month).ToList();

            var dataSource = PagingHelper<CapCompleteRate>.GetPagedDataTable(pageIndex, 20, pmList.Count(), pmList);
            UIHelper.Grid("Grid1").DataSource(dataSource, Grid1_fields);
            Alert.Show("操作成功！");

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_PageIndexChanged(string[] Grid1_fields, int Grid1_pageIndex, DateTime? yearMonth, string deportment)
        {
            List<CapCompleteRate> pmList;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (yearMonth != null)
                pmList = db.CapCompleteRate.Where(p => p.PlanDate.Value.Year == yearMonth.Value.Year && p.PlanDate.Value.Month == yearMonth.Value.Month).ToList();
            else
                pmList = db.CapCompleteRate.Where(p => p.PlanDate.Value.Year == year && p.PlanDate.Value.Month == month).ToList();

            if (!string.IsNullOrEmpty(deportment))
            {
                pmList = pmList.Where(p => p.FAB_NAME == deportment).ToList();
            }

            var grid1 = UIHelper.Grid("Grid1");

            var recordCount = pmList.Count();

            grid1.RecordCount(recordCount);

            var dataSource = PagingHelper<CapCompleteRate>.GetPagedDataTable(Grid1_pageIndex, 20, recordCount, pmList);
            grid1.DataSource(dataSource, Grid1_fields);

            return UIHelper.Result();
        }
    }
}