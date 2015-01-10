using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CyberBlog.DataAccess.Services;
using CyberBlog.ViewModel;
using System.Threading.Tasks;
using CyberBlog.Web.AuthProvider;

namespace CyberBlog.Web.Controllers
{
    public class AdminController : Controller
    {
		private PostService _postService;
		private IAuthProvider _authProvider;

		public AdminController()
		{
			_postService = new PostService();
			_authProvider = new AuthProvider.AuthProvider();
		}

		/// <summary>
		/// Return collection of tags and categories. Works for typeahead and categories drop down list.
		/// </summary>
		/// <returns></returns>
		[Authorize]
        public ActionResult List()
        {
			var tags = _postService.Tags();
			var categories = _postService.Categories();
			CategoriesTagsList list = new CategoriesTagsList();
			list.Categories = categories;
			list.Tags = tags;
			return View("List", list);
        }

		/// <summary>
		/// return collection of posts for data grid
		/// </summary>
		/// <param name="param"></param>
		/// <returns></returns>
		[HttpGet]
		public JsonResult Posts(DataTablePager param)
		{
			string Status = "";
			string Tags = "";
			int i = 1;
			List<string[]> dataList = new List<string[]>();
			IEnumerable<CyberBlog.BlogEntity.Post> filterdPosts;
			List<CyberBlog.BlogEntity.Post> allPosts;
			var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
			var sortDirection = Request["sSortDir_0"];
			allPosts = _postService.Posts();
			if (!string.IsNullOrEmpty(param.sSearch))
			{
				allPosts = allPosts.Where(x => x.Title.Contains(param.sSearch) || x.ShortDesc.Contains(param.sSearch) || x.FullDesc.Contains(param.sSearch) || x.Category.Name.Contains(param.sSearch)).ToList();
			}
			else
			{
				//allEntries = _service.GetAllEntries(BlogId).ToList(); ;
			}
			switch (sortColumnIndex)
			{
				case 1: //Sorted by title
					if (sortDirection == "asc")
					{
						allPosts = allPosts.OrderBy(x => x.Title).ToList();
					}
					else
					{
						allPosts = allPosts.OrderByDescending(x => x.Title).ToList();
					}
					break;
				case 2: //Sorted by category
					if (sortDirection == "asc")
					{
						allPosts = allPosts.OrderBy(x => x.Category.Name).ToList();
					}
					else
					{
						allPosts = allPosts.OrderByDescending(x => x.Category.Name).ToList();
					}
					break;
				case 5: //Sorted by date
					if (sortDirection == "asc")
					{
						allPosts = allPosts.OrderBy(x => x.PostedDate).ToList();
					}
					else
					{
						allPosts = allPosts.OrderByDescending(x => x.PostedDate).ToList();
					}
					break;

			}

			filterdPosts = allPosts;
			var displayEntries = filterdPosts.Skip(param.iDisplayStart).Take(param.iDisplayLength);
			foreach (var entry in displayEntries)
			{
				switch (entry.Published)
				{
					case true:
						Status = "Published";
						break;
					case false:
						Status = "Draft";
						break;
				}
				foreach (var tag in entry.Tags)
				{
					if (i == entry.Tags.Count)
					{
						Tags += tag.Name;
					}
					else
					{
						Tags += tag.Name + ", ";
					}
					i++;
				}
				dataList.Add(new[] { entry.Id.ToString(), entry.Title,entry.Category.Name, Tags, entry.Author, entry.PostedDate.ToString("dd/MM/yyyy"), Status });
				Tags = "";
				i = 1;
			}
			return Json(new
			{
				sEcho = param.sEcho,
				iTotalRecords = allPosts.Count,
				iTotalDisplayRecords = filterdPosts.ToList().Count,
				aaData = dataList
			},
				JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// return a post based on paticular id
		/// </summary>
		/// <param name="postId"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public JsonResult Post(int postId)
		{
			PostViewModel post = new PostViewModel();
			List<TagViewModel> tagList = new List<TagViewModel>();
			var result = _postService.Post(postId);
			if (result != null)
			{
				post.PostId = result.Id;
				post.Author = result.Author;
				post.Title = result.Title;
				post.ShortDesc = result.ShortDesc;
				post.FullDesc = result.FullDesc;
				post.Published = result.Published;
				post.Category = result.Category.Name;
				post.CategoryId = result.CategoryId;
				post.UrlSlug = result.UrlSlug;
				foreach (var item in result.Tags)
				{
					TagViewModel tag = new TagViewModel();
					tag.Tag = item.Name;
					tagList.Add(tag);
				}
				post.Tags = tagList;
			}
			return Json(post, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// save a modified post
		/// </summary>
		/// <param name="post"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize]
		public JsonResult Save(PostViewModel post)
		{
			bool isSaved = false;
			if (ModelState.IsValid)
			{
				if (post.Tags != null && post.Tags.Count > 0)
				{
					var tags = _postService.Tags();
					foreach (var tag in post.Tags)
					{
						var _result = tags.Where(x => x.Tag.ToLower() == tag.Tag.ToLower()).FirstOrDefault();
						if (_result != null) //exsiting tag
						{
							tag.TagId = _result.TagId;
						}
						else //new tag
						{
							tag.TagId = 0;
						}
					}
					isSaved = _postService.SavePost(post);
				}
				else
				{
					isSaved = _postService.SavePost(post);
				}
			}
			else
			{
				/*
				foreach (ModelState modelState in ViewData.ModelState.Values)
				{
					foreach (ModelError error in modelState.Errors)
					{
						var info = error.ErrorMessage;
					}
				}
				 */
				isSaved= false;
			}
			
			return Json(isSaved);
		}

		/// <summary>
		/// delete a post
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete]
		[Authorize]
		public JsonResult DeletePost(int id = 0)
		{
			if (id >0)
			{
				return Json(_postService.DeletePost(id));
			}
			else
			{
				return Json(false);
			}
		}

		/// <summary>
		/// show writing post view
		/// </summary>
		/// <returns></returns>
		[Authorize]
		public ActionResult Write()
		{
			var tags = _postService.Tags();
			var categories = _postService.Categories();
			CategoriesTagsList list = new CategoriesTagsList();
			list.Categories = categories;
			list.Tags = tags;
			return View("Write", list);
		}

		/// <summary>
		/// Add a new post.
		/// </summary>
		/// <param name="post"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize]
		public JsonResult AddPost(PostViewModel post)
		{
			bool isSaved = false;
			if (ModelState.IsValid)
			{
				if (post.Tags != null && post.Tags.Count > 0)
				{
					var tags = _postService.Tags();
					foreach (var tag in post.Tags)
					{
						var _result = tags.Where(x => x.Tag.ToLower() == tag.Tag.ToLower()).FirstOrDefault();
						if (_result != null) //exsiting tag
						{
							tag.TagId = _result.TagId;
						}
						else //new tag
						{
							tag.TagId = 0;
						}
					}
					isSaved = _postService.AddPost(post);
				}
				else
				{
					isSaved = _postService.AddPost(post);
				}
			}
			else
			{
				/*
				foreach (ModelState modelState in ViewData.ModelState.Values)
				{
					foreach (ModelError error in modelState.Errors)
					{
						var info = error.ErrorMessage;
					}
				}
				 */
				isSaved = false;
			}
			return Json(isSaved);
		}

		/// <summary>
		/// shows collection of categories
		/// </summary>
		/// <returns></returns>
		[Authorize]
		public ActionResult Category()
		{
			var categories = _postService.Categories();
			return View(categories);
		}

		/// <summary>
		/// Show collection of tags
		/// </summary>
		/// <returns></returns>
		[Authorize]
		public ActionResult Tag()
		{
			var tags = _postService.Tags();
			return View(tags);
		}

		/// <summary>
		/// create a new tag
		/// </summary>
		/// <param name="tag">new tag name</param>
		/// <returns>return new tag id</returns>
		[HttpPost]
		[Authorize]
		public int NewTag(string tag)
		{
			int tagId = 0;
			if (string.IsNullOrEmpty(tag))
			{
				return tagId;
			}
			else
			{
				tagId=_postService.NewTag(tag.ToLower());
				if(tagId>0)
				{
					return tagId;
				}
				else
				{
					return 0;
				}
			}
		}

		/// <summary>
		/// update a tag
		/// </summary>
		/// <param name="tag">tag name</param>
		/// <param name="id">tag id</param>
		/// <returns></returns>
		[HttpPut]
		[Authorize]
		public JsonResult UpdateTag(string tag,int tagId)
		{
			if (string.IsNullOrEmpty(tag) || tagId == 0)
			{
				return Json(false);
			}
			else
			{
				return Json(_postService.UpdateTag(tag,tag,tagId));
			}
		}

		/// <summary>
		/// delete a tag by id
		/// </summary>
		/// <param name="id">tag id</param>
		/// <returns></returns>
		[HttpDelete]
		[Authorize]
		public JsonResult DeleteTag(int tagId)
		{
			if (tagId == 0)
			{
				return Json(false);
			}
			else
			{
				return Json(_postService.DeleteTag(tagId));
			}
		}

		/// <summary>
		/// add a new category
		/// </summary>
		/// <param name="category"></param>
		/// <returns>the new category's id</returns>
		[HttpPost]
		[Authorize]
		public int NewCategory(string category)
		{
			if (string.IsNullOrEmpty(category))
			{
				return 0;
			}
			else
			{
				return _postService.NewCategory(category, category);
			}
		}

		/// <summary>
		/// update a category
		/// </summary>
		/// <param name="category">new name</param>
		/// <param name="categoryId">id</param>
		/// <returns></returns>
		[HttpPost]
		[Authorize]
		public JsonResult UpdateCategory(string category,int categoryId)
		{
			if (category == null)
			{
				return Json(false);
			}
			else
			{

				return Json(_postService.UpdateCategory(category,category,categoryId));
			}
		}

		/// <summary>
		/// delete a category by id
		/// </summary>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		[HttpDelete]
		[Authorize]
		public JsonResult DeleteCategory(int categoryId)
		{
			if (categoryId == 0)
			{
				return Json(false);
			}
			else
			{

				return Json(_postService.DeleteCategory(categoryId));
			}
		}

		/// <summary>
		/// Return the login page
		/// </summary>
		/// <param name="returnUrl"></param>
		/// <returns></returns>
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			if (_authProvider.IsLoggedIn)
			{
				return Redirect(returnUrl);
			}
			else
			{
				ViewBag.ReturnUrl = returnUrl;
				return View();
			}
		}

		/// <summary>
		/// Execute the authentication
		/// </summary>
		/// <param name="login"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginModel login)
		{
			if (ModelState.IsValid && _authProvider.Login(login.UserName, login.Password))
			{
				if (login.ReturnUrl == null)
				{
					return RedirectToAction("List");
				}
				else
				{
					return Redirect(login.ReturnUrl);
				}
				
			}
			else
			{
				ModelState.AddModelError("Login", "The user name or password provided is incorrect.");
				return View(login);
			}
		}


		public ActionResult Logout()
		{
			_authProvider.Logout();
			return RedirectToAction("Login");
		}

    }
}