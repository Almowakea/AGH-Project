using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AGH
{
    [MetadataType(typeof(CourseAttribs))]
    public partial class Course
    {

    }

    public class CourseAttribs
    {

        [Display(Name = "Course Name")]
        [Required(ErrorMessage = "Course name field is required")]
        public string Course_Name { get; set; }

        [Display(Name = "Course Code")]
        [Required(ErrorMessage = "Course code field is required")]
        public string Course_Code { get; set; }

        [Display(Name = "Course Description")]
        [Required(ErrorMessage = "Course description field is required")]
        public string Course_Description { get; set; }

        [Display(Name = "Course Credit")]
        [Required(ErrorMessage = "Course credit field is required")]
        public int Course_Credit { get; set; }

        [Display(Name = "Instructor Name")]
        [Required(ErrorMessage = "Instructor name field is required")]
        public int Course_Instructor_ID { get; set; }

        [Display(Name = "Assistant Name")]
        [Required(ErrorMessage = "Assistant name field is required")]
        public int Course_Assistant_ID { get; set; }
    }
}