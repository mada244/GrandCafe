using CoffeeShopClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace MongoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : MongoController<CoffeeShopClassLibrary.Models.Product>
    {
        UserController _userController;

        public ProductController() : base("product")
        {
            _userController = new UserController();
        }

        [HttpPost("create-checkout-session")]
        public ActionResult<string> CreateCheckoutSession(List<CoffeeShopClassLibrary.Models.Product> products)
        {
            const int factor = 100;
            var sum = (products.Sum(x => int.Parse(x.Pret.Split(' ')[0]))) * factor;
            StripeConfiguration.ApiKey = "sk_test_51PVEn5LkYUXdq6uGONYAvKuDFzZZtQ7hksyFWcvPDbMOunggtDX5WNLPuQFzsoUQPk7xO7N9DBNZFdbmJcgmB71u009t5mTFu7";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
            "card",
        },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = sum,
                    Currency = "ron",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "GrandCaffe Pay",
                    },
                },
                Quantity = 1,
            },

        },
                Mode = "payment",
                SuccessUrl = "https://example.com/success",
                CancelUrl = "https://example.com/cancel",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return System.Text.Json.JsonSerializer.Serialize(session.Url);
        }
    }
}