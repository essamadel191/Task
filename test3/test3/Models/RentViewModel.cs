using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test3.Models
{
    public class RentViewModel
    {
        public List<Book> MyBooks { get; set; }
        
        
        //userID
      //  public int me { get; set; }

        public List<Transaction> rentThat { get; set; }

    }
}