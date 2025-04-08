using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System;
using VOTEAPPMvc.Data;
using VOTEAPPMvc.Middleware;
using VOTEAPPMvc.Models;
using VOTEAPPMvc.Services;

namespace VOTEAPPMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<VotingDbContext>(options => options.UseInMemoryDatabase("VotingDB"));

            builder.Services.AddScoped<IVoteService, VoteService>();
            builder.Services.AddScoped<IReportService, ReportService>();

            //builder.Services.AddDbContext<VotingDbContext>(options =>
            //options.UseInMemoryDatabase("VotingAppDb"));

            //builder.Services.AddSingleton<IVoteService, VoteService>();
            //builder.Services.AddScoped<IReportService, ReportService>();


           
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            // Add our custom middleware
            app.UseMiddleware<VoteRestrictionMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            // Seed initial data for testing
            //SeedData(app.ApplicationServices);

            app.Run();
        }

                    //private void SeedData(IServiceProvider serviceProvider)
                    //{
                    //    using var scope = serviceProvider.CreateScope();
                    //    var dbContext = scope.ServiceProvider.GetRequiredService<VotingDbContext>();

                    //    if (!dbContext.Topics.Any())
                    //    {
                    //        dbContext.Topics.Add(new Topic
                    //        {
                    //            Title = "Sample Topic 1",
                    //            Description = "This is the first sample voting topic",
                    //            IsActive = true
                    //        });

                    //        dbContext.Topics.Add(new Topic
                    //        {
                    //            Title = "Sample Topic 2",
                    //            Description = "This is the second sample voting topic",
                    //            IsActive = true
                    //        });
                    //        dbContext.SaveChanges();
                    //    }
                    //}
    }
}
