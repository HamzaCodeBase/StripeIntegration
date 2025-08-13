using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using StripeIntegration.Models;

namespace StripeIntegration.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IConfiguration _configuration;
        public CheckoutController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult CreateCheckoutSession(CheckoutFormModel model)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = model.Currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = model.ProductName,
                            Description = model.ProductDescription
                        },
                        UnitAmount = model.Amount
                    },
                    Quantity = 1
                }
            },
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel"
            };

            var service = new SessionService();
            var session = service.Create(options);

            // Redirect to Stripe Checkout
            return Redirect(session.Url);
        }

        [HttpGet]
        public IActionResult Success() => View();

        [HttpGet]
        public IActionResult Cancel() => View();
    }
}
