﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Vertical.Product.Service.Data
{
    /// <summary>
    /// Customer information.
    /// </summary>
    [Table("Customer", Schema = "SalesLT")]
    [Index(nameof(rowguid), Name = "AK_Customer_rowguid", IsUnique = true)]
    [Index(nameof(EmailAddress), Name = "IX_Customer_EmailAddress")]
    public partial class Customer
    {
        public Customer()
        {
            CustomerAddresses = new HashSet<CustomerAddress>();
            SalesOrderHeaders = new HashSet<SalesOrderHeader>();
        }


        /// <summary>
        /// Primary key for Customer records.
        /// </summary>
        [Key]
        public int CustomerID { get; set; }

        /// <summary>
        /// 0 = The data in FirstName and LastName are stored in western style (first name, last name) order.  1 = Eastern style (last name, first name) order.
        /// </summary>
        public bool NameStyle { get; set; }

        /// <summary>
        /// A courtesy title. For example, Mr. or Ms.
        /// </summary>
        [StringLength(8)]
        public string Title { get; set; }

        /// <summary>
        /// First name of the person.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// Middle name or middle initial of the person.
        /// </summary>
        [StringLength(50)]
        public string MiddleName { get; set; }

        /// <summary>
        /// Last name of the person.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        /// <summary>
        /// Surname suffix. For example, Sr. or Jr.
        /// </summary>
        [StringLength(10)]
        public string Suffix { get; set; }

        /// <summary>
        /// The customer&apos;s organization.
        /// </summary>
        [StringLength(128)]
        public string CompanyName { get; set; }

        /// <summary>
        /// The customer&apos;s sales person, an employee of AdventureWorks Cycles.
        /// </summary>
        [StringLength(256)]
        public string SalesPerson { get; set; }

        /// <summary>
        /// E-mail address for the person.
        /// </summary>
        [StringLength(50)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Phone number associated with the person.
        /// </summary>
        [StringLength(25)]
        public string Phone { get; set; }

        /// <summary>
        /// Password for the e-mail account.
        /// </summary>
        [Required]
        [StringLength(128)]
        [Unicode(false)]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Random value concatenated with the password string before the password is hashed.
        /// </summary>
        [Required]
        [StringLength(10)]
        [Unicode(false)]
        public string PasswordSalt { get; set; }

        /// <summary>
        /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
        /// </summary>
        public Guid rowguid { get; set; }

        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [InverseProperty(nameof(CustomerAddress.Customer))]
        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
        [InverseProperty(nameof(SalesOrderHeader.Customer))]
        public virtual ICollection<SalesOrderHeader> SalesOrderHeaders { get; set; }
    }
}
