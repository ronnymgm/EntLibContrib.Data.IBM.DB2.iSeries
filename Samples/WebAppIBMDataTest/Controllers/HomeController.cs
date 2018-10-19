using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebAppIBMDataTest.Models;

namespace WebAppIBMDataTest.Controllers
{
    public class HomeController : Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            CustomerInfo custInf = CustomerInfo.GetCustomerInfo("ALFKI");

            ViewBag.Message = custInf.CustName;

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            UserInfo usr = UserInfo.GetUserInfo(1);

            return View();
        }
    }
}