using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartInvoiceParser.Data;
using SmartInvoiceParser.Models;

namespace SmartInvoiceParser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InvoicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            return await _context.Invoices
                .Include(i => i.Items)
                .ToListAsync();
        }

        // GET: api/invoices/2
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return invoice;
        }

        // POST: api/invoices
        [HttpPost]
        public async Task<ActionResult<Invoice>> CreateInvoice(Invoice invoice)
        {
            if (invoice.Items != null && invoice.Items.Count > 0)
            {
                foreach (var item in invoice.Items)
                {
                    item.Id = 0;
                }
            }

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, invoice);
        }

        // PUT: api/invoices/2
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(int id, Invoice updatedInvoice)
        {
            if (id != updatedInvoice.Id)
            {
                return BadRequest("Invoice ID in URL does not match ID in body.");
            }

            var existingInvoice = await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (existingInvoice == null)
            {
                return NotFound();
            }

            existingInvoice.VendorName = updatedInvoice.VendorName;
            existingInvoice.InvoiceNumber = updatedInvoice.InvoiceNumber;
            existingInvoice.InvoiceDate = updatedInvoice.InvoiceDate;
            existingInvoice.SubTotal = updatedInvoice.SubTotal;
            existingInvoice.TaxAmount = updatedInvoice.TaxAmount;
            existingInvoice.TotalAmount = updatedInvoice.TotalAmount;
            existingInvoice.FilePath = updatedInvoice.FilePath;

            _context.InvoiceItems.RemoveRange(existingInvoice.Items);

            if (updatedInvoice.Items != null && updatedInvoice.Items.Count > 0)
            {
                foreach (var item in updatedInvoice.Items)
                {
                    item.Id = 0;
                    item.InvoiceId = existingInvoice.Id;
                    existingInvoice.Items.Add(item);
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/invoices/2
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}