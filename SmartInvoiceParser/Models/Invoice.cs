namespace SmartInvoiceParser.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string? VendorName { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // علاقة One-to-Many (الفاتورة ليها أكتر من صنف)
        public List<InvoiceItem> Items { get; set; } = new();
    }
}