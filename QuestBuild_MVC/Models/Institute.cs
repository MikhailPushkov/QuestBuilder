using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuestBuild_MVC.Models
{
    public class Institute
    {
        public Institute() { }

        public Institute(string nameOfInstitute)
        {
            this.nameOfInstitute = nameOfInstitute;
        }

        [Key]
        public int instituteId { get; set; }
        public string nameOfInstitute { get; set; }

        public List<Faculties> faculties { get; set; }
        public List<Groups> groups { get; set; }
    }
}