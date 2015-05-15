using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuestBuild_MVC.Models
{
    public class Students
    {
        [Key]
        public int studentId { get; set; }
        public string firstNameOfStudent { get; set; }
        public string secondNameOfStudent { get; set; }
        public string patronymicOfStudent { get; set; }

        public int groupId { get; set; }
        public Groups group { get; set; }

        public List<Works> works { get; set; }
    }
}