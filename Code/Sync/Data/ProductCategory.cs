using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Vertical.Product.Service.Data
{
    /// <summary>
    /// High-level product categorization.
    /// </summary>
    [Table("ProductCategory", Schema = "SalesLT")]
    [Index(nameof(Name), Name = "AK_ProductCategory_Name", IsUnique = true)]
    [Index(nameof(rowguid), Name = "AK_ProductCategory_rowguid", IsUnique = true)]
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            InverseParentProductCategory = new HashSet<ProductCategory>();
            Products = new HashSet<Product>();
        }


        /// <summary>
        /// Primary key for ProductCategory records.
        /// </summary>
        [Key]
        public int ProductCategoryID { get; set; }

        /// <summary>
        /// Product category identification number of immediate ancestor category. Foreign key to ProductCategory.ProductCategoryID.
        /// </summary>
        public int? ParentProductCategoryID { get; set; }

        /// <summary>
        /// Category description.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
        /// </summary>
        public Guid rowguid { get; set; }

        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey(nameof(ParentProductCategoryID))]
        [InverseProperty(nameof(ProductCategory.InverseParentProductCategory))]
        public virtual ProductCategory ParentProductCategory { get; set; }
        [InverseProperty(nameof(ProductCategory.ParentProductCategory))]
        public virtual ICollection<ProductCategory> InverseParentProductCategory { get; set; }
        [InverseProperty(nameof(Product.ProductCategory))]
        public virtual ICollection<Product> Products { get; set; }
    }
}
