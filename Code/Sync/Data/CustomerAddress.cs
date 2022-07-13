using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Vertical.Product.Service.Data
{
    /// <summary>
    /// Cross-reference table mapping customers to their address(es).
    /// </summary>
    [Table("CustomerAddress", Schema = "SalesLT")]
    [Index(nameof(rowguid), Name = "AK_CustomerAddress_rowguid", IsUnique = true)]
    public partial class CustomerAddress
    {

        /// <summary>
        /// Primary key. Foreign key to Customer.CustomerID.
        /// </summary>
        [Key]
        public int CustomerID { get; set; }

        /// <summary>
        /// Primary key. Foreign key to Address.AddressID.
        /// </summary>
        [Key]
        public int AddressID { get; set; }

        /// <summary>
        /// The kind of Address. One of: Archive, Billing, Home, Main Office, Primary, Shipping
        /// </summary>
        [Required]
        [StringLength(50)]
        public string AddressType { get; set; }

        /// <summary>
        /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
        /// </summary>
        public Guid rowguid { get; set; }

        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey(nameof(AddressID))]
        [InverseProperty("CustomerAddresses")]
        public virtual Address Address { get; set; }
        [ForeignKey(nameof(CustomerID))]
        [InverseProperty("CustomerAddresses")]
        public virtual Customer Customer { get; set; }
    }
}
