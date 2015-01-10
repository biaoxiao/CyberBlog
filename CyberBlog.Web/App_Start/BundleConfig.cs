using System.Web;
using System.Web.Optimization;

namespace CyberBlog.Web
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			//bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
			//			"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js",
					  "~/Scripts/respond.js"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrapvalidator").Include("~/Scripts/BootstrapValidator/js/bootstrapValidator.js"));

			bundles.Add(new ScriptBundle("~/bundles/admin-js").Include("~/Scripts/DataTables-1.10.4/js/jquery.dataTables.js").Include("~/Scripts/DataTables-1.10.4/js/dataTables.bootstrap.js").Include("~/Scripts/Summernote/summernote.js").Include("~/Scripts/bootstrap3-typeahead.min.js").Include("~/Scripts/Bootstrap-Tagsinput/bootstrap-tagsinput.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css", "~/Content/font-awesome.css"));

			bundles.Add(new StyleBundle("~/Content/validator-css").Include(
					   "~/Scripts/BootstrapValidator/css/bootstrapValidator.css"));

			bundles.Add(new StyleBundle("~/Content/admin-css").Include(
					   "~/Scripts/DataTables-1.10.4/css/dataTables.bootstrap.css").Include("~/Scripts/Summernote/summernote.css").Include("~/Scripts/Bootstrap-Tagsinput/bootstrap-tagsinput.css"));

			BundleTable.EnableOptimizations = true;
		}
	}
}
