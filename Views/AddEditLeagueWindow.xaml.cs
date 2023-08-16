using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;

namespace Wpf_Kurvovaya_BD.Views;

/// <summary>
/// Логика взаимодействия для AddEditLeagueWindow.xaml
/// </summary>
public partial class AddEditLeagueWindow : Window
{
    PostgresContext db = new PostgresContext();

    bool flag = true;
    int Id = 0;

    // вызов окна как окна добавления новой записи
    public AddEditLeagueWindow()
    {
        InitializeComponent();
    }

    // вызов окна как окна изменения записи
    public AddEditLeagueWindow(Leages league)
    {
        InitializeComponent();

        // обнавление информации окна
        Title = "Редактирование названия лиги";
        TblInfo.Text = "Редактирование названия лиги(Таблица 'Лиги')";

        flag = false;

        // назначение полям нужных значений
        Id = league.Id;
        TbxLeague.Text = league.Leage;

    }

    // нажатие на кнопку ОК
    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        Leages league = null!;

        try
        {
            league = new Leages
            {
                Leage = TbxLeague.Text,
            };

            // вставить запись в коллекцию
            if (flag)
                db.Leages.Add(league);
            else
            {
                league.Id = Id;
                // изменить запись в коллекции
                db.Entry(league).State = EntityState.Modified;
                db.Leages.Update(league);
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
                db.Leages.Remove(league);
            else
                db.Entry(league).State = EntityState.Detached;
        }
    }
}
