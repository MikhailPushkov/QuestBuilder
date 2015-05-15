using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuestBuild_MVC.Models
{
    public class Subjects
    {
        public Subjects() 
        {
            teachers = new List<Teachers>();
        }
        public Subjects(string nameOfSubject)
        {
            this.nameOfSubject = nameOfSubject;
        }

        [Key]
        public int subjectId { get; set; }
        public string nameOfSubject { get; set; }

        public List<Teachers> teachers { get; set; }
        public List<Themes> themes { get; set; }
        public List<Questions> questions { get; set; }

        public List<Works> works { get; set; }
    }
}