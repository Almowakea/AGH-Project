using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AGH.Services;
using AGH.Models;
using iTextSharp.text;
using System.Web;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Text;

namespace AGH.Controllers
{
    [AuthorizeAdminOnly]
    public class UsersController : Controller
    {
        private AGH_DBContext db = new AGH_DBContext();

        // GET: Users
        public ActionResult UsersList()
        {
            try
            {
                var users = db.Users.Include(u => u.User_Type).Where(u => u.Is_User_Deleted != true);

                return View(users.ToList());
            }

            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }

        }

        // GET: Users/Details/5
        public ActionResult UserDetails(int? id)
        {
            if (id is null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                User user = db.Users.Find(id);
                if (user is null || user.Is_User_Deleted)
                {
                    return HttpNotFound();
                }
                return View(user);
            }

            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        // GET: Users/Create
        public ActionResult CreateUser()
        {
            try
            {
                ViewBag.User_Type_ID = new SelectList(db.User_Type, "ID", "Type");
                return View();
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser([Bind(Include = "ID,User_Type_ID,User_First_Name,User_Last_Name,User_Phone_Number,User_Email,User_ID,User_Password")] User user)
        {
            try
            {
                ViewBag.User_Type_ID = new SelectList(db.User_Type, "ID", "Type", user.User_Type_ID);

                if (ModelState.IsValid)
                {
                    //using (SHA512 sha512Hash = SHA512.Create())
                    //{
                    //    // Generate unique salt for each user
                    //    user.User_Password_Salt = Crypto.GenerateSalt();

                    //    // From String to byte array + salt
                    //    byte[] sourceBytes = Encoding.UTF8.GetBytes(user.User_Password + user.User_Password_Salt);
                    //    byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);

                    //    // Converting hashed byte array back to string format
                    //    user.User_Password = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                    //}

                    user.User_Password_Salt = HashPasswordService.CreateSalt();

                    user.User_Password = HashPasswordService.CreateHash(user.User_Password, user.User_Password_Salt);

                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("UsersList");
                }

                return View(user);
            }

            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        // GET: Users/Edit/5
        public ActionResult EditUser(int? id)
        {
            try
            {
                User user = db.Users.Find(id);
                ViewBag.User_Type_ID = new SelectList(db.User_Type, "ID", "Type", user.User_Type_ID);
                if (id is null || user.Is_User_Deleted)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (user is null)
                {
                    return HttpNotFound();
                }

                return View(user);
            }

            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser([Bind(Include = "ID,User_Type_ID,User_First_Name,User_Last_Name,User_Phone_Number,User_Email,User_ID,User_Password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.User_Password_Salt = HashPasswordService.CreateSalt();

                user.User_Password = HashPasswordService.CreateHash(user.User_Password, user.User_Password_Salt);

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UsersList");
            }

            ViewBag.User_Type_ID = new SelectList(db.User_Type, "ID", "Type", user.User_Type_ID);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult DeleteUser(int? id)
        {
            try
            {
                User user = db.Users.Find(id);

                if (id is null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (user.Is_User_Deleted)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                return View(user);
            }
            catch (Exception)
            {
                //One way of throwing exception "Didn't generalize because it reveals info to users"
                throw new Exception("delete operation unsuccessful (user not found)");
            }
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                User user = db.Users.Find(id);

                user.Is_User_Deleted = true;

                db.SaveChanges();

                return RedirectToAction("UsersList");
            }

            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }

        }

        public byte[] GeneratePDF(string pHTML)
        {
            byte[] bytePDF = null;

            using (MemoryStream ms = new MemoryStream())
            {
                TextReader txtReader = new StringReader(pHTML);

                // itextsharp object from document class
                // Page size & margins
                using (Document doc = new Document(PageSize.A4, 25, 25, 25, 25))
                {
                    // Create an itextsharp pdfWriter that listens to the document and directs an XML-stream to a file
                    using (PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms))
                    {

                        // Create a worker to parse the document
                        HTMLWorker htmlWorker = new HTMLWorker(doc);

                        // Open document and start the worker on the document
                        doc.Open();

                        doc.HtmlStyleClass = "table";

                        htmlWorker.StartDocument();

                        // Parse the html into the document
                        htmlWorker.Parse(txtReader);

                        // Close the document and the worker
                        htmlWorker.EndDocument();
                        htmlWorker.Close();
                        doc.Close();

                        bytePDF = ms.ToArray();

                        return bytePDF;
                    }
                }
            }
        }

        public void DownloadPDF()
        {
            //string HTMLContent = "Hello <b>World</b>";
            var sb = new StringBuilder();
            sb.Append(@"<html>
<head>
</head>
<body>
<table class='table bordered'>
        <tr>
            <th>
                First Name
            </th>
            <th>
                Last Name
            </th>
            <th>
                Phone Number
            </th>
            <th>
                Email
            </th>
            <th>
                ID Number
            </th>
            <th>
                Role
            </th>
        </tr>");

            var users = db.Users.Include(u => u.User_Type).Where(u => u.Is_User_Deleted != true);

            foreach (var user in users)
            {
                sb.AppendFormat($@"<tr>
<td>{user.User_First_Name}</td>
<td>{user.User_Last_Name}</td>
<td>{user.User_Phone_Number}</td>
<td>{user.User_Email}</td>
<td>{user.User_ID}</td>
<td>{user.User_Type.Type}</td>
    </tr>");
            }

            sb.Append(@"</table>
</body>
</html>");

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + "PDFfile.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(GeneratePDF(sb.ToString()));
            Response.End();
        }


        protected override void Dispose(bool disposing)
        {
            //Should this also be wrapped in Try/Catch
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
