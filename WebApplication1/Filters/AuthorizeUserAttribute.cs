using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace WebApplication1
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public static string SessionId = "user";

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //base.OnAuthorization(filterContext);
            //if (filterContext.Result is HttpUnauthorizedResult) {
            //    if (!filterContext.IsChildAction) {

            //    }
            //}
                    //filterContext.Result = new RedirectToRouteResult(
                    //    new RouteValueDictionary{
                    //        {"controller","Login"},
                    //        {"action","Index"},
                    //        {"returnUrl",filterContext.HttpContext.Request.RawUrl}
                    //    });

                    //base.OnAuthorization(filterContext);
                    if (filterContext.Result == null)
                    {
                        if (filterContext.HttpContext.Session != null)
                        {
                            //add checks for your configuration
                            //add session data

                            // if you have a url you can use RedirectResult
                            // in this example I use RedirectToRouteResult

                            if (filterContext.HttpContext.Session[SessionId] == null)
                            {
                                RouteValueDictionary rd = new RouteValueDictionary();
                                rd.Add("controller", "Home");
                                rd.Add("action", "Login");//Index
                                rd.Add("returnUrl", filterContext.HttpContext.Request.RawUrl);
                                filterContext.Result = new RedirectToRouteResult("Default", rd);
                            }
                        }
                    }
        }
    }
}