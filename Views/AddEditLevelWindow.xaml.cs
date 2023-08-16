using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;

namespace Wpf_Kurvovaya_BD.Views;

/// <summary>
/// Логика взаимодействия для AddEditLevelWindow.xaml
/// </summary>
public partial class AddEditLevelWindow : Window
{
    PostgresContext db = new PostgresContext();

    bool flag = true;
    int Id = 0;

    // вызов окна как окна добавления новой записи
    public AddEditLevelWindow()
    {
        InitializeComponent();
    }

    // вызов окна как окна изменения записи
    public AddEditLevelWindow(GameLevels level)
    {
        InitializeComponent();

        // обнавление информации окна
        Title = "Редактирование названия уровня игры";
        TblInfo.Text = "Редактирование названия уровня игры(Таблица 'Уровни игры')";

        flag = false;

        // назначение полям нужных значений
        Id = level.Id;
        TbxLevel.Text = level.GameLevel;

    }

    // нажатие на кнопку ОК
    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        GameLevels level = null!;

        try
        {
            level = new GameLevels
            {
                GameLevel = TbxLevel.Text,
            };

            // вставить запись в коллекцию
            if (flag)
                db.GameLevels.Add(level);
            else
            {
                level.Id = Id;
                // изменить запись в коллекции
                db.Entry(level).State = EntityState.Modified;
                db.GameLevels.Update(level);
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
                db.GameLevels.Remove(level);
            else
                db.Entry(level).State = EntityState.Detached;
        }
    }
}
