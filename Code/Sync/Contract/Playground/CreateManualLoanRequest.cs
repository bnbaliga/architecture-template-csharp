using MediatR;

namespace Vertical.Product.Service.Contract.Playground
{
    public class CreateManualLoanRequest : IRequest<string>
    {
        public string? BorrowerFirstName { get; set; }
        public string? BorrowerLastName { get; set; }
        public DateTime BorrowerDOB { get; set; }
        public string? PropertyAddress { get; set; }
        public int LoanType { get; set; }
    }
}
