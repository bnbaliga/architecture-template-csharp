using MediatR;
using Microsoft.Extensions.Options;
using Vertical.Product.Service.Contract.AppSettings;
using Vertical.Product.Service.Contract.Playground;
using Vertical.Product.Service.Data;

namespace Vertical.Product.Service.Manager.Loan.Command
{
    public class CreateManualLoanHandler : IRequestHandler<CreateManualLoanRequest, string>
    {
        private readonly IOptionsMonitor<DataConfiguration> _dataConfiguration;
        public CreateManualLoanHandler(IOptionsMonitor<DataConfiguration> dataConfiguration)
        {
            _dataConfiguration = dataConfiguration;
        }

        public async Task<string> Handle(CreateManualLoanRequest request, CancellationToken cancellationToken)
        {

            return await Task.FromResult(Guid.NewGuid().ToString());
        }

        private void GetData()
        {
            //using (var context = new AdventureWorksLT2019Context( _dataConfiguration.CurrentValue.DefaultConnection))
            //{

            //}

        }
    }
}
