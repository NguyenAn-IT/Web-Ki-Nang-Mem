using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using System.Net;

namespace Web.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Course(string Searching, string Search)
        {
            Soft context = new Soft();
            var upcommingCourse = context.Courses.Where(p => p.DateTime > DateTime.Now).OrderBy(p => p.DateTime).ToList();
            var loginUser = User.Identity.GetUserId();
            ViewBag.LoginUser = loginUser;
            foreach (Course i in upcommingCourse)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.LecturerId);
                i.Name = user.Name;

                // Sử dụng thuộc tính "gender" của đối tượng "Course"
                string gender = i.Gender;
                // Thực hiện các thao tác khác với thuộc tính "gender"
            }
         

            if (Searching == "FullName")
            {
                return View(context.Courses.Where(c => c.FullName.StartsWith(Search)).ToList());
            }
            else if (Searching == "CategoryId")
            {
                return View(context.Courses.Where(c => c.Category.Name.StartsWith(Search)).ToList());
            }

            return View(context.Courses.ToList());
            
        }

        public ActionResult Create()
        {
            Soft context = new Soft();

            Course objCourse = new Course();

            objCourse.ListCategory = context.Categories.ToList();
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;
            return View(objCourse);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course newCourse)
        {
            Soft context = new Soft();

            ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                newCourse.ListCategory = context.Categories.ToList();
                return View("Create", newCourse);
            }

            context.Courses.Add(newCourse);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");

        }
        public ActionResult Edit(int id)
        {
            using (var context = new Soft())
            {
                var course = context.Courses.SingleOrDefault(c => c.Id == id);
                if (course == null)
                {
                    return HttpNotFound();
                }
                course.ListCategory = context.Categories.ToList();
                return View("Edit", course);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, Course course)
        {

            using (var context = new Soft())
            {
                var loginUser = User.Identity.GetUserId();
                if (!ModelState.IsValid)
                {
                    course.ListCategory = context.Categories.ToList();
                    return View("Create", course);
                }
                var courseInDb = context.Courses.SingleOrDefault(c => c.LecturerId == loginUser && c.Id == course.Id);
                if (courseInDb == null)
                {
                    return HttpNotFound();
                }
                courseInDb.FullName = course.FullName;
                courseInDb.DateTime = course.DateTime;
                courseInDb.CategoryId = course.CategoryId;
                courseInDb.Gender = course.Gender;

                context.SaveChanges();
            }
            return RedirectToAction("Course", "Course");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (Soft context = new Soft())
            {
                Course course = context.Courses.Find(id);
                if (course == null)
                {
                    return HttpNotFound();
                }
                context.Courses.Remove(course);
                context.SaveChanges();
            }

            return RedirectToAction("Course");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            using (var context = new Soft())
            {
                var course = context.Courses.SingleOrDefault(c => c.Id == id);
                if (course == null)
                {
                    return HttpNotFound();
                }
                context.Courses.Remove(course);
                context.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (Soft context = new Soft())
            {
                Course course = context.Courses.Find(id);
                if (course == null)
                {
                    return HttpNotFound();
                }
                return View(course);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int id)
        {
            using (var context = new Soft())
            {
                var course = context.Courses.SingleOrDefault(c => c.Id == id);
                if (course == null)
                {
                    return HttpNotFound();
                }
                return View(course);
            }
        }

        public ActionResult Lesson(int id)
        {

            if (id == 1)
            {
                return RedirectToAction("Lesson1", "Lesson");
            }
            else if (id == 2)
            {
                return RedirectToAction("Lesson2", "Lesson");
            }
            else
            {
                return RedirectToAction("Lesson3", "Lesson");
            }
        }


    }
}