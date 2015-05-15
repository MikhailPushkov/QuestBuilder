using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuestBuild_MVC.Models
{
    public class Chairs
    {
        public Chairs() { }
        public Chairs(string nameOfChair, int facultiId)
        {
            this.nameOfChair = nameOfChair;
            this.facultiId = facultiId;
        }

        [Key]
        public int chairId { get; set; }
        public string nameOfChair { get; set; }


        public int facultiId { get; set; }
        public Faculties faculti { get; set; }

        public List<Teachers> teachers { get; set; }

    }
}