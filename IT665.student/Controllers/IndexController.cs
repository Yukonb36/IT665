using IT665.Attributes;
using IT665.Models.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IT665.Controllers
{
    [SessionAuthorize]
    public class IndexController : BaseController
    {
        // GET: Index
        public ActionResult Index()
        {
            return View(new Index());
        }
    }
}