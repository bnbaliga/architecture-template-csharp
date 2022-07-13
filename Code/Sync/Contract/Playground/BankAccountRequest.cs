using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Vertical.Product.Service.Contract.Playground
{
    public class BankAccountRequest : IRequest<BankAccountResponse>
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int AccountNumber { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{4,4}$", ErrorMessage = "Pin must be of 4 digits")]
        public int Pin { get; set; }
        [Required]
        public TransactionTypes TransactionType { get; set; }
        [Required]
        public double Amount { get; set; }
    }

    public enum TransactionTypes
    {
        Debit,
        Credit
    }

    public class BankAccountResponse
    {
        public int AccountNumber { get; set; }
        public string? Customer { get; set; }
        public double Balance { get; set; }
        public double AmountDebited { get; set; }
        public double AmountCredited { get; set; }
    }
}
