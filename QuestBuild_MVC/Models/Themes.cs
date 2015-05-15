using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuestBuild_MVC.Models
{
    public class Themes
    {
        public Themes() { }
        public Themes(string nameOfTheme, int teacherId, int subjectId)
        {
            this.nameOfTheme = nameOfTheme;
            this.teacherId = teacherId;
            this.subjectId = subjectId;
        }

        [Key]
        public int themeId { get; set; }
        public string nameOfTheme { get; set; }


        public int parentTheme_themeId { get; set; }
        public Themes parentTheme { get; set; }

        public int teacherId { get; set; }
        public Teachers teacher { get; set; }

        public int subjectId { get; set; }
        public Subjects subject { get; set; }

        public List<Questions> questions { get; set; }
    }
}