using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FineUIMvc.EmptyProject.Models;

namespace FineUIMvc.EmptyProject.Controllers
{
    public class LoginController : BaseController
    {
        public MoJuDataEntities mojuEntity = new MoJuDataEntities();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public string Login(string userName,string password)
        {
            UserInfor user = mojuEntity.UserInfor.Where(p => p.UserName == userName).FirstOrDefault();

            if (user != null && user.PassWord == password)
            {
                Session["User"] = user;

                return "{\"success\":true,\"message\":\"登录成功\"}";
            }
            else
            {
                return "{\"success\":false,\"message\":\"用户名或密码错误\"}";
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}