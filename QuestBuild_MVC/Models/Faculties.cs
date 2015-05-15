using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuestBuild_MVC.Models
{
    public class Faculties
    {
        public Faculties() { }
        public Faculties(string nameOfFaculti, int instituteId)
        {
            this.nameOfFaculti = nameOfFaculti;
            this.instituteId = instituteId;
        }

        [Key]
        public int facultiId { get; set; }
        public string nameOfFaculti { get; set; }

        public int instituteId { get; set; }
        public Institute institute { get; set; }

        public virtual List<Chairs> chair { get; set; }

    }
}