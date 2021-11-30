using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Authorize]
    public class DoacoesController : Controller
    {
        private readonly AppDbContext _context;

        public DoacoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Doacoes
        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var doacoes = from d in _context.Doacoes
                          select d;

            doacoes = doacoes.Where(d => d.Cpf.Equals(searchString));

            return View(await doacoes.AsNoTracking().ToListAsync());
        }

        // GET: Doacoes/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doacao = await _context.Doacoes
                .Include(d => d.Instituicao)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doacao == null)
            {
                return NotFound();
            }

            return View(doacao);
        }

        // GET: Doacoes/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
            ViewData["instituicaoCNPJ"] = new SelectList(_context.Instituicoes, "CNPJ", "CNPJ");
            return View();
        }

        // POST: Doacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create([Bind("Id,Data,Nome,Email,Cpf,NomeCartao,NumeroCartao,ValidadeCartao,CodSegurancaCartao,Valor,instituicaoCNPJ")] Doacao doacao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doacao);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            ViewData["instituicaoCNPJ"] = new SelectList(_context.Instituicoes, "CNPJ", "CNPJ", doacao.instituicaoCNPJ);
            return View(doacao);
        }

        // GET: Doacoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doacao = await _context.Doacoes.FindAsync(id);
            if (doacao == null)
            {
                return NotFound();
            }
            ViewData["instituicaoCNPJ"] = new SelectList(_context.Instituicoes, "CNPJ", "CNPJ", doacao.instituicaoCNPJ);
            return View(doacao);
        }

        // POST: Doacoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,Nome,Email,Cpf,NomeCartao,NumeroCartao,ValidadeCartao,CodSegurancaCartao,Valor,instituicaoCNPJ")] Doacao doacao)
        {
            if (id != doacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoacaoExists(doacao.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["instituicaoCNPJ"] = new SelectList(_context.Instituicoes, "CNPJ", "CNPJ", doacao.instituicaoCNPJ);
            return View(doacao);
        }

        // GET: Doacoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doacao = await _context.Doacoes
                .Include(d => d.Instituicao)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doacao == null)
            {
                return NotFound();
            }

            return View(doacao);
        }

        // POST: Doacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doacao = await _context.Doacoes.FindAsync(id);
            _context.Doacoes.Remove(doacao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoacaoExists(int id)
        {
            return _context.Doacoes.Any(e => e.Id == id);
        }
    }
}
