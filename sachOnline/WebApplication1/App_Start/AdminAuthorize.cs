using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Web.Routing;
namespace WebApplication1.App_Start
{
    public class AdminAuthorize : AuthorizeAttribute
    {
        QLBanSachEntities db = new QLBanSachEntities();
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            ADMIN adSession = (ADMIN)HttpContext.Current.Session["Admin"];
            if(adSession!=null)
            {
                return;
            }
            else
            {
                var retunUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectToRouteResult(new 
                    RouteValueDictionary( 
                    new { 
                        controller ="UserAdmin", 
                        action="DangNhap",
                        area="Admin",
                        retunUrl=retunUrl.ToString()
                    }));
            }
        }
    }
}