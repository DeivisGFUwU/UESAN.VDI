using System;
using System.Collections.Generic;

namespace UESAN.VDI.CORE.Core.Entities;

public partial class Revistas
{
    public string Issn { get; set; } = null!;

    public string Titulo { get; set; } = null!;

    public string? Categoria { get; set; }

    public string? Cuartil { get; set; }

    public bool EnListaCerrada { get; set; }

    public bool EsNueva { get; set; }

    public bool Activa { get; set; }

    public string? WoSQ { get; set; }

    public string? WoSS { get; set; }

    public bool? WoSEsci { get; set; }

    public string? EsciQ { get; set; }

    public bool? Ajg4Star { get; set; }

    public string? AjgNivel { get; set; }

    public string? AjgS { get; set; }

    public string? Cnrs { get; set; }

    public string? CnrsS { get; set; }

    public bool? CnrsExiste { get; set; }

    public string? Abdc { get; set; }

    public string? AbdcS { get; set; }

    public bool? AbdcExiste { get; set; }

    public bool? SoloEnUnaLista { get; set; }

    public bool? SoloScieloExiste { get; set; }

    public bool? WoSLatam { get; set; }

    public bool? Top50 { get; set; }

    public bool? BeallsList { get; set; }

    public bool? Mdpi { get; set; }

    public bool? Insights { get; set; }

    public bool? WoSTopExiste { get; set; }

    public bool? WoSEsciExiste { get; set; }

    public bool? ScopusExiste { get; set; }

    public bool? EsciScieloSinScopus { get; set; }

    public bool? LatamSinEsciExiste { get; set; }

    public bool? Multiple { get; set; }

    public bool? MultidisciplinaryWoS { get; set; }

    public bool? MultidisciplinaryScopus { get; set; }

    public bool? MultidisciplinaryWoSScopus { get; set; }

    public decimal? FactorImpacto { get; set; }

    public string? Pais { get; set; }

    public virtual ICollection<FormulariosInvestigacion> FormulariosInvestigacion { get; set; } = new List<FormulariosInvestigacion>();

    public virtual ICollection<Publicaciones> Publicaciones { get; set; } = new List<Publicaciones>();
}
