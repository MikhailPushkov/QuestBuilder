using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestBuild_MVC.Models
{
    public class DataForQuestion
    {
        public int idOfQuestion { get; set; }
        public int idOfAnswer { get; set; }
        public string nameOfSubject { get; set; }
        public string typeOfQuestion { get; set; }
        public string nameOfQuestion { get; set; }
        public string textOfQuestion { get; set; }
        public int themeOfQuestion { get; set; }
        public bool trueOrLie { get; set; }
        public string answer { get; set; }
        public int complexity { get; set; }
    }
}