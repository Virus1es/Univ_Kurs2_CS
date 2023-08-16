using System;
using System.Collections.Generic;

namespace Wpf_Kurvovaya_BD;

public partial class GameLevels
{
    /// <summary>
    /// Код уровня игры
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Уровень игры
    /// </summary>
    public string GameLevel { get; set; } = null!;

    public virtual ICollection<Games> Games { get; set; } = new List<Games>();
}
