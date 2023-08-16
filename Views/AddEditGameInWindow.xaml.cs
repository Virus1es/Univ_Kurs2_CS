using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wpf_Kurvovaya_BD.Controllers;

namespace Wpf_Kurvovaya_BD.Views;

/// <summary>
/// Логика взаимодействия для AddEditGameInWindow.xaml
/// </summary>
public partial class AddEditGameInWindow : Window
{
    PostgresContext db = new PostgresContext();
    List<Gamers> gamers = QuarytiesController.ShowTableGamers();
    List<Games> games = QuarytiesController.ShowTableGames();
    List<EnemyClubs> enemyClubs = QuarytiesController.ShowTableEnemyClubs();

    bool flag = true;
    int Id = 0;

    // вызов окна как окна добавления новой записи
    public AddEditGameInWindow()
    {
        InitializeComponent();
    }

    // вызов окна как окна изменения записи
    public AddEditGameInWindow(GameIns gameIn)
    {
        InitializeComponent();

        // обнавление информации окна
        Title = "Редактирование участия игрока в игре";
        TblInfo.Text = "Редактирование участия игрока в игре(Таблица 'Участие игроков в матчах')";

        flag = false;

        // назначение полям нужных значений
        Id = gameIn.Id;

        Gamers temp = gamers.First(gamer => gameIn.IdGamer == gamer.Id);
        Games temp1 = games.First(game => gameIn.IdGame == game.Id);

        CbxGamer.SelectedItem = $"{temp.Surname} {temp.Name} {temp.Patronymic}";
        CbxGame.SelectedItem = $"{temp1.DateGame}: {enemyClubs.First(club => temp1.IdEnemyClub == club.Id).Opposing}";
        ChbxOrder.IsChecked = gameIn.Order;
        TbxCountBalls.Text = gameIn.CountStart.ToString();
        TbxSalary.Text = gameIn.Salary.ToString();

    }

    // действия при загрузке окна
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // заполнение комбобоксов информацией
        CbxGamer.ItemsSource = gamers.Select(g => $"{g.Surname} {g.Name} {g.Patronymic}");
        CbxGame.ItemsSource = games.Select(g => $"{g.DateGame}: " +
        $"{enemyClubs.First(club => g.IdEnemyClub == club.Id).Opposing}");
    }

    // нажатие на кнопку ОК
    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        GameIns gameIn = null!;

        try
        {
            gameIn = new GameIns
            { 
                IdGamer = gamers.First(g => CbxGamer.Text == $"{g.Surname} {g.Name} {g.Patronymic}").Id,
                IdGame = games.First(g => CbxGame.Text == $"{enemyClubs.First(club => g.IdEnemyClub == club.Id).Opposing}").Id,
                Order = (bool)ChbxOrder.IsChecked,
                CountStart = int.Parse(TbxCountBalls.Text),
                Salary = int.Parse(TbxSalary.Text)
            };

            // вставить запись в коллекцию
            if (flag)
                db.GameIns.Add(gameIn);
            else
            {
                gameIn.Id = Id;
                // изменить запись в коллекции
                db.Entry(gameIn).State = EntityState.Modified;
                db.GameIns.Update(gameIn);
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
                db.GameIns.Remove(gameIn);
            else
                db.Entry(gameIn).State = EntityState.Detached;
        }
    }



    // разрешаем только нажатия цифр для TextBox'ов
    private void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (Key.D0 <= e.Key && e.Key <= Key.D9) return;
        e.Handled = true;
    }

}
