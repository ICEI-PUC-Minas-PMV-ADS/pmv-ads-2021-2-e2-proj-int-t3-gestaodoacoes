using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{

    public class AdministradoresController : Controller
    {
        private readonly AppDbContext _context;

        public AdministradoresController(AppDbContext context)
        {
            _context = context;
        }
        //------------------------------------------------------------------------------------------------------------------------------------
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([Bind("Email,senha")] Administrador administrador)
        {
            var user = await _context.Administradores.FirstOrDefaultAsync(m => m.Email == administrador.Email);

            if (user == null)
            {
                ViewBag.Message = "Email e/ou senha inválidos!";
                return View();
            }
            bool isSenhaOk = BCrypt.Net.BCrypt.Verify(administrador.senha, user.senha);

            if (isSenhaOk)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Nome),
                    new Claim(ClaimTypes.Name, user.Nome),        //user.Nome             
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
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Administradores");
        }
        //------------------------------------------------------------------------------------------------------------------------------------
        // GET: administradores
        [Authorize(Roles = "Adm")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Administradores.ToListAsync());
        }

        // GET: administradores/Details/5
        [Authorize(Roles = "Adm")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrador = await _context.Administradores
                .FirstOrDefaultAsync(m => m.CPF == id);
            if (administrador == null)
            {
                return NotFound();
            }

            return View(administrador);
        }

        // GET: administradores/Create
        [Authorize(Roles = "Adm")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: administradores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Adm")]
        public async Task<IActionResult> Create([Bind("Nome,CPF,Email,senha")] Administrador administrador)
        {
            if (ModelState.IsValid)
            {
                administrador.senha = BCrypt.Net.BCrypt.HashPassword(administrador.senha);
                _context.Add(administrador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(administrador);
        }

        // GET: administradores/Edit/5
        [Authorize(Roles = "Adm")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrador = await _context.Administradores.FindAsync(id);
            if (administrador == null)
            {
                return NotFound();
            }
            return View(administrador);
        }

        // POST: administradores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Adm")]
        public async Task<IActionResult> Edit(string id, [Bind("Nome,CPF,Email,senha")] Administrador administrador)
        {
            if (id != administrador.CPF)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(administrador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!administradorExists(administrador.CPF))
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
            return View(administrador);
        }

        // GET: administradores/Delete/5
        [Authorize(Roles = "Adm")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrador = await _context.Administradores
                .FirstOrDefaultAsync(m => m.CPF == id);
            if (administrador == null)
            {
                return NotFound();
            }

            return View(administrador);
        }

        // POST: administradores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Adm")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var administrador = await _context.Administradores.FindAsync(id);
            _context.Administradores.Remove(administrador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
   
        private bool administradorExists(string id)
        {
            return _context.Administradores.Any(e => e.CPF == id);
        }
    }
}
