using System;
using System.Collections.Generic;
using System.Linq;
using CyberBlog.DataAccess.Interfaces;
using CyberBlog.ViewModel;
using CyberBlog.DataAccess.Repositories;
using CyberBlog.BlogEntity;
using System.Threading.Tasks;
using CyberBlog.BlogHelper;

namespace CyberBlog.DataAccess.Services
{
	public class PostService
	{
		private IPostRepository _postRepository;

		public PostService()
		{
			this._postRepository = new PostRepository();
		}

		public PostService(IPostRepository postRepository)
		{
			this._postRepository = postRepository;
		}

		/// <summary>
		/// Return all of posts based on pagination parameters.
		/// </summary>
		/// <param name="pageNo">Page index</param>
		/// <param name="pageSize">Page size</param>
		/// <returns></returns>
		public PostsListViewModel Posts(int pageNo, int pageSize)
		{
			PostsListViewModel postsList = new PostsListViewModel();
			postsList.Posts = _postRepository.Posts(pageNo, pageSize);
			postsList.TotalPosts = _postRepository.TotalPosts(true);
			return postsList;
		}

		/// <summary>
		/// Return all of posts for admin.
		/// </summary>
		/// <returns></returns>
		public List<Post> Posts()
		{
			return _postRepository.Posts().ToList();
		}

		/// <summary>
		/// Return post based on the published year, month and title slug.
		/// </summary>
		/// <param name="year">Published year</param>
		/// <param name="month">Published month</param>
		/// <param name="title">Post's url slug</param>
		/// <returns></returns>
		public PostViewModel Post(string year, string month, string urlSlug)
		{
			int _month = Convert.ToInt16(month);
			PostViewModel postViewModel = new PostViewModel();
			var result = _postRepository.Query(x => x.Published == true && x.PostedDate.Year.ToString() == year && x.PostedDate.Month == _month && x.UrlSlug == urlSlug).FirstOrDefault();
			if (result != null)
			{
				postViewModel.Title = result.Title;
				postViewModel.PostId = result.Id;
				postViewModel.Category = result.Category.Name;
				postViewModel.CategoryUrlSlug = result.Category.UrlSlug;
				postViewModel.UrlSlug = result.UrlSlug;
				postViewModel.Author = result.Author;
				postViewModel.ShortDesc = result.ShortDesc;
				postViewModel.PostedDate = result.PostedDate.ToString("MMMM") + " " + result.PostedDate.ToString("dd") + ", " + result.PostedDate.ToString("yyyy");
				postViewModel.PostedDate_Year = result.PostedDate.ToString("yyyy");
				postViewModel.FullDesc = result.FullDesc;
				List<TagViewModel> tagsList = new List<TagViewModel>();
				foreach (var item in result.Tags)
				{
					TagViewModel tagViewModel = new TagViewModel();
					tagViewModel.TagId = item.Id;
					tagViewModel.Tag = item.Name;
					tagViewModel.UrlSlug = item.UrlSlug;
					tagsList.Add(tagViewModel);
				}
				postViewModel.Tags = tagsList;
				postViewModel.PostedDate_Month = result.PostedDate.ToString("MM");
			}
			return postViewModel;
		}

		/// <summary>
		/// Return post based on the post Id.
		/// </summary>
		/// <param name="title">Post's url slug</param>
		/// <returns></returns>
		public Post Post(int Id)
		{
			return _postRepository.Query(x => x.Id == Id).FirstOrDefault();
		}

		/// <summary>
		/// Return all the categories.
		/// </summary>
		/// <returns></returns>
		public List<CategoryViewModel> Categories()
		{
			List<CategoryViewModel> categoriesList = new List<CategoryViewModel>();
			var result = _postRepository.Categories();
			foreach (var item in result)
			{
				CategoryViewModel categoryViewModel = new CategoryViewModel();
				categoryViewModel.CategoryId = item.Id;
				categoryViewModel.Category = item.Name;
				categoryViewModel.UrlSlug = item.UrlSlug;
				categoryViewModel.Count = item.Posts.Count;
				categoriesList.Add(categoryViewModel);
			}
			return categoriesList;
		}

		/// <summary>
		/// Return collection of posts belongs to a particular category.
		/// </summary>
		/// <param name="categoryUrlSlug">Category's url slug</param>
		/// <param name="pageNo">Page index</param>
		/// <param name="pageSize">Page size</param>
		/// <returns></returns>
		public PostsListViewModel PostsByCategory(string categoryUrlSlug,int pageNo,int pageSize)
		{
			PostsListViewModel postsList = new PostsListViewModel();
			postsList.Posts = _postRepository.PostsByCategory(categoryUrlSlug, pageNo, pageSize);
			postsList.TotalPosts = _postRepository.TotalPostsByCategory(categoryUrlSlug);
			return postsList;
		}

