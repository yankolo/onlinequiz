using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineWebApp;

namespace OnlineWebApp.Controllers
{
    public class HomeController : Controller
    {
        private QuizDBContext db = new QuizDBContext();

        // GET: Index
        public ActionResult Index()
        {
            List<CategoryCount> categories;
            if (User.Identity.IsAuthenticated)
            {
                var answeredQuestions = from q in db.Questions
                                                    join answer in db.Answers on q.ID equals answer.Questions_ID
                                                    where answer.Username == User.Identity.Name
                                                    select q;
                List<Question> unansweredQuestions = (from qte in db.Questions
                                                      select qte).Except(answeredQuestions).ToList();
                categories = (from q in unansweredQuestions
                              group q by q.Category.Name
                              into newGroup
                              select new CategoryCount
                              {
                                  CategoryName = newGroup.Key.ToString(),
                                  QuestionCount = newGroup.Count()
                              }).OrderByDescending(x => x.QuestionCount).ToList();
            } else
            {
                categories = (from c in db.Categories
                              orderby c.Questions.Count descending
                              select new CategoryCount
                              {
                                  CategoryName = c.Name,
                                  QuestionCount = c.Questions.Count
                              }).ToList();
            }
            

            return View(categories);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
