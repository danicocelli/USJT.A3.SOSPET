using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PROJETO.A3.USJT.Models;

    public class dbSOSPET : DbContext
    {
        public dbSOSPET (DbContextOptions<dbSOSPET> options)
            : base(options)
        {
        }

        public DbSet<Animal> Animal { get; set; } = default!;
       
        public DbSet<Voluntario> Voluntario { get; set; } = default!;

        public DbSet<Evento> Evento { get; set; } = default!;

        public DbSet<Recurso> Recurso { get; set; } = default!;

        public DbSet<Dashboard> Dashboard { get; set; } = default!;

        public DbSet<PROJETO.A3.USJT.Models.Usuario> Usuario { get; set; } = default!;
    }
