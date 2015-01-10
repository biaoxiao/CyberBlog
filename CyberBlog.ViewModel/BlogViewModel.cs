using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyberBlog.BlogEntity;
using System.ComponentModel.DataAnnotations;

namespace CyberBlog.ViewModel
{
	public class PostViewModel
	{
		public int PostId { get; set; }
		[Required]
		public int CategoryId { get; set; }
		public string Category { get; set; }
		public string CategoryUrlSlug { get; set; }
		[Required]
		[StringLength(200)]
		public string Title { get; set; }
		[Required]
		public string ShortDesc { get; set; }
		[Required]
		public string FullDesc { get; set; }
		[Required]
		[StringLength(400)]
		public string UrlSlug { get; set; }
		public string PostedDate { get; set; }
		public string PostedDate_Year { get; set; }
		public string PostedDate_Month { get; set; }
		[Required]
		public string Author { get; set; }
		public bool Published { get; set; }
		public List<TagViewModel> Tags { get; set; }
	}

	public class TagViewModel{
		public int TagId{get;set;}
		[Required]
		[StringLength(50)]
		public string Tag{get;set;}
		public string UrlSlug{get;set;}
	}

	public class CategoryViewModel
	{
		public int CategoryId { get; set; }
		[Required]
		[StringLength(50)]
		public string Category { get; set; }
		public string UrlSlug { get; set; }
		public int Count { get; set; }
	}

	public class PostsListViewModel
	{
		public IEnumerable<Post> Posts { get; set; }
		public int TotalPosts { get; set; }
	}

	public class CategoriesTagsList
	{
		public List<CategoryViewModel> Categories { get; set; }
		public List<TagViewModel> Tags { get; set; }
	}

	public class LoginModel
	{
		[Required(ErrorMessage = "User name is required")]
		[Display(Name = "User name (*)")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[Display(Name = "Password (*)")]
		public string Password { get; set; }
		public string ReturnUrl { get; set; }
	}

}
