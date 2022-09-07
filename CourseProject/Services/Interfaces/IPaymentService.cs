using Braintree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.Services.Interfaces
{
    public interface IPaymentService
    {
        public Task<string> GenerateToken();
        public TransactionRequest CreateTransaction(decimal amount, string nonceMethod);
        public Result<Transaction> StartTransaction(TransactionRequest request);
    }
}
