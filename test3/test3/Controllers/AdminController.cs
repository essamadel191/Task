using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test3.Models;

namespace test3.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            DbModel dbmodel = new DbModel();
            var all_books = dbmodel.Books.ToList();
            return View(all_books);

        }
        

        [HttpGet]
        public ActionResult Add_user()
        {
            User usermodel = new User();
            return View(usermodel);

        }
        [HttpPost]
        public ActionResult Add_user(User usermodel)
        {
            using (DbModel dbmodel = new DbModel())
            {
                if (dbmodel.Users.Any(x => x.username == usermodel.username))
                {
                    ViewBag.DuplicationMessage = "User name is already exist.";
                    return View("Add_user", usermodel);
                }

                dbmodel.Users.Add(usermodel);
                dbmodel.SaveChanges();

            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Regestration Complete.";
            return View("Add_user", new User());
        }


        [HttpGet]
        public ActionResult Add_book()
        {
            Book book = new Book();
            return View(book);
        }
        [HttpPost]
        public ActionResult Add_book(Book book)
        {
            DbModel dbmodel = new DbModel();
            

            string fileName = Path.GetFileNameWithoutExtension(book.imgFile.FileName);
            string extension = Path.GetExtension(book.imgFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            book.book_img = "~/Images/" + fileName;

            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
            book.imgFile.SaveAs(fileName);

            book.date_added = DateTime.Now;

            dbmodel.Books.Add(book);
            dbmodel.SaveChanges();

            ModelState.Clear();
            ViewBag.AddCompleted = "The book has succussfully added.";
            return View("Add_book", new Book());
        }

        [HttpGet]
        public ActionResult Delete(int id, int num)
        {

            using (DbModel dbmodel = new DbModel())
            {

                //var item = dbmodel.Books.Single(x => x.book_id == id);
                Book item2 = dbmodel.Books.Find(id);
                if (item2.copies > 0 & num < item2.copies)
                {
                    item2.copies -= num;

                }
                if (item2.copies == 0)
                {
                    item2.status = "Out Of Stock";
                }

                dbmodel.SaveChanges();
                return RedirectToAction("Index");
            }

        }
        public ActionResult DeleteAll(int id)
        {
            using (DbModel dbmodel = new DbModel())
            {
                Book item2 = dbmodel.Books.Find(id);
                dbmodel.Books.Remove(item2);
                dbmodel.SaveChanges();
                return RedirectToAction("Index");
            }
        }


    }
}