using Microsoft.AspNetCore.Identity;
using PhoneStore.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneStore.Core.Domain
{
    public class ApplicationUser : IdentityUser
    {
        private ICollection<Product> _product;

        public virtual ICollection<Product> Products
        {
            get { return _product ?? (_product = new List<Product>()); }
            protected set { _product = value; }
        }
    }
}
