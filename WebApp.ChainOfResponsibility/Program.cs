using WebApp.ChainOfResponsibility.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ChainOfResponsibility
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var identityDbContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            identityDbContext.Database.Migrate();
            if(!userManager.Users.Any())
            {
                userManager.CreateAsync(new AppUser() { UserName = "user1", Email = "user1@outlook.com"},"Password12*").Wait();
                userManager.CreateAsync(new AppUser() { UserName = "user2", Email = "user2@outlook.com"},"Password12*").Wait();
                userManager.CreateAsync(new AppUser() { UserName = "user3", Email = "user3@outlook.com"},"Password12*").Wait();
                userManager.CreateAsync(new AppUser() { UserName = "user4", Email = "user4@outlook.com"},"Password12*").Wait();
                userManager.CreateAsync(new AppUser() { UserName = "user5", Email = "user5@outlook.com"},"Password12*").Wait(); //Asenkron i�lemi senkron yapmak i�in yaz�lmas� gereken komut.
            }
            Enumerable.Range(1, 20).ToList().ForEach(x =>
            {
                identityDbContext.Products.Add(new Product()
                {
                    Name = "Kalem " + x,
                    Price = 100 * x,
                    Stock = 50 + x
                });
            });
            identityDbContext.SaveChanges();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
