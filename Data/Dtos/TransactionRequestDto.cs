using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Commons;

namespace Data.Dtos
{
    public class TransactionRequestDto
    {
        //public string TransactionUniqueReference { get; set; }
        public decimal TransactionAmount { get; set; }
        //public TranStatus TransactionStatus { get; set; }
        //public bool IsSuccessful => TransactionStatus.Equals(TranStatus.Success);
        public string TransactionSourceAccount { get; set; }
        public string TransactionDestinationAccount { get; set; }
        //public string TransactionParticulars { get; set; }
        public TranType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
