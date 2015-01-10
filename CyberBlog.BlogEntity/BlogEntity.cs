namespace CyberBlog.BlogEntity
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class Entity : DbContext
	{
		public Entity()
			: base("name=BlogEntity")
		{
			//this.Configuration.LazyLoadingEnabled = false;
		}

		//public virtual DbSet<BlogUser> BlogUsers { get; set; }
		public virtual DbSet<Category> Categories { get; set; }
		public virtual DbSet<Post> Posts { get; set; }
		public virtual DbSet<Tag> Tags { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			//modelBuilder.Entity<BlogUser>()
			//	.Property(e => e.UserName)
			//	.IsUnicode(false);

			//modelBuilder.Entity<BlogUser>()
			//	.Property(e => e.Password)
			//	.IsUnicode(false);

			//modelBuilder.Entity<BlogUser>()
			//	.HasMany(e => e.Posts)
			//	.WithRequired(e => e.BlogUser)
			//	.HasForeignKey(e => e.UserId)
			//	.WillCascadeOnDelete(false);

			modelBuilder.Entity<Category>()
				.HasMany(e => e.Posts)
				.WithRequired(e => e.Category)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Post>()
				.HasMany(e => e.Tags)
				.WithMany(e => e.Posts)
				.Map(m => m.ToTable("PostTag").MapLeftKey("PostId").MapRightKey("TagId"));
		}
	}
}
