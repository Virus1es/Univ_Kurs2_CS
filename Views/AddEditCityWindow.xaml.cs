using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;

namespace Wpf_Kurvovaya_BD.Views;

/// <summary>
/// Логика взаимодействия для AddEditCityWindow.xaml
/// </summary>
public partial class AddEditCityWindow : Window
{
    PostgresContext db = new PostgresContext();

    bool flag = true;
    int Id = 0;

    // вызов окна как окна добавления новой записи

    public AddEditCityWindow()
    {
        InitializeComponent();
    }

    // вызов окна как окна изменения записи
    public AddEditCityWindow(Cities city)
    {
        InitializeComponent();

        // обнавление информации окна
        Title = "Редактирование названий городов";
        TblInfo.Text = "Редактирование названий городов(Таблица 'Города')";

        flag = false;

        // назначение полям нужных значений
        Id = city.Id;
        TbxCity.Text = city.City;

    }

    // нажатие на кнопку ОК
    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        Cities city = null!;

        try
        {
            city = new Cities
            {
                City = TbxCity.Text,
            };

            // вставить запись в коллекцию
            if (flag)
                db.Cities.Add(city);
            else
            {
                city.Id = Id;
                // изменить запись в коллекции
                db.Entry(city).State = EntityState.Modified;
                db.Cities.Update(city);
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
                db.Cities.Remove(city);
            else
                db.Entry(city).State = EntityState.Detached;
        }
    }
}
