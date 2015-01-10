using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CyberBlog.Web
{
	//--If changed routing settings, please change relative parts in _Pager.cshtml
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Error",
				"Error",
				new { controller = "Error", action = "Error"}
			);

			routes.MapRoute(
				"Portfolio",
				"portfolio/{action}",
				new { controller = "Portfolio", action = "Portfolio" }
			);

			routes.MapRoute(
				"Blog",
				"blog/{year}/{month}/{title}",
				new { controller = "Post", action = "Post", year = UrlParameter.Optional, month = UrlParameter.Optional, title = UrlParameter.Optional }
			);

			routes.MapRoute(
				"Category",
				"category/{category}/{catPageNo}",
				new { controller = "Post", action = "Category", catPageNo = UrlParameter.Optional },
				constraints: new { catPageNo = @"^\d*$" }
			);

			routes.MapRoute(
				"Tag",
				"tag/{tag}/{tagPageNo}",
				new { controller = "Post", action = "Tag",tagPageNo=UrlParameter.Optional },
				constraints: new { tagPageNo = @"^\d*$" }
			);

			routes.MapRoute(
				"Search",
				"search/{term}/{searchPageNo}",
				new { controller = "Post", action = "Search", searchPageNo = UrlParameter.Optional },
				constraints: new { searchPageNo = @"^\d*$" }
			);

			routes.MapRoute(
				"About",
				"About",
				new { controller = "About", action = "About" }
			);

			routes.MapRoute(
				"Contact",
				"Contact",
				new { controller = "Contact", action = "Contact" }
			);

			routes.MapRoute(
				"Login",
				"Login",
				new { controller = "Admin", action = "Login" }
			 );

			routes.MapRoute(
			  "Logout",
			  "Logout",
			  new { controller = "Admin", action = "Logout" }
			);

			routes.MapRoute(
			  "Admin",
			  "Admin/{action}/{id}",
			  new { controller = "Admin", action = "List",id=UrlParameter.Optional}
			);

			routes.MapRoute(
				"Default",
				"{action}/{pageNo}",
				new { controller = "Post", action = "PostsList", pageNo = UrlParameter.Optional },
				constraints: new { pageNo = @"^\d*$" }
			);
			
		}
	}
}
