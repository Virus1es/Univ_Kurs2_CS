using System;
using System.Collections.Generic;

namespace Wpf_Kurvovaya_BD;

public partial class Managers
{
    /// <summary>
    /// Код руководителя клуба/ов
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Фамилия руководителя
    /// </summary>
    public string Surname { get; set; } = null!;

    /// <summary>
    /// Имя руководителя
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Отчество руководителя
    /// </summary>
    public string Patronymic { get; set; } = null!;

    /// <summary>
    /// Номер телефона руководителя
    /// </summary>
    public string Phone { get; set; } = null!;

    public virtual ICollection<OurClubs> OurClubs { get; set; } = new List<OurClubs>();
}
