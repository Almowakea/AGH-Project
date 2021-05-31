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
using iTextSharp.tool.xml;

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

            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        // POST: Users/Delete/5
        [HttpPost]
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
                StringReader txtReader = new StringReader(pHTML);

                // itextsharp object from document class
                using (Document doc = new Document(PageSize.A4, 25, 25, 25, 25)) // Page size & margins
                {
                    // Create an itextsharp pdfWriter that listens to the document and directs an XML-stream to a file
                    using (PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms))
                    {
                        // Open document and start the worker on the document
                        doc.Open();

                        // Create an XML worker to parse the document
                        XMLWorkerHelper.GetInstance().ParseXHtml(oPdfWriter, doc, txtReader);

                        doc.Close();

                        bytePDF = ms.ToArray();

                        return bytePDF;
                    }
                }
            }
        }

        public void DownloadPDF()
        {
            string tableStyle = "cellpadding='3' cellspacing='0' style='border: 1px solid #ccc;font-size: 14pt;' align='center'";

            string thStyle = "style='background-color: #d9edf7; border: 1px solid #ccc; font-weight:bold'";

            string tdStyle = "style='width: 120px; border: 1px solid #ccc;'";

            try
            {
                #region Mark-up string
                var sb = new StringBuilder();
                sb.Append($@"<html>
<head>
</head>
<body>
<table {tableStyle}>
        <tr>
            <th {thStyle}>
                First Name
            </th>
            <th {thStyle}>
                Last Name
            </th>
            <th {thStyle}>
                Phone Number
            </th>
            <th {thStyle}>
                Email
            </th>
            <th {thStyle}>
                ID Number
            </th>
            <th {thStyle}>
                Role
            </th>
        </tr>");

                var users = db.Users.Include(u => u.User_Type).Where(u => u.Is_User_Deleted != true);

                foreach (var user in users)
                {
                    sb.AppendFormat($@"<tr>
<td {tdStyle}>{user.User_First_Name}</td>
<td {tdStyle}>{user.User_Last_Name}</td>
<td {tdStyle}>{user.User_Phone_Number}</td>
<td {tdStyle}>{user.User_Email}</td>
<td {tdStyle}>{user.User_ID}</td>
<td {tdStyle}>{user.User_Type.Type}</td>
    </tr>");
                }

                sb.Append(@"</table>
</body>
</html>");

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + "Users-List.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(GeneratePDF(sb.ToString()));
                Response.End();
                #endregion
            }

            catch (Exception e)
            {
                //ViewBag.ErrorMessage = e.Message;

                throw new Exception(e.Message);
            }
            
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
