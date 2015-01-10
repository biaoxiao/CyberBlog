namespace CyberBlog.BlogEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Post")]
    public partial class Post
    {
        public Post()
        {
            Tags = new HashSet<Tag>();
        }

        public int Id { get; set; }

        public int CategoryId { get; set; }

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

        public bool Published { get; set; }

        public DateTime PostedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

		//public int UserId { get; set; }

		//public virtual BlogUser BlogUser { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}
