
using Grpc.Core;
using ITI.GRPCLAB.Client.Models;
using ITI.GRPCLAB.Client.Protos;
using static ITI.GRPCLAB.Client.Protos.InventoryService;


namespace ITI.GRPCLAB.Client.Services
{
    public class ProductService 
    {
        private readonly ILogger<ProductService> _logger;

        public ProductService(ILogger<ProductService> logger)
        {
            _logger = logger;
        }
        public async Task<GetByIdResponse> GetById(GetByIdRequest request, ServerCallContext context)
        {
            var prod = ProductList.Products.FirstOrDefault(p => p.Id == request.Id);
            bool res;
            if (prod == null)
            {
                res = false;
            }
            else
            {
                res = true;
            }
            return await Task.FromResult(new GetByIdResponse
            {
                Result = res
            });
        }
        public async  Task<ProductRequest> AddProduct(ProductRequest request, ServerCallContext context)
        {
            ProductList.Products.Add(new Product() { Id = request.Id, Name = request.Name, Price = request.Price, Category = (int)request.Category });
            return await Task.FromResult(request);
        }
        public async  Task<ProductRequest> UpdateProduct(ProductRequest request, ServerCallContext context)
        {
            var prod = ProductList.Products.FirstOrDefault(p => p.Id == request.Id);
            bool res;
            if (prod == null)
            {
                prod.Id = request.Id; prod.Name = request.Name; prod.Price = request.Price; prod.Category = (int)request.Category;
            }
            return await Task.FromResult(request);
        }

        public async  Task<BulkResponse> AddBulkProducts(IAsyncStreamReader<ProductRequest> requestStream, ServerCallContext context)
        {
            int num = 0;
            await foreach (var request in requestStream.ReadAllAsync())
            {
                AddProduct(request, context);
                num++;
            }
            return await Task.FromResult(new BulkResponse() { NumOfProducts = num });
        }

        public async Task getReport(ReportCriteria request, IServerStreamWriter<ProductRequest> responseStream, ServerCallContext context)
        {
            var filteredProducts = ProductList.Products.AsQueryable();
            filteredProducts = filteredProducts.Where(p => p.Category == (int)request.Category);

            if (request.IsOrderedByPrice)
                filteredProducts = filteredProducts.OrderBy(p => p.Price);

            foreach (var product in filteredProducts)
            {
                var NewProduct = new ProductRequest
                {
                    Id = product.Id,
                    Name = product.Name,
                    Quantity = product.Quantity,
                    Price = product.Price,
                    ExpireDate = product.ExpireDate,
                    Category = (Protos.Category)product.Category
                };

                await responseStream.WriteAsync(NewProduct);
            }
        }


    }
}
