using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UESAN.VDI.CORE.Core.Entities;

namespace UESAN.VDI.CORE.Infrastructure.data;

public partial class VdiDbContext : DbContext
{
    public VdiDbContext()
    {
    }

    public VdiDbContext(DbContextOptions<VdiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AsignacionProyecto> AsignacionProyecto { get; set; }

    public virtual DbSet<AutoresPublicacion> AutoresPublicacion { get; set; }

    public virtual DbSet<FormulariosInvestigacion> FormulariosInvestigacion { get; set; }

    public virtual DbSet<LineasInvestigacion> LineasInvestigacion { get; set; }

    public virtual DbSet<MensajeChat> MensajeChat { get; set; }

    public virtual DbSet<Profesores> Profesores { get; set; }

    public virtual DbSet<Proyectos> Proyectos { get; set; }

    public virtual DbSet<Publicaciones> Publicaciones { get; set; }

    public virtual DbSet<Revistas> Revistas { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<SesionChat> SesionChat { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AsignacionProyecto>(entity =>
        {
            entity.HasKey(e => e.AsignacionId).HasName("PK__Asignaci__D82B5BB704B17ECF");

            entity.Property(e => e.AsignacionId).HasColumnName("AsignacionID");
            entity.Property(e => e.FechaAsignacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProfesorId).HasColumnName("ProfesorID");
            entity.Property(e => e.ProyectoId).HasColumnName("ProyectoID");
            entity.Property(e => e.RolEnProyecto).HasMaxLength(100);

            entity.HasOne(d => d.Profesor).WithMany(p => p.AsignacionProyecto)
                .HasForeignKey(d => d.ProfesorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asig_Profesor");

            entity.HasOne(d => d.Proyecto).WithMany(p => p.AsignacionProyecto)
                .HasForeignKey(d => d.ProyectoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asig_Proyecto");
        });

        modelBuilder.Entity<AutoresPublicacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AutoresP__3214EC27F734512A");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PorcentajeParticipacion).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.ProfesorId).HasColumnName("ProfesorID");
            entity.Property(e => e.PublicacionId).HasColumnName("PublicacionID");

            entity.HasOne(d => d.Profesor).WithMany(p => p.AutoresPublicacion)
                .HasForeignKey(d => d.ProfesorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Autor_Profesor");

            entity.HasOne(d => d.Publicacion).WithMany(p => p.AutoresPublicacion)
                .HasForeignKey(d => d.PublicacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Autor_Publicacion");
        });

        modelBuilder.Entity<FormulariosInvestigacion>(entity =>
        {
            entity.HasKey(e => e.FormularioId).HasName("PK__Formular__02C09CF3568C2D43");

            entity.Property(e => e.FormularioId).HasColumnName("FormularioID");
            entity.Property(e => e.Doi)
                .HasMaxLength(100)
                .HasColumnName("DOI");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Issn)
                .HasMaxLength(20)
                .HasColumnName("ISSN");
            entity.Property(e => e.MedioPublicacion).HasMaxLength(200);
            entity.Property(e => e.Presupuesto).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.ProyectoId).HasColumnName("ProyectoID");
            entity.Property(e => e.TipoFormulario)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.IssnNavigation).WithMany(p => p.FormulariosInvestigacion)
                .HasForeignKey(d => d.Issn)
                .HasConstraintName("FK_Form_Revista");

            entity.HasOne(d => d.Proyecto).WithMany(p => p.FormulariosInvestigacion)
                .HasForeignKey(d => d.ProyectoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Form_Proyecto");
        });

        modelBuilder.Entity<LineasInvestigacion>(entity =>
        {
            entity.HasKey(e => e.LineaId).HasName("PK__LineasIn__78106D115E060D82");

            entity.HasIndex(e => e.Nombre, "UQ__LineasIn__75E3EFCFFA200D43").IsUnique();

            entity.Property(e => e.LineaId).HasColumnName("LineaID");
            entity.Property(e => e.Nombre).HasMaxLength(200);
        });

        modelBuilder.Entity<MensajeChat>(entity =>
        {
            entity.HasKey(e => e.MensajeId).HasName("PK__MensajeC__FEA0557F8C9620DE");

            entity.Property(e => e.MensajeId).HasColumnName("MensajeID");
            entity.Property(e => e.FechaEnvio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Remitente).HasMaxLength(50);
            entity.Property(e => e.SesionId).HasColumnName("SesionID");

            entity.HasOne(d => d.Sesion).WithMany(p => p.MensajeChat)
                .HasForeignKey(d => d.SesionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mensaje_Sesion");
        });

        modelBuilder.Entity<Profesores>(entity =>
        {
            entity.HasKey(e => e.ProfesorId).HasName("PK__Profesor__4DF3F0280444D2B7");

            entity.HasIndex(e => e.UsuarioId, "UQ__Profesor__2B3DE799DF0DAC66").IsUnique();

            entity.Property(e => e.ProfesorId).HasColumnName("ProfesorID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Categoria).HasMaxLength(100);
            entity.Property(e => e.Departamento).HasMaxLength(100);
            entity.Property(e => e.FechaIngreso).HasColumnType("datetime");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithOne(p => p.Profesores)
                .HasForeignKey<Profesores>(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Profesores_Usuarios");
        });

        modelBuilder.Entity<Proyectos>(entity =>
        {
            entity.HasKey(e => e.ProyectoId).HasName("PK__Proyecto__CF241D454859FB8B");

            entity.HasIndex(e => e.Estatus, "IDX_Proyectos_Estatus");

            entity.Property(e => e.ProyectoId).HasColumnName("ProyectoID");
            entity.Property(e => e.Estatus).HasMaxLength(50);
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.LineaId).HasColumnName("LineaID");
            entity.Property(e => e.Titulo).HasMaxLength(200);

            entity.HasOne(d => d.AdminCreaNavigation).WithMany(p => p.Proyectos)
                .HasForeignKey(d => d.AdminCrea)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Proyectos_AdminCrea");

            entity.HasOne(d => d.Linea).WithMany(p => p.Proyectos)
                .HasForeignKey(d => d.LineaId)
                .HasConstraintName("FK_Proyectos_Linea");
        });

        modelBuilder.Entity<Publicaciones>(entity =>
        {
            entity.HasKey(e => e.PublicacionId).HasName("PK__Publicac__10DF15AA6654D0D4");

            entity.HasIndex(e => e.FechaPublicacion, "IDX_Publicaciones_Fecha");

            entity.Property(e => e.PublicacionId).HasColumnName("PublicacionID");
            entity.Property(e => e.Doi)
                .HasMaxLength(100)
                .HasColumnName("DOI");
            entity.Property(e => e.FechaPublicacion).HasColumnType("datetime");
            entity.Property(e => e.Incentivo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Issn)
                .HasMaxLength(20)
                .HasColumnName("ISSN");
            entity.Property(e => e.ProfesorId).HasColumnName("ProfesorID");
            entity.Property(e => e.Titulo).HasMaxLength(200);

            entity.HasOne(d => d.IssnNavigation).WithMany(p => p.Publicaciones)
                .HasForeignKey(d => d.Issn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Public_Revista");

            entity.HasOne(d => d.Profesor).WithMany(p => p.Publicaciones)
                .HasForeignKey(d => d.ProfesorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Public_Profesor");
        });

        modelBuilder.Entity<Revistas>(entity =>
        {
            entity.HasKey(e => e.Issn).HasName("PK__Revistas__447D3E97677D8CFA");

            entity.Property(e => e.Issn)
                .HasMaxLength(20)
                .HasColumnName("ISSN");
            entity.Property(e => e.Abdc)
                .HasMaxLength(10)
                .HasColumnName("ABDC");
            entity.Property(e => e.AbdcExiste)
                .HasDefaultValue(false)
                .HasColumnName("ABDC_Existe");
            entity.Property(e => e.AbdcS)
                .HasMaxLength(10)
                .HasColumnName("ABDC_S");
            entity.Property(e => e.Activa).HasDefaultValue(true);
            entity.Property(e => e.Ajg4Star)
                .HasDefaultValue(false)
                .HasColumnName("AJG_4_Star");
            entity.Property(e => e.AjgNivel)
                .HasMaxLength(10)
                .HasColumnName("AJG_NIVEL");
            entity.Property(e => e.AjgS)
                .HasMaxLength(10)
                .HasColumnName("AJG_S");
            entity.Property(e => e.BeallsList)
                .HasDefaultValue(false)
                .HasColumnName("BEALLS_LIST");
            entity.Property(e => e.Categoria).HasMaxLength(10);
            entity.Property(e => e.Cnrs)
                .HasMaxLength(10)
                .HasColumnName("CNRS");
            entity.Property(e => e.CnrsExiste)
                .HasDefaultValue(false)
                .HasColumnName("CNRS_Existe");
            entity.Property(e => e.CnrsS)
                .HasMaxLength(10)
                .HasColumnName("CNRS_S");
            entity.Property(e => e.Cuartil).HasMaxLength(5);
            entity.Property(e => e.EsNueva).HasDefaultValue(true);
            entity.Property(e => e.EsciQ)
                .HasMaxLength(10)
                .HasColumnName("ESCI_Q");
            entity.Property(e => e.EsciScieloSinScopus)
                .HasDefaultValue(false)
                .HasColumnName("ESCI_Scielo_Sin_Scopus");
            entity.Property(e => e.FactorImpacto).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Insights)
                .HasDefaultValue(false)
                .HasColumnName("INSIGHTS");
            entity.Property(e => e.LatamSinEsciExiste)
                .HasDefaultValue(false)
                .HasColumnName("Latam_Sin_ESCI_Existe");
            entity.Property(e => e.Mdpi)
                .HasDefaultValue(false)
                .HasColumnName("MDPI");
            entity.Property(e => e.MultidisciplinaryScopus)
                .HasDefaultValue(false)
                .HasColumnName("Multidisciplinary_Scopus");
            entity.Property(e => e.MultidisciplinaryWoS)
                .HasDefaultValue(false)
                .HasColumnName("Multidisciplinary_WoS");
            entity.Property(e => e.MultidisciplinaryWoSScopus)
                .HasDefaultValue(false)
                .HasColumnName("Multidisciplinary_WoS_Scopus");
            entity.Property(e => e.Multiple).HasDefaultValue(false);
            entity.Property(e => e.Pais).HasMaxLength(100);
            entity.Property(e => e.ScopusExiste)
                .HasDefaultValue(false)
                .HasColumnName("Scopus_Existe");
            entity.Property(e => e.SoloEnUnaLista).HasDefaultValue(false);
            entity.Property(e => e.SoloScieloExiste).HasDefaultValue(false);
            entity.Property(e => e.Titulo).HasMaxLength(200);
            entity.Property(e => e.Top50)
                .HasDefaultValue(false)
                .HasColumnName("TOP50");
            entity.Property(e => e.WoSEsci)
                .HasDefaultValue(false)
                .HasColumnName("WoS_ESCI");
            entity.Property(e => e.WoSEsciExiste)
                .HasDefaultValue(false)
                .HasColumnName("WoS_ESCI_Existe");
            entity.Property(e => e.WoSLatam)
                .HasDefaultValue(false)
                .HasColumnName("WoS_LATAM");
            entity.Property(e => e.WoSQ)
                .HasMaxLength(10)
                .HasColumnName("WoS_Q");
            entity.Property(e => e.WoSS)
                .HasMaxLength(10)
                .HasColumnName("WoS_S");
            entity.Property(e => e.WoSTopExiste)
                .HasDefaultValue(false)
                .HasColumnName("WoS_Top_Existe");
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A2F15A25C");

            entity.HasIndex(e => e.Nombre, "UQ__Roles__75E3EFCFC1F77A14").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<SesionChat>(entity =>
        {
            entity.HasKey(e => e.SesionId).HasName("PK__SesionCh__52FD7C06D9EB19E8");

            entity.Property(e => e.SesionId).HasColumnName("SesionID");
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.SesionChat)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sesion_Usuario");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE798368D46F8");

            entity.HasIndex(e => e.Correo, "IDX_Usuarios_Correo");

            entity.HasIndex(e => e.Correo, "UQ__Usuarios__60695A19E4DD0AB4").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.ClaveHash).HasMaxLength(256);
            entity.Property(e => e.Correo).HasMaxLength(200);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
