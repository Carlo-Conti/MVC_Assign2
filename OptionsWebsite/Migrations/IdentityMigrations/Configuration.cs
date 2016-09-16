namespace OptionsWebsite.Migrations.IdentityMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    internal sealed class Configuration : DbMigrationsConfiguration<OptionsWebsite.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\IdentityMigrations";
        }

        protected override void Seed(OptionsWebsite.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            if (!context.Users.Any())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                string[] emails = { "a@a.a", "s@s.s" , "b@b.b", "c@c.c", "d@d.d", "e@e.e", "f@f.f", "g@g.g", "h@h.h", "j@j.j"};
                string[] users = { "A00111111", "A00222222", "A00333333", "A00444444", "A00555555", "A00666666", "A00777777", "A00888888", "A00999999", "A00101010" };

                if (!roleManager.RoleExists("Admin"))
                    roleManager.Create(new IdentityRole("Admin"));

                if (!roleManager.RoleExists("Student"))
                    roleManager.Create(new IdentityRole("Student"));

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                if (userManager.FindByEmail(emails[0]) == null)
                {
                    var user = new ApplicationUser
                    {
                        Email = emails[0],
                        UserName = users[0],
                    };
                    var result = userManager.Create(user, "P@$$w0rd");
                    if (result.Succeeded)
                        userManager.AddToRole(userManager.FindByEmail(user.Email).Id, "Admin");
                }
                if (userManager.FindByEmail(emails[1]) == null)
                {
                    var user = new ApplicationUser
                    {
                        Email = emails[1],
                        UserName = users[1],
                    };
                    var result = userManager.Create(user, "P@$$w0rd");
                    if (result.Succeeded)
                        userManager.AddToRole(userManager.FindByEmail(user.Email).Id, "Student");
                }
                if (userManager.FindByEmail(emails[2]) == null)
                {
                    var user = new ApplicationUser
                    {
                        Email = emails[2],
                        UserName = users[2],
                    };
                    var result = userManager.Create(user, "P@$$w0rd");
                    if (result.Succeeded)
                        userManager.AddToRole(userManager.FindByEmail(user.Email).Id, "Admin");
                }
                if (userManager.FindByEmail(emails[3]) == null)
                {
                    var user = new ApplicationUser
                    {
                        Email = emails[3],
                        UserName = users[3],
                    };
                    var result = userManager.Create(user, "P@$$w0rd");
                    if (result.Succeeded)
                        userManager.AddToRole(userManager.FindByEmail(user.Email).Id, "Admin");
                }
                if (userManager.FindByEmail(emails[4]) == null)
                {
                    var user = new ApplicationUser
                    {
                        Email = emails[4],
                        UserName = users[4],
                    };
                    var result = userManager.Create(user, "P@$$w0rd");
                    if (result.Succeeded)
                        userManager.AddToRole(userManager.FindByEmail(user.Email).Id, "Admin");
                }
                if (userManager.FindByEmail(emails[5]) == null)
                {
                    var user = new ApplicationUser
                    {
                        Email = emails[5],
                        UserName = users[5],
                    };
                    var result = userManager.Create(user, "P@$$w0rd");
                    if (result.Succeeded)
                        userManager.AddToRole(userManager.FindByEmail(user.Email).Id, "Admin");
                }
                if (userManager.FindByEmail(emails[6]) == null)
                {
                    var user = new ApplicationUser
                    {
                        Email = emails[6],
                        UserName = users[6],
                    };
                    var result = userManager.Create(user, "P@$$w0rd");
                    if (result.Succeeded)
                        userManager.AddToRole(userManager.FindByEmail(user.Email).Id, "Admin");
                }
                if (userManager.FindByEmail(emails[7]) == null)
                {
                    var user = new ApplicationUser
                    {
                        Email = emails[7],
                        UserName = users[7],
                    };
                    var result = userManager.Create(user, "P@$$w0rd");
                    if (result.Succeeded)
                        userManager.AddToRole(userManager.FindByEmail(user.Email).Id, "Admin");
                }
                if (userManager.FindByEmail(emails[8]) == null)
                {
                    var user = new ApplicationUser
                    {
                        Email = emails[8],
                        UserName = users[8],
                    };
                    var result = userManager.Create(user, "P@$$w0rd");
                    if (result.Succeeded)
                        userManager.AddToRole(userManager.FindByEmail(user.Email).Id, "Admin");
                }
                if (userManager.FindByEmail(emails[9]) == null)
                {
                    var user = new ApplicationUser
                    {
                        Email = emails[9],
                        UserName = users[9],
                    };
                    var result = userManager.Create(user, "P@$$w0rd");
                    if (result.Succeeded)
                        userManager.AddToRole(userManager.FindByEmail(user.Email).Id, "Admin");
                }
            }

        }
    }
}
