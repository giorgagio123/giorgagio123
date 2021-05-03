using PhoneStore.Models.Product;
using PhoneStore.Services.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Factories
{
    public interface IProductModelFactory
    {
        PagedProductViewModel PrepareProductmodel(ProductFilterViewModel filterModel = null);

        ProductFilterViewModel PrepreProductFilterModel();

        ProductDetailsViewModel PrepareProductDetailsModel(int productId);
    }
}
