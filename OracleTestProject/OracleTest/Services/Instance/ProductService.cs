using OracleTest.Models.Data;
using OracleTest.Models.Request;
using OracleTest.Models.Response;
using OracleTest.Repositories.Interface;
using OracleTest.Services.Interface;
using System.Transactions;

namespace OracleTest.Services.Instance
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductService(
            IProductRepository productRepository,
            IProductCategoryRepository productCategoryRepository
        )
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
        }

        public GetProductRP GetProduct(long id)
        {
            return _productRepository.GetProduct(id);
        }

        public PageDataRP<IEnumerable<QueryProductRP>> QueryProduct(QueryProductRQ request, string version)
        {
            Dictionary<string, object> dicParams = new Dictionary<string, object>();
            if (request.CategoryId.HasValue)
            {
                dicParams["CategoryId"] = request.CategoryId;
            }
            if (string.IsNullOrWhiteSpace(request.CategoryIds) == false)
            {
                var categoryIds = request.CategoryIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                     .Select(e => Convert.ToInt64(e));
                if (categoryIds.Any())
                {
                    dicParams["CategoryIds"] = categoryIds;
                }
            }
            if (string.IsNullOrWhiteSpace(request.ProductName) == false)
            {
                dicParams["ProductName"] = $"{request.ProductName}%".ToLower();
            }
            dicParams["RowStart"] = (request.PageIndex - 1) * request.PageSize;
            dicParams["RowLength"] = request.PageSize;

            var res = (0, (IEnumerable<QueryProductRP>)null);
            switch (version)
            {
                case "V1":
                    res = _productRepository.QueryProductV1(dicParams);
                    break;

                case "V2":
                    res = _productRepository.QueryProductV2(dicParams);
                    break;

                case "V3":
                    res = _productRepository.QueryProductV3(dicParams);
                    break;

                case "V4":
                    res = _productRepository.QueryProductV4(dicParams);
                    break;

                case "V5":
                    res = _productRepository.QueryProductV5(dicParams);
                    break;

                case "V6":
                    res = _productRepository.QueryProductV6(dicParams);
                    break;
            }

            return new PageDataRP<IEnumerable<QueryProductRP>>
            {
                PageInfo = new PageInfoRP
                {
                    PageIndex = request.PageIndex,
                    PageSize = res.Item2.Count(),
                    PageCnt = (res.Item1 % request.PageSize == 0 ? res.Item1 / request.PageSize : res.Item1 / request.PageSize + 1),
                    TotalCnt = res.Item1
                },
                Data = res.Item2
            };
        }

        public GetProductRP InsertProduct(InsertProductRQ request)
        {
            var objCategory = _productCategoryRepository.GetProductCategory(request.CategoryId.Value);
            if (objCategory == null)
            {
                return null;
            }

            var objProduct = new Product
            {
                ProductName = request.ProductName,
                Description = request.Description,
                StandardCost = request.StandardCost,
                ListPrice = request.ListPrice,
                CategoryId = request.CategoryId.Value
            };
            var res = _productRepository.InsertProduct(objProduct);

            return res == false ? null : _productRepository.GetProduct(objProduct.ProductId);
        }

        public GetProductRP UpdateProduct(long id, UpdateProductRQ request)
        {
            var objOrigin = _productRepository.GetProduct(id);
            if (objOrigin == null)
            {
                return null;
            }
            if (request.CategoryId.HasValue)
            {
                var objCategory = _productCategoryRepository.GetProductCategory(request.CategoryId.Value);
                if (objCategory == null)
                {
                    return null;
                }
            }

            var objProduct = new Product
            {
                ProductId = id,
                ProductName = string.IsNullOrWhiteSpace(request.ProductName) ? objOrigin.ProductName : request.ProductName,
                Description = string.IsNullOrWhiteSpace(request.Description) ? objOrigin.Description : request.Description,
                StandardCost = request.StandardCost ?? objOrigin.StandardCost,
                ListPrice = request.ListPrice ?? objOrigin.ListPrice,
                CategoryId = request.CategoryId ?? objOrigin.CategoryId
            };
            var res = _productRepository.UpdateProduct(objProduct);

            return res == false ? null : _productRepository.GetProduct(objProduct.ProductId);
        }

        public bool DeleteProduct(IEnumerable<long> ids)
        {
            if (ids.Any())
            {
                return _productRepository.DeleteProduct(ids);
            }
            else
            {
                return false;
            }
        }

        public GetProductRP InsertProductAndCategoryV1(InsertProductAndCategoryRQ request)
        {
            var objProduct = new Product
            {
                ProductName = request.ProductName,
                Description = request.Description,
                StandardCost = request.StandardCost,
                ListPrice = request.ListPrice
            };
            var objProductCategory = new ProductCategory
            {
                CategoryName = request.CategoryName
            };
            var res = _productRepository.InsertProductAndCategory(objProduct, objProductCategory);

            return res == false ? null : _productRepository.GetProduct(objProduct.ProductId);
        }

        public GetProductRP InsertProductAndCategoryV2(InsertProductAndCategoryRQ request)
        {
            var objProduct = new Product
            {
                ProductName = request.ProductName,
                Description = request.Description,
                StandardCost = request.StandardCost,
                ListPrice = request.ListPrice
            };
            var objProductCategory = new ProductCategory
            {
                CategoryName = request.CategoryName
            };

            bool resCategory = false;
            bool resProduct = false;
            using (var scope = new TransactionScope())
            {
                resCategory = _productCategoryRepository.InsertProductCategory(objProductCategory);
                objProduct.CategoryId = objProductCategory.CategoryId;
                resProduct = _productRepository.InsertProduct(objProduct);

                scope.Complete();
            }

            return resCategory & resProduct == false ? null : _productRepository.GetProduct(objProduct.ProductId);
        }
    }
}
