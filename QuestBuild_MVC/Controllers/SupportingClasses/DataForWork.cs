using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestBuild_MVC.Controllers.SupportingClasses
{
    public class DataForWork
    {
        public int subjectId { get; set; }
        public int themeId { get; set; }
        public bool usingSubThemes { get; set; }
        public int complexity { get; set; }
        public string typeOfWork { get; set; }
        public DateTime dateOfWork { get; set; }
        public string students { get; set; }
        public string typesAndCounts { get; set; }
    }
}