using System;
using System.Collections.Generic;

namespace Wpf_Kurvovaya_BD;

public partial class EnemyClubs
{
    /// <summary>
    /// Код клуба противника
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название команды
    /// </summary>
    public string Opposing { get; set; } = null!;

    /// <summary>
    /// Код страны размещения клуба
    /// </summary>
    public int IdCountry { get; set; }

    /// <summary>
    /// Фамилия тренера клуба 
    /// </summary>
    public string SurnameCoach { get; set; } = null!;

    /// <summary>
    /// Имя тренера клуба
    /// </summary>
    public string NameCoach { get; set; } = null!;

    /// <summary>
    /// Отчество тренера клуба
    /// </summary>
    public string PatronymicCoach { get; set; } = null!;

    public virtual ICollection<Games> Games { get; set; } = new List<Games>();

    public virtual Countries IdCountryNavigation { get; set; } = null!;
}
