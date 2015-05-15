using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuestBuild_MVC.Models
{
    public class Works
    {
        [Key]
        public int workId { get; set; }
        public DateTime dateOfWork { get; set; }
        public string themeOfWork { get; set; }
        public string rating { get; set; }
        public int complexity { get; set; }

        public int studentId { get; set; }
        public Students student { get; set; }

        public int teacherId { get; set; }
        public Teachers teacher { get; set; }

        public int subjectId { get; set; }
        public Subjects subject { get; set; }

        public List<Questions> questions { get; set; }
    }
}