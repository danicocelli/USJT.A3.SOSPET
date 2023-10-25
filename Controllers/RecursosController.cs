using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROJETO.A3.USJT.Models;

namespace PROJETO.A3.USJT.Controllers
{
    public class RecursosController : Controller
    {
        private readonly dbSOSPET _context;

        public RecursosController(dbSOSPET context)
        {
            _context = context;
        }

        // GET: Recursos
        public async Task<IActionResult> Index()
        {
              return _context.Recurso != null ? 
                          View(await _context.Recurso.ToListAsync()) :
                          Problem("Entity set 'dbSOSPET.Recurso'  is null.");
        }

        // GET: Recursos/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Recurso == null)
            {
                return NotFound();
            }

            var recurso = await _context.Recurso
                .FirstOrDefaultAsync(m => m.RecursoId == id);
            if (recurso == null)
            {
                return NotFound();
            }

            return View(recurso);
        }

        // GET: Recursos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recursos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecursoId,NomeRecurso,Categoria,Descricao,UsuarioInclusao,DataInclusao,UsuarioAlteracao,DataAlteracao")] Recurso recurso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recurso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recurso);
        }

        // GET: Recursos/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Recurso == null)
            {
                return NotFound();
            }

            var recurso = await _context.Recurso.FindAsync(id);
            if (recurso == null)
            {
                return NotFound();
            }
            return View(recurso);
        }

        // POST: Recursos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RecursoId,NomeRecurso,Categoria,Descricao,UsuarioInclusao,DataInclusao,UsuarioAlteracao,DataAlteracao")] Recurso recurso)
        {
            if (id != recurso.RecursoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recurso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecursoExists(recurso.RecursoId))
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
            return View(recurso);
        }

        // GET: Recursos/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Recurso == null)
            {
                return NotFound();
            }

            var recurso = await _context.Recurso
                .FirstOrDefaultAsync(m => m.RecursoId == id);
            if (recurso == null)
            {
                return NotFound();
            }

            return View(recurso);
        }

        // POST: Recursos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Recurso == null)
            {
                return Problem("Entity set 'dbSOSPET.Recurso'  is null.");
            }
            var recurso = await _context.Recurso.FindAsync(id);
            if (recurso != null)
            {
                _context.Recurso.Remove(recurso);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecursoExists(string id)
        {
          return (_context.Recurso?.Any(e => e.RecursoId == id)).GetValueOrDefault();
        }
    }
}
