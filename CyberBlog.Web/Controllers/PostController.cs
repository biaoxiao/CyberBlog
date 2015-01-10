using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CyberBlog.DataAccess.Services;
using CyberBlog.BlogHelper;

namespace CyberBlog.Web.Controllers
{
	public class PostController : Controller
	{
		
		private  PostService _postService;
		private int pageSize;

		public PostController()
		{
			pageSize = Helper.GetAppSettings().PageSize;
			_postService = new PostService();
		}

		/// <summary>
		/// Return the list page with latest posts.
		/// </summary>
		/// <param name="pageNo">pagination number</param>
		/// <returns></returns>
		public ActionResult PostsList(int pageNo = 1)
		{
			ViewBag.Title = "";
			var viewModel = _postService.Posts(pageNo,pageSize);
			return View("PostsList",viewModel);
		}

		/// <summary>
		/// Return a particular post based on the puslished year, month and url slug.
		/// </summary>
		/// <param name="year">Published year</param>
		/// <param name="month">Published month</param>
		/// <param name="title">Url slug</param>
		/// <returns></returns>
		public ActionResult Post(string year,string month,string title)
		{
			var viewModel = _postService.Post(year,month,title);
			ViewBag.Title = viewModel.Title;
			return View(viewModel);
		}

		/// <summary>
		/// Return collection of categories.
		/// </summary>
		/// <returns></returns>
		
		public ActionResult Categories()
		{
			var viewModel = _postService.Categories();
			return PartialView("_Categories",viewModel);
		}

		/// <summary>
		/// Return collection of posts belongs to a particular category.
		/// </summary>
		/// <param name="category">Category's url slug</param>
		/// <param name="catPageNo">Page index</param>
		/// <returns></returns>
		public ActionResult Category(string category, int catPageNo= 1)
		{
			CyberBlog.ViewModel.PostsListViewModel viewModel = _postService.PostsByCategory(category, catPageNo, pageSize);
			foreach (var item in viewModel.Posts)
			{
				ViewBag.Title = item.Category.Name + " Archives - ";
				ViewBag.SubTitle = "Categories - " + item.Category.Name;
				break;
			}
			return View("PostsList",viewModel);
		}

		/// <summary>
		/// Return collection of tags.
		/// </summary>
		/// <returns></returns>
		
		public ActionResult Tags()
		{
			var viewModel = _postService.ValidTags();
			return PartialView("_Tags",viewModel);
		}
		

		/// <summary>
		/// Return collection of posts belongs to a particular tag.
		/// </summary>
		/// <param name="tag">Tag's url slug</param>
		/// <param name="tagPageNo">Page index</param>
		/// <returns></returns>
		public ActionResult Tag(string tag, int tagPageNo = 1)
		{
			string _tag;
			ViewBag.Title = null;
			var viewModel = _postService.PostsByTag(tag, tagPageNo, pageSize);
			foreach (var item in viewModel.Posts)
			{
				_tag = item.Tags.Where(x => x.UrlSlug.ToLower() == tag.ToLower()).Single().Name;
				ViewBag.Title =_tag+" Archives - ";
				ViewBag.SubTitle = "Tags - " +_tag;
				break;
			}
			return View("PostsList", viewModel);
		}

		/// <summary>
		/// Return the posts that matches the search term.
		/// </summary>
		/// <param name="term">search text</param>
		/// <param name="searchPageNo">Pagination number</param>
		/// <returns></returns>
		public ViewResult Search(string term, int searchPageNo = 1)
		{
			var viewModel = _postService.SearchPosts(term, searchPageNo, pageSize);
			ViewBag.Title = term + " - " + "Blog Search";
			ViewBag.SubTitle = "Search By - " + term;
			return View("PostsList",viewModel);
		}

	}
}