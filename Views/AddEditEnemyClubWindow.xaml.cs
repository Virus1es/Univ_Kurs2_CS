using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Wpf_Kurvovaya_BD.Views;

/// <summary>
/// Логика взаимодействия для AddEditEnemyClubWindow.xaml
/// </summary>
public partial class AddEditEnemyClubWindow : Window
{
    PostgresContext db = new PostgresContext();
    List<Countries> countries = new List<Countries>();
    bool flag = true;
    int Id = 0;

    // вызов окна как окна добавления новой записи
    public AddEditEnemyClubWindow()
    {
        InitializeComponent();
    }

    // вызов окна как окна изменения записи
    public AddEditEnemyClubWindow(EnemyClubs enemyClub)
    {
        InitializeComponent();

        // обнавление информации окна
        Title = "Редактирование иформации о клубе противника";
        TblInfo.Text = "Редактирование иформации о клубе противника(Таблица 'Клубы противника')";

        flag = false;

        // назначение полям нужных значений
        Id = enemyClub.Id;
        TbxClubName.Text = enemyClub.Opposing;
        CbxCountry.SelectedItem = countries.First(coun => enemyClub.IdCountry == coun.Id).Country;
        TbxSurnameCoach.Text = enemyClub.SurnameCoach;
        TbxNameCoach.Text = enemyClub.NameCoach;
        TbxPatronymicCoach.Text = enemyClub.PatronymicCoach;

    }

    // действия при загрузке окна
    private void Window_Loaded(object sender, RoutedEventArgs e) =>
        // заполнение комбобоксов информацией
        CbxCountry.ItemsSource = countries.Select(b => b.Country);

    // нажатие на кнопку ОК
    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        EnemyClubs enemyClub = null!;

        try
        {
            enemyClub = new EnemyClubs
            {
                Opposing = TbxClubName.Text,
                IdCountry = countries.First(coun => CbxCountry.Text == coun.Country).Id,
                SurnameCoach = TbxSurnameCoach.Text,
                NameCoach = TbxNameCoach.Text,
                PatronymicCoach = TbxPatronymicCoach.Text
            };

            // вставить запись в коллекцию
            if (flag)
                db.EnemyClubs.Add(enemyClub);
            else
            {
                enemyClub.Id = Id;
                // изменить запись в коллекции
                db.Entry(enemyClub).State = EntityState.Modified;
                db.EnemyClubs.Update(enemyClub);
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
                db.EnemyClubs.Remove(enemyClub);
            else
                db.Entry(enemyClub).State = EntityState.Detached;
        }
    }
}
