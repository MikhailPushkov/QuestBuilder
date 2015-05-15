using QuestBuild_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestBuild_MVC.Controllers.SupportingClasses
{
    public class GroupsAndPeople
    {
        public GroupsAndPeople(int idGroup)
        {
            this.idGroup = idGroup;
            GetGroup();
            GetPeoplesAndId();
        }
        public int idGroup { get; set; }
        public string group { get; set; }
        public List<string> peoples { get; set; }
        public List<int> idPeoples { get; set; }

        private void GetGroup()
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                group = (from gr in db.Groups
                         where gr.groupId == idGroup
                         select gr.numberOfCours + "/" + gr.numberOfGroup).Single();
            }
        }

        private void GetPeoplesAndId()
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                peoples = (from student in db.Students
                           where student.groupId == idGroup
                           select student.secondNameOfStudent + " " + student.firstNameOfStudent + " " + student.patronymicOfStudent).ToList();
                idPeoples = db.Students.Where(m => m.groupId == idGroup).Select(m => m.studentId).ToList();
            }
        }
    }
}