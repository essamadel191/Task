using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test3.Models;

namespace test3.Controllers
{
    public class UserController : Controller
    {
        
        public ActionResult Index()
        {

            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            DbModel dbmodel = new DbModel();
            
            var all_books = dbmodel.Books.ToList();
            
            return View(all_books);
        }
        [HttpGet]
        public ActionResult Login()
        {
            User usermodel = new User();
            return View(usermodel);
        }
        [HttpPost]
        public ActionResult Login(User usermodel)
        {
            if(usermodel != null)
            {
                DbModel dbmodel = new DbModel();
                var user1 = dbmodel.Users.SingleOrDefault(x => x.username == usermodel.username);
                if(user1 == null)
                {
                    ViewBag.NotFound = "User doesn't exist.";
                    return View("Login", usermodel);
                }
                var userID = user1.user_id;
                if (user1.user_type == true)
                {
                    Session["Admin"] = userID;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    Session["User"] = userID;
                    return RedirectToAction("Index", "User");
                }


            }
            else { return HttpNotFound(); }
            
        }
        [HttpGet]
        public ActionResult Registration()
        {
            User usermodel = new User();
            return View(usermodel);
        }
        [HttpPost]
        public ActionResult Registration(User usermodel)
        {
            using (DbModel dbmodel = new DbModel())
            {
                if (dbmodel.Users.Any(x => x.username == usermodel.username))
                {
                    ViewBag.DuplicationMessage = "User name is already exist.";
                    return View("Add_user", usermodel);
                }


                usermodel.user_type = false;
                Session["User"] = usermodel.user_id;
                dbmodel.Users.Add(usermodel);
                dbmodel.SaveChanges();

            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Regestration Complete.";
            return View("Login", new User());

        }
       
        public ActionResult Rent(int? book_id)
        {
            Session["book"] = book_id;
            if (Session["User"] == null)
            {
                return RedirectToAction("User","login");
            }
            DbModel dbmodle = new DbModel();

            if (book_id != null)
            {
                
                Book one_book = dbmodle.Books.FirstOrDefault(x => x.book_id == book_id);
                if (one_book == null)
                {
                    return HttpNotFound();
                }
                //if (book_id == 4) { return HttpNotFound(); }

                if (one_book.copies > 0)
                {
                    int userID = Convert.ToInt32(Session["User"]);
                    var user2 = dbmodle.Users.Single(x => x.user_id == userID);
                    one_book.copies--;
                    Transaction trans = new Transaction()
                    {
                        book_id = one_book.book_id,
                        book_name = one_book.book_title,
                        trans_date = DateTime.Now,
                        user_id = userID,
                        user_name = user2.username

                    };
                    dbmodle.Transactions.Add(trans);
                    dbmodle.SaveChanges();
                    ViewBag.RentCompleted = "Rent Completed";
                }
                if(one_book.copies == 0)
                {
                    dbmodle.Books.Find(book_id).status = "out of stock";
                    dbmodle.SaveChanges();

                    Session["out"] = true;
                    ViewBag.outof = "Out Of Stock";
                    return RedirectToAction("outof");
                }
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        
        public ActionResult outof()
        {
            int book_id =Convert.ToInt32(Session["book"]) ;
            
            DbModel db = new DbModel();
            var op = db.Books.Find(book_id);
            op.status = "out of stock";
            return View(op);
        }

        

        public ActionResult ViewBooks(int? userID)
        {
            Session["UserID"] = userID;
            
            List<Book> all = new List<Book>();
            List<Transaction> all_tr = new List<Transaction>();

            


            if(userID == null)
            {
                return RedirectToAction("Login");
            }

            DbModel db = new DbModel();
            RentViewModel tests = new RentViewModel() {
                MyBooks = all,
                rentThat=all_tr
            };
            var rented_books_trans = db.Transactions.Where(x => x.user_id == 1);//tmam
            
            foreach (var item in rented_books_trans.ToList())
            {
                var one_item = db.Books.First(x=>x.book_id == item.book_id);
                tests.MyBooks.Add(one_item);
                tests.rentThat.Add(item);

            }

            
            return View(tests);

        }
        public ActionResult BackTo(int id1,int id2,int id3)
        {
            DbModel db = new DbModel();
            var bo = db.Books.Single(c => c.book_id == id1);
            bo.copies = bo.copies + 1;
            bo.status = null;

            Transaction t = db.Transactions.Find(id2);
            db.Transactions.Remove(t);
            db.SaveChanges();

            return RedirectToAction("Index","User");

        }
            
       


        public ActionResult Logout(int? id)
        {/*
            using (DbModel db = new DbModel()) 
            {
                var i = db.Users.Find(id);
                if (i.user_type==true)
                {
                    Session.Remove("Admin");
                }
                else
                {
                    Session.Remove("User");
                }
            }
            ModelState.Clear();
            ViewBag.logout = "logout Completed.";
            */
            return View("Index");
        }
    }


}