using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineQuizConsoleApp
{
    public class Question
    {
        public String Option1 { get; set; }
        public String Option2 { get; set; }
        public String Option3 { get; set; }
        public String Option4 { get; set; }
        public String RightOption { get; set; }
        public String Title { get; set; }
        public User User{ get; set; }
        public Category Category { get; set; }
    }
}
