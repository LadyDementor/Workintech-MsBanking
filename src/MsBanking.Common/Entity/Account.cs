using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsBanking.Common.Entity
{
    public class Account:BaseEntity//Vertabanından silinmemesi gereken datalardan olduğu için BaseEntity'den türetildi.
    {

        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string IbanNumber { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public string UsetId { get; set; }
        public int BranchId { get; set; }
        public int AccountSuffix { get; set; }//Hesap türüne göre 1,2,3 gibi.Is active eklenecek silinsede databasede olsun diye 
    }
}
 