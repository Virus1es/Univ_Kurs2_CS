using System;
using System.Collections.Generic;

namespace Wpf_Kurvovaya_BD;

public partial class Countries
{
    /// <summary>
    /// Код страны
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название страны
    /// </summary>
    public string Country { get; set; } = null!;

    public virtual ICollection<EnemyClubs> EnemyClubs { get; set; } = new List<EnemyClubs>();

    public virtual ICollection<Games> Games { get; set; } = new List<Games>();
}
