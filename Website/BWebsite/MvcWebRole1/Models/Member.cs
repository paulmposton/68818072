using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Bettery.WebRole.Services;
using System.Text.RegularExpressions;
using MvcWebRole1.Common;

namespace Bettery.WebRole.Models
{
    public class Member
    {

        public int MemberID {get; set; }
        public string CustomerProfileID { get; set; }

        [Required(ErrorMessage = "First Name is required")] 
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")] 
        public string LastName { get; set; }
        
        //[Required] 
        public string Address1 { get; set; }
        
        public string Address2 { get; set; }
        
        //[Required] 
        public string City { get; set; }
        
        //[Required] 
        public string State { get; set; }

        [Required(ErrorMessage = "Zipcode is required")] 
        public string Zipcode { get; set; }
        
        //[Required] 
        public string Country { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [EmailUsed(ErrorMessage = "You or someone has already created a membership account with this email address.")]
        [ValidEmail(ErrorMessage = "Please enter a valid email address.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")] 
        [StringLength(12, MinimumLength=6, ErrorMessage="Your password must be at least 6 characters and less than 50 characters in length.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords must match")]
        public string RegConfirmPassword { get; set; }

        public bool EmailOptIn { get; set; }

        public int FreeCases { get; set; }

        [Required]
        [TermsAccept(ErrorMessage = "You must agree to the terms and conditions.")]
        public bool AcceptTerms { get; set; }

    }
    public class EditMember
    {

        public string Command { get; set; }

        public int MemberID { get; set; }
        
        public string CustomerProfileID { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        //[Required] 
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        //[Required] 
        public string City { get; set; }

        //[Required] 
        public string State { get; set; }

        [Required(ErrorMessage = "Zipcode is required")]
        public string Zipcode { get; set; }

        //[Required] 
        public string Country { get; set; }

        public bool EmailOptIn { get; set; }

        public int FreeCases { get; set; }

        //public List<SelectListItem> CorporateAccount { get; set; }

        //[Display(Name = "Corporate Account")]
        //public string CorpAccountID { get; set; }

    }   
    public class DeleteMember
    {
        public int MemberID { get; set; }
    }

    public class EmailUsed : ValidationAttribute
    {
        public override bool IsValid(object propertyValue)
        {
            AccountService accountServices = new AccountService();
            return accountServices.CheckEmailDoesntExist(propertyValue.ToString());
        }
    }
    public class ValidEmail : ValidationAttribute
    {
        public override bool IsValid(object propertyValue)
        {
            Regex regex = new Regex(Constants.Messages.MatchEmailPattern);
            bool isValidEmail = regex.IsMatch(propertyValue.ToString());

            return isValidEmail;
        }
    }
    public class TermsAccept : ValidationAttribute
    {
        public override bool IsValid(object propertyValue)
        {
            return propertyValue != null
                && propertyValue is bool
                && (bool)propertyValue;
        }
    }
    public class MemberDBContext : DbContext
    {
        public DbSet<Member> Members { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>().ToTable("dbo.Member");

        }

    }
}