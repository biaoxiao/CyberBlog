using CyberBlog.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CyberBlog.BlogEntity;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CyberBlog.DataAccess.Repositories
{
	public class PostRepository:IPostRepository
	{
		private Entity _dbContext;

		public PostRepository()
		{
			_dbContext = new Entity();
		}

		public void Dispose()
		{
			if (_dbContext != null)
			{
				_dbContext.Dispose();
			}
		}


		/// <summary>
		/// Return collection of posts based on serach expression and pagination parameters.
		/// </summary>
		/// <param name="pageNo">Page index</param>
		/// <param name="pageSize">Page size</param>
		/// <returns></returns>
		public IEnumerable<Post> Query(Expression<Func<Post, bool>> filter,int pageNo,int pageSize)
		{
			try
			{
				return _dbContext.Posts.Where(filter).OrderByDescending(x => x.PostedDate).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Return collection of posts based on serach expression.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Post> Query(Expression<Func<Post, bool>> filter)
		{
			try
			{
				return _dbContext.Posts.Where(filter).OrderByDescending(x => x.PostedDate).ToList();
			}
			catch
			{
				return null;
			}
		}


		/// <summary>
		/// Return all of posts for admin.
		/// <returns></returns>
		public IList<Post> Posts()
		{
			var posts =_dbContext.Posts.OrderByDescending(x=>x.PostedDate).ToList();
			return posts;
		}

		/// <summary>
		/// Return collection of posts based on pagination parameters.
		/// </summary>
		/// <param name="pageNo">Page index</param>
		/// <param name="pageSize">Page size</param>
		/// <returns></returns>
		public IEnumerable<Post> Posts(int pageNo, int pageSize)
		{
			var posts = _dbContext.Posts.OrderByDescending(x=>x.PostedDate).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
			return posts;
		}

		/// <summary>
		/// Return total numbers of all or published posts.
		/// </summary>
		/// <param name="isPublished">True to count only published posts</param>
		/// <returns></returns>
		public int TotalPosts(bool isPublished)
		{
			return _dbContext.Posts.Where(x => x.Published == isPublished || isPublished).Count();
		}

		/// <summary>
		/// Return total no. of posts belongs to a particular tag.
		/// </summary>
		/// <param name="tagSlug">Tag's url slug</param>
		/// <returns></returns>
		public int TotalPostsByTag(string tagSlug)
		{
			return _dbContext.Posts.Where(x => x.Published == true && x.Tags.Any(t => t.UrlSlug.Equals(tagSlug, StringComparison.OrdinalIgnoreCase))).Count();
		}

		/// <summary>
		/// Return total no. of posts belongs to a particular category.
		/// </summary>
		/// <param name="categorySlug">Category's url slug</param>
		/// <returns></returns>
		public int TotalPostsByCategory(string categorySlug)
		{
			return _dbContext.Posts.Where(x => x.Published == true && x.Category.UrlSlug==categorySlug).Count();
		}

		/// <summary>
		/// Return total no. of posts that matches the search term.
		/// </summary>
		/// <param name="search">search term</param>
		/// <returns></returns>
		public int TotalPostsBySearch(string term)
		{
			return _dbContext.Posts.Where(x => x.Published == true &&(x.ShortDesc.Contains(term) || x.Author.Contains(term) || x.Title.Contains(term) || x.Tags.Any(t=>t.Name.Contains(term)) || x.Category.Name.Contains(term))).Count();
		}


		/// <summary>
		/// Return all the categories.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Category> Categories()
		{
			var categories = _dbContext.Categories.OrderBy(x=>x.Name).ToList();
			return categories;
		}

		/// <summary>
		/// Return collection of posts belongs to a particular category.
		/// </summary>
		/// <param name="categoryUrlSlug">Category's url slug</param>
		/// <param name="pageNo">Page index</param>
		/// <param name="pageSize">Page size</param>
		/// <returns></returns>
		public IEnumerable<Post> PostsByCategory(string categoryUrlSlug, int pageNo, int pageSize)
		{
			var posts = _dbContext.Posts.Where(x => x.Published == true && x.Category.UrlSlug == categoryUrlSlug).OrderByDescending(x => x.PostedDate).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
			return posts;
		}

		/// <summary>
		/// Return all the tags.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Tag> Tags()
		{
			return _dbContext.Tags.Where(x=>x.Name.Length>0 && x.UrlSlug.Length>0).OrderBy(x=>x.Name).ToList();
		}

		/// <summary>
		/// Return collection of posts belongs to a particular tag.
		/// </summary>
		/// <param name="tagUrlSlug">tag's url slug</param>
		/// <param name="pageNo">Page index</param>
		/// <param name="pageSize">Page size</param>
		/// <returns></returns>
		public IEnumerable<Post> PostsByTag(string tagUrlSlug, int pageNo, int pageSize)
		{
			var posts = _dbContext.Posts.Where(x => x.Published == true && x.Tags.Any(t => t.UrlSlug.Equals(tagUrlSlug, StringComparison.OrdinalIgnoreCase))).OrderByDescending(x => x.PostedDate).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
			return posts;
		}

		/// <summary>
		/// Update an existing post.
		/// </summary>
		/// <param name="post"></param>
		public bool SavePost(Post post)
		{
			try
			{
				Post _post;
				_post =_dbContext.Posts.Where(x => x.Id == post.Id).FirstOrDefault<Post>();
			if (_post != null)
			{
				_post.Author = post.Author;
				_post.Published = post.Published;
				_post.CategoryId = post.CategoryId;
				_post.FullDesc = post.FullDesc;
				_post.ShortDesc = post.ShortDesc;
				_post.Title = post.Title;
				_post.UrlSlug = post.UrlSlug;
				_post.ModifiedDate = post.ModifiedDate;
				_dbContext.Entry(_post).State = System.Data.Entity.EntityState.Modified;
				_dbContext.SaveChanges();
				return true;
			}
			else
			{
				return false;
			}
			}
			catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
			{
				Exception raise = dbEx;
				foreach (var validationErrors in dbEx.EntityValidationErrors)
				{
					foreach (var validationError in validationErrors.ValidationErrors)
					{
						string message = string.Format("{0}:{1}",
							validationErrors.Entry.Entity.ToString(),
							validationError.ErrorMessage);
						// raise a new exception nesting  
						// the current instance as InnerException  
						raise = new InvalidOperationException(message, raise);
					}
				}
				throw raise;
			}
		}

		/// <summary>
		/// Delete the post permanently from database.
		/// </summary>
		/// <param name="id"></param>
		public bool DeletePost(int id)
		{
			var post = _dbContext.Posts.Where(x => x.Id == id).FirstOrDefault<Post>();
			DeletePostTag(id);
			_dbContext.Entry(post).State = System.Data.Entity.EntityState.Deleted;
			if (_dbContext.SaveChanges() == 1)
			{
				return true;
			}
			else
			{
				return false;
			}
			
		}

		/// <summary>
		/// Delete all relative tags based on post id
		/// </summary>
		/// <param name="id">post id</param>
		public bool DeletePostTag(int id)
		{
			var isDeleted = false;
			Post post = _dbContext.Posts.Where(x => x.Id == id).FirstOrDefault<Post>();
			foreach (var tag in post.Tags.ToList())
			{
				post.Tags.Remove(tag);
				
			}
			try
			{
				_dbContext.SaveChanges();
				isDeleted = true;
			}
			catch
			{
				isDeleted = false;
			}
			return isDeleted;
		}

		/// <summary>
		/// add post id and tag id into PostTag
		/// </summary>
		/// <param name="postId">post id</param>
		/// <param name="tagId">tag id</param>
		public bool AddPostTag(int postId, int tagId)
		{
			try
			{
				Post post = _dbContext.Posts.Where(x => x.Id == postId).FirstOrDefault<Post>();
				Tag tag = _dbContext.Tags.Where(x => x.Id == tagId).FirstOrDefault<Tag>();
				post.Tags.Add(tag);
				_dbContext.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// add a new tag
		/// </summary>
		/// <param name="tagName">new tag name</param>
		/// <param name="tagSluge">new tag url slug</param>
		public int NewTag(string tagName,string tagSlug)
		{
			try
			{
				Tag tag = new Tag();
				tag.Name = tagName;
				tag.UrlSlug = tagSlug;
				_dbContext.Tags.Add(tag);
				_dbContext.SaveChanges();
				return tag.Id;
			}
			catch
			{
				return 0;
			}
		}

		/// <summary>
		/// add a new post
		/// </summary>
		/// <param name="post"></param>
		/// <returns>new post id</returns>
		public int AddPost(Post post)
		{
			try
			{
				_dbContext.Posts.Add(post);
				_dbContext.SaveChanges();
				return post.Id;
			}
			catch (Exception ex)
			{
				return 0;
			}
		}

		/// <summary>
		/// check url slug
		/// </summary>
		/// <param name="urlSlug">url slug</param>
		/// <param name="urlType">which type of url that it belong to</param>
		/// <returns></returns>
		public bool IsDuplicateSlug(string urlSlug, string urlType,int? id)
		{
			urlSlug = urlSlug.ToLower();
			bool result = false;
			switch (urlType){
				case"post":
					if (id != null)
					{
						result = _dbContext.Posts.Where(x => x.UrlSlug.ToLower() == urlSlug && x.Id!=id).Count() > 0 ? true : false;
					}
					else
					{
						result = _dbContext.Posts.Where(x => x.UrlSlug.ToLower() == urlSlug).Count() > 0 ? true : false;
					}
					break;
				case"category":
					if (id != null)
					{
						result = _dbContext.Categories.Where(x => x.UrlSlug.ToLower() == urlSlug && x.Id!=id).Count() > 0 ? true : false;
					}
					else
					{
						result = _dbContext.Categories.Where(x => x.UrlSlug.ToLower() == urlSlug).Count() > 0 ? true : false;
					}
					break;
				case"tag":
					if (id != null)
					{
						result = _dbContext.Tags.Where(x => x.UrlSlug.ToLower() == urlSlug && x.Id!=id).Count() > 0 ? true : false;
					}
					else
					{
						result = _dbContext.Tags.Where(x => x.UrlSlug.ToLower() == urlSlug).Count() > 0 ? true : false;
					}
					break;
			}
			return result;
		}


		/// <summary>
		/// add a new category
		/// </summary>
		/// <param name="name"></param>
		/// <param name="urlSlug"></param>
		/// <returns>new category id</returns>
		public int NewCategory(string name, string urlSlug)
		{
			try
			{
				Category category = new Category();
				category.Name = name;
				category.UrlSlug = urlSlug;
				_dbContext.Categories.Add(category);
				_dbContext.SaveChanges();
				return category.Id;
			}
			catch (Exception ex)
			{
				return 0;
			}
			
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
			Category category = _dbContext.Categories.Where(x => x.Id == id).SingleOrDefault<Category>();
			if (category != null)
			{
				category.Name = name;
				category.UrlSlug = urlSlug;
				_dbContext.Entry(category).State = System.Data.Entity.EntityState.Modified;
				_dbContext.SaveChanges();
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// delete a category by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool DeleteCategory(int id)
		{
			Category category = _dbContext.Categories.Where(x => x.Id == id).SingleOrDefault<Category>();
			try
			{
				if (category != null)
				{
					_dbContext.Entry(category).State = System.Data.Entity.EntityState.Deleted;
					_dbContext.SaveChanges();
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
			
		}


		/// <summary>
		/// delete a tag by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool DeleteTag(int id)
		{
			Tag tag = _dbContext.Tags.Where(x => x.Id == id).SingleOrDefault<Tag>();
			try
			{
				if (tag != null)
				{
					_dbContext.Entry(tag).State = System.Data.Entity.EntityState.Deleted;
					_dbContext.SaveChanges();
					return true;
				}
				else
				{
					return false;
				}
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// update a tag
		/// </summary>
		/// <param name="name">tag name</param>
		/// <param name="urlSlug">tag url slug</param>
		/// <param name="id">tag id</param>
		/// <returns></returns>
		public bool UpdateTag(string name, string urlSlug, int id)
		{
			Tag tag = _dbContext.Tags.Where(x => x.Id == id).SingleOrDefault<Tag>();
			try
			{
				if (tag != null)
				{
					tag.Name = name;
					tag.UrlSlug = urlSlug;
					_dbContext.Entry(tag).State = System.Data.Entity.EntityState.Modified;
					_dbContext.SaveChanges();
					return true;
				}
				else
				{
					return false;
				}
			}
			catch
			{
				return false;
			}
		}
	}
}
