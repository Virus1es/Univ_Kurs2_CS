using System;
using System.Collections.Generic;

namespace Wpf_Kurvovaya_BD;

public partial class Games
{
    /// <summary>
    /// Код игры
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Дата проведения игры
    /// </summary>
    public DateOnly DateGame { get; set; }

    /// <summary>
    /// Код страны проведения игры
    /// </summary>
    public int IdCountry { get; set; }

    /// <summary>
    /// Код уровня игры
    /// </summary>
    public int IdLevel { get; set; }

    /// <summary>
    /// Количество пропущенных мячей
    /// </summary>
    public int CountFinish { get; set; }

    /// <summary>
    /// Код нашего клуба
    /// </summary>
    public int IdOurClub { get; set; }

    /// <summary>
    /// Код команды противника
    /// </summary>
    public int IdEnemyClub { get; set; }

    public virtual ICollection<GameIns> GameIns { get; set; } = new List<GameIns>();

    public virtual Countries IdCountryNavigation { get; set; } = null!;

    public virtual EnemyClubs IdEnemyClubNavigation { get; set; } = null!;

    public virtual GameLevels IdLevelNavigation { get; set; } = null!;

    public virtual OurClubs IdOurClubNavigation { get; set; } = null!;
}
