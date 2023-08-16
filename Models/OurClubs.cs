using System;
using System.Collections.Generic;

namespace Wpf_Kurvovaya_BD;

public partial class OurClubs
{
    /// <summary>
    /// Код нашего клуба
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название нашего клуба
    /// </summary>
    public string Club { get; set; } = null!;

    /// <summary>
    /// Код тренировочной базы клуба
    /// </summary>
    public int IdBase { get; set; }

    /// <summary>
    /// Год создания клуба
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Код лиги клуба
    /// </summary>
    public int IdLeage { get; set; }

    /// <summary>
    /// Код руководителя клуба
    /// </summary>
    public int IdManager { get; set; }

    /// <summary>
    /// Код города размещения клуба
    /// </summary>
    public int IdCity { get; set; }

    public virtual ICollection<Gamers> Gamers { get; set; } = new List<Gamers>();

    public virtual ICollection<Games> Games { get; set; } = new List<Games>();

    public virtual TrainingBasis IdBaseNavigation { get; set; } = null!;

    public virtual Cities IdCityNavigation { get; set; } = null!;

    public virtual Leages IdLeageNavigation { get; set; } = null!;

    public virtual Managers IdManagerNavigation { get; set; } = null!;
}
