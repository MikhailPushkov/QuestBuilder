using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuestBuild_MVC.Models
{
    public class Groups
    {
        [Key]
        public int groupId { get; set; }
        public int numberOfCours { get; set; }
        public int numberOfGroup { get; set; }

        public int instituteId { get; set; }
        public Institute institute { get; set; }

        public int teacherId { get; set; }
        public Teachers teacher { get; set; }

        public List<Students> students { get; set; }
    }
}