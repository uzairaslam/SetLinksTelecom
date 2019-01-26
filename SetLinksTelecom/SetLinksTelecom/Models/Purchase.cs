using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }

        [MaxLength(150)]
        public string Comments { get; set; }
        [Display(Name = "Quantity")]
        public int Qty { get; set; }
        public decimal PaidAmount { get; set; }

        [Display(Name = "Item")]
        public int ItemId { get; set; }

        [NotMapped]
        [ForeignKey("ItemId")]
        public Item Item { get; set; }


        [MinLength(3), MaxLength(50)]
        public string Subname { get; set; }

        [Display(Name = "Portal")]
        public int PortalId { get; set; }
        [NotMapped]
        [ForeignKey("PortalId")]
        public Portal Portal { get; set; }

        public Purchase()
        {
            Item = new Item();
        }
    }
}