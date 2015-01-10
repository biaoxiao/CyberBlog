using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Configuration;

namespace CyberBlog.BlogHelper
{
	public static class Helper
	{

		public static string GenerateSlug(this string phrase)
		{
			string str = phrase.RemoveAccent().ToLower();
			// invalid chars           
			str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
			// convert multiple spaces into one space   
			str = Regex.Replace(str, @"\s+", " ").Trim();
			// cut and trim 
			str = str.Substring(0, str.Length <= 400 ? str.Length : 400).Trim();
			str = Regex.Replace(str, @"\s", "-"); // hyphens   
			return str;
		}

		static string RemoveAccent(this string txt)
		{
			byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
			return System.Text.Encoding.ASCII.GetString(bytes);
		}

		public static AppSetting GetAppSettings()
		{
			AppSetting appSetting = new AppSetting();
			appSetting.Author = ConfigurationManager.AppSettings["Author"];
			appSetting.BlogName = ConfigurationManager.AppSettings["BlogName"];
			appSetting.SubBlogName = ConfigurationManager.AppSettings["SubBlogName"];
			appSetting.PageSize = ConfigurationManager.AppSettings["PageSize"] != null || Convert.ToInt16(ConfigurationManager.AppSettings["PageSize"])>0 ? Convert.ToInt16(ConfigurationManager.AppSettings["PageSize"]) : 10;
			appSetting.ShortName = ConfigurationManager.AppSettings["ShortName"];
			return appSetting;
		}

		public class AppSetting
		{
			public string BlogName { get;set; }
			public string SubBlogName { get; set; }
			public string Author { get; set; }
			public int PageSize { get; set; }
			public string ShortName { get; set; }
		}

	}
}
