using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestBuild_MVC.Controllers.SupportingClasses
{
    public class SubjectData
    {
        public string subjectName { get; set; }
        public double averRait { get; set; }
        public int countOfWorks { get; set; }
        public List<string> themesOfWorks { get; set; }
        public List<DateTime> dayOfWorks { get; set; }
        public List<string> raitings { get; set; }
    }
}