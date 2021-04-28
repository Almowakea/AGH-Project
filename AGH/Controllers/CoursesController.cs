using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AGH.Models;

namespace AGH.Controllers
{
    public class CoursesController : Controller
    {
        private AGH_DBContext db = new AGH_DBContext();

        public ActionResult MyCoursesList()
        {
            var userRoleID = (Int32)Session["UserRoleID"];
            var userID = (Int32)Session["UserID"];

            switch (userRoleID)
            {
                case 1:
                    var instructorCourses = (from I in db.Instructors
                                             join C in db.Courses
                                             on I.ID equals C.Course_Instructor_ID
                                             where I.User.User_ID == userID
                                             select C).ToList();
                    return View(instructorCourses.ToList());

                case 2:
                    var assistantCourses = (from A in db.Assistants
                                            join C in db.Courses
                                            on A.ID equals C.Course_Assistant_ID
                                            where A.User.User_ID == userID
                                            select C).ToList();
                    return View(assistantCourses.ToList());

                case 3:
                    var studentCourses = (from S in db.Students
                                          join C in db.Courses
                                          on S.ID equals C.Course_Instructor_ID
                                          where S.User.User_ID == userID
                                          select C).ToList();
                    return View(studentCourses.ToList());

                default:
                    return RedirectToAction("Error");
            }
        }


        // GET: Courses
        //[AuthorizeAdminOnly]
        public ActionResult CoursesList()
        {
            var courses = db.Courses.Include(c => c.Assistant).Include(c => c.Instructor);
            return View(courses.ToList());
        }

        // GET: Courses/Details/5
        //[AuthorizeAdminOnly]
        public ActionResult CourseDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        //[AuthorizeAdminOnly]
        public ActionResult CreateCourse()
        {
            //ViewBag.Assign_Course_Assistant_Name
            ViewBag.Assign_Course_Assistant_Name = new List<SelectListItem>(db.Users
                .Where(o => o.User_Type_ID == 2)
                .Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.User_First_Name + " " + x.User_Last_Name }));
            
            ViewBag.Assign_Course_Instructor_Name = new List<SelectListItem>(db.Users
                .Where(o => o.User_Type_ID == 1)
            .Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.User_First_Name + " " + x.User_Last_Name }));

            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[AuthorizeAdminOnly]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCourse([Bind(Include = "CourseID,Course_Name,Course_Code,Course_Description,Course_Credit,Course_Instructor_ID,Course_Assistant_ID")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("CoursesList");
            }

            ViewBag.Course_Assistant_ID = new SelectList(db.Assistants, "ID", "Assistant_Title", course.Course_Assistant_ID);
            ViewBag.Course_Instructor_ID = new SelectList(db.Instructors, "ID", "Instructor_Title", course.Course_Instructor_ID);
            return View(course);
        }

        // GET: Courses/Edit/5
        //[AuthorizeAdminOnly]
        public ActionResult EditCourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.Course_Assistant_ID = new SelectList(db.Assistants, "ID", "Assistant_Title", course.Course_Assistant_ID);
            ViewBag.Course_Instructor_ID = new SelectList(db.Instructors, "ID", "Instructor_Title", course.Course_Instructor_ID);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[AuthorizeAdminOnly]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourse([Bind(Include = "CourseID,Course_Name,Course_Code,Course_Description,Course_Credit,Course_Instructor_ID,Course_Assistant_ID")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CoursesList");
            }
            ViewBag.Course_Assistant_ID = new SelectList(db.Assistants, "ID", "Assistant_Title", course.Course_Assistant_ID);
            ViewBag.Course_Instructor_ID = new SelectList(db.Instructors, "ID", "Instructor_Title", course.Course_Instructor_ID);
            return View(course);
        }

        // GET: Courses/Delete/5
        //[AuthorizeAdminOnly]
        public ActionResult DeleteCourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        //[AuthorizeAdminOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("CoursesList");
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
