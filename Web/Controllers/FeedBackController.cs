using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class FeedBackController : Controller
    {
        // GET: FeedBack
        public ActionResult Index()
        {
            return View();
        }

        // POST: Feedback
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FeedBack feedback)
        {
            Soft context = new Soft();
            if (ModelState.IsValid)
            {
                context.FeedBacks.Add(feedback);
                context.SaveChanges();
                return RedirectToAction("ThankYou");
            }

            return View(feedback);
        }

        // GET: Feedback/ThankYou
        public ActionResult ThankYou()
        {
            return View();
        }
    }
}