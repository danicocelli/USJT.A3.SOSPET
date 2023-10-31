using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROJETO.A3.USJT.Models;
using PROJETO.A3.USJT.Utils;

namespace PROJETO.A3.USJT.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly dbSOSPET _context;

        public UsuariosController(dbSOSPET context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return View("Login");
            var dbSOSPET = _context.Usuario.Include(u => u.Voluntario);
            return View(await dbSOSPET.ToListAsync());
        }

        // GET: Usuarios
        public async Task<IActionResult> Login()
        {
            return View("Login");
        }


        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("IsLoggedIn");

            return View("Login");
        }

        public IActionResult Authenticate(string username, string senha)
        {
            if (IsValidUser(username, senha))
            {
                // If the credentials are valid, create a user session.

                HttpContext.Session.SetString("IsLoggedIn", "true");
                return Redirect("/Dashboard/Index");
            }
            else
            {
                // Display an error message or redirect to a login error page.
                ViewData["IncorrectLogin"] = "Usuário ou senha inválidos.";
                return View("Login");
            }
        }


        private bool IsValidUser(string username, string senha)
        {
            var hasValue = _context.Usuario
                .Where(x => x.Username == username && x.Senha == senha)
                .Any();

            if (hasValue == true) return true;
            else return false;
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return View("Login");
            
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .Include(u => u.Voluntario)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return View("Login");
            ViewData["VoluntarioId"] = new SelectList(_context.Voluntario, "VoluntarioId", "VoluntarioId");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,Username,Senha,Ativo,VoluntarioId,UsuarioInclusao,DataInclusao,UsuarioAlteracao,DataAlteracao")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VoluntarioId"] = new SelectList(_context.Voluntario, "VoluntarioId", "VoluntarioId", usuario.VoluntarioId);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return View("Login");

            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["VoluntarioId"] = new SelectList(_context.Voluntario, "VoluntarioId", "VoluntarioId", usuario.VoluntarioId);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UsuarioId,Username,Senha,Ativo,VoluntarioId,UsuarioInclusao,DataInclusao,UsuarioAlteracao,DataAlteracao")] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
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
            ViewData["VoluntarioId"] = new SelectList(_context.Voluntario, "VoluntarioId", "VoluntarioId", usuario.VoluntarioId);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return View("Login");
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .Include(u => u.Voluntario)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Usuario == null)
            {
                return Problem("Entity set 'dbSOSPET.Usuario'  is null.");
            }
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(string id)
        {
          return (_context.Usuario?.Any(e => e.UsuarioId == id)).GetValueOrDefault();
        }


    }
}
