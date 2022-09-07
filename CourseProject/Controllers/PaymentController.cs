using CourseProject.Models.ViewModels;
using CourseProject.Services;
using CourseProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    public class PaymentController : Controller
    {
        private IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = await _paymentService.GenerateToken();
            ViewBag.Token = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(TransactionViewModel transactionVM)
        {
            
            var request = _paymentService.CreateTransaction(transactionVM.Amount, transactionVM.NonceMethod);
            var transaction = _paymentService.StartTransaction(request);

            ViewBag.IsSuccess = transaction.IsSuccess();
            if (transaction.IsSuccess())
            {
                ViewBag.Result = "Transaction was succesful";
            }
            else
            {
                ViewBag.Result = "Error. Transaction was canceled";
            }
            return View();
        }
    }
}
