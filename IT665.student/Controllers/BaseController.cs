using IT665.Attributes;
using IT665.Models.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IT665.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.IsLoggedIn = Session["LoginToken"] != null;
            ViewBag.IsCustomerManager = Session["Roles"] != null 
                && ((string[])Session["Roles"]).Contains("CustomerManager");
            ViewBag.IsProductManager = Session["Roles"] != null
                && ((string[])Session["Roles"]).Contains("ProductManager");
            base.OnActionExecuting(context);
        }
    }
}