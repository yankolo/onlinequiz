using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OnlineQuizConsoleApp
{
    class ConsoleApp
    {
        public static void Main(string[] args)
        {
            XElement userXml = XElement.Load("users.xml");
            var users = from user in userXml.Elements()
                        select new
                        {
                            Username = user.Attribute("userId").Value.Trim(),
                            FirstName = user.Attribute("firstName").Value.Trim(),
                            LastName = user.Attribute("lastName").Value.Trim()
                        };
            foreach(var usr in users)
            {
                Console.Write(usr.Username + " " + usr.FirstName + " " + usr.LastName); 
            }
        }
    }
}
