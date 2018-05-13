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
            List<User> users = CreateUsers();
            List<Category> categories = CreateCategories();
            List<Question> questions = CreateQuestions(users, categories);
            SetPasswords(users);
            //InsertUserToDatabse(users);
            //InsertCategories(categories);
            InsertQuestions(questions);
        }
        public static List<User> CreateUsers()
        {
            XElement userXml = XElement.Load("users.xml");
            List<User> users = (from user in userXml.Elements()
                                select new User
                                {
                                    Username = user.Attribute("userId").Value.Trim(),
                                    FirstName = user.Attribute("firstName").Value.Trim(),
                                    LastName = user.Attribute("lastName").Value.Trim()
                                }).ToList<User>();
            return users;
        }

        public static List<Category> CreateCategories()
        {
            XElement categoryXml = XElement.Load("questions.xml");
            List<Category> categories = (from category in categoryXml.Descendants("category")
                                         group category by category.Value.Trim()
                                         into value
                                         select new Category
                                         {
                                             Name = value.Key.ToString()
                                         }).ToList<Category>();
            return categories;
        }

        public static List<Question> CreateQuestions(List<User> users, List<Category> categories)
        {
            XElement questionXml = XElement.Load("questions.xml");
            var anonymousQuesitons = from question in questionXml.Elements("question")
                                     let options = question.Elements("option")
                                     select new
                                     {
                                         Option1 = options.First(x => x.Attribute("num").Value == "1").Value.Trim(),
                                         Option2 = options.First(x => x.Attribute("num").Value == "2").Value.Trim(),
                                         Option3 = options.First(x => x.Attribute("num").Value == "3").Value.Trim(),
                                         Option4 = options.First(x => x.Attribute("num").Value == "4").Value.Trim(),
                                         RightOption = Int32.Parse(options.First(x => x.Attribute("correct")?.Value == "true").Attribute("num").Value.Trim()),
                                         Title = question.Element("title").Value.Trim(),
                                         Username = question.Element("user").Value.Trim(),
                                         CategoryName = question.Element("category").Value.Trim()
                                     };
            List<Question> questions = (from question in anonymousQuesitons
                                        join user in users on question.Username equals user.Username
                                        join category in categories on question.CategoryName equals category.Name
                                        select new Question
                                        {
                                            option_1 = question.Option1,
                                            option_2 = question.Option2,
                                            option_3 = question.Option3,
                                            option_4 = question.Option4,
                                            right_option = question.RightOption,
                                            Title = question.Title,
                                            User = user,
                                            Category = category
                                        }).ToList<Question>();
            return questions;
        }
        public static  void InsertUserToDatabse(List<User> users)
        {
            using (var db =new QuizDBContext())
            {
                foreach(var x in users)
                {
                    db.Users.Add(x);
                    db.SaveChanges();
                }
            }

        }
        public static void SetPasswords(List<User> users)
        {
            String[] s = { "helloWorld", "Sammy", "Yanik", "C#isBetterthanJava" };
            int counter = 0;
            foreach (var x in users)
            {
                x.Password = s[counter];
                counter++;
            }
        }

        public static void InsertCategories(List<Category> categories)
        {
            using (var db = new QuizDBContext())
            {
                foreach(var x in categories)
                {
                    db.Categories.Add(x);
                    db.SaveChanges();

                }
            }
        }

        public static  void InsertQuestions(List<Question> questions)
        {
            using (var db = new QuizDBContext())
            {
                foreach(var x in questions)
                { 
                    db.Questions.Add(x);
                    db.SaveChanges();
                }
            }
        }
    }
}
