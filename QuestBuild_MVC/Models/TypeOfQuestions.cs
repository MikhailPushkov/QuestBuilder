using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuestBuild_MVC.Models
{
    public class TypeOfQuestions
    {
        public TypeOfQuestions() { }
        public TypeOfQuestions(string nameOfType)
        {
            this.nameOfType = nameOfType;
        }

        [Key]
        public int typeOfQuestionsId { get; set; }
        public string nameOfType { get; set; }

        public List<Questions> questions { get; set; }
    }
}