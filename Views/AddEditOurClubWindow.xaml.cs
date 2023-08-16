using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wpf_Kurvovaya_BD.Controllers;

namespace Wpf_Kurvovaya_BD.Views;

/// <summary>
/// Логика взаимодействия для AddEditOurClubWindow.xaml
/// </summary>
public partial class AddEditOurClubWindow : Window
{
    PostgresContext db = new PostgresContext();
    List<TrainingBasis> bases = QuarytiesController.ShowTableTrainingBasis();
    List<Leages> leages = QuarytiesController.ShowTableLeages();
    List<Managers> managers = QuarytiesController.ShowTableManagers();
    List<Cities> cities = QuarytiesController.ShowTableCities();
    bool flag = true;
    int Id = 0;

    // вызов окна как окна добавления новой записи
    public AddEditOurClubWindow()
    {
        InitializeComponent();
    }

    // вызов окна как окна изменения записи
    public AddEditOurClubWindow(OurClubs ourClub)
    {
        InitializeComponent();

        // обнавление информации окна
        Title = "Редактирование иформации о нашем клубе";
        TblInfo.Text = "Редактирование иформации о нашем клубе(Таблица 'Наши клубы')";

        flag = false;

        // назначение полям нужных значений
        Id = ourClub.Id;
        TbxClubName.Text = ourClub.Club;
        CbxTraningBase.SelectedItem = bases.First(b => ourClub.IdBase == b.Id).Base;
        TbxYearCreate.Text = ourClub.Year.ToString();
        CbxClubLeage.SelectedItem = leages.First(l => ourClub.IdLeage == l.Id).Leage;

        Managers temp = managers.First(m => ourClub.IdManager == m.Id);

        CbxManagerClub.Text = $"{temp.Surname} {temp.Name} {temp.Patronymic}";
        CbxCity.Text = cities.First(c => ourClub.IdCity == c.Id).City;

    }

    // действия при загрузке окна
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // заполнение комбобоксов информацией
        CbxTraningBase.ItemsSource = bases.Select(b => b.Base);
        CbxClubLeage.ItemsSource = leages.Select(l => l.Leage);
        CbxManagerClub.ItemsSource = managers.Select(m => $"{m.Surname} {m.Name} {m.Patronymic}");
        CbxCity.ItemsSource = cities.Select(c => c.City);
    }

    // нажатие на кнопку ОК
    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        OurClubs ourClub = null!;

        try
        {
            ourClub = new OurClubs
            { 
                Club = TbxClubName.Text,
                IdBase = bases.First(b => CbxTraningBase.Text == b.Base).Id,
                Year = int.Parse(TbxYearCreate.Text),
                IdLeage = leages.First(b => CbxClubLeage.Text == b.Leage).Id,
                IdManager = managers.First(g => CbxManagerClub.Text == $"{g.Surname} {g.Name} {g.Patronymic}").Id,
                IdCity = cities.First(c => CbxCity.Text == c.City).Id
            };

            // вставить запись в коллекцию
            if (flag)
                db.OurClubs.Add(ourClub);
            else
            {
                ourClub.Id = Id;
                // изменить запись в коллекции
                db.Entry(ourClub).State = EntityState.Modified;
                db.OurClubs.Update(ourClub);
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
                db.OurClubs.Remove(ourClub);
            else
                db.Entry(ourClub).State = EntityState.Detached;
        }
    }


    // разрешаем только нажатия цифр для TextBox'ов
    private void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (Key.D0 <= e.Key && e.Key <= Key.D9) return;
        e.Handled = true;
    }
}