		/// <summary>
		/// Return collection of tags.
		/// </summary>
		/// <returns></returns>
		public List<TagViewModel> Tags()
		{
			List<TagViewModel> tagsList = new List<TagViewModel>();
			var result = _postRepository.Tags().Select(x => new { x.Id, x.Name, x.UrlSlug });
			foreach (var item in result)
			{
				TagViewModel tagViewModel = new TagViewModel();
				tagViewModel.TagId = item.Id;
				tagViewModel.Tag = item.Name;
				tagViewModel.UrlSlug = item.UrlSlug;
				tagsList.Add(tagViewModel);
			}
			return tagsList;
		}

		/// <summary>
		/// Return collection of tags have been assigned to posts
		/// </summary>
		/// <returns></returns>
		public List<TagViewModel> ValidTags()
		{
			List<TagViewModel> tagsList = new List<TagViewModel>();
			var result = _postRepository.Tags().Where(x => x.Posts.Count > 0).Select(x => new {x.Id,x.Name,x.UrlSlug });
			foreach (var item in result)
			{
				TagViewModel tagViewModel = new TagViewModel();
				tagViewModel.TagId = item.Id;
				tagViewModel.Tag = item.Name;
				tagViewModel.UrlSlug = item.UrlSlug;
				tagsList.Add(tagViewModel);
			}
			return tagsList;
		}


		/// <summary>
		/// Return collection of posts belongs to a particular tag.
		/// </summary>
		/// <param name="tagUrlSlug">tag's url slug</param>
		/// <param name="pageNo">Page index</param>
		/// <param name="pageSize">Page size</param>
		/// <returns></returns>
		public PostsListViewModel PostsByTag(string tagUrlSlug, int pageNo,int pageSize)
		{
			PostsListViewModel postsList = new PostsListViewModel();
			postsList.Posts = _postRepository.PostsByTag(tagUrlSlug, pageNo, pageSize);
			postsList.TotalPosts = _postRepository.TotalPostsByTag(tagUrlSlug);
			return postsList;
		}

		/// <summary>
		/// Return the posts that matches the search text.
		/// </summary>
		/// <param name="term">search text</param>
		/// <param name="pageNo">Page index</param>
		/// <param name="pageSize">Page size</param>
		/// <returns></returns>
		public PostsListViewModel SearchPosts(string term, int pageNo, int pageSize)
		{
			PostsListViewModel postsList = new PostsListViewModel();
			postsList.Posts = _postRepository.Query(x => x.Published == true && (x.ShortDesc.Contains(term) || x.Author.Contains(term) || x.Title.Contains(term) || x.Tags.Any(t => t.Name.Contains(term))), pageNo, pageSize);
			postsList.TotalPosts =_postRepository.TotalPostsBySearch(term);
			return postsList;
		}

		/// <summary>
		/// Update an existing post.
		/// </summary>
		/// <param name="post"></param>
		public bool SavePost(PostViewModel post)
		{
			bool isSaved = false;
			Post _post = new Post();
			_post.Id = post.PostId;
			_post.Author = post.Author;
			_post.CategoryId = post.CategoryId;
			_post.FullDesc = post.FullDesc;
			_post.ShortDesc = post.ShortDesc;
			_post.Title = post.Title;
			_post.Published = post.Published;
			_post.UrlSlug = Helper.GenerateSlug(post.UrlSlug.ToLower());
			_post.ModifiedDate = DateTime.Now;
			isSaved=_postRepository.SavePost(_post);
			if (post.Tags!=null && post.Tags.Count > 0) // the post assigned with tags
			{
				if (_postRepository.DeletePostTag(post.PostId))
				{
					foreach (var tag in post.Tags)
					{
						if (tag.TagId > 0) //exsiting tag
						{
							_postRepository.AddPostTag(post.PostId, tag.TagId);
						}
						else //new tag
						{
							int tagId = _postRepository.NewTag(tag.Tag.ToLower(),Helper.GenerateSlug(tag.Tag.ToLower()));
							_postRepository.AddPostTag(post.PostId,tagId);
						}
					}
				}
				
			}
			else //takea action when the post's tags have been removed by the user
			{
				isSaved = _postRepository.DeletePostTag(post.PostId);
			}
			return isSaved;
		}

