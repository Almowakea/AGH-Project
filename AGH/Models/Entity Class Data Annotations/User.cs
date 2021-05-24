using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AGH
{
    [MetadataType(typeof(UserAttribs))]
    public partial class User
    {

    }

    public class UserAttribs
    {

        [Display(Name = "User Type")]
        [Required(ErrorMessage = "User type is required")]
        public int User_Type_ID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name field is required")]
        public string User_First_Name { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name field is required")]
        public string User_Last_Name { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone number field is required")]
        public string User_Phone_Number { get; set; }

        [Display(Name = "Email Adress")]
        [Required(ErrorMessage = "Email field is required")]
        [EmailAddress(ErrorMessage = "Kindly enter a valid email address")]
        public string User_Email { get; set; }

        [Display(Name = "ID Number")]
        [Required(ErrorMessage = "User ID number is required")]
        public int User_ID { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password field is required")]
        [DataType(DataType.Password)]
        public string User_Password { get; set; }
    }
}