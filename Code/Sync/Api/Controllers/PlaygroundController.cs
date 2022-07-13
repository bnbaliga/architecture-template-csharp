using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Vertical.Product.Service.Contract.Playground;

namespace Vertical.Product.Service.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("v1/[controller]")]
    public class PlaygroundController : Controller
    {

        private readonly IMediator _mediator;
        private readonly ILogger<PlaygroundController> _logger;

        public PlaygroundController(IMediator mediator, ILogger<PlaygroundController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [Route("CreateManualLoan")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<string>> CreateManualLoan(CreateManualLoanRequest request)
        {
            var x = HttpContext.User.Identity.Name;
            var result = await _mediator.Send(request);

            return Ok(result);

        }

        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [Route("BankTransaction")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult<string>> BankTransaction(BankAccountRequest request)
        {
            var result = await _mediator.Send(request);

            return Ok(result);

        }
    }
}
