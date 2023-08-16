using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wpf_Kurvovaya_BD.Controllers;

namespace Wpf_Kurvovaya_BD.Views;

/// <summary>
/// Логика взаимодействия для AddEditGameWindow.xaml
/// </summary>
public partial class AddEditGameWindow : Window
{
    PostgresContext db = new PostgresContext();
    List<Countries> countries = QuarytiesController.ShowTableCountries();
    List<GameLevels> levels = QuarytiesController.ShowTableLevels();
    List<OurClubs> ourClubs = QuarytiesController.ShowTableOurClubs();
    List<EnemyClubs> enemyClubs = QuarytiesController.ShowTableEnemyClubs();
    bool flag = true;
    int Id = 0;

    // вызов окна как окна добавления новой записи
    public AddEditGameWindow()
    {
        InitializeComponent();
    }

    // вызов окна как окна изменения записи
    public AddEditGameWindow(Games game)
    {
        InitializeComponent();

        // обнавление информации окна
        Title = "Редактирование иформации о игре";
        TblInfo.Text = "Редактирование иформации о игре(Таблица 'Информация о играх')";

        flag = false;

        // назначение полям нужных значений
        Id = game.Id;
        DateGameDate.Text = game.DateGame.ToString();
        CbxCountry.SelectedItem = countries.First(coun => game.IdCountry == coun.Id).Country;
        CbxGameLevel.SelectedItem = levels.First(l => game.IdLevel == l.Id).GameLevel;
        TbxCountFinish.Text = game.CountFinish.ToString();
        CbxOurClub.SelectedItem = enemyClubs.First(c => game.IdEnemyClub == c.Id).Opposing;
        CbxEnemyClub.SelectedItem = ourClubs.First(c => game.IdOurClub == c.Id).Club;

    }

    // действия при загрузке окна
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // заполнение комбобоксов информацией
        CbxCountry.ItemsSource = countries.Select(c => c.Country);
        CbxGameLevel.ItemsSource = levels.Select(l => l.GameLevel);
        CbxOurClub.ItemsSource = ourClubs.Select(c => c.Club);
        CbxEnemyClub.ItemsSource = enemyClubs.Select(c => c.Opposing);
    }

    // нажатие на кнопку ОК
    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        Games game = null!;

        try
        {
            game = new Games
            {
                DateGame = DateOnly.FromDateTime((DateTime)DateGameDate?.SelectedDate),
                IdCountry = countries.First(coun => CbxCountry.Text == coun.Country).Id,
                IdLevel = levels.First(l => CbxGameLevel.Text == l.GameLevel).Id,
                CountFinish = int.Parse(TbxCountFinish.Text),
                IdOurClub = ourClubs.First(c => CbxOurClub.Text == c.Club).Id,
                IdEnemyClub = enemyClubs.First(c => CbxEnemyClub.Text == c.Opposing).Id
            };

            // вставить запись в коллекцию
            if (flag)
                db.Games.Add(game);
            else
            {
                game.Id = Id;
                // изменить запись в коллекции
                db.Entry(game).State = EntityState.Modified;
                db.Games.Update(game);
            }
            // сохранить в базу данных
            db.SaveChanges();

            DialogResult = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            // если вставка или обновление не прошло
            // сбравываем нужные показатели для повтора
            if (flag)
                db.Games.Remove(game);
            else
                db.Entry(game).State = EntityState.Detached;
        }
    }


    // разрешаем только нажатия цифр для TextBox'ов
    private void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (Key.D0 <= e.Key && e.Key <= Key.D9) return;
        e.Handled = true;
    }

}
