 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBlazor.Server.Data;
using ProjectBlazor.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class WalletController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public WalletController(ApplicationDbContext context)
    {
        _context = context;
    }
     [HttpGet]
    public async Task<IEnumerable<WalletDate>> GetWalletItems()
    {
        return await _context.WalletItems.ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<WalletDate>> GetWalletById(string id)
    {
        var walletItem = await _context.WalletItems.FromSqlRaw("SELECT * FROM \"WalletItems\" WHERE \"UserId\" = {0}", id).FirstOrDefaultAsync();

        if (walletItem == null)
        {
            return NotFound(); // Retorna um status 404 (Not Found) se o registro não for encontrado.
        }

        return walletItem;
    }
    

    [HttpGet("id/{id}")]
    public async Task<ActionResult<WalletDate>> GetWalletValueById(Guid id)
    {
        var walletItem = await _context.WalletItems.FindAsync(id);

        if (walletItem == null)
        {
            return NotFound(); // Retorna um status 404 (Not Found) se o registro não for encontrado.
        }

        return walletItem;
    }

    [HttpPost]
    public async Task<ActionResult<WalletDate>> PostWalletItem(WalletDate walletDate)
    {
        _context.WalletItems.Add(walletDate);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetWalletItems), new { id = walletDate.Id }, walletDate);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutWalletItem(Guid id, WalletDate walletItem)
    {
        if (id != walletItem.Id)
        {
            return BadRequest();
        }

        _context.Entry(walletItem).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!WalletItemExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }
    [HttpPut("change/{id}")]
    public IActionResult UpdateWalletState(Guid Id, [FromBody] decimal result)
    {

        var sql = "UPDATE \"WalletItems\" SET \"Wallet\" = {1} WHERE \"Id\" = {0}";
        _context.Database.ExecuteSqlRaw(sql, Id, result);

        // Lógica de tratamento de erro e resposta apropriada, se necessário
        return Ok(); // Retorna um status HTTP 200 OK, por exemplo
    }
    private bool WalletItemExists(Guid id)
    {
        return _context.WalletItems.Any(e => e.Id == id);
    }
}