using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestBuild_MVC.Controllers.SupportingClasses
{
    public class StudAndR
    {
        public StudAndR(string nameOfStudent)
        {
            this.nameOfStudent = nameOfStudent;
            this.raitingWithDay = new Dictionary<DateTime, string>();
        }
        public string nameOfStudent { get; set; }
        public Dictionary<DateTime, string> raitingWithDay { get; set; }
    }
}