using Google.Protobuf.WellKnownTypes;

namespace ITI.GRPCLAB.Server.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int Category { get; set; }
        public Timestamp ExpireDate { get; set; }
    }
}




