using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineQuizConsoleApp.Entities
{
    public class Questions
    {
        public String QuestionName { get; set; }
        public Categories Categerory { get; set; }
        public String Option1 { get; set; }
        public String Option2 { get; set; }
        public String Option3 { get; set; }
        public String option4 { get; set; }
        public String RightOption { get; set; }
        public Users Username { get; set; }
    }
}
