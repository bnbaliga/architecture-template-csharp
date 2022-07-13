using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Vertical.Product.Service.Data
{
    [Keyless]
    public partial class vGetAllCategory
    {
        [Required]
        [StringLength(50)]
        public string ParentProductCategoryName { get; set; }
        [StringLength(50)]
        public string ProductCategoryName { get; set; }
        public int? ProductCategoryID { get; set; }
    }
}
