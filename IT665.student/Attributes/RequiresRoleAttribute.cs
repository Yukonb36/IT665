using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IT665.Attributes
{
    public class RequiresRoleAttribute : AuthorizeAttribute
    {
        string _role = null;
        public RequiresRoleAttribute(string role) : base()
        {
            if (string.IsNullOrWhiteSpace(role)) throw new ArgumentException("role is missing");
            _role = role;
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Security/Logout");
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.Session["Roles"] != null
                && ((string[])httpContext.Session["Roles"]).Contains(_role);
        }
    }
}