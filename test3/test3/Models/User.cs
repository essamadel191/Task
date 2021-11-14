//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace test3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Transactions = new HashSet<Transaction>();
        }
    
        public int user_id { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("User Name")]
        public string username { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string pass { get; set; }

        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string email { get; set; }
        [DisplayName("Full Name")]
        public string fullName { get; set; }
        [DisplayName("Age")]
        public Nullable<int> age { get; set; }
        [DisplayName("Is Admin")]
        public Nullable<bool> user_type { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}