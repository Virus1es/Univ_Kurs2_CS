using System;
using System.Collections.Generic;

namespace Wpf_Kurvovaya_BD;

public partial class Positions
{
    /// <summary>
    /// Код позиции игрока на поле
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Позиция игрока на поле
    /// </summary>
    public string Position { get; set; } = null!;

    public virtual ICollection<Gamers> Gamers { get; set; } = new List<Gamers>();
}
