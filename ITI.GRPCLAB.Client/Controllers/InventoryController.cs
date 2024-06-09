using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using ITI.GRPCLAB.Client.Protos;
using Grpc.Core;


namespace ITI.GRPCLAB.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        public GrpcChannel channel;
        InventoryService.InventoryServiceClient client;
        public InventoryController()
        {
            channel = GrpcChannel.ForAddress("https://localhost:7070");
            client = new InventoryService.InventoryServiceClient(channel);

        }
        [HttpPost]
        public async Task<ActionResult> Add(Models.Product product)
        {

            var productRequest = new ProductRequest
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Category = (Protos.Category)product.Category,
                ExpireDate = product.ExpireDate,

            };

            var response = await client.GetByIdAsync(new GetByIdRequest() { Id = product.Id });
            if (response.Result == false)
            {
                var addedProduct = await client.AddProductAsync(productRequest);
                return Ok(addedProduct);
            }
            else
            {
                var updatedProduct = await client.UpdateProductAsync(productRequest);
                return Ok(updatedProduct);
            }
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> AddBulk(List<Models.Product> productList)
        {
            var call = client.AddBulkProducts();

            foreach (var i in productList)
            {
                await call.RequestStream.WriteAsync(new ProductRequest
                {
                    Id = i.Id,
                    ExpireDate = i.ExpireDate,
                    Name = i.Name,
                    Price = i.Price,
                });
                await Task.Delay(1000);
            }
            await call.RequestStream.CompleteAsync();
            var response = await call.ResponseAsync;

            return Ok(response);
        }

        [HttpPost("report")]
        public async Task<ActionResult> getReport(ReportCriteria criteria)
        {
            var call = client.getReport(criteria);
            var products = new List<ProductRequest>();

           
                await foreach (var product in call.ResponseStream.ReadAllAsync())
                {
                    products.Add(new ProductRequest
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Quantity = product.Quantity,
                        Category = product.Category,
                        ExpireDate = product.ExpireDate
                    });
                }
                return Ok(products);
        }
    }
}
