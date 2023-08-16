using System;
using System.Collections.Generic;

namespace Wpf_Kurvovaya_BD;

public partial class Leages
{
    /// <summary>
    /// Код названия лиги
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название лиги
    /// </summary>
    public string Leage { get; set; } = null!;

    public virtual ICollection<OurClubs> OurClubs { get; set; } = new List<OurClubs>();
}
