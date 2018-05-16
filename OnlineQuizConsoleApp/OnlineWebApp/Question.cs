//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineWebApp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            this.Answers = new HashSet<Answer>();
        }
    
        public int ID { get; set; }
        public string Author_Username { get; set; }
        [Display(Name = "Option 1")]
        [Required(ErrorMessage = "This field is required")]
        public string option_1 { get; set; }
        [Display(Name = "Option 2")]
        [Required(ErrorMessage = "This field is required")]
        public string option_2 { get; set; }
        [Display(Name = "Option 3")]
        [Required(ErrorMessage = "This field is required")]
        public string option_3 { get; set; }
        [Display(Name = "Option 4")]
        [Required(ErrorMessage = "This field is required")]
        public string option_4 { get; set; }
        [Display(Name = "Right Option (1-4)")]
        [Required(ErrorMessage = "This field is required")]
        [Range(1, 4, ErrorMessage = "Right option should be between 1 and 4")]
        public int right_option { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Title { get; set; }
        [Display(Name = "Category")]
        public int Categories_Category_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
    }
}
