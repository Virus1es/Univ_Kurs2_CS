using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;

namespace Wpf_Kurvovaya_BD.Infrostructure;

internal class Utils
{
    public static Random random = new Random();

    // формирование случайных чисел в диапазоне от lo до hi
    public static int GetRandom(int lo, int hi) =>
        random.Next(lo, hi + 1);
    public static double GetRandom(double lo, double hi) =>
        lo + (hi - lo) * random.NextDouble();


    // списки столбцов таблиц
    public static List<List<string>> columnsName = new List<List<string>>()
    {
        // талица Игры
        new List<string>
        {
            "Дата проведения",
            "Страна проведения",
            "Уровень игры",
            "Пропущенные мячи",
            "Клуб противника",
            "Наш клуб"
        },

        // таблица наши клубы
        new List<string>
        {
            "Название",
            "Тренировочная база", 
            "Год создания",
            "Лига",
            "Фамилия менеджера",
            "Имя менеджера",
            "Отчество менеджера",
            "Телефон менеджера",
            "Город размещения"
        },

        // таблица клубы противника
        new List<string>
        {
            "Название",
            "Страна размещения",
            "Фамилия тренера",
            "Имя тренера",
            "Отчество тренера"
        },

        // таблица Наши игроки
        new List<string>
        {
            "Фамилия игрока",
            "Имя игрока",
            "Отчество игрока",
            "Позиция",
            "Дата рождения",
            "Клуб игрока",
            "Год принятия",
            "Контракт",
            "Стоимость контракта"
        },

        // таблица Участи е матчах
        new List<string>
        {
            "Фамилия игрока",
            "Имя игрока",
            "Отчество игрока",
            "Дата проведения",
            "Клуб противника",
            "Наш клуб",
            "Участие в игре",
            "Забитые мячи",
            "Премия"
        },

        // таблица Руководители клубов
        new List<string>
        {
            "Фамилия менеджера",
            "Имя менеджера",
            "Отчество менеджера",
            "Телефон менеджера"
        }
    };



}
