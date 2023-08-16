using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;
using System.Windows.Input;

namespace Wpf_Kurvovaya_BD.Views;

/// <summary>
/// Логика взаимодействия для AddEditManagerWindow.xaml
/// </summary>
public partial class AddEditManagerWindow : Window
{
    PostgresContext db = new PostgresContext();

    bool flag = true;
    int Id = 0;

    // вызов окна как окна добавления новой записи
    public AddEditManagerWindow()
    {
        InitializeComponent();
    }

    // вызов окна как окна изменения записи
    public AddEditManagerWindow(Managers manager)
    {
        InitializeComponent();

        // обнавление информации окна
        Title = "Редактирование информации о руководителе клубов";
        TblInfo.Text = "Редактирование информации о руководителе клубов(Таблица 'Руководители клубов')";

        flag = false;

        // назначение полям нужных значений
        Id = manager.Id;
        TbxSurname.Text = manager.Surname;
        TbxName.Text = manager.Name;
        TbxPatronymic.Text = manager.Patronymic;
        TbxPhone.Text = manager.Phone;


    }

    // нажатие на кнопку ОК
    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        Managers manager = null!;

        try
        {
            manager = new Managers
            {
                Surname = TbxSurname.Text,
                Name = TbxName.Text,
                Patronymic = TbxPatronymic.Text,
                Phone = TbxPhone.Text
            };

            // вставить запись в коллекцию
            if (flag)
                db.Managers.Add(manager);
            else
            {
                manager.Id = Id;
                // изменить запись в коллекции
                db.Entry(manager).State = EntityState.Modified;
                db.Managers.Update(manager);
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
                db.Managers.Remove(manager);
            else
                db.Entry(manager).State = EntityState.Detached;
        }
    }

    // разрешаем только нажатия цифр для TextBox'ов
    private void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (Key.D0 <= e.Key && e.Key <= Key.D9) return;
        e.Handled = true;
    }
}