		/// <summary>
		/// delete a post by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool DeletePost(int id)
		{
			return _postRepository.DeletePost(id);
		}

		/// <summary>
		/// add a new post
		/// </summary>
		/// <param name="post"></param>
		/// <returns></returns>
		public bool AddPost(PostViewModel post)
		{
			bool isSaved = false;
			Post _post = new Post();
			_post.Id = post.PostId;
			_post.Author = post.Author;
			_post.CategoryId = post.CategoryId;
			_post.FullDesc = post.FullDesc;
			_post.ShortDesc = post.ShortDesc;
			_post.Title = post.Title;
			_post.Published = post.Published;
			_post.PostedDate = DateTime.Now;
			_post.UrlSlug = Helper.GenerateSlug(post.UrlSlug.ToLower());
			if (_postRepository.IsDuplicateSlug(_post.UrlSlug,"post",null))
			{
				_post.UrlSlug += "-"+DateTime.Now.ToString("yyyyMMddmmss");
			}
			int postId = _postRepository.AddPost(_post);
			if (post.Tags != null && post.Tags.Count > 0) // the post assigned with tags
			{
				foreach (var tag in post.Tags)
				{
					if (tag.TagId > 0) //exsiting tag
					{
						isSaved=_postRepository.AddPostTag(postId, tag.TagId);
					}
					else //new tag
					{
						int tagId = _postRepository.NewTag(tag.Tag.ToLower(), Helper.GenerateSlug(tag.Tag.ToLower()));
						isSaved = _postRepository.AddPostTag(postId, tagId);
					}
				}
			}
			else if(postId>0) //the post without assigned with tags
			{
				isSaved = true;
			}
			return postId>0&&isSaved?true:false;
		}

		/// <summary>
		/// add a new category
		/// </summary>
		/// <param name="name"></param>
		/// <param name="urlSlug"></param>
		/// <returns>return new category id</returns>
		public int NewCategory(string name,string urlSlug)
		{
			string _urlSlug = Helper.GenerateSlug(urlSlug.ToLower());
			if (_postRepository.IsDuplicateSlug(_urlSlug,"category",null))
			{
				_urlSlug += "-" + DateTime.Now.ToString("yyyyMMddmmss");
			}
			return _postRepository.NewCategory(name,_urlSlug);
		}

		/// <summary>
		/// update a category
		/// </summary>
		/// <param name="name">category name</param>
		/// <param name="urlSlug">category slug</param>
		/// <param name="id">category id</param>
		/// <returns></returns>
		public bool UpdateCategory(string name, string urlSlug, int id)
		{
			string _urlSlug = Helper.GenerateSlug(urlSlug.ToLower());
			if (_postRepository.IsDuplicateSlug(_urlSlug, "category",id))
			{
				_urlSlug += "-" + DateTime.Now.ToString("yyyyMMddmmss");
			}
			return _postRepository.UpdateCategory(name,_urlSlug,id);
		}

		/// <summary>
		/// delete a category by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool DeleteCategory(int id)
		{
			var result = _postRepository.Query(x => x.CategoryId == id).Select(x=> new{x.Id}).FirstOrDefault();
			if (result !=null)
			{
				return false;
			}
			else
			{
				return _postRepository.DeleteCategory(id);
			}
		}

		/// <summary>
		/// add a new tag
		/// </summary>
		/// <param name="tagName">new tag name</param>
		public int NewTag(string tagName)
		{
			return _postRepository.NewTag(tagName, Helper.GenerateSlug(tagName.ToLower()));
		}

		/// <summary>
		/// update a tag
		/// </summary>
		/// <param name="tagName">tag name</param>
		/// <param name="urlSlug">tag url slug</param>
		/// <param name="tagId">tag id</param>
		/// <returns></returns>
		public bool UpdateTag(string tagName,string urlSlug,int tagId)
		{
			string _urlSlug = Helper.GenerateSlug(urlSlug.ToLower());
			if (_postRepository.IsDuplicateSlug(_urlSlug, "tag",tagId))
			{
				_urlSlug += "-" + DateTime.Now.ToString("yyyyMMddmmss");
			}
			return _postRepository.UpdateTag(tagName.ToLower(), _urlSlug, tagId);
		}


		public bool DeleteTag(int id)
		{
			var result = _postRepository.Query(x => x.Tags.Any(t => t.Id == id)).Select(x => new {x.Id}).FirstOrDefault();
			if (result == null)
			{
				return _postRepository.DeleteTag(id);
			}
			else
			{
				return false;
			}

		}
	}
}
