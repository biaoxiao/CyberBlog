using System.Net;
using System.Web.Mvc;

namespace CyberBlog.Web.Controllers
{
    public class ErrorController : Controller
    {
		/// <summary>
		/// Shows a generic error message.
		/// </summary>
		/// <returns>A view showing a generic error message.</returns>
		public virtual ActionResult Error()
		{
			return this.View("Error");
		}

		/// <summary>
		/// Shows a 404 error message.
		/// </summary>
		/// <returns>A view showing a 404 error message.</returns>
		public virtual ActionResult NotFound()
		{
			Response.StatusCode = (int)HttpStatusCode.NotFound;

			// Response.TrySkipIisCustomErrors = true;
			return this.View();
		}
    }
}