using Microsoft.EntityFrameworkCore;
using PhoneStore.Core;
using PhoneStore.Core.Domain;
using PhoneStore.Core.Infrastructure.Data;
using PhoneStore.Data;
using PhoneStore.Services.Accounts;
using PhoneStore.Services.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PhoneStore.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IAccountService _accountService;

        public ProductService(IRepository<Product> productRepository, IAccountService accountService)
        {
            _productRepository = productRepository;
            _accountService = accountService;
        }
        
        public IPagedList<Product> GetAllProducts(ProductFilterModel model = null, bool forConcreteUser = false, int pageSize = int.MaxValue, int page = 0)
        {
            var query = _productRepository.Table;

            if (forConcreteUser && !_accountService.IsInRole(UserRoles.Admin))
            {
                query = query.Where(p => p.UserId == _accountService.GetUserId());
            }

            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.Processor))
                {
                    query = query.Where(p => p.Processor == model.Processor);
                }

                if (!string.IsNullOrEmpty(model.OperatingSystem))
                {
                    query = query.Where(p => p.OperatingSystem == model.OperatingSystem);
                }

                if (model.Storage.HasValue)
                {
                    query = query.Where(p => p.Storage == model.Storage);
                }

                if (!string.IsNullOrEmpty(model.Name))
                {
                    query = query.Where(p => p.Name.Contains(model.Name));
                }

                if (model.ToPrice.HasValue)
                {
                    query = query.Where(p => p.Price < model.ToPrice);
                }

                if (model.FromPrice.HasValue)
                {
                    query = query.Where(p => p.Price > model.FromPrice);
                }
            }

            query = query.Include(p => p.Pictures);

            var pagedProducts = new PagedList<Product>(
                query,
                page,
                pageSize
            );

            return pagedProducts;
        }

        public virtual void UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            _productRepository.Update(product);
        }

        public virtual void InsertProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            _productRepository.Insert(product);
        }

        public virtual (Product product, bool? hasProduct) GetProductByIdForCustomer(int productId)
        {
            if (productId == 0)
                return (null, null);

            var product = _productRepository.Table.Include(p => p.Pictures).FirstOrDefault(p => p.Id == productId && 
            (p.UserId == _accountService.GetUserId() || _accountService.IsInRole(UserRoles.Admin)));
            
            return (product, product != null ? true : false);
        }

        public virtual Product GetProductById(int productId)
        {
            if (productId == 0)
                return null;

            return _productRepository.Table.Include(p => p.Pictures).FirstOrDefault(p => p.Id == productId);
        }

        public void DeleteProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            _productRepository.Delete(product);
        }
    }
}
