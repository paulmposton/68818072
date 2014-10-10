using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;

namespace Bettery.WebRole.Models
{

    public class ChangePasswordModel
    {
        public int MemberID { get; set; }
        
        //[Required]
        //[DataType(DataType.Password)]
        //[Display(Name = "Current password")]
        //public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LogInModel
    {

        [Required]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public int MemberID { get; set; }
        public int GroupID { get; set; }
    }

    public class RegisterModel
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }


    public class RequestRecoverPasswordModel
    {

        [Required]
        [ValidEmail(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Display(Name = "Confirm Email")]
        [Compare("EmailAddress", ErrorMessage = "The Email and confirmation Email do not match.")]
        public string ConfirmEmailAddress { get; set; }

        public string PasswordVerifiationToken;


    }


    public class RecoverPasswordModel
    {

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string RecoverID;
        public string PassCode;

    }

    public class Credit
    {

        public decimal AccountBalance { get; set; }
        //public DateTime IssueDate { get; set; }
        //public string InvoiceNumber{ get; set; }
         
    }

    public class Transaction
    {
        private int vend;
        public int TransactionID { get; set; }
        public decimal TotalAmount { get; set; }
        public int AAVend { get; set; }
        public int AAAVend { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public int Vend
        {
            get
            {
                return AAAVend + AAVend;
            }
            set
            {
                vend = value;
            }
        }

    }

    public class TransactionDetail
    {
        public decimal TotalAmount { get; set; }
        public decimal Deposits { get; set; }
        public int AAVend { get; set; }
        public int AAReturn { get; set; }
        public int AAAVend { get; set; }
        public decimal SwapPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public string Store { get; set; }
        public string CardInfo { get; set; }
        public decimal PromoCodeAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public DateTime TransactionDateTime { get; set; }
    }

    public class CreditCard
    {
        public string CCNumber { get; set; }
        public string CCLastFourDigits { get; set; }
        public long CustomerProfileID { get; set; }
        public long PaymentProfileID { get; set; }
        public string EmailAddress { get; set; }
        
        
        public string CCExpireDate { get; set; }


        [Display(Name = "Zipcode")]
        public string Zipcode { get; set; }


        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        public string LastName { get; set; }

    }
    //public class MovieDBContext : DbContext
    //{
    //    public DbSet<CreditsModel> Credits { get; set; }
        
    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Entity<CreditsModel>().ToTable("dbo.Member");
                    
    //    }

    //}
}
