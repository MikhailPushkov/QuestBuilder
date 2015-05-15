using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuestBuild_MVC.Models
{
    public class Questions
    {
        [Key]
        public int questionId { get; set; }
        public string nameOfQuestion { get; set; }
        public byte[] textOfQuestion { get; set; }
        public int complexity { get; set; }

        public int teacherId { get; set; }
        public Teachers teacher { get; set; }

        public int typeOfQuestionsId { get; set; }
        public TypeOfQuestions typeOfQuestion { get; set; }

        public int subjectId { get; set; }
        public Subjects subject { get; set; }

        public int themeId { get; set; }
        public Themes theme { get; set; }

        public List<Answers> answers { get; set; }

        public List<Works> works { get; set; }
      
    }
}