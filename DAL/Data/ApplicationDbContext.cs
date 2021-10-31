using DAL.DTO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data
{
	public class ApplicationDbContext : IdentityDbContext<ReaderUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ClubMember>()
				.HasKey(c => new { c.ClubID, c.UserID });
			modelBuilder.Entity<ReadBook>()
				.HasKey(c => new { c.ReaderID, c.BookID });
			modelBuilder.Entity<ClubBook>()
				.HasKey(c => new { c.ClubID, c.BookID });
			modelBuilder.Entity<ClubMember>()
				.Property(e => e.PermissionLevel)
				.HasConversion(
					v => v.ToString(),
					v => Enum.Parse<MemberPermissions>(v));
			modelBuilder.Entity<ClubDiscussionBook>()
				.HasKey(c => new { c.BookID, c.DiscussionID });
			modelBuilder.Entity<ClubInvite>()
				.HasKey(c => new { c.ClubID, c.ReceiverID });
			base.OnModelCreating(modelBuilder);
		}

		public DbSet<Club> Clubs { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<ClubMember> ClubsMembers { get; set; }
	}
}
