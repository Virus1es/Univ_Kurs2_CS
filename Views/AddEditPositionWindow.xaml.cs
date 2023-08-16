using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;

namespace Wpf_Kurvovaya_BD.Views;

/// <summary>
/// Логика взаимодействия для AddEditPositionWindow.xaml
/// </summary>
public partial class AddEditPositionWindow : Window
{
    PostgresContext db = new PostgresContext();

    bool flag = true;
    int Id = 0;

    // вызов окна как окна добавления новой записи
    public AddEditPositionWindow()
    {
        InitializeComponent();
    }

    // вызов окна как окна изменения записи
    public AddEditPositionWindow(Positions position)
    {
        InitializeComponent();

        // обнавление информации окна
        Title = "Редактирование названий городов";
        TblInfo.Text = "Редактирование названий городов(Таблица 'Города')";

        flag = false;

        // назначение полям нужных значений
        Id = position.Id;
        TbxPosition.Text = position.Position;

    }

    // нажатие на кнопку ОК
    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        Positions position = null!;

        try
        {
            position = new Positions
            {
                Position = TbxPosition.Text,
            };

            // вставить запись в коллекцию
            if (flag)
                db.Positions.Add(position);
            else
            {
                position.Id = Id;
                // изменить запись в коллекции
                db.Entry(position).State = EntityState.Modified;
                db.Positions.Update(position);
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
                db.Positions.Remove(position);
            else
                db.Entry(position).State = EntityState.Detached;
        }
    }
}
