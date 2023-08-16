using System;
using System.Collections.Generic;

namespace Wpf_Kurvovaya_BD;

public partial class TrainingBasis
{
    /// <summary>
    /// Код тренировочной базы
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название тренировчной базы
    /// </summary>
    public string Base { get; set; } = null!;

    public virtual ICollection<OurClubs> OurClubs { get; set; } = new List<OurClubs>();
}
