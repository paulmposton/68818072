using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;

namespace Bettery.WebRole.Models
{
    public enum PromoTypes
    {
        [Display(Name = "Swap", Order = 0)]
        Swap = 5,
        [Display(Name = "Purchase", Order = 1)]
        Purchase = 6,
        [Display(Name = "Both", Order = 2)]
        Both = 7
    }

    public class AddPromoModel
    {
        public string Message { get; set; }

        public string Command { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [Display(Name = "Promo Code")]
        public string PromoCode { get; set; }

        [Display(Name = "Promo Amount")]
        public decimal PromoAmount { get; set; }

        [Required]
        [Display(Name = "Promo Type")]
        public PromoTypes PromoTypes { get; set; }

        [Display(Name = "Multiple Use")]
        public bool MultipleUse { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Expiration Date")]
        public DateTime ExpireDate { get; set; }

        [Required]
        [Display(Name = "Notes")]
        public string Description { get; set; }

        public int CreatedBy { get; set; }

    }

}