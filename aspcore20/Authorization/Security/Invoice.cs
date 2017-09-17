namespace Authorization.Security
{
    public class Invoice
    {
        public int Id { get; set; }
        public int MandantId { get; set; }
        public decimal Value { get; set; }
    }
}