using Microsoft.AspNetCore.Mvc;
using OracleTest.Models.Request;
using OracleTest.Models.Response;
using OracleTest.Services.Interface;

namespace OracleTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// 單筆查詢
        /// </summary>
        [HttpGet]
        [Route("GetProduct")]
        public GetProductRP GetProduct([FromQuery] IdRQ request)
        {
            return _productService.GetProduct(request.ID.Value);
        }

        /// <summary>
        /// 多筆查詢 (Query Twice範例)
        /// </summary>
        [HttpGet]
        [Route("QueryProductV1")]
        public PageDataRP<IEnumerable<QueryProductRP>> QueryProductV1([FromQuery] QueryProductRQ request)
        {
            return _productService.QueryProduct(request, "V1");
        }

        /// <summary>
        /// 多筆查詢 (Collection範例)
        /// </summary>
        [HttpGet]
        [Route("QueryProductV2")]
        public PageDataRP<IEnumerable<QueryProductRP>> QueryProductV2([FromQuery] QueryProductRQ request)
        {
            return _productService.QueryProduct(request, "V2");
        }

        /// <summary>
        /// 多筆查詢 (Count Over範例)
        /// </summary>
        [HttpGet]
        [Route("QueryProductV3")]
        public PageDataRP<IEnumerable<QueryProductRP>> QueryProductV3([FromQuery] QueryProductRQ request)
        {
            return _productService.QueryProduct(request, "V3");
        }

        /// <summary>
        /// 多筆查詢 (With As範例)
        /// </summary>
        [HttpGet]
        [Route("QueryProductV4")]
        public PageDataRP<IEnumerable<QueryProductRP>> QueryProductV4([FromQuery] QueryProductRQ request)
        {
            return _productService.QueryProduct(request, "V4");
        }

        /// <summary>
        /// 多筆查詢 (UDT範例)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("QueryProductV5")]
        public PageDataRP<IEnumerable<QueryProductRP>> QueryProductV5([FromQuery] QueryProductRQ request)
        {
            return _productService.QueryProduct(request, "V5");
        }

        /// <summary>
        /// 多筆查詢 (Package範例)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("QueryProductV6")]
        public PageDataRP<IEnumerable<QueryProductRP>> QueryProductV6([FromQuery] QueryProductRQ request)
        {
            return _productService.QueryProduct(request, "V6");
        }

        /// <summary>
        /// 新增
        /// </summary>
        [HttpPost]
        [Route("InsertProduct")]
        public GetProductRP InsertProduct([FromBody] InsertProductRQ request)
        {
            return _productService.InsertProduct(request);
        }

        /// <summary>
        /// 修改
        /// </summary>
        [HttpPatch]
        [Route("UpdateProduct/{id}")]
        public GetProductRP UpdateProduct([FromRoute] long id, [FromBody] UpdateProductRQ request)
        {
            return _productService.UpdateProduct(id, request);
        }

        /// <summary>
        /// 刪除
        /// </summary>
        [HttpDelete]
        [Route("DeleteProduct/{id}")]
        public bool DeleteProduct([FromRoute] long id)
        {
            return _productService.DeleteProduct(new List<long> { id });
        }

        /// <summary>
        /// 新增 (Transaction範例)
        /// </summary>
        [HttpPost]
        [Route("InsertProductAndCategoryV1")]
        public GetProductRP InsertProductAndCategoryV1([FromBody] InsertProductAndCategoryRQ request)
        {
            return _productService.InsertProductAndCategoryV1(request);
        }

        /// <summary>
        /// 新增 (TransactionScope範例)
        /// </summary>
        [HttpPost]
        [Route("InsertProductAndCategoryV2")]
        public GetProductRP InsertProductAndCategoryV2([FromBody] InsertProductAndCategoryRQ request)
        {
            return _productService.InsertProductAndCategoryV2(request);
        }
    }
}
