using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CyberBlog.Web.Controllers;

namespace CyberBlog.Web
{
	public class MvcApplication : System.Web.HttpApplication
	{

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			GlobalFilters.Filters.Add(new HandleErrorAttribute());
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			//Server.ClearError();
			//Response.Redirect("~/Error");

			var httpContext = ((MvcApplication)sender).Context;
			var ex = Server.GetLastError();
			var status = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;

			// Is Ajax request? return json
			if (httpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{
				httpContext.ClearError();
				httpContext.Response.Clear();
				httpContext.Response.StatusCode = status;
				httpContext.Response.TrySkipIisCustomErrors = true;
				httpContext.Response.ContentType = "application/json";
				httpContext.Response.Write("{ success: false, message: \"Error occured in server.\" }");
				httpContext.Response.End();
			}
			else
			{
				var currentController = " ";
				var currentAction = " ";
				var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

				if (currentRouteData != null)
				{
					if (currentRouteData.Values["controller"] != null &&
						!String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
					{
						currentController = currentRouteData.Values["controller"].ToString();
					}

					if (currentRouteData.Values["action"] != null &&
						!String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
					{
						currentAction = currentRouteData.Values["action"].ToString();
					}
				}

				var controller = new ErrorController();
				var routeData = new RouteData();

				httpContext.ClearError();
				httpContext.Response.Clear();
				httpContext.Response.StatusCode = status;
				httpContext.Response.TrySkipIisCustomErrors = true;

				routeData.Values["controller"] = "Error";
				routeData.Values["action"] = "Error";

				controller.ViewData.Model = new HandleErrorInfo(ex, currentController, currentAction);
				((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
			}
		}

	}
}
