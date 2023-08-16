using System;
using System.Collections.Generic;

namespace Wpf_Kurvovaya_BD;

public partial class Gamers
{
    /// <summary>
    /// Код игрока
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Имя игрока
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Фамилия игрока
    /// </summary>
    public string Surname { get; set; } = null!;

    /// <summary>
    /// Отчество игрока
    /// </summary>
    public string Patronymic { get; set; } = null!;

    /// <summary>
    /// Код позиции игрока на поле
    /// </summary>
    public int IdPosition { get; set; }

    /// <summary>
    /// День рождения игрока
    /// </summary>
    public DateOnly Birthday { get; set; }

    /// <summary>
    /// Год принятия игрока в клуб
    /// </summary>
    public int YearFact { get; set; }

    /// <summary>
    /// Код клуба игрока
    /// </summary>
    public int IdClub { get; set; }

    /// <summary>
    /// Фото игрока
    /// </summary>
    public byte[] Photo { get; set; } = null!;

    /// <summary>
    /// Контракт с игроком
    /// </summary>
    public string Comments { get; set; } = null!;

    /// <summary>
    /// Стоимость контракта
    /// </summary>
    public long Cost { get; set; }

    public virtual ICollection<GameIns> GameIns { get; set; } = new List<GameIns>();

    public virtual OurClubs IdClubNavigation { get; set; } = null!;

    public virtual Positions IdPositionNavigation { get; set; } = null!;
}
