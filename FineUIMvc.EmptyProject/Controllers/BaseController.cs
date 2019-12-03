using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using FineUIMvc.EmptyProject.Models;

namespace FineUIMvc.EmptyProject.Controllers
{
    public class BaseController : Controller
    {
        public MoJuDataEntities mojuEntity = new MoJuDataEntities();

        /// <summary>
        /// 显示通知对话框
        /// </summary>
        /// <param name="message"></param>
        public virtual void ShowNotify(string message)
        {
            ShowNotify(message, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示通知对话框
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageIcon"></param>
        public virtual void ShowNotify(string message, MessageBoxIcon messageIcon)
        {
            ShowNotify(message, messageIcon, Target.Top);
        }

        /// <summary>
        /// 显示通知对话框
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageIcon"></param>
        /// <param name="target"></param>
        public virtual void ShowNotify(string message, MessageBoxIcon messageIcon, Target target)
        {
            Notify n = new Notify();
            n.Target = target;
            n.Message = message;
            n.MessageBoxIcon = messageIcon;
            n.PositionX = Position.Center;
            n.PositionY = Position.Top;
            n.DisplayMilliseconds = 3000;
            n.ShowHeader = false;

            n.Show();
        }

        /// <summary>
        /// 登录验证跳转
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UserInfor user = (UserInfor)filterContext.HttpContext.Session["User"];

            if (
                !(filterContext.RouteData.GetRequiredString("controller") == "Login") &&
                !(filterContext.ActionDescriptor.ActionName == "Login")
                )
            {
                if (user == null)
                {
                    filterContext.Result = new RedirectResult("/Login/Login");
                    return;
                }

                base.OnActionExecuting(filterContext);
            }

        }

        /// <summary>
        /// 转换DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public DataTable ToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static class PagingHelper<T>
        {
            public static List<T> GetPagedDataTable(int pageIndex, int pageSize, int recordCount, List<T> list)
            {
                //// 对传入的 pageIndex 进行有效性验证//////////////
                int pageCount = recordCount / pageSize;
                if (recordCount % pageSize != 0)
                {
                    pageCount++;
                }
                if (pageIndex > pageCount - 1)
                {
                    pageIndex = pageCount - 1;
                }
                if (pageIndex < 0)
                {
                    pageIndex = 0;
                }
                ///////////////////////////////////////////////

                List<T> retList = new List<T>();

                int rowbegin = pageIndex * pageSize;
                int rowend = (pageIndex + 1) * pageSize;
                if (rowend > list.Count)
                {
                    rowend = list.Count;
                }

                retList = list.Skip(pageIndex * pageSize).Take(pageSize).ToList();

                return retList;
            }
        }
    }
}