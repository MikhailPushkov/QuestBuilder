using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestBuild_MVC.Models
{
    public class AnswerForAddInBase
    {
        public int idOfAnswer { get; set; }
        public string textOfAnswer { get; set; }
        public bool verity { get; set; }
        public string nameOfQuestion { get; set; }
    }
}