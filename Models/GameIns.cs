using System;
using System.Collections.Generic;

namespace Wpf_Kurvovaya_BD;

public partial class GameIns
{
    /// <summary>
    /// Код участия в игре
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Код игрока
    /// </summary>
    public int IdGamer { get; set; }

    /// <summary>
    /// Код игры
    /// </summary>
    public int IdGame { get; set; }

    /// <summary>
    /// Участие в игре
    /// </summary>
    public bool Order { get; set; }

    /// <summary>
    /// Забитые игроком мячи
    /// </summary>
    public int CountStart { get; set; }

    /// <summary>
    /// Премия за игру
    /// </summary>
    public int Salary { get; set; }

    public virtual Games IdGameNavigation { get; set; } = null!;

    public virtual Gamers IdGamerNavigation { get; set; } = null!;
}
