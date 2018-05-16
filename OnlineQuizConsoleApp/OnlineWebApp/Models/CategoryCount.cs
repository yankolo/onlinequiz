using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineWebApp
{
    public class CategoryCount
    {
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
        public int QuestionCount { get; set; }
    }
}