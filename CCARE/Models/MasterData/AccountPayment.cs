using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCARE.Models
{
    public class AccountPayment
    {
        [Key]
        public Guid? AccountPaymentID { get; set; }

        [StringLength(200)]
        public string NamaPerusahaan { get; set; }

        [StringLength(200)]
        public string JenisPembayaran { get; set; }
        
        [StringLength(200)]
        public string KodePerusahaan { get; set; }
        
        [StringLength(200)]
        public string NoRekPerusahaan { get; set; }
        
        [StringLength(200)]
        public string CabangKoordinator { get; set; }
        
        [StringLength(200)]
        public string JenisKerjasama { get; set; }
        
        [StringLength(200)]
        public string AlurTransaksiATM { get; set; }
        
        [StringLength(200)]
        public string EBanking { get; set; }
        
        [StringLength(200)]
        public string SandiPerusahaanMBCA { get; set; }
        
        [StringLength(200)]
        public string Limit { get; set; }
        
        [StringLength(200)]
        public string DenominasiVoucher { get; set; }

        public Guid? CreatedBy { get; set; }

        [StringLength(160)]
        public string CreatedByName { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        [StringLength(160)]
        public string ModifiedByName { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [StringLength(160)]
        public string StateLabel { get; set; }

        public int DeletionStateCode { get; set; }

        public int StateCode { get; set; }

        public int StatusCode { get; set; }
    }
}