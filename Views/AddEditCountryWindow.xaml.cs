using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;

namespace Wpf_Kurvovaya_BD.Views;

/// <summary>
/// Логика взаимодействия для AddEditCountryWindow.xaml
/// </summary>
public partial class AddEditCountryWindow : Window
{
    PostgresContext db = new PostgresContext();

    bool flag = true;
    int Id = 0;

    // вызов окна как окна добавления новой записи
    public AddEditCountryWindow()
    {
        InitializeComponent();
    }


    // вызов окна как окна изменения записи
    public AddEditCountryWindow(Countries country)
    {
        InitializeComponent();

        // обнавление информации окна
        Title = "Редактирование названий стран";
        TblInfo.Text = "Редактирование названий стран(Таблица 'Страны')";

        flag = false;

        // назначение полям нужных значений
        Id = country.Id;
        TbxCountry.Text = country.Country;

    }

    // нажатие на кнопку ОК
    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        Countries country = null!;

        try
        {
            country = new Countries
            {
                Country = TbxCountry.Text,
            };

            // вставить запись в коллекцию
            if (flag)
                db.Countries.Add(country);
            else
            {
                country.Id = Id;
                // изменить запись в коллекции
                db.Entry(country).State = EntityState.Modified;
                db.Countries.Update(country);
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
                db.Countries.Remove(country);
            else
                db.Entry(country).State = EntityState.Detached;
        }
    }
}
