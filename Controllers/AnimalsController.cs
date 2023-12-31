﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PROJETO.A3.USJT.Models;
using PROJETO.A3.USJT.Utils;

namespace PROJETO.A3.USJT.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly dbSOSPET _context;
        private readonly IWebHostEnvironment webHostEnvironment;




        public AnimalsController(dbSOSPET context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: Animals
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return View(SessionValidator.LoginUrl);

            return _context.Animal != null ?
                        View(await _context.Animal.Include(a => a.Voluntario).ToListAsync()) :
                        Problem("Entity set 'dbSOSPET.Animal'  is null.");
        }

        // GET: Animals/Details/5
        public async Task<IActionResult> Details(String id)
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return View(SessionValidator.LoginUrl);

            if (id == null || _context.Animal == null)
            {
                return NotFound();
            }

            var animal = await _context.Animal
                .FirstOrDefaultAsync(m => m.AnimalId == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // GET: Animals/Create


        public IActionResult Create()
        {
            //var animals = _context.Animal.ToList();
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return View(SessionValidator.LoginUrl);

            var voluntarios = _context.Voluntario.ToList();


            if (voluntarios != null)
            {
                ViewBag.Voluntarios = voluntarios;
                return View();
            }
            else
            {
                return Problem("Entity set 'dbSOSPET.Animal' or 'dbSOSPET.Voluntario' is null.");
            }
        }



        // POST: Animals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AnimalId,NomeAnimal,DataNascimento,DataAcolhimento,Disponivel,Doado,Descricao,LocalEncontro,ObservacaoSaude,VoluntarioId,Voluntario,Cor,SituacaoAnimal,CategoriaAnimal,Genero")] Animal animal)
        {

            TempData[TempDataConsts.InsertSuccess] = MessageConsts.CreateSuccessMessage;

            animal.DataAlteracao = DateTimeOffset.Now;
            animal.UsuarioAlteracao = SessionValidator.UserData;
            animal.DataInclusao = DateTimeOffset.Now;
            animal.UsuarioInclusao = SessionValidator.UserData;



            _context.Add(animal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Animals/Edit/5
        public async Task<IActionResult> Edit(String id)
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return View(SessionValidator.LoginUrl);

            if (id == null || _context.Animal == null)
            {
                return NotFound();
            }
            var voluntarios = _context.Voluntario.ToList();
            ViewBag.Voluntarios = voluntarios;

            var animal = await _context.Animal.FindAsync(id);

            if (animal == null)
            {
                return NotFound();
            }


            return View(animal);
        }

        // POST: Animals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(String id, [Bind(("AnimalId,NomeAnimal,DataNascimento,DataAcolhimento,DataDoacao,Disponivel,Descricao,LocalEncontro,ObservacaoSaude,VoluntarioId,Cor,SituacaoAnimal,CategoriaAnimal,Genero"))] Animal animal)
        {
            if (id != animal.AnimalId)
            {
                return NotFound();
            }

            animal.DataAlteracao = DateTimeOffset.Now;
            animal.UsuarioAlteracao = SessionValidator.UserData;

            if (animal.DataInclusao == null || animal.DataAlteracao == null)
            {
                animal.DataInclusao = DateTimeOffset.Now;
                animal.UsuarioInclusao = SessionValidator.UserData;
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    
                    _context.Update(animal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalExists(animal.AnimalId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            return View(animal);
        }

        // GET: Animals/Delete/5
        public async Task<IActionResult> Delete(String id)
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return View(SessionValidator.LoginUrl);

            if (id == null || _context.Animal == null)
            {
                return NotFound();
            }

            var animal = await _context.Animal
                .FirstOrDefaultAsync(m => m.AnimalId == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // POST: Animals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(String id)
        {
            if (_context.Animal == null)
            {
                return Problem("Entity set 'dbSOSPET.Animal'  is null.");
            }
            var animal = await _context.Animal.FindAsync(id);
            if (animal != null)
            {
                _context.Animal.Remove(animal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimalExists(String id)
        {
            return (_context.Animal?.Any(e => e.AnimalId == id)).GetValueOrDefault();
        }
    }
}
