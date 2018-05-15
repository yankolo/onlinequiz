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
    public class QuizController : Controller
    {
        private QuizDBContext db = new QuizDBContext();

        [Authorize]
        public ActionResult ShowQuiz(String nameCategory)
        {
            if (nameCategory == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var answeredQuestions = from q in db.Questions
                                    join answer in db.Answers on q.ID equals answer.Questions_ID
                                    where answer.Username == User.Identity.Name
                                    select q;
            List<Question> unansweredQuestions = (from qte in db.Questions
                                                  select qte).Except(answeredQuestions).ToList();

            List<Question> questions = unansweredQuestions
                .Where(x => x.Category.Name == nameCategory)
                .Take(5)
                .ToList();

            if (questions.Count == 0)
            {
                return HttpNotFound();
            }

            Random r = new Random();
            questions = questions.OrderBy(x => r.Next()).ToList();

            return View("Quiz", questions);
        }

        [Authorize]
        public ActionResult SubmitForm(QuestionResult[] questionsResults)
        {
            List<Question> questions = (from questionResult in questionsResults
                                        join question in db.Questions on questionResult.QuestionId equals question.ID
                                        select question).ToList();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "All questions should be answered");
                return View("Quiz", questions);
            }
            else
            {
                List<Answer> rightAnswers = (from result in questionsResults
                                             join question in db.Questions on result.QuestionId equals question.ID
                                             where result.Selected == question.right_option
                                             select new Answer
                                             {
                                                 Questions_ID = question.ID,
                                                 Username = User.Identity.Name,
                                                 Correct_Answer = false
                                             }).ToList();
                //We can already define the wrong answers and display them 
                List<Answer> wrongAnswers = (from result in questionsResults
                                             join question in db.Questions on result.QuestionId equals question.ID
                                             where result.Selected != question.right_option
                                             select new Answer
                                             {
                                                 Questions_ID = question.ID,
                                                 Username = User.Identity.Name,
                                                 Correct_Answer = false
                                             }).ToList();

                db.Answers.AddRange(rightAnswers);
                db.Answers.AddRange(wrongAnswers);
                db.SaveChanges();

                ViewBag.QuestionResuslts = questionsResults;
                ViewBag.Score = rightAnswers.Count + "/" + questionsResults.Length;
                return View("QuizResults", questions);
            }
        }

        // GET: Questions/AddQuestion
        [Authorize]
        public ActionResult AddQuestion()
        {
            ViewBag.Categories_Category_ID = new SelectList(db.Categories, "Category_ID", "Name");
            ViewBag.Author_Username = new SelectList(db.Users, "Username", "Password");
            return View();
        }

        // POST: Questions/AddQuestion
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult AddQuestion([Bind(Include = "option_1,option_2,option_3,option_4,right_option,Title,Categories_Category_ID")] Question question)
        {
            question.Author_Username = User.Identity.Name;

            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Categories_Category_ID = new SelectList(db.Categories, "Category_ID", "Name", question.Categories_Category_ID);
            ViewBag.Author_Username = new SelectList(db.Users, "Username", "Password", question.Author_Username);

            return View(question);
        }

        [Authorize]
        // GET: Categories/Create
        public ActionResult AddCategory()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult AddCategory([Bind(Include = "Category_ID,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(category);
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
