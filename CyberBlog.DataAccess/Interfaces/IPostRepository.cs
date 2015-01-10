using CyberBlog.BlogEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CyberBlog.DataAccess.Interfaces
{
	public interface IPostRepository
	{

		/// <summary>
		/// Return collection of posts based on serach expression and pagination parameters.
		/// </summary>
		/// <param name="pageNo">Page index</param>
		/// <param name="pageSize">Page size</param>
		/// <returns></returns>
		IEnumerable<Post> Query(Expression<Func<Post, bool>> filter,int pageNo,int pageSize);

		/// <summary>
		/// Return collection of posts based on serach expression.
		/// </summary>
		/// <returns></returns>
		IEnumerable<Post> Query(Expression<Func<Post, bool>> filter);
		
		/// <summary>
		/// Return all of posts.
		/// </summary>
		/// <returns></returns>
		IList<Post> Posts();

		/// <summary>
		/// Return collection of posts based on pagination parameters.
		/// </summary>
		/// <param name="pageNo">Page index</param>
		/// <param name="pageSize">Page size</param>
		/// <returns></returns>
		IEnumerable<Post> Posts(int pageNo,int pageSize);

		/// <summary>
		/// Return total numbers of all or published posts.
		/// </summary>
		/// <param name="isPublished">True to count only published posts</param>
		/// <returns></returns>
		int TotalPosts(bool isPublished);

		/// <summary>
		/// Return total no. of posts belongs to a particular category.
		/// </summary>
		/// <param name="categorySlug">Category's url slug</param>
		/// <returns></returns>
		int TotalPostsByCategory(string categorySlug);

		/// <summary>
		/// Return total no. of posts belongs to a particular tag.
		/// </summary>
		/// <param name="tagSlug">Tag's url slug</param>
		/// <returns></returns>
		int TotalPostsByTag(string tagSlug);

		/// <summary>
		/// Return total no. of posts that matches the search term.
		/// </summary>
		/// <param name="search">search term</param>
		/// <returns></returns>
		int TotalPostsBySearch(string term);

		/// <summary>
		/// Return post based on the published year, month and title slug.
		/// </summary>
		/// <param name="year">Published year</param>
		/// <param name="month">Published month</param>
		/// <param name="urlSlug">Post's url slug</param>
		/// <returns></returns>
		//Post Post(int year,int month,string urlSlug);

		///// <summary>
		///// Return post based on unique id. Used in admin.
		///// </summary>
		///// <param name="id">Post unique id</param>
		///// <returns></returns>
		//Post Post(int id);

		/// <summary>
		/// Return all the categories.
		/// </summary>
		/// <returns></returns>
		IEnumerable<Category> Categories();

		/// <summary>
		/// Return collection of posts belongs to a particular category.
		/// </summary>
		/// <param name="categoryUrlSlug">Category's url slug</param>
		/// <param name="pageNo">Page index</param>
		/// <param name="pageSize">Page size</param>
		/// <returns></returns>
		IEnumerable<Post> PostsByCategory(string categoryUrlSlug,int pageNo,int pageSize);

		/// <summary>
		/// Return all the tags.
		/// </summary>
		/// <returns></returns>
		IEnumerable<Tag> Tags();

		/// <summary>
		/// Return collection of posts belongs to a particular tag.
		/// </summary>
		/// <param name="tagUrlSlug">tag's url slug</param>
		/// <param name="pageNo">Page index</param>
		/// <param name="pageSize">Page size</param>
		/// <returns></returns>
		IEnumerable<Post> PostsByTag(string tagUrlSlug, int pageNo, int pageSize);

		/// <summary>
		/// Update an existing post.
		/// </summary>
		/// <param name="post"></param>
		bool SavePost(Post post);

		/// <summary>
		/// Delete the post permanently from database.
		/// </summary>
		/// <param name="id"></param>
		bool DeletePost(int id);

		/// <summary>
		/// Delete all relative tags based on post id
		/// </summary>
		/// <param name="id">post id</param>
		bool DeletePostTag(int id);

		/// <summary>
		/// add post id and tag id into PostTag
		/// </summary>
		/// <param name="postId">post id</param>
		/// <param name="tagId">tag id</param>
		bool AddPostTag(int postId, int tagId);

		/// <summary>
		/// add a new tag
		/// </summary>
		/// <param name="tagName">new tag name</param>
		/// <param name="tagSluge">new tag url slug</param>
		int NewTag(string tagName,string tagSlug);

		/// <summary>
		/// add a new post
		/// </summary>
		/// <param name="post"></param>
		/// <returns>new post id</returns>
		int AddPost(Post post);

		/// <summary>
		/// check url slug
		/// </summary>
		/// <param name="urlSlug"></param>
		/// <param name="urlType"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		bool IsDuplicateSlug(string urlSlug, string urlType,int? id);

		/// <summary>
		/// add a new category
		/// </summary>
		/// <param name="name"></param>
		/// <param name="urlSlug"></param>
		/// <returns>new category id</returns>
		int NewCategory(string name,string urlSlug);

		/// <summary>
		/// update a category
		/// </summary>
		/// <param name="name">category name</param>
		/// <param name="urlSlug">category slug</param>
		/// <param name="id">category id</param>
		/// <returns></returns>
		bool UpdateCategory(string name,string urlSlug,int id);

		/// <summary>
		/// delete a category by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		bool DeleteCategory(int id);

		/// <summary>
		/// delete a tag by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		bool DeleteTag(int id);


		/// <summary>
		/// update a tag
		/// </summary>
		/// <param name="name">tag name</param>
		/// <param name="urlSlug">tag url slug</param>
		/// <param name="id">tag id</param>
		/// <returns></returns>
		bool UpdateTag(string name, string urlSlug, int id);
	}
}
