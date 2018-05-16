using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineWebApp
{
    public class QuestionResult
    {
        public int QuestionId { get; set; }
        [Range(1, 4)]
        public int Selected { get; set; }
    }
}