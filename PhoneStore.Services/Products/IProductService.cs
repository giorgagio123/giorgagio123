using PhoneStore.Core;
using PhoneStore.Core.Domain;
using PhoneStore.Services.Models.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneStore.Services.Products
{
    public interface IProductService
    {
        IPagedList<Product> GetAllProducts(ProductFilterModel model = null, bool forConcreteUser = false, int pageSize = int.MaxValue, int page = 0);

        Product GetProductById(int productId);

        (Product product, bool? hasProduct) GetProductByIdForCustomer(int productId);

        void InsertProduct(Product product);

        void UpdateProduct(Product product);

        void DeleteProduct(Product product);
    }
}
