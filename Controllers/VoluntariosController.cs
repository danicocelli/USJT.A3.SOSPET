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
    public class VoluntariosController : Controller
    {
        private readonly dbSOSPET _context;

        public VoluntariosController(dbSOSPET context)
        {
            _context = context;
        }

        // GET: Voluntarios
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return View(SessionValidator.LoginUrl);
            return _context.Voluntario != null ? 
                          View(await _context.Voluntario.ToListAsync()) :
                          Problem("Entity set 'dbSOSPET.Voluntario'  is null.");
        }

        // GET: Voluntarios/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return View(SessionValidator.LoginUrl);
            if (id == null || _context.Voluntario == null)
            {
                return NotFound();
            }

            var voluntario = await _context.Voluntario
                .FirstOrDefaultAsync(m => m.VoluntarioId == id);
            if (voluntario == null)
            {
                return NotFound();
            }

            return View(voluntario);
        }

        // GET: Voluntarios/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return View(SessionValidator.LoginUrl);
            return View();
        }

        // POST: Voluntarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VoluntarioId,Nome,Sobrenome,Cargo,Email,Telefone,Descricao,Endereco,Situacao,UsuarioInclusao,DataInclusao,UsuarioAlteracao,DataAlteracao")] Voluntario voluntario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(voluntario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(voluntario);
        }

        // GET: Voluntarios/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return View(SessionValidator.LoginUrl);
            if (id == null || _context.Voluntario == null)
            {
                return NotFound();
            }

            var voluntario = await _context.Voluntario.FindAsync(id);
            if (voluntario == null)
            {
                return NotFound();
            }
            return View(voluntario);
        }

        // POST: Voluntarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("VoluntarioId,Nome,Sobrenome,Cargo,Email,Telefone,Descricao,Endereco,Situacao,UsuarioInclusao,DataInclusao,UsuarioAlteracao,DataAlteracao")] Voluntario voluntario)
        {
            if (id != voluntario.VoluntarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voluntario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoluntarioExists(voluntario.VoluntarioId))
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
            return View(voluntario);
        }

        // GET: Voluntarios/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return View(SessionValidator.LoginUrl);
            if (id == null || _context.Voluntario == null)
            {
                return NotFound();
            }

            var voluntario = await _context.Voluntario
                .FirstOrDefaultAsync(m => m.VoluntarioId == id);
            if (voluntario == null)
            {
                return NotFound();
            }

            return View(voluntario);
        }

        // POST: Voluntarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Voluntario == null)
            {
                return Problem("Entity set 'dbSOSPET.Voluntario'  is null.");
            }
            var voluntario = await _context.Voluntario.FindAsync(id);
            if (voluntario != null)
            {
                _context.Voluntario.Remove(voluntario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoluntarioExists(string id)
        {
          return (_context.Voluntario?.Any(e => e.VoluntarioId == id)).GetValueOrDefault();
        }
    }
}
