using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuestBuild_MVC.Models
{
    public class Teachers
    {
        public Teachers()
        {
            subjects = new List<Subjects>();
        }

        [Key]
        public int teacherId { get; set; }
        public string userId { get; set; }
        public string firstNameOfTeacher { get; set; }
        public string secondNameOfTeacher { get; set; }
        public string patronymicOfTeacher { get; set; }
        public byte[] foto { get; set; }

        public int chairId { get; set; }
        public Chairs chair { get; set; }

        public List<Themes> themes { get; set; } 
        public List<Subjects> subjects { get; set; }
        public List<Questions> questions { get; set; }
        public List<Groups> groups { get; set; }
        public List<Works> works { get; set; }

    }
}