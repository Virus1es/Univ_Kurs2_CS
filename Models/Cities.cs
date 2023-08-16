using System;
using System.Collections.Generic;

namespace Wpf_Kurvovaya_BD;

public partial class Cities
{
    /// <summary>
    /// Код города
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название города
    /// </summary>
    public string City { get; set; } = null!;

    public virtual ICollection<OurClubs> OurClubs { get; set; } = new List<OurClubs>();
}
