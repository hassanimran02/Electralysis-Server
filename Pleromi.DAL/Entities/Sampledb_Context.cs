using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Pleromi.DAL.Entities;

public partial class Sampledb_Context : DbContext
{
    public Sampledb_Context()
    {
    }

    public Sampledb_Context(DbContextOptions<Sampledb_Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Sample> Samples { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sample>(entity =>
        {
            entity.ToTable("Sample");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
