using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QuestBuild_MVC.Models
{
    public class AppDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
 
            // создаем две роли
            var role1 = new IdentityRole { Name = "teacher" };
            var role2 = new IdentityRole { Name = "student" };
 
            // добавляем роли в бд
            roleManager.Create(role1);
            roleManager.Create(role2);
 
            base.Seed(context);
        }
    }
}