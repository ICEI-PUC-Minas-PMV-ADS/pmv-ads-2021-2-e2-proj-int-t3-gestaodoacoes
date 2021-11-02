using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using doee.Models;
using doee.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace doee.Controllers
{
    public class InstituicoesController : Controller
    {
        private readonly DoeeContext _context;

        public InstituicoesController(DoeeContext context)
        {
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

        // GET: Instituicoes
        /* public async Task<IActionResult> Index()
         {
            return View(await _context.Instituicoes.ToListAsync());
         }*/
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
                                       || s.Descricao.Contains(searchString));
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
            return View(await PaginatedList<Instituicao>.CreateAsync(instituicoes.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Instituicoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instituicao = await _context.Instituicoes.Include(i => i.Doacoes).AsNoTracking().FirstOrDefaultAsync(m => m.CNPJ == id);

            //var instituicao = await _context.Instituicoes.FirstOrDefaultAsync(m => m.CNPJ == id);

            if (instituicao == null)
            {
                return NotFound();
            }

            return View(instituicao);
        }

        // GET: Instituicoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instituicoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,CNPJ,setor,Descricao,MetaArrecadacao,Email,senha")] Instituicao instituicao)
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
            return View(instituicao);
        }

        // POST: Instituicoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Nome,CNPJ,setor,Descricao,MetaArrecadacao,Email,senha")] Instituicao instituicao)
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
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Instituicoes");
        }

        private bool InstituicaoExists(string id)
        {
            return _context.Instituicoes.Any(e => e.CNPJ == id);
        }
    }
}
