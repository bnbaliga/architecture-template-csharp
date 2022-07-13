
using MediatR;
using Vertical.Product.Service.Contract.Playground;
using Vertical.Product.Service.Data;

namespace Vertical.Product.Service.Manager.Loan.Command
{

    public class BankAccountHandler : IRequestHandler<BankAccountRequest, BankAccountResponse>
    {
        private readonly BankAccountRequest _bankAccountRequest;
        private readonly AdventureWorksLT2019Context _adventureWorksLT2019Context;
        private double JustForKicksBalance = 10000;

        public BankAccountHandler(BankAccountRequest bankAccountRequest, AdventureWorksLT2019Context adventureWorksLT2019Context)
        {
            _bankAccountRequest = bankAccountRequest;
            _adventureWorksLT2019Context = adventureWorksLT2019Context;
        }

        private double Debit(double amount)
        {
            if (amount > JustForKicksBalance)
            {
                throw new ArgumentOutOfRangeException("amount");
            }

            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("amount");
            }

            JustForKicksBalance += amount;
            return JustForKicksBalance;
        }

        private double Credit(double amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("amount");
            }

            JustForKicksBalance += amount;

            return JustForKicksBalance;
        }

        //public async Task<string> Handle(BankAccountRequest request, CancellationToken cancellationToken)
        //{
        //    //if (request == null)
        //    //    throw new ArgumentNullException("Request is null");
        //    //var response = new BankAccountResponse();
        //    //double balance = 0;
        //    //if (request.TransactionType == TransactionTypes.Debit)
        //    //    balance = Debit(request.Amount);

        //    //if (request.TransactionType == TransactionTypes.Credit)
        //    //    balance = Credit(request.Amount);

        //    //response.AccountNumber = request.AccountNumber;
        //    //response.AmountCredited = request.Amount;
        //    //response.AmountDebited = request.Amount;
        //    //response.Balance = balance;

        //    return await Task.FromResult(string.Empty);
        //}

        public async Task<BankAccountResponse> Handle(BankAccountRequest request, CancellationToken cancellationToken)
        {
            var x = _adventureWorksLT2019Context.Products.Take(10);
            var y = x.ToList();

            return await Task.FromResult(new BankAccountResponse());
        }
    }
}