using Microsoft.AspNetCore.Mvc;

namespace TicketBuyerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ILogger<TicketController> _logger;

        public TicketController(ILogger<TicketController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/flights", Name = "GetFlights")]
        public IEnumerable<string> Get()
        {
            return new string[] { "DE0001", "DE0002", "DE9999" };
        }

        [HttpGet("/discounts", Name = "GetDiscounts")]
        public IEnumerable<int> GetDiscounts()
        {
            return new int[] { 1, 2, 3, 4 };
        }

        [HttpPost()]
        public string Post([FromQuery] string flightNumber, [FromQuery] Int16 amount, [FromQuery] Int16 discount) 
        {
            decimal total = 0.0m;

            _logger.LogInformation($"Buying {amount} ticket(s) for flight {flightNumber} with discount code {discount}");

            if (amount > 1)
            {
                var ticketManager = new TicketManager();

                for (int i = 0; i < amount; i++)
                {
                    var ticket = ticketManager.BuyTicket(flightNumber, DateTime.Today, discount);
                    _logger.LogInformation($"Your ticket: {ticket}");

                    total = total + ticket.Price;
                }

                _logger.LogInformation($"Total: {total:0.00} EUR");
            }
            else
            {
                var ticketManager = new TicketManager();
                var ticket = ticketManager.BuyTicket(flightNumber, DateTime.Today, discount);
                _logger.LogInformation($"Your ticket: {ticket}");
                _logger.LogInformation($"Total: {total:0.00} EUR");
            }

            return total.ToString();
        }
    }
}