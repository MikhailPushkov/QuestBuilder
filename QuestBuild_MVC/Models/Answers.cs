using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuestBuild_MVC.Models
{
    public class Answers
    {
        [Key]
        public int answerId { get; set; }
        public bool truth { get; set; }
        public byte[] textOfAnswer { get; set; }

        public int questionId { get; set; }
        public Questions question { get; set; }
    }
}