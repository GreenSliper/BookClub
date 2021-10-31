using AutoMapper;
using DAL.Data;
using DAL.DTO;
using DAL.Models.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookClub
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseLazyLoadingProxies()
				.UseNpgsql(
					Configuration.GetConnectionString("DefaultConnection")));
			services.AddDatabaseDeveloperPageExceptionFilter();

			services.AddIdentity<ReaderUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultUI()
				.AddDefaultTokenProviders();

			/*services.AddAuthentication().AddGoogle(options =>
			{
				IConfigurationSection googleAuthNSection =
					Configuration.GetSection("Authentication:Google");

				options.ClientId = googleAuthNSection["ClientId"];
				options.ClientSecret = googleAuthNSection["ClientSecret"];
			});*/

			//auto-mapping
			var mapperConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MappingProfile());
			});
			IMapper mapper = mapperConfig.CreateMapper();
			services.AddSingleton(mapper);

			//repository
			services.AddScoped<IRepository<ReaderUser, string>, UserRepository<ApplicationDbContext>>();
			services.AddScoped<IRepository<Club, int>, ClubRepository<ApplicationDbContext>>();
			services.AddScoped<IRepository<Book, int>, BookRepository<ApplicationDbContext>>();
			services.AddScoped<IRepository<ClubDiscussion, int>, DiscussionRepository<ApplicationDbContext>>();
			services.AddScoped<IExpirableRepos<ClubInvite, (int clubId, string receiverId)>, 
				InviteRepository<ApplicationDbContext>>();
			//services
			services.AddTransient<IAccessService, AccessService>();
			services.AddTransient<IClubService, ClubService>();
			services.AddTransient<IDiscussionService, DiscussionService>();
			services.AddTransient<IBookService, BookService>();
			services.AddTransient<IClubMemberService, ClubMemberService>();

			services.AddControllersWithViews();
			services.AddRazorPages();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseMigrationsEndPoint();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
			});
		}
	}
}
