using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Repositories.Interfaces;
//using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class InstituicoesController : Controller
    {
        //private readonly AppDbContext _context;
        //private ICategoriaRepository _categoriaRepository { get; }
        //private readonly IInstituicaoRepository _instituicaoRepository;
        private readonly AppDbContext _context;

        public InstituicoesController(AppDbContext context)
        {
            //_categoriaRepository = categoriaRepository;
            //_instituicaoRepository = instituicaoRepository;
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([Bind("Email,senha")] Instituicao instituicao)
        {
            var user = await _context.Instituicoes.FirstOrDefaultAsync(m => m.Email == instituicao.Email);

            if (user == null)
            {
                ViewBag.Message = "Email e/ou senha inválidos!";
                return View();
            }
            bool isSenhaOk = BCrypt.Net.BCrypt.Verify(instituicao.senha, user.senha);

            if (isSenhaOk)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Nome),
                    new Claim(ClaimTypes.Name, user.CNPJ),        //user.Nome             
                    new Claim(ClaimTypes.Role, user.Perfil)
                };

                var userIdentity = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                var props = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.Now.ToLocalTime().AddMinutes(10),
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(principal, props);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = "Email ou senha inválidos!";
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Instituicoes");
        }

        //---------------------------------------------------------------
       

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["MetaSortParm"] = sortOrder == "Meta" ? "meta_desc" : "meta";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var instituicoes = from s in _context.Instituicoes
                               select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                instituicoes = instituicoes.Where(s => s.Nome.Contains(searchString)
                                       || s.Categoria.CategoriaNome.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    instituicoes = instituicoes.OrderByDescending(s => s.Nome);
                    break;
                case "meta":
                    instituicoes = instituicoes.OrderBy(s => s.MetaArrecadacao);
                    break;
                case "meta_desc":
                    instituicoes = instituicoes.OrderByDescending(s => s.MetaArrecadacao);
                    break;
                default:
                    instituicoes = instituicoes.OrderBy(s => s.Nome);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Instituicao>.CreateAsync(instituicoes.Include(c => c.Categoria).AsNoTracking(), pageNumber ?? 1, pageSize));
        }


        // GET: Instituicoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instituicao = await _context.Instituicoes.Include(i => i.Doacoes).Include(c => c.Categoria).AsNoTracking().FirstOrDefaultAsync(m => m.CNPJ == id);
            if (instituicao == null)
            {
                return NotFound();
            }

            return View(instituicao);
        }

        // GET: Instituicoes/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome");
            return View();
        }

        // POST: Instituicoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DataRegistro,Nome,CNPJ,CategoriaId,DescricaoCurta,DescricaoDetalhada,MetaArrecadacao,Email,senha,Estado,Cidade,Logradouro,CEP")] Instituicao instituicao)
        {
            if (ModelState.IsValid)
            {
                instituicao.senha = BCrypt.Net.BCrypt.HashPassword(instituicao.senha);
                _context.Add(instituicao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(instituicao);
        }
     
        // GET: Instituicoes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instituicao = await _context.Instituicoes.FindAsync(id);
            if (instituicao == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome", instituicao.CategoriaId);
            return View(instituicao);
        }

        // POST: Instituicoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("DataRegistro,Nome,CNPJ,CategoriaId,DescricaoCurta,DescricaoDetalhada,MetaArrecadacao,Email,senha,Estado,Cidade,Logradouro,CEP")] Instituicao instituicao)
        {
            if (id != instituicao.CNPJ)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    instituicao.senha = BCrypt.Net.BCrypt.HashPassword(instituicao.senha);
                    _context.Update(instituicao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstituicaoExists(instituicao.CNPJ))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome", instituicao.CategoriaId);
            return View(instituicao);
        }

        // GET: Instituicoes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instituicao = await _context.Instituicoes
                .Include(i => i.Categoria)
                .FirstOrDefaultAsync(m => m.CNPJ == id);
            if (instituicao == null)
            {
                return NotFound();
            }

            return View(instituicao);
        }

        // POST: Instituicoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var instituicao = await _context.Instituicoes.FindAsync(id);
            _context.Instituicoes.Remove(instituicao);
            await _context.SaveChangesAsync();

            if (User.IsInRole("Ong")) 
            { 
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Instituicoes");
            }
            return View(instituicao);
        }    
        private bool InstituicaoExists(string id)
        {
            return _context.Instituicoes.Any(e => e.CNPJ == id);
        }
        public async Task<IActionResult> Lista(string categoria)
        {
            var instituicoes = from s in _context.Instituicoes
                               select s;

            ViewData["Categoria"] = categoria;

            if (categoria == null)
            {
                var appDbContext = _context.Instituicoes.Include(i => i.Categoria);
                return View(await appDbContext.ToListAsync());
            }
            instituicoes = _context.Instituicoes
                            .Where(c => c.Categoria.CategoriaNome.Equals(categoria)).OrderBy(c => c.Nome);

            if (instituicoes == null)
            {
                return NotFound();
            }
            return View(await instituicoes.AsNoTracking().ToListAsync());
        }
        //---------------------------------------------------------------
    }
}

//public async Task<IActionResult> Lista(string categoria)
//{
//    IEnumerable<Instituicao> instituicoes;

//    if (categoria == null)
//    {
//        var appDbContext = _context.Instituicoes.Include(i => i.Categoria);
//        return View(await appDbContext.ToListAsync());
//    }
//    instituicoes = _context.Instituicoes
//                    .Where(c => c.Categoria.CategoriaNome.Equals(categoria)).OrderBy(c => c.Nome);

//    if (instituicoes == null)
//    {
//        return NotFound();
//    }
//    return View(instituicoes);
//}