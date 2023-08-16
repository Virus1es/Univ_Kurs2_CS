using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using Wpf_Kurvovaya_BD.Controllers;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using Wpf_Kurvovaya_BD.Infrostructure;
using Microsoft.VisualBasic;
using System.Threading;
using System.Windows.Markup;
using System.Windows.Controls.DataVisualization.Charting;

namespace Wpf_Kurvovaya_BD.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    // основные таблицы
    List<GameIns> gameIns = QuarytiesController.ShowTableGameIns();
    List<Games> games = QuarytiesController.ShowTableGames();
    List<Gamers> gamers = QuarytiesController.ShowTableGamers();
    List<OurClubs> ourClubs = QuarytiesController.ShowTableOurClubs();
    List<EnemyClubs> enemyClubs = QuarytiesController.ShowTableEnemyClubs();

    // справочники
    List<Cities> cities = QuarytiesController.ShowTableCities();
    List<Countries> countries = QuarytiesController.ShowTableCountries();
    List<GameLevels> levels = QuarytiesController.ShowTableLevels();
    List<Positions> positions = QuarytiesController.ShowTablePositions();
    List<Managers> managers = QuarytiesController.ShowTableManagers();
    List<Leages> leages = QuarytiesController.ShowTableLeages();
    List<TrainingBasis> bases = QuarytiesController.ShowTableTrainingBasis();

    public MainWindow()
    {
        InitializeComponent();

        // вызов окна входа в приложение
        // если данные введены верно то мы пускаем пользователя
        // иначе закрываем приложение
        LogInWindow logInWindow = new LogInWindow();
        if (logInWindow.ShowDialog() == false)
            Application.Current.Shutdown(0);

        // Scaffold-DbContext "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=781227xy" Npgsql.EntityFrameworkCore.PostgreSQL
        // начальный вывод всех таблиц
        ShowGameInsTable();
        ShowGamesTable();
        ShowGamersTable();
        ShowOurClubTable();
        ShowEnemyClubTables();

        CbxColumn.ItemsSource = Utils.columnsName[0];
        CbxHelpTables.ItemsSource = new List<string> { 
            "Города",
            "Страны",
            "Уровни игры",
            "Позиции игроков",
            "Лиги",
            "Тренировочные базы",
            "Руководители клубов"
        };
        CbxHelpTables.SelectedIndex = 0;

        // закрываем окно ожидания
    }

    // завершение работы программы
    private void Shutdown_Click(object sender, RoutedEventArgs e) =>
        Application.Current.Shutdown(0);

    #region Простые_запросы

    // выполнить простой запрос 1
    private void QueryGamersByPosition(object sender, RoutedEventArgs e) {
        // ввод параметра запроса
        string value = Interaction.InputBox("Введите название позции игрока(с заглавной)",
                                            "Позиция игрока", 
                                            "Нападающий");

        // если ничего не ввели выходим
        if (string.IsNullOrEmpty(value))
            return;

        // назначаем колонки для вывода
        SetColGamers(DgSelect);

        var list = QuarytiesController.ExecGamersByPosition(value);

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Учёт игроков определённой позиции на поле' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list.Select(g => new ShowGamer(g.Id,
                                                          g.Surname, g.Name, g.Patronymic,
                                                          g.Position,
                                                          g.Birthday,
                                                          g.Club,
                                                          g.YearFact,
                                                          BitmapToBitmapSource(new Bitmap(new MemoryStream(g.Photo))),
                                                          g.Сomments, g.Cost));

        TbcCollections.SelectedIndex = 8;
    }

    // выполнить простой запрос 2
    private void QueryGamesByCountry(object sender, RoutedEventArgs e) {
        // ввод параметра запроса
        string value = Interaction.InputBox("Введите страну проведения игры",
                                            "Страна проведения игры", 
                                            "Аргентина");

        // если ничего не ввели выходим
        if (string.IsNullOrEmpty(value))
            return;

        // назначаем колонки для вывода
        SetColGames(DgSelect);

        var list = QuarytiesController.ExecGamesByCountry(value);

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Игры, проходящие в определённой стране' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }

    // выполнить простой запрос 3
    private void QueryGamesByDate(object sender, RoutedEventArgs e) {
        // ввод параметра запроса
        string value = Interaction.InputBox("Введите дату проведения игры(формат dd.MM.yyyy)",
                                            "Дата проведения игры", 
                                            "09.07.2020");

        // если ничего не ввели выходим
        if (string.IsNullOrEmpty(value))
            return;

        // назначаем колонки для вывода
        SetColGames(DgSelect);

        var list = QuarytiesController.ExecGamesByDate(value);

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Игры определённой даты' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }


    // выполнить простой запрос 4
    private void QueryGamersByBirthday(object sender, RoutedEventArgs e)
    {
        // ввод параметра запроса
        string value = Interaction.InputBox("Введите дату рождения игрока(формат dd.MM.yyyy)",
                                            "Дата рождения игрока",
                                            "30.05.1996");

        // если ничего не ввели выходим
        if (string.IsNullOrEmpty(value))
            return;

        // назначаем колонки для вывода
        SetColGamers(DgSelect);

        var list = QuarytiesController.ExecGamersByBirthday(value);

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Игроки родившиеся в определённую дату' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list.Select(g => new ShowGamer(g.Id,
                                                          g.Surname, g.Name, g.Patronymic,
                                                          g.Position,
                                                          g.Birthday,
                                                          g.Club,
                                                          g.YearFact,
                                                          BitmapToBitmapSource(new Bitmap(new MemoryStream(g.Photo))),
                                                          g.Сomments, g.Cost));

        TbcCollections.SelectedIndex = 8;
    }


    // выполнить простой запрос 5
    private void QueryFullGamers(object sender, RoutedEventArgs e)
    {
        // назначаем колонки для вывода
        SetColGamers(DgSelect);

        var list = QuarytiesController.ExecFullGamers();

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Игроки' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }


    // выполнить простой запрос 6
    private void QueryFullGames(object sender, RoutedEventArgs e)
    {
        // назначаем колонки для вывода
        SetColGames(DgSelect);

        var list = QuarytiesController.ExecFullGames();

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Игры' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }


    // выполнить простой запрос 7
    private void QueryFullOurClubs(object sender, RoutedEventArgs e)
    {
        // назначаем колонки для вывода
        SetColOurClubs(DgSelect);

        var list = QuarytiesController.ExecFullOurClub();

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Наши клубы' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }


    // выполнить простой запрос 8
    private void QueryNullGamerIns(object sender, RoutedEventArgs e)
    {
        // очищаем предыдущие столбцы
        DgSelect.ItemsSource = null!;
        DgSelect.Columns.Clear();

        // назначаем колонки для вывода
        var colFam = new DataGridTextColumn();
        colFam.Header = "Фамилия игрока";
        colFam.Binding = new Binding("Surname");
        DgSelect.Columns.Add(colFam);

        var colName = new DataGridTextColumn();
        colName.Header = "Имя игрока";
        colName.Binding = new Binding("Name");
        DgSelect.Columns.Add(colName);

        var colPat = new DataGridTextColumn();
        colPat.Header = "Отчество игрока";
        colPat.Binding = new Binding("Patronymic");
        DgSelect.Columns.Add(colPat);

        var colClub = new DataGridTextColumn();
        colClub.Header = "Позиция на поле";
        colClub.Binding = new Binding("Position");
        DgSelect.Columns.Add(colClub);

        var colYear = new DataGridTextColumn();
        colYear.Header = "Клуб";
        colYear.Binding = new Binding("Club");
        DgSelect.Columns.Add(colYear);

        var list = QuarytiesController.ExecNullGamerIns();

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Игроки, не учувствовавшие в матчах' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }


    // выполнить простой запрос 9
    private void QueryNullCountryGames(object sender, RoutedEventArgs e)
    {
        // очищаем предыдущие столбцы
        DgSelect.ItemsSource = null!;
        DgSelect.Columns.Clear();

        // назначаем колонки для вывода
        var colCoun= new DataGridTextColumn();
        colCoun.Header = "Cтрана";
        colCoun.Binding = new Binding();
        DgSelect.Columns.Add(colCoun);

        var list = QuarytiesController.ExecNullCountryGames();

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Страны, в которых не проводились игры' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }


    // выполнить простой запрос 10
    private void QueryGamersNotSelDate(object sender, RoutedEventArgs e)
    {
        // ввод параметра запроса
        string value = Interaction.InputBox("Введите дату проведения игры(формат dd.MM.yyyy)",
                                            "Дата проведения игры",
                                            "05.04.2023");

        // если ничего не ввели выходим
        if (string.IsNullOrEmpty(value))
            return;

        // очищаем предыдущие столбцы
        DgSelect.ItemsSource = null!;
        DgSelect.Columns.Clear();

        // назначаем колонки для вывода
        var colFam = new DataGridTextColumn();
        colFam.Header = "Фамилия игрока";
        colFam.Binding = new Binding("Surname");
        DgSelect.Columns.Add(colFam);

        var colName = new DataGridTextColumn();
        colName.Header = "Имя игрока";
        colName.Binding = new Binding("Name");
        DgSelect.Columns.Add(colName);

        var colPat = new DataGridTextColumn();
        colPat.Header = "Отчество игрока";
        colPat.Binding = new Binding("Patronymic");
        DgSelect.Columns.Add(colPat);

        var colClub = new DataGridTextColumn();
        colClub.Header = "Позиция на поле";
        colClub.Binding = new Binding("Position");
        DgSelect.Columns.Add(colClub);

        var colYear = new DataGridTextColumn();
        colYear.Header = "Клуб";
        colYear.Binding = new Binding("Club");
        DgSelect.Columns.Add(colYear);

        var list = QuarytiesController.ExecGamersNotSelDate(value);

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Игроки, которые не участвовали в играх определённой даты' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }
    #endregion


    #region Итоговые_запросы

    // выполнить итоговый запрос 1
    private void QueryGamesCountryCount(object sender, RoutedEventArgs e)
    {
        // очищаем предыдущие столбцы
        DgSelect.ItemsSource = null!;
        DgSelect.Columns.Clear();

        // назначаем колонки для вывода
        var colCoun = new DataGridTextColumn();
        colCoun.Header = "Cтрана";
        colCoun.Binding = new Binding("Country");
        DgSelect.Columns.Add(colCoun);

        var colAmount = new DataGridTextColumn();
        colAmount.Header = "Количество игр";
        colAmount.Binding = new Binding("Amount");
        DgSelect.Columns.Add(colAmount);

        var list = QuarytiesController.ExecGamesCountryCount();

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Количество проводимых игр всего и в странах' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }

    // выполнить итоговый запрос 2
    private void QueryClubGamersCost(object sender, RoutedEventArgs e)
    {
        string value = Interaction.InputBox("Введите стоимость контракта с игроком",
                                    "Стоимость контракта с игроком",
                                    "5000000");

        // если ничего не ввели выходим
        if (string.IsNullOrEmpty(value))
            return;

        // очищаем предыдущие столбцы
        DgSelect.ItemsSource = null!;
        DgSelect.Columns.Clear();

        // назначаем колонки для вывода
        var colCoun = new DataGridTextColumn();
        colCoun.Header = "Название клуба";
        colCoun.Binding = new Binding("Name");
        DgSelect.Columns.Add(colCoun);

        var colAmount = new DataGridTextColumn();
        colAmount.Header = "Количество игроков";
        colAmount.Binding = new Binding("Amount");
        DgSelect.Columns.Add(colAmount);

        var list = QuarytiesController.ExecClubGamersCost(value);

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Количество игроков в каждой команде со стоимостью контракта больше указанной' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }

    // выполнить итоговый запрос 3
    private void QueryClubGamersAvgCost(object sender, RoutedEventArgs e)
    {
        string value = Interaction.InputBox("Введите среднюю стоимость контракта с игроком",
                                    "Средняя стоимость контракта с игроком",
                                    "50000000");

        // если ничего не ввели выходим
        if (string.IsNullOrEmpty(value))
            return;

        // очищаем предыдущие столбцы
        DgSelect.ItemsSource = null!;
        DgSelect.Columns.Clear();

        // назначаем колонки для вывода
        var colCoun = new DataGridTextColumn();
        colCoun.Header = "Название клуба";
        colCoun.Binding = new Binding("Name");
        DgSelect.Columns.Add(colCoun);

        var colAmount = new DataGridTextColumn();
        colAmount.Header = "Средняя стоимость контракта";
        colAmount.Binding = new Binding("Amount");
        DgSelect.Columns.Add(colAmount);

        var list = QuarytiesController.ExecClubGamersAvgCost(value);

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Команды с средней стоимостью контракта игроков больше указанной' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }

    // выполнить итоговый запрос 4
    private void QueryExecClubsGamerPosSumCost(object sender, RoutedEventArgs e)
    {
        // позиция игроков
        string value = Interaction.InputBox("Введите позицию игроков (с залавной буквы)",
                                    "Позиция игроков",
                                    "Полузащитник");

        // если ничего не ввели выходим
        if (string.IsNullOrEmpty(value))
            return;

        // суммарная стоимсоть контракта
        string cost = Interaction.InputBox($"Введите суммарную стоимость контракта с игроками на позиции {value}",
                                    "Суммарная стоимость контракта с игроками",
                                    "60000000");

        // если ничего не ввели выходим
        if (string.IsNullOrEmpty(cost))
            return;

        // очищаем предыдущие столбцы
        DgSelect.ItemsSource = null!;
        DgSelect.Columns.Clear();

        // назначаем колонки для вывода
        var colCoun = new DataGridTextColumn();
        colCoun.Header = "Название клуба";
        colCoun.Binding = new Binding("Name");
        DgSelect.Columns.Add(colCoun);

        var colAmount = new DataGridTextColumn();
        colAmount.Header = "Суммарная стоимость контракта";
        colAmount.Binding = new Binding("Amount");
        DgSelect.Columns.Add(colAmount);

        var list = QuarytiesController.ExecClubsGamerPosSumCost(value, cost);

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Команды, где суммарная стоимость контракта игроков на определённой позиции больше указанной' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }

    // выполнить итоговый запрос 5
    private void QueryClubGamersMaxCostCount(object sender, RoutedEventArgs e)
    {
        // очищаем предыдущие столбцы
        DgSelect.ItemsSource = null!;
        DgSelect.Columns.Clear();

        // назначаем колонки для вывода
        var colCoun = new DataGridTextColumn();
        colCoun.Header = "Название клуба";
        colCoun.Binding = new Binding("Name");
        DgSelect.Columns.Add(colCoun);

        var colAmount = new DataGridTextColumn();
        colAmount.Header = "Количество игроков";
        colAmount.Binding = new Binding("Amount");
        DgSelect.Columns.Add(colAmount);

        var list = QuarytiesController.ExecClubGamersMaxCostCount();

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Количество игроков с минимальной стоимостью контракта в каждой команде' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }

    // выполнить итоговый запрос 6
    private void QueryMoscowClubs(object sender, RoutedEventArgs e)
    {
        // очищаем предыдущие столбцы
        DgSelect.ItemsSource = null!;
        DgSelect.Columns.Clear();

        // назначаем колонки для вывода
        var colCoun = new DataGridTextColumn();
        colCoun.Header = "Название клуба";
        colCoun.Binding = new Binding();
        DgSelect.Columns.Add(colCoun);

        var list = QuarytiesController.ExecMoscowClubs();

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Клубы, находящиеся в городе Москва' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }

    // выполнить итоговый запрос 7
    private void QueryNotMoscowClubs(object sender, RoutedEventArgs e)
    {
        // очищаем предыдущие столбцы
        DgSelect.ItemsSource = null!;
        DgSelect.Columns.Clear();

        // назначаем колонки для вывода
        var colCoun = new DataGridTextColumn();
        colCoun.Header = "Название клуба";
        colCoun.Binding = new Binding();
        DgSelect.Columns.Add(colCoun);

        var list = QuarytiesController.ExecNotMoscowClubs();

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Клубы, находящиеся не в городе Москва' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }

    // выполнить case запрос
    private void QueryCaseGameIns(object sender, RoutedEventArgs e)
    {
        // очищаем предыдущие столбцы
        DgSelect.ItemsSource = null!;
        DgSelect.Columns.Clear();

        var colName = new DataGridTextColumn();
        colName.Header = "Фамилия игрока";
        colName.Binding = new Binding("Surname");
        DgSelect.Columns.Add(colName);

        var colPat = new DataGridTextColumn();
        colPat.Header = "Имя игрока";
        colPat.Binding = new Binding("Name");
        DgSelect.Columns.Add(colPat);

        var colCoun = new DataGridTextColumn();
        colCoun.Header = "Отчество игрока";
        colCoun.Binding = new Binding("Patronymic");
        DgSelect.Columns.Add(colCoun);

        var colFam = new DataGridTextColumn();
        colFam.Header = "Дата проведения игры";
        colFam.Binding = new Binding("DateGame");
        colFam.Binding.StringFormat = "dd.MM.yyyy";
        DgSelect.Columns.Add(colFam);

        var colEnemy = new DataGridTextColumn();
        colEnemy.Header = "Клуб противника";
        colEnemy.Binding = new Binding("EnemyClub");
        DgSelect.Columns.Add(colEnemy);

        var col = new DataGridTextColumn();
        col.Header = "Наш клуб";
        col.Binding = new Binding("OurClub");
        DgSelect.Columns.Add(col);

        var colOrder = new DataGridTextColumn();
        colOrder.Header = "Участие в игре";
        colOrder.Binding = new Binding("Order");
        DgSelect.Columns.Add(colOrder);

        var list = QuarytiesController.ExecCaseGameIns();

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Участие игроков в матчах (case)' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }

    // выполнить запрос без идекса
    private void QueryWithoutIndex(object sender, RoutedEventArgs e)
    {
        // очищаем предыдущие столбцы
        DgSelect.ItemsSource = null!;
        DgSelect.Columns.Clear();

        // назначаем колонки для вывода
        var colCoun = new DataGridTextColumn();
        colCoun.Header = "Название страны";
        colCoun.Binding = new Binding("Name");
        DgSelect.Columns.Add(colCoun);

        var colAmount = new DataGridTextColumn();
        colAmount.Header = "Количество игр";
        colAmount.Binding = new Binding("Amount");
        DgSelect.Columns.Add(colAmount);

        var list = QuarytiesController.ExecWithoutIndex();

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Количество провведённых игр по странам за последний год' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }

    // выполнить запрос с идексом
    private void QueryWithIndex(object sender, RoutedEventArgs e)
    {
        // очищаем предыдущие столбцы
        DgSelect.ItemsSource = null!;
        DgSelect.Columns.Clear();

        // назначаем колонки для вывода
        var colCoun = new DataGridTextColumn();
        colCoun.Header = "Название клуба";
        colCoun.Binding = new Binding("Name");
        DgSelect.Columns.Add(colCoun);

        var colAmount = new DataGridTextColumn();
        colAmount.Header = "Количество игроков";
        colAmount.Binding = new Binding("Amount");
        DgSelect.Columns.Add(colAmount);

        var list = QuarytiesController.ExecWithIndex();

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Количество игроков в клубах со стоимостью контракта больше 50 000 000' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }

    // выполнить запрос по маске
    private void QueryMask(object sender, RoutedEventArgs e)
    {
        // очищаем предыдущие столбцы
        DgSelect.ItemsSource = null!;
        DgSelect.Columns.Clear();

        // назначаем колонки для вывода
        var colCoun = new DataGridTextColumn();
        colCoun.Header = "Название клуба";
        colCoun.Binding = new Binding("Name");
        DgSelect.Columns.Add(colCoun);

        var colAmount = new DataGridTextColumn();
        colAmount.Header = "Количество игроков";
        colAmount.Binding = new Binding("Amount");
        DgSelect.Columns.Add(colAmount);

        var list = QuarytiesController.ExecMask();

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Игроки фамилия которых начинается на букву М' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }

    // выполнить запрос 1 по варианту курсовой
    private void QueryFirstCurs(object sender, RoutedEventArgs e)
    {
        // очищаем предыдущие столбцы
        DgSelect.ItemsSource = null!;
        DgSelect.Columns.Clear();

        var colName = new DataGridTextColumn();
        colName.Header = "Фамилия игрока";
        colName.Binding = new Binding("Surname");
        DgSelect.Columns.Add(colName);

        var colPat = new DataGridTextColumn();
        colPat.Header = "Имя игрока";
        colPat.Binding = new Binding("Name");
        DgSelect.Columns.Add(colPat);

        var colCoun = new DataGridTextColumn();
        colCoun.Header = "Отчество игрока";
        colCoun.Binding = new Binding("Patronymic");
        DgSelect.Columns.Add(colCoun);

        var col = new DataGridTextColumn();
        col.Header = "Наш клуб";
        col.Binding = new Binding("OurClub");
        DgSelect.Columns.Add(col);

        var colOrder = new DataGridTextColumn();
        colOrder.Header = "Всего голов";
        colOrder.Binding = new Binding("SumGols");
        DgSelect.Columns.Add(colOrder);

        var list = QuarytiesController.ExecFirstCurs();

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Определить 3 лучших футболиста каждой команды (по количеству забитых голов) и 3 лучшие команды' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }

    // выполнить запрос 2 по варианту курсовой
    private void QuerySecondCurs(object sender, RoutedEventArgs e)
    {
        // очищаем предыдущие столбцы
        DgSelect.ItemsSource = null!;
        DgSelect.Columns.Clear();

        var colName = new DataGridTextColumn();
        colName.Header = "Фамилия игрока";
        colName.Binding = new Binding("Surname");
        DgSelect.Columns.Add(colName);

        var colPat = new DataGridTextColumn();
        colPat.Header = "Имя игрока";
        colPat.Binding = new Binding("Name");
        DgSelect.Columns.Add(colPat);

        var colCoun = new DataGridTextColumn();
        colCoun.Header = "Отчество игрока";
        colCoun.Binding = new Binding("Patronymic");
        DgSelect.Columns.Add(colCoun);

        var col = new DataGridTextColumn();
        col.Header = "Наш клуб";
        col.Binding = new Binding("OurClub");
        DgSelect.Columns.Add(col);

        var colOrder = new DataGridTextColumn();
        colOrder.Header = "Среднее количетво голов";
        colOrder.Binding = new Binding("AvgGols");
        DgSelect.Columns.Add(colOrder);

        var colAvg = new DataGridTextColumn();
        colAvg.Header = "Среднее количетво пропущенных";
        colAvg.Binding = new Binding("AvgMiss");
        DgSelect.Columns.Add(colAvg);

        var list = QuarytiesController.ExecSecondCurs();

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Cреднее количество забитых и пропущенных мячей игроками и нашими командами в целом' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }

    // выполнить запрос 3 по варианту курсовой
    private void QueryThirdCurs(object sender, RoutedEventArgs e)
    {
        // позиция игроков
        string value = Interaction.InputBox("Введите название команды (с залавной буквы)",
                                    "Название команды",
                                    "ЦСКА");

        // если ничего не ввели выходим
        if (string.IsNullOrEmpty(value))
            return;

        // суммарная стоимсоть контракта
        string year = Interaction.InputBox("Введите год для которого нужно выполнить запрос",
                                    "Год учёта",
                                    "2019");

        // если ничего не ввели выходим
        if (string.IsNullOrEmpty(year))
            return;

        // очищаем предыдущие столбцы
        DgSelect.ItemsSource = null!;
        DgSelect.Columns.Clear();

        // назначаем колонки для вывода
        var colCoun = new DataGridTextColumn();
        colCoun.Header = "Название клуба";
        colCoun.Binding = new Binding("Club");
        DgSelect.Columns.Add(colCoun);

        var colAmount = new DataGridTextColumn();
        colAmount.Header = "Количество игр";
        colAmount.Binding = new Binding("CountGames");
        DgSelect.Columns.Add(colAmount);

        var colSalary = new DataGridTextColumn();
        colSalary.Header = "Финансирование клуба";
        colSalary.Binding = new Binding("SumSalary");
        DgSelect.Columns.Add(colSalary);

        var list = QuarytiesController.ExecThirdCurs(value, year);

        // выводим поясняющий текст
        TblSelect.Text = $"Запрос 'Определить количество игр и финансирование выбранного клуба за указанный год' вернул {list.Count} записей";

        // выводим результат запроса
        DgSelect.ItemsSource = list;

        TbcCollections.SelectedIndex = 8;
    }

    #endregion

    // вывод результата запроса в Эксель
    private void PrinExcelFile(object sender, RoutedEventArgs e) =>
        QuarytiesController.PrintExcelFile();

    // вывод диаграммы запроса
    private void PrintDiagram(object sender, RoutedEventArgs e)
    {
        var data = QuarytiesController.GetKeyValueList();

        Series series = new ColumnSeries
        {
            Title = "Количество игр",
            ItemsSource = data,
            DependentValuePath = "Value",
            IndependentValuePath = "Key"
        };

        Histogram.Series.Add(series);

        TbcCollections.SelectedIndex = 6;
    }



    #region ReRead

    // перечитать таблицу GameIns из БД
    private void RereadGameIns_Click(object sender, RoutedEventArgs e)
    {
        // перечитать из базы таблицу
        gameIns = QuarytiesController.ShowTableGameIns();

        // вывести таблицу и нформацию о ней
        ShowGameInsTable();
    }

    // перечитать таблицу Games из БД
    private void RereadGames_Click(object sender, RoutedEventArgs e)
    {
        // перечитать из базы таблицу
        games = QuarytiesController.ShowTableGames();

        // вывести таблицу и нформацию о ней
        ShowGamesTable();
    }

    // перечитать таблицу Gamers из БД
    private void RereadGamers_Click(object sender, RoutedEventArgs e)
    {
        // перечитать из базы таблицу
        gamers = QuarytiesController.ShowTableGamers();

        // вывести таблицу и нформацию о ней
        ShowGamersTable();
    }

    // перечитать таблицу OurClubs из БД
    private void RereadOurClubs_Click(object sender, RoutedEventArgs e)
    {
        // перечитать из базы таблицу
        ourClubs = QuarytiesController.ShowTableOurClubs();

        // вывести таблицу и нформацию о ней
        ShowOurClubTable();
    }

    // перечитать таблицу EnemyClubs из БД
    private void RereadEnemyClubs_Click(object sender, RoutedEventArgs e)
    {
        // перечитать из базы таблицу
        enemyClubs = QuarytiesController.ShowTableEnemyClubs();

        // вывести таблицу и нформацию о ней
        ShowEnemyClubTables();
    }


    #endregion


    #region GameIns_CRUD
    // подтвержение удаления
    private void AskGameInDelete_Click(object sender, RoutedEventArgs e)
    {
        // если подтвердили удаление то выполняем
        // иначе ничего не делаем
        if (MessageBox.Show("Вы точно хотите удалить эту запись?",
                            "Подтверждение",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            DeleteGameIn_Click(sender, e);
    }

    // вызов окна добавления записи
    private void AddWindowGameIn_Click(object sender, RoutedEventArgs e)
    {
        // вызов окна
        AddEditGameInWindow addEditGameInWindow = new AddEditGameInWindow();

        // если нажали "ОК" тогда перечитываем данные
        // иначе ничего не делаем
        if (addEditGameInWindow.ShowDialog() == true)
        {
            gameIns = QuarytiesController.ShowTableGameIns();
            // MessageBox.Show("Данные обновляются", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowGameInsTable();
        }
    }
    // вызов окна редактирования записи
    private void EditWindowGameIn_Click(object sender, RoutedEventArgs e)
    {
        // запоминаем индекс выбранного игрока
        int index = DgParticipation.SelectedIndex;

        // если никого не выбрали молча уходим
        if (index == -1)
            return;

        AddEditGameInWindow addEditGameInWindow = new AddEditGameInWindow(gameIns[index]);
        if (addEditGameInWindow.ShowDialog() == true)
        {
            gameIns = QuarytiesController.ShowTableGameIns();
            // MessageBox.Show("Данные обновляются", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowGameInsTable();
        }
    }

    // удаление игрока из таблицы
    private void DeleteGameIn_Click(object sender, RoutedEventArgs e)
    {
        // запоминаем индекс выбранного игрока
        int index = DgParticipation.SelectedIndex;

        // если никого не выбрали молча уходим
        if (index == -1)
            return;

        PostgresContext db = new PostgresContext();

        // удалить выбранную запись
        db.Entry(gameIns[index]).State = EntityState.Deleted;
        db.GameIns.Remove(gameIns[index]);

        // записать в базу данных
        db.SaveChanges();

        // перечитать данные таблицы
        gameIns = QuarytiesController.ShowTableGameIns();
        ShowGameInsTable();
    }
    #endregion


    #region Gamers_CRUD
    // реализация каскадного удаления для игроков
    private void CascadeGameIns_Click(object sender, RoutedEventArgs e)
    {
        int index = DgGamers.SelectedIndex;

        // если ничего не выбранно выходим
        if (index < 0)
            return;

        // если подтвердили удаление то выполняем и перечитываем таблицы
        // иначе ничего не делаем
        if (QuarytiesController.CascadeDelete(index))
        {
            // перечитать таблицу игроков
            RereadGamers_Click(sender, e);

            // перечитать таблицу участий в играх
            RereadGameIns_Click(sender, e);
        }
            
    }

    // вызов окна добавления записи
    private void AddWindowGamer_Click(object sender, RoutedEventArgs e)
    {
        // вызов окна
        AddEditWindow addEditWindow = new AddEditWindow();

        // если нажали "ОК" тогда перечитываем данные
        // иначе ничего не делаем
        if (addEditWindow.ShowDialog() == true)
        {
            gamers = QuarytiesController.ShowTableGamers();
            // MessageBox.Show("Данные обновляются", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowGamersTable();
        }
    }
    // вызов окна редактирования записи
    private void EditWindowGamer_Click(object sender, RoutedEventArgs e)
    {
        // запоминаем индекс выбранного игрока
        int index = DgGamers.SelectedIndex;

        // если никого не выбрали молча уходим
        if (index == -1)
            return;

        AddEditWindow addEditWindow = new AddEditWindow(gamers[index]);
        if (addEditWindow.ShowDialog() == true)
        {
            gamers = QuarytiesController.ShowTableGamers();
            // MessageBox.Show("Данные обновляются", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowGamersTable();
        }
    }

    // удаление игрока из таблицы
    private void DeleteGamer_Click(object sender, RoutedEventArgs e)
    {
        // запоминаем индекс выбранного игрока
        int index = DgGamers.SelectedIndex;

        // если никого не выбрали молча уходим
        if (index == -1)
            return;

        PostgresContext db = new PostgresContext();

        // удалить выбранную запись
        db.Entry(gamers[index]).State = EntityState.Deleted;
        db.Gamers.Remove(gamers[index]);

        // записать в базу данных
        db.SaveChanges();

        // перечитать данные таблицы
        gamers = QuarytiesController.ShowTableGamers();
        ShowGamersTable();
    }
    #endregion


    #region Games_CRUD
    // подтвержение удаления
    private void AskGameDelete_Click(object sender, RoutedEventArgs e)
    {
        // если подтвердили удаление то выполняем
        // иначе ничего не делаем
        if (MessageBox.Show("Вы точно хотите удалить эту запись?",
                            "Подтверждение",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            DeleteGame_Click(sender, e);
    }

    // вызов окна добавления записи
    private void AddWindowGame_Click(object sender, RoutedEventArgs e)
    {
        // вызов окна
        AddEditGameWindow addEditGameWindow = new AddEditGameWindow();

        // если нажали "ОК" тогда перечитываем данные
        // иначе ничего не делаем
        if (addEditGameWindow.ShowDialog() == true)
        {
            games = QuarytiesController.ShowTableGames();
            // MessageBox.Show("Данные обновляются", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowGamesTable();
        }
    }
    // вызов окна редактирования записи
    private void EditWindowGame_Click(object sender, RoutedEventArgs e)
    {
        // запоминаем индекс выбранного игрока
        int index = DgGames.SelectedIndex;

        // если никого не выбрали молча уходим
        if (index == -1)
            return;

        AddEditGameWindow addEditGameWindow = new AddEditGameWindow(games[index]);
        if (addEditGameWindow.ShowDialog() == true)
        {
            games = QuarytiesController.ShowTableGames();
            // MessageBox.Show("Данные обновляются", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowGamesTable();
        }
    }

    // удаление игрока из таблицы
    private void DeleteGame_Click(object sender, RoutedEventArgs e)
    {
        // запоминаем индекс выбранного игрока
        int index = DgGames.SelectedIndex;

        // если никого не выбрали молча уходим
        if (index == -1)
            return;

        PostgresContext db = new PostgresContext();

        // удалить выбранную запись
        db.Entry(games[index]).State = EntityState.Deleted;
        db.Games.Remove(games[index]);

        // записать в базу данных
        db.SaveChanges();

        // перечитать данные таблицы
        games = QuarytiesController.ShowTableGames();
        ShowGamesTable();
    }
    #endregion


    #region OurClubs_CRUD
    // подтвержение удаления
    private void AskOurClubDelete_Click(object sender, RoutedEventArgs e)
    {
        // если подтвердили удаление то выполняем
        // иначе ничего не делаем
        if (MessageBox.Show("Вы точно хотите удалить эту запись?",
                            "Подтверждение",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            DeleteOurClub_Click(sender, e);
    }

    // вызов окна добавления записи
    private void AddWindowOurClub_Click(object sender, RoutedEventArgs e)
    {
        // вызов окна
        AddEditOurClubWindow addEditOurClubWindow = new AddEditOurClubWindow();

        // если нажали "ОК" тогда перечитываем данные
        // иначе ничего не делаем
        if (addEditOurClubWindow.ShowDialog() == true)
        {
            ourClubs = QuarytiesController.ShowTableOurClubs();
            // MessageBox.Show("Данные обновляются", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowOurClubTable();
        }
    }
    // вызов окна редактирования записи
    private void EditWindowOurClub_Click(object sender, RoutedEventArgs e)
    {
        // запоминаем индекс выбранного игрока
        int index = DgOurClubs.SelectedIndex;

        // если никого не выбрали молча уходим
        if (index == -1)
            return;

        AddEditOurClubWindow addEditOurClubWindow = new AddEditOurClubWindow(ourClubs[index]);
        if (addEditOurClubWindow.ShowDialog() == true)
        {
            ourClubs = QuarytiesController.ShowTableOurClubs();
            // MessageBox.Show("Данные обновляются", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowOurClubTable();
        }
    }

    // удаление игрока из таблицы
    private void DeleteOurClub_Click(object sender, RoutedEventArgs e)
    {
        // запоминаем индекс выбранного игрока
        int index = DgOurClubs.SelectedIndex;

        // если никого не выбрали молча уходим
        if (index == -1)
            return;

        PostgresContext db = new PostgresContext();

        // удалить выбранную запись
        db.Entry(ourClubs[index]).State = EntityState.Deleted;
        db.OurClubs.Remove(ourClubs[index]);

        // записать в базу данных
        db.SaveChanges();

        // перечитать данные таблицы
        ourClubs = QuarytiesController.ShowTableOurClubs();
        ShowOurClubTable();
    }
    #endregion


    #region EnemyClubs_CRUD
    // подтвержение удаления
    private void AskEnemyClubDelete_Click(object sender, RoutedEventArgs e)
    {
        // если подтвердили удаление то выполняем
        // иначе ничего не делаем
        if (MessageBox.Show("Вы точно хотите удалить эту запись?",
                            "Подтверждение",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            DeleteEnemyClub_Click(sender, e);
    }

    // вызов окна добавления записи
    private void AddWindowEnemyClub_Click(object sender, RoutedEventArgs e)
    {
        // вызов окна
        AddEditEnemyClubWindow addEditEnemyClubWindow = new AddEditEnemyClubWindow();

        // если нажали "ОК" тогда перечитываем данные
        // иначе ничего не делаем
        if (addEditEnemyClubWindow.ShowDialog() == true)
        {
            enemyClubs = QuarytiesController.ShowTableEnemyClubs();
            // MessageBox.Show("Данные обновляются", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowEnemyClubTables();
        }
    }
    // вызов окна редактирования записи
    private void EditWindowEnemyClub_Click(object sender, RoutedEventArgs e)
    {
        // запоминаем индекс выбранного игрока
        int index = DgEnemyClubs.SelectedIndex;

        // если никого не выбрали молча уходим
        if (index == -1)
            return;

        AddEditEnemyClubWindow addEditEnemyClubWindow = new AddEditEnemyClubWindow(enemyClubs[index]);
        if (addEditEnemyClubWindow.ShowDialog() == true)
        {
            enemyClubs = QuarytiesController.ShowTableEnemyClubs();
            // MessageBox.Show("Данные обновляются", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowEnemyClubTables();
        }
    }

    // удаление игрока из таблицы
    private void DeleteEnemyClub_Click(object sender, RoutedEventArgs e)
    {
        // запоминаем индекс выбранного игрока
        int index = DgEnemyClubs.SelectedIndex;

        // если никого не выбрали молча уходим
        if (index == -1)
            return;

        PostgresContext db = new PostgresContext();

        // удалить выбранную запись
        db.Entry(enemyClubs[index]).State = EntityState.Deleted;
        db.EnemyClubs.Remove(enemyClubs[index]);

        // записать в базу данных
        db.SaveChanges();

        // перечитать данные таблицы
        enemyClubs = QuarytiesController.ShowTableEnemyClubs();
        ShowEnemyClubTables();
    }
    #endregion


    #region HelpTables_CRUD

    // подтвержение удаления
    private void AskHelpTableDelete_Click(object sender, RoutedEventArgs e)
    {
        // если подтвердили удаление то выполняем
        // иначе ничего не делаем
        if (MessageBox.Show("Вы точно хотите удалить эту запись?",
                            "Подтверждение",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            DeleteHelpTable_Click(sender, e);
    }

    // вызов окна добавления записи
    private void AddWindowHelpTable_Click(object sender, RoutedEventArgs e)
    {
        // вызов окна
        Window addEditHelpTablesWindow = CbxHelpTables.SelectedIndex switch
        {
            0 => new AddEditCityWindow(),
            1 => new AddEditCountryWindow(),
            2 => new AddEditLevelWindow(),
            3 => new AddEditPositionWindow(),
            4 => new AddEditLeagueWindow(),
            5 => new AddEditBasesWindow(),
            _ => new AddEditManagerWindow()
        };

        // если нажали "ОК" тогда перечитываем данные
        // иначе ничего не делаем
        if (addEditHelpTablesWindow.ShowDialog() == true)
        {
            switch (CbxHelpTables.SelectedIndex)
            {
                case 0:
                    DgAdditional.ItemsSource = cities = QuarytiesController.ShowTableCities();
                    break;
                case 1:
                    DgAdditional.ItemsSource = countries = QuarytiesController.ShowTableCountries();
                    break;
                case 2:
                    DgAdditional.ItemsSource = levels = QuarytiesController.ShowTableLevels();
                    break;
                case 3:
                    DgAdditional.ItemsSource = positions = QuarytiesController.ShowTablePositions();
                    break;
                case 4:
                    DgAdditional.ItemsSource = leages = QuarytiesController.ShowTableLeages();
                    break;
                case 5:
                    DgAdditional.ItemsSource = bases = QuarytiesController.ShowTableTrainingBasis();
                    break;
                default:
                    DgAdditional.ItemsSource = managers = QuarytiesController.ShowTableManagers();
                    break;
            }
        }
    }
    // вызов окна редактирования записи
    private void EditWindowHelpTable_Click(object sender, RoutedEventArgs e)
    {
        // запоминаем индекс выбранного игрока
        int index = DgAdditional.SelectedIndex;

        // если никого не выбрали молча уходим
        if (index == -1)
            return;

        // вызов окна
        Window addEditHelpTablesWindow = CbxHelpTables.SelectedIndex switch
        {
            0 => new AddEditCityWindow(cities[index]),
            1 => new AddEditCountryWindow(countries[index]),
            2 => new AddEditLevelWindow(levels[index]),
            3 => new AddEditPositionWindow(positions[index]),
            4 => new AddEditLeagueWindow(leages[index]),
            5 => new AddEditBasesWindow(bases[index]),
            _ => new AddEditManagerWindow(managers[index])
        };

        // если нажали "ОК" тогда перечитываем данные
        // иначе ничего не делаем
        if (addEditHelpTablesWindow.ShowDialog() == true)
        {
            switch (CbxHelpTables.SelectedIndex)
            {
                case 0:
                    DgAdditional.ItemsSource = cities = QuarytiesController.ShowTableCities();
                    break;
                case 1:
                    DgAdditional.ItemsSource = countries = QuarytiesController.ShowTableCountries();
                    break;
                case 2:
                    DgAdditional.ItemsSource = levels = QuarytiesController.ShowTableLevels();
                    break;
                case 3:
                    DgAdditional.ItemsSource = positions = QuarytiesController.ShowTablePositions();
                    break;
                case 4:
                    DgAdditional.ItemsSource = leages = QuarytiesController.ShowTableLeages();
                    break;
                case 5:
                    DgAdditional.ItemsSource = bases = QuarytiesController.ShowTableTrainingBasis();
                    break;
                default:
                    DgAdditional.ItemsSource = managers = QuarytiesController.ShowTableManagers();
                    break;
            }
            
        }
    }

    // удаление игрока из таблицы
    private void DeleteHelpTable_Click(object sender, RoutedEventArgs e)
    {
        // запоминаем индекс выбранного игрока
        int index = DgAdditional.SelectedIndex;

        // если никого не выбрали молча уходим
        if (index == -1)
            return;

        PostgresContext db = new PostgresContext();

        // удалить выбранную запись
        switch (CbxHelpTables.SelectedIndex)
        {
            case 0:
                db.Entry(cities[index]).State = EntityState.Deleted;
                db.Cities.Remove(cities[index]);
                break;
            case 1:
                db.Entry(countries[index]).State = EntityState.Deleted;
                db.Countries.Remove(countries[index]);
                break;
            case 2:
                db.Entry(levels[index]).State = EntityState.Deleted;
                db.GameLevels.Remove(levels[index]);
                break;
            case 3:
                db.Entry(positions[index]).State = EntityState.Deleted;
                db.Positions.Remove(positions[index]);
                break;
            case 4:
                db.Entry(leages[index]).State = EntityState.Deleted;
                db.Leages.Remove(leages[index]);
                break;
            case 5:
                db.Entry(bases[index]).State = EntityState.Deleted;
                db.TrainingBases.Remove(bases[index]);
                break;
            default:
                db.Entry(managers[index]).State = EntityState.Deleted;
                db.Managers.Remove(managers[index]);
                break;
        }
        
        // записать в базу данных
        db.SaveChanges();

        // перечитать данные таблицы
        switch (CbxHelpTables.SelectedIndex)
        {
            case 0:
                DgAdditional.ItemsSource = cities = QuarytiesController.ShowTableCities();
                break;
            case 1:
                DgAdditional.ItemsSource = countries = QuarytiesController.ShowTableCountries();
                break;
            case 2:
                DgAdditional.ItemsSource = levels = QuarytiesController.ShowTableLevels();
                break;
            case 3:
                DgAdditional.ItemsSource = positions = QuarytiesController.ShowTablePositions();
                break;
            case 4:
                DgAdditional.ItemsSource = leages = QuarytiesController.ShowTableLeages();
                break;
            case 5:
                DgAdditional.ItemsSource = bases = QuarytiesController.ShowTableTrainingBasis();
                break;
            default:
                DgAdditional.ItemsSource = managers = QuarytiesController.ShowTableManagers();
                break;
        }
        
    }

    #endregion

    // сортировка по возрастанию выбранной таблицы по выбранному полю
    private void SortAscByChoose(object sender, RoutedEventArgs e)
    {
        switch (TbcCollections.SelectedIndex)
        {
            case 0:
                games = QuarytiesController.SortGamesAsc(CbxColumn.SelectedIndex);
                ShowGamesTable();
                break;
            
            case 1:
                ourClubs = QuarytiesController.SortOurClubsAsc(CbxColumn.SelectedIndex);
                ShowOurClubTable();
                break;

            case 2:
                enemyClubs = QuarytiesController.SortEnemyClubsAsc(CbxColumn.SelectedIndex);
                ShowEnemyClubTables();
                break;

            case 3:
                gamers = QuarytiesController.SortGamersAsc(CbxColumn.SelectedIndex);
                ShowGamersTable();
                break;

            case 4:
                gameIns = QuarytiesController.SortGameInsAsc(CbxColumn.SelectedIndex);
                ShowGameInsTable();
                break;

            case 5:
                switch (CbxHelpTables.SelectedIndex)
                {
                    case 0:
                        DgAdditional.ItemsSource = cities = cities.OrderBy(c => c.City).ToList();
                        break;
                    case 1:
                        DgAdditional.ItemsSource = countries = countries.OrderBy(c => c.Country).ToList();
                        break;
                    case 2:
                        DgAdditional.ItemsSource = levels = levels.OrderBy(l => l.GameLevel).ToList();
                        break;
                    case 3:
                        DgAdditional.ItemsSource = positions = positions.OrderBy(p => p.Position).ToList();
                        break;
                    case 4:
                        DgAdditional.ItemsSource = leages = leages.OrderBy(l => l.Leage).ToList();
                        break;
                    case 5:
                        DgAdditional.ItemsSource = bases = bases.OrderBy(b => b.Base).ToList();
                        break;
                    case 6:
                        DgAdditional.ItemsSource = managers = CbxColumn.SelectedIndex switch
                        {
                            0 => managers.OrderBy(m => m.Surname).ToList(),
                            1 => managers.OrderBy(m => m.Name).ToList(),
                            2 => managers.OrderBy(m => m.Patronymic).ToList(),
                            3 => managers.OrderBy(m => m.Phone).ToList(),
                            _ => managers
                        };
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        
    }

    // сортировка по убыванию выбранной таблицы по выбранному полю
    private void SortDescByChoose(object sender, RoutedEventArgs e)
    {
        switch (TbcCollections.SelectedIndex)
        {
            case 0:
                games = QuarytiesController.SortGamesDesc(CbxColumn.SelectedIndex);
                ShowGamesTable();
                break;

            case 1:
                ourClubs = QuarytiesController.SortOurClubsDesc(CbxColumn.SelectedIndex);
                ShowOurClubTable();
                break;

            case 2:
                enemyClubs = QuarytiesController.SortEnemyClubsDesc(CbxColumn.SelectedIndex);
                ShowEnemyClubTables();
                break;

            case 3:
                gamers = QuarytiesController.SortGamersDesc(CbxColumn.SelectedIndex);
                ShowGamersTable();
                break;

            case 4:
                gameIns = QuarytiesController.SortGameInsDesc(CbxColumn.SelectedIndex);
                ShowGameInsTable();
                break;

            case 5:
                switch (CbxHelpTables.SelectedIndex)
                {
                    case 0:
                        DgAdditional.ItemsSource = cities = cities.OrderByDescending(c => c.City).ToList();
                        break;
                    case 1:
                        DgAdditional.ItemsSource = countries = countries.OrderByDescending(c => c.Country).ToList();
                        break;
                    case 2:
                        DgAdditional.ItemsSource = levels = levels.OrderByDescending(l => l.GameLevel).ToList();
                        break;
                    case 3:
                        DgAdditional.ItemsSource = positions = positions.OrderByDescending(p => p.Position).ToList();
                        break;
                    case 4:
                        DgAdditional.ItemsSource = leages = leages.OrderByDescending(l => l.Leage).ToList();
                        break;
                    case 5:
                        DgAdditional.ItemsSource = bases = bases.OrderByDescending(b => b.Base).ToList();
                        break;
                    case 6:
                        DgAdditional.ItemsSource = managers = CbxColumn.SelectedIndex switch
                        {
                            0 => managers.OrderByDescending(m => m.Surname).ToList(),
                            1 => managers.OrderByDescending(m => m.Name).ToList(),
                            2 => managers.OrderByDescending(m => m.Patronymic).ToList(),
                            3 => managers.OrderByDescending(m => m.Phone).ToList(),
                            _ => managers
                        };
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

    }

    // удаление группы записей выбранной таблицы по выбранному полю
    private void DeleteGroupByChoose(object sender, RoutedEventArgs e)
    {
        switch (TbcCollections.SelectedIndex)
        {
            case 0:
                if (QuarytiesController.DeleteGamesByCol(CbxColumn.SelectedIndex, TbxValue.Text))
                    ShowGamesTable();
                break;

            case 1:
                if (QuarytiesController.DeleteOurClubsByCol(CbxColumn.SelectedIndex, TbxValue.Text))
                    ShowOurClubTable();
                break;

            case 2:
                if (QuarytiesController.DeleteEnemyClubsByCol(CbxColumn.SelectedIndex, TbxValue.Text))
                    ShowEnemyClubTables();
                break;

            case 3:
                if (QuarytiesController.DeleteGamersByCol(CbxColumn.SelectedIndex, TbxValue.Text))
                    ShowGamersTable();
                break;

            case 4:
                if (QuarytiesController.DeleteGameInsByCol(CbxColumn.SelectedIndex, TbxValue.Text))
                    ShowGameInsTable();
                break;

            case 5:
                switch (CbxHelpTables.SelectedIndex)
                {
                    case 0:
                        if (QuarytiesController.DeleteCitiesByCol(0, TbxValue.Text))
                            DgAdditional.ItemsSource = cities = QuarytiesController.ShowTableCities();
                        break;
                    case 1:
                        if (QuarytiesController.DeleteCountriesByCol(0, TbxValue.Text))
                            DgAdditional.ItemsSource = countries = QuarytiesController.ShowTableCountries();
                        break;
                    case 2:
                        if (QuarytiesController.DeleteGameLevelsByCol(0, TbxValue.Text))
                            DgAdditional.ItemsSource = levels = QuarytiesController.ShowTableLevels();
                        break;
                    case 3:
                        if (QuarytiesController.DeletePositionsByCol(0, TbxValue.Text))
                            DgAdditional.ItemsSource = positions = QuarytiesController.ShowTablePositions();
                        break;
                    case 4:
                        if (QuarytiesController.DeleteLeagesByCol(0, TbxValue.Text))
                            DgAdditional.ItemsSource = leages = QuarytiesController.ShowTableLeages();
                        break;
                    case 5:
                        if (QuarytiesController.DeleteTrainingBasesByCol(0, TbxValue.Text))
                            DgAdditional.ItemsSource = bases = QuarytiesController.ShowTableTrainingBasis();
                        break;
                    case 6:
                        if (QuarytiesController.DeleteManagersByCol(CbxColumn.SelectedIndex, TbxValue.Text))
                            DgAdditional.ItemsSource = managers = QuarytiesController.ShowTableManagers();
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

    }
    
    // удаление группы записей выбранной таблицы по выбранному полю
    private void FindGroupByChoose(object sender, RoutedEventArgs e)
    {
        DgChoose.ItemsSource = null!;
        switch (TbcCollections.SelectedIndex)
        {
            case 0:
                var list0 = QuarytiesController.FindGamesByCol(CbxColumn.SelectedIndex, TbxValue.Text);
                TblChooseInfo.Text = $"Из таблицы 'Информация о играх' по условию поле = значение отобрано {list0.Count} записей";
                SetColGames(DgChoose);
                DgChoose.ItemsSource = list0.Select(g => new ShowGame(g.Id,
                                                             g.DateGame,
                                                             countries.First(coun => g.IdCountry == coun.Id).Country,
                                                             levels.First(l => g.IdLevel == l.Id).GameLevel,
                                                             g.CountFinish,
                                                             enemyClubs.First(c => g.IdEnemyClub == c.Id).Opposing,
                                                             ourClubs.First(c => g.IdOurClub == c.Id).Club));
                break;

            case 1:
                var list1 = QuarytiesController.FindOurClubsByCol(CbxColumn.SelectedIndex, TbxValue.Text);
                TblChooseInfo.Text = $"Из таблицы 'Наши клубы' по условию поле = значение отобрано {list1.Count} записей";
                SetColOurClubs(DgChoose);
                DgChoose.ItemsSource = list1.Select(c => new ShowOurClub(c.Id,
                                                                      c.Club, bases.First(b => c.IdBase == b.Id).Base, c.Year,
                                                                      leages.First(l => c.IdLeage == l.Id).Leage,
                                                                      managers.First(m => c.IdManager == m.Id).Surname,
                                                                      managers.First(m => c.IdManager == m.Id).Name,
                                                                      managers.First(m => c.IdManager == m.Id).Patronymic,
                                                                      managers.First(m => c.IdManager == m.Id).Phone,
                                                                      cities.First(city => c.IdCity == city.Id).City));
                break;

            case 2:
                var list2 = QuarytiesController.FindEnemyClubsByCol(CbxColumn.SelectedIndex, TbxValue.Text);
                TblChooseInfo.Text = $"Из таблицы 'Клубы противника' по условию поле = значение отобрано {list2.Count} записей";
                SetColEnemyClubs(DgChoose);
                DgChoose.ItemsSource = list2.Select(c => new ShowEnemyClub(c.Id, c.Opposing,
                                                                            countries.First(coun => c.IdCountry == coun.Id).Country,
                                                                            c.SurnameCoach, c.NameCoach, c.PatronymicCoach));
                break;

            case 3:
                var list3 = QuarytiesController.FindGamersByCol(CbxColumn.SelectedIndex, TbxValue.Text);
                TblChooseInfo.Text = $"Из таблицы 'Наши игроки' по условию поле = значение отобрано {list3.Count} записей";
                SetColGamers(DgChoose);
                DgChoose.ItemsSource = list3.Select(g => new ShowGamer(g.Id,
                                                                g.Surname, g.Name, g.Patronymic,
                                                                positions.First(p => g.IdPosition == p.Id).Position,
                                                                g.Birthday,
                                                                ourClubs.First(c => g.IdClub == c.Id).Club,
                                                                g.YearFact,
                                                                BitmapToBitmapSource(new Bitmap(new MemoryStream(g.Photo))),
                                                                g.Comments, g.Cost));
                break;

            case 4:
                var list4 = QuarytiesController.FindGameInsByCol(CbxColumn.SelectedIndex, TbxValue.Text);
                TblChooseInfo.Text = $"Из таблицы 'Участвие в играх' по условию поле = значение отобрано {list4.Count} записей";
                SetColGameIns(DgChoose);
                DgChoose.ItemsSource = list4.Select(g => new ShowGameIn(g.Id,
                                                                         gamers.First(gamer => g.IdGamer == gamer.Id).Surname,
                                                                         gamers.First(gamer => g.IdGamer == gamer.Id).Name,
                                                                         gamers.First(gamer => g.IdGamer == gamer.Id).Patronymic,
                                                                         games.First(game => g.IdGame == game.Id).DateGame,
                                                                         enemyClubs.First(club => games.First(game => g.IdGame == game.Id).IdEnemyClub == club.Id).Opposing,
                                                                         ourClubs.First(club => games.First(game => g.IdGame == game.Id).IdOurClub == club.Id).Club,
                                                                         g.Order, g.CountStart, g.Salary));
                break;

            case 5:
                SetHelpCol(DgChoose);
                break;
            default:
                break;
        }

        TbcCollections.SelectedIndex = 7;
    }

    // действия при смене выбранного клуба(состовная форма)
    private void DgOurClubs_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // запоминаем выбранный клуб
        int index = DgOurClubs.SelectedIndex;

        // если ничего не выбранно молча выходим
        if (index == -1) return;

        // выбраем нужных нам игроков
        // показываем их
        DgGamersByClub.ItemsSource = gamers.Where(g => g.IdClub == index + 1)
                                           ?.Select(g => new ShowGamer(g.Id, g.Surname, g.Name,  g.Patronymic,
                                                                       positions[g.IdPosition - 1].Position,
                                                                       g.Birthday, ourClubs[g.IdClub - 1].Club,
                                                                       g.YearFact,
                                                                       BitmapToBitmapSource(new Bitmap(new MemoryStream(g.Photo))),
                                                                       g.Comments,  g.Cost));
    }

    // действия при выборе справочника
    private void CbxHelpTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // инициализируем переменные для добавления полей в таблицу
        // 2 минимум - справедливо для любой таблицы
        var col = new DataGridTextColumn();
        var colId = new DataGridTextColumn();

        // очищаем предыдущие столбцы
        DgAdditional.ItemsSource = null!;
        DgAdditional.Columns.Clear();

        // добавляем столбец номера
        colId.Header = "№";
        colId.Binding = new Binding("Id");
        DgAdditional.Columns.Add(colId);

        // добавляем остальные столбцы
        switch (CbxHelpTables.SelectedIndex)
        {
            case 0:
                col.Header = "Название города";
                col.Binding = new Binding("City");
                DgAdditional.Columns.Add(col);
                DgAdditional.ItemsSource = cities;
                break;

            case 1:
                col.Header = "Название страны";
                col.Binding = new Binding("Country");
                DgAdditional.Columns.Add(col);
                DgAdditional.ItemsSource = countries;
                break;

            case 2:
                col.Header = "Название уровня игры";
                col.Binding = new Binding("GameLevel");
                DgAdditional.Columns.Add(col);
                DgAdditional.ItemsSource = levels;
                break;

            case 3:
                col.Header = "Название позиции";
                col.Binding = new Binding("Position");
                DgAdditional.Columns.Add(col);
                DgAdditional.ItemsSource = positions;
                break;

            case 4:
                col.Header = "Название лиги";
                col.Binding = new Binding("Leage");
                DgAdditional.Columns.Add(col);
                DgAdditional.ItemsSource = leages;
                break;

            case 5:
                col.Header = "Название тренировочной базы";
                col.Binding = new Binding("Base");
                DgAdditional.Columns.Add(col);
                DgAdditional.ItemsSource = bases;
                break;

            case 6:
                var colFam = new DataGridTextColumn();
                colFam.Header = "Фамилия руководителя";
                colFam.Binding = new Binding("Surname");
                DgAdditional.Columns.Add(colFam);

                var colName = new DataGridTextColumn();
                colName.Header = "Имя руководителя";
                colName.Binding = new Binding("Name");
                DgAdditional.Columns.Add(colName);

                var colPat = new DataGridTextColumn();
                colPat.Header = "Отчество руководителя";
                colPat.Binding = new Binding("Patronymic");
                DgAdditional.Columns.Add(colPat);

                col.Header = "Телефон руководителя";
                col.Binding = new Binding("Phone");
                DgAdditional.Columns.Add(col);

                DgAdditional.ItemsSource = managers;
                break;

            default:
                break;
        }
    }

    // действия при смене выбранной таблицы
    private void TbcCollections_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        int index = TbcCollections.SelectedIndex;

        if (index < 0 || index > 5) return;

        CbxColumn.ItemsSource = null!;
        CbxColumn.ItemsSource = Utils.columnsName[index];
    }



    #region Helping

    // заполнение столбцов для GameIns
    public void SetColGameIns(DataGrid data)
    {

        // инициализируем переменные для добавления полей в таблицу
        // 2 минимум - справедливо для любой таблицы
        var col = new DataGridTextColumn();
        var colId = new DataGridTextColumn();

        // очищаем предыдущие столбцы
        data.ItemsSource = null!;
        data.Columns.Clear();

        // добавляем столбец номера
        colId.Header = "№";
        colId.Binding = new Binding("Id");
        data.Columns.Add(colId);

        var colFam = new DataGridTextColumn();
        colFam.Header = "Фамилия игрока";
        colFam.Binding = new Binding("Surname");
        data.Columns.Add(colFam);

        var colName = new DataGridTextColumn();
        colName.Header = "Имя игрока";
        colName.Binding = new Binding("Name");
        data.Columns.Add(colName);

        var colPat = new DataGridTextColumn();
        colPat.Header = "Отчество игрока";
        colPat.Binding = new Binding("Patronymic");
        data.Columns.Add(colPat);

        var colDate = new DataGridTextColumn();
        colDate.Header = "Дата проведения";
        colDate.Binding = new Binding("DateGame");
        colDate.Binding.StringFormat = "dd.MM.yyyy";
        data.Columns.Add(colDate);

        var colEnemy = new DataGridTextColumn();
        colEnemy.Header = "Клуб противника";
        colEnemy.Binding = new Binding("Opposing");
        data.Columns.Add(colEnemy);

        col.Header = "Наш клуб";
        col.Binding = new Binding("Club");
        data.Columns.Add(col);


        var colOrder = new DataGridTextColumn();
        colOrder.Header = "Участие в игре";
        colOrder.Binding = new Binding("Order");
        data.Columns.Add(colOrder);

        var colCoun = new DataGridTextColumn();
        colCoun.Header = "Пропущенные мячи";
        colCoun.Binding = new Binding("CountStart");
        data.Columns.Add(colCoun);

        var colSal = new DataGridTextColumn();
        colSal.Header = "Премия";
        colSal.Binding = new Binding("Salary");
        data.Columns.Add(colSal);


    }

    // заполнение столбцов для Games
    public void SetColGames(DataGrid data)
    {

        // инициализируем переменные для добавления полей в таблицу
        // 2 минимум - справедливо для любой таблицы
        var col = new DataGridTextColumn();
        var colId = new DataGridTextColumn();

        // очищаем предыдущие столбцы
        data.ItemsSource = null!;
        data.Columns.Clear();

        // добавляем столбец номера
        colId.Header = "№";
        colId.Binding = new Binding("Id");
        data.Columns.Add(colId);

        var colFam = new DataGridTextColumn();
        colFam.Header = "Дата проведения";
        colFam.Binding = new Binding("DateGame");
        colFam.Binding.StringFormat = "dd.MM.yyyy";
        data.Columns.Add(colFam);

        var colName = new DataGridTextColumn();
        colName.Header = "Страна проведения";
        colName.Binding = new Binding("Country");
        data.Columns.Add(colName);

        var colPat = new DataGridTextColumn();
        colPat.Header = "Уровень игры";
        colPat.Binding = new Binding("GameLevel");
        data.Columns.Add(colPat);

        var colCoun = new DataGridTextColumn();
        colCoun.Header = "Пропущенные мячи";
        colCoun.Binding = new Binding("CountFinish");
        data.Columns.Add(colCoun);

        var colEnemy = new DataGridTextColumn();
        colEnemy.Header = "Клуб противника";
        colEnemy.Binding = new Binding("Opposing");
        data.Columns.Add(colEnemy);

        col.Header = "Наш клуб";
        col.Binding = new Binding("Club");
        data.Columns.Add(col);
    }

    // заполнение столбцов для EnemyClubs
    public void SetColEnemyClubs(DataGrid data)
    {

        // инициализируем переменные для добавления полей в таблицу
        // 2 минимум - справедливо для любой таблицы
        var col = new DataGridTextColumn();
        var colId = new DataGridTextColumn();

        // очищаем предыдущие столбцы
        data.ItemsSource = null!;
        data.Columns.Clear();

        // добавляем столбец номера
        colId.Header = "№";
        colId.Binding = new Binding("Id");
        data.Columns.Add(colId);

        var colClub = new DataGridTextColumn();
        colClub.Header = "Название клуба";
        colClub.Binding = new Binding("Opposing");
        data.Columns.Add(colClub);

        var colBase = new DataGridTextColumn();
        colBase.Header = "Страна размещения";
        colBase.Binding = new Binding("Country");
        data.Columns.Add(colBase);

        var colFam = new DataGridTextColumn();
        colFam.Header = "Фамилия тренера";
        colFam.Binding = new Binding("Surname");
        data.Columns.Add(colFam);

        var colName = new DataGridTextColumn();
        colName.Header = "Имя тренера";
        colName.Binding = new Binding("Name");
        data.Columns.Add(colName);

        var colPat = new DataGridTextColumn();
        colPat.Header = "Отчество тренера";
        colPat.Binding = new Binding("Patronymic");
        data.Columns.Add(colPat);
    }

    // заполнение столбцов для OurClubs
    public void SetColOurClubs(DataGrid data)
    {

        // инициализируем переменные для добавления полей в таблицу
        // 2 минимум - справедливо для любой таблицы
        var col = new DataGridTextColumn();
        var colId = new DataGridTextColumn();

        // очищаем предыдущие столбцы
        data.ItemsSource = null!;
        data.Columns.Clear();

        // добавляем столбец номера
        colId.Header = "№";
        colId.Binding = new Binding("Id");
        data.Columns.Add(colId);

        var colClub = new DataGridTextColumn();
        colClub.Header = "Название клуба";
        colClub.Binding = new Binding("Club");
        data.Columns.Add(colClub);

        var colBase = new DataGridTextColumn();
        colBase.Header = "Тренировочная база";
        colBase.Binding = new Binding("Base");
        data.Columns.Add(colBase);

        var colYear = new DataGridTextColumn();
        colYear.Header = "Год создания";
        colYear.Binding = new Binding("Year");
        data.Columns.Add(colYear);

        var colLea = new DataGridTextColumn();
        colLea.Header = "Лига клуба";
        colLea.Binding = new Binding("League");
        data.Columns.Add(colLea);

        var colFam = new DataGridTextColumn();
        colFam.Header = "Фамилия менеджера";
        colFam.Binding = new Binding("Surname");
        data.Columns.Add(colFam);

        var colName = new DataGridTextColumn();
        colName.Header = "Имя менеджера";
        colName.Binding = new Binding("Name");
        data.Columns.Add(colName);

        var colPat = new DataGridTextColumn();
        colPat.Header = "Отчество менеджера";
        colPat.Binding = new Binding("Patronymic");
        data.Columns.Add(colPat);

        var colCoun = new DataGridTextColumn();
        colCoun.Header = "Телефон руководителя";
        colCoun.Binding = new Binding("Phone");
        data.Columns.Add(colCoun);

        col.Header = "Город размещения";
        col.Binding = new Binding("City");
        data.Columns.Add(col);
    }

    // заполнение столбцов для Gamers
    public void SetColGamers(DataGrid data)
    {
        // инициализируем переменные для добавления полей в таблицу
        // 2 минимум - справедливо для любой таблицы
        var col = new DataGridTextColumn();
        var colId = new DataGridTextColumn();

        // очищаем предыдущие столбцы
        data.ItemsSource = null!;
        data.Columns.Clear();
        

        // добавляем столбец номера
        colId.Header = "№";
        colId.Binding = new Binding("Id");
        colId.Width = 50;
        data.Columns.Add(colId);

        var colFam = new DataGridTextColumn();
        colFam.Header = "Фамилия игрока";
        colFam.Binding = new Binding("Surname");
        data.Columns.Add(colFam);

        var colName = new DataGridTextColumn();
        colName.Header = "Имя игрока";
        colName.Binding = new Binding("Name");
        data.Columns.Add(colName);

        var colPat = new DataGridTextColumn();
        colPat.Header = "Отчество игрока";
        colPat.Binding = new Binding("Patronymic");
        data.Columns.Add(colPat);

        var colClub = new DataGridTextColumn();
        colClub.Header = "Позиция на поле";
        colClub.Binding = new Binding("Position");
        data.Columns.Add(colClub);

        var colBase = new DataGridTextColumn();
        colBase.Header = "Дата рождения";
        colBase.Binding = new Binding("Birthday");
        colBase.Binding.StringFormat = "dd.MM.yyyy";
        data.Columns.Add(colBase);

        var colYear = new DataGridTextColumn();
        colYear.Header = "Клуб";
        colYear.Width = 100;
        colYear.Binding = new Binding("Club");
        data.Columns.Add(colYear);

        var colLea = new DataGridTextColumn();
        colLea.Header = "Год принятия в команду";
        colLea.Binding = new Binding("YearFact");
        data.Columns.Add(colLea);

        //var colPhoto = new DataGridTemplateColumn();
        //colPhoto.Header = "Год принятия в команду";
        //FrameworkElementFactory imageFactory = new FrameworkElementFactory();
        //imageFactory.SetBinding(System.Windows.Controls.Image.SourceProperty, new Binding("Photo"));
        //data.Columns.Add(colPhoto);

        var colCom = new DataGridTextColumn();
        colCom.Header = "Контракт игрока";
        colCom.Binding = new Binding("Сomments");
        data.Columns.Add(colCom);

        col.Header = "Стоимость контракта, руб.";
        col.Binding = new Binding("Cost");
        data.Columns.Add(col);
    }

    public void SetHelpCol(DataGrid data)
    {
        // инициализируем переменные для добавления полей в таблицу
        // 2 минимум - справедливо для любой таблицы
        var col = new DataGridTextColumn();
        var colId = new DataGridTextColumn();

        // очищаем предыдущие столбцы
        data.ItemsSource = null!;
        data.Columns.Clear();

        // добавляем столбец номера
        colId.Header = "№";
        colId.Binding = new Binding("Id");
        data.Columns.Add(colId);

        // добавляем остальные столбцы
        switch (CbxHelpTables.SelectedIndex)
        {
            case 0:
                var list0 = QuarytiesController.FindCitiesByCol(TbxValue.Text);
                col.Header = "Название города";
                col.Binding = new Binding("City");
                data.Columns.Add(col);
                TblChooseInfo.Text = $"Из таблицы 'Города' по условию поле = значение отобрано {list0.Count} записей";
                data.ItemsSource = list0;
                break;

            case 1:
                var list1 = QuarytiesController.FindCountriesByCol(TbxValue.Text);
                col.Header = "Название страны";
                col.Binding = new Binding("Country");
                data.Columns.Add(col);
                TblChooseInfo.Text = $"Из таблицы 'Страны' по условию поле = значение отобрано {list1.Count} записей";
                data.ItemsSource = list1;
                break;

            case 2:
                var list2 = QuarytiesController.FindGameLevelsByCol(TbxValue.Text);
                col.Header = "Название уровня игры";
                col.Binding = new Binding("GameLevel");
                data.Columns.Add(col);
                TblChooseInfo.Text = $"Из таблицы 'Уровни игры' по условию поле = значение отобрано {list2.Count} записей";
                data.ItemsSource = list2;
                break;

            case 3:
                var list3 = QuarytiesController.FindPositionsByCol(TbxValue.Text);
                col.Header = "Название позиции";
                col.Binding = new Binding("Position");
                data.Columns.Add(col);
                TblChooseInfo.Text = $"Из таблицы 'Позиции на поле' по условию поле = значение отобрано {list3.Count} записей";
                data.ItemsSource = list3;
                break;

            case 4:
                var list4 = QuarytiesController.FindLeagesByCol(TbxValue.Text);
                col.Header = "Название лиги";
                col.Binding = new Binding("Leage");
                data.Columns.Add(col);
                TblChooseInfo.Text = $"Из таблицы 'Названия лиг' по условию поле = значение отобрано {list4.Count} записей"; 
                data.ItemsSource = list4;
                break;

            case 5:
                var list5 = QuarytiesController.FindTrainingBasesByCol(TbxValue.Text);
                col.Header = "Название тренировочной базы";
                col.Binding = new Binding("Base");
                data.Columns.Add(col);
                TblChooseInfo.Text = $"Из таблицы 'Тренировочные базы' по условию поле = значение отобрано {list5.Count} записей";
                data.ItemsSource = list5;
                break;

            case 6:
                var list6 = QuarytiesController.FindManagersByCol(CbxColumn.SelectedIndex, TbxValue.Text);
                TblChooseInfo.Text = $"Из таблицы 'Руководители клубов' по условию поле = значение отобрано {list6.Count} записей";

                var colFam = new DataGridTextColumn();
                colFam.Header = "Фамилия руководителя";
                colFam.Binding = new Binding("Surname");
                data.Columns.Add(colFam);

                var colName = new DataGridTextColumn();
                colName.Header = "Имя руководителя";
                colName.Binding = new Binding("Name");
                data.Columns.Add(colName);

                var colPat = new DataGridTextColumn();
                colPat.Header = "Отчество руководителя";
                colPat.Binding = new Binding("Patronymic");
                data.Columns.Add(colPat);

                col.Header = "Телефон руководителя";
                col.Binding = new Binding("Phone");
                data.Columns.Add(col);

                data.ItemsSource = list6;
                break;

            default:
                break;
        }
    }

    // вывод таблицы Участие в играх
    public void ShowGameInsTable()
    {
        TblParticipation.Text = $"В таблице {gameIns.Count} записей";
        DgParticipation.ItemsSource = null!;
        DgParticipation.ItemsSource = gameIns.Select(g => new ShowGameIn(g.Id,
                                                                         gamers.First(gamer => g.IdGamer == gamer.Id).Surname,
                                                                         gamers.First(gamer => g.IdGamer == gamer.Id).Name,
                                                                         gamers.First(gamer => g.IdGamer == gamer.Id).Patronymic,
                                                                         games.First(game => g.IdGame == game.Id).DateGame,
                                                                         enemyClubs.First(club => games.First(game => g.IdGame == game.Id).IdEnemyClub == club.Id).Opposing,
                                                                         ourClubs.First(club => games.First(game => g.IdGame == game.Id).IdOurClub == club.Id).Club,
                                                                         g.Order, g.CountStart, g.Salary));
    }

    // вывод таблицы Игры
    private void ShowGamesTable()
    {
        TblGames.Text = $"В таблице {games.Count} записей";
        DgGames.ItemsSource = null!;
        DgGames.ItemsSource = games.Select(g => new ShowGame(g.Id,
                                                             g.DateGame,
                                                             countries.First(coun => g.IdCountry == coun.Id).Country,
                                                             levels.First(l => g.IdLevel == l.Id).GameLevel,
                                                             g.CountFinish,
                                                             enemyClubs.First(c => g.IdEnemyClub == c.Id).Opposing,
                                                             ourClubs.First(c => g.IdOurClub == c.Id).Club));
    }

    // вывод таблицы Игроки
    private void ShowGamersTable()
    {
        TblGamers.Text = $"В таблице {gamers.Count} записей";
        DgGamers.ItemsSource = null!;
        DgGamers.ItemsSource = gamers.Select(g => new ShowGamer(g.Id,
                                                                g.Surname, g.Name, g.Patronymic,
                                                                positions.First(p => g.IdPosition == p.Id).Position,
                                                                g.Birthday,
                                                                ourClubs.First(c => g.IdClub == c.Id).Club,
                                                                g.YearFact,
                                                                BitmapToBitmapSource(new Bitmap(new MemoryStream(g.Photo))),
                                                                g.Comments, g.Cost));
    }

    // вывод таблицы Наши клубы
    private void ShowOurClubTable()
    {
        TblOurClubs.Text = $"В таблице {ourClubs.Count} записей";
        DgOurClubs.ItemsSource = null!;
        DgOurClubs.ItemsSource = ourClubs.Select(c => new ShowOurClub(c.Id,
                                                                      c.Club, bases.First(b => c.IdBase == b.Id).Base, c.Year,
                                                                      leages.First(l => c.IdLeage == l.Id).Leage,
                                                                      managers.First(m => c.IdManager == m.Id).Surname,
                                                                      managers.First(m => c.IdManager == m.Id).Name,
                                                                      managers.First(m => c.IdManager == m.Id).Patronymic,
                                                                      managers.First(m => c.IdManager == m.Id).Phone,
                                                                      cities.First(city => c.IdCity == city.Id).City));
    }

    // вывод таблицы Клубы противника
    private void ShowEnemyClubTables()
    {
        TblEnemyClubs.Text = $"В таблице {enemyClubs.Count} записей";
        DgEnemyClubs.ItemsSource = null!;
        DgEnemyClubs.ItemsSource = enemyClubs.Select(c => new ShowEnemyClub(c.Id, c.Opposing, 
                                                                            countries.First(coun => c.IdCountry == coun.Id).Country,
                                                                            c.SurnameCoach, c.NameCoach, c.PatronymicCoach));
    }

    // преобразование картинки для вывода
    BitmapSource BitmapToBitmapSource(Bitmap bitmap) =>
        System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(),
                                                                     IntPtr.Zero,
                                                                     Int32Rect.Empty,
                                                                     BitmapSizeOptions.FromEmptyOptions());

    #endregion
    
}

#region Records
public record ShowGame(int Id, DateOnly DateGame, string Country, string GameLevel, int CountFinish, string Opposing, string Club);
public record ShowGameIn(int Id, string Surname, string Name, string Patronymic,
                  DateOnly DateGame, string Opposing, string Club,
                  bool Order, int CountStart, int Salary);
public record ShowGamer(int Id,
                 string Surname, string Name, string Patronymic, string Position, DateOnly Birthday,
                 string Club, int YearFact, BitmapSource Photo, string Сomments, long Cost);

public record ShowOurClub(int Id, string Club, string Base, int Year,
                   string League, string Surname, string Name, string Patronymic,
                   string Phone, string City);
public record ShowEnemyClub(int Id, string Opposing, string Country,
                     string SurnameCoach, string NameCoach, string PatronymicCoach);
public record GamersForNull(string Surname, string Name, string Patronymic, string Position, string Club);
#endregion
