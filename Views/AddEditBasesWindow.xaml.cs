using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;

namespace Wpf_Kurvovaya_BD.Views;

/// <summary>
/// Логика взаимодействия для AddEditBasesWindow.xaml
/// </summary>
public partial class AddEditBasesWindow : Window
{
    PostgresContext db = new PostgresContext();

    bool flag = true;
    int Id = 0;

    // вызов окна как окна добавления новой записи
    public AddEditBasesWindow()
    {
        InitializeComponent();
    }

    // вызов окна как окна изменения записи
    public AddEditBasesWindow(TrainingBasis trainingBase)
    {
        InitializeComponent();

        // обнавление информации окна
        Title = "Редактирование названия тренировочной базы";
        TblInfo.Text = "Редактирование тренировочной базы(Таблица 'Тренировочные базы')";

        flag = false;

        // назначение полям нужных значений
        Id = trainingBase.Id;
        TbxBase.Text = trainingBase.Base;

    }

    // нажатие на кнопку ОК
    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        TrainingBasis trainingBase = null!;

        try
        {
            trainingBase = new TrainingBasis
            {
                Base = TbxBase.Text,
            };

            // вставить запись в коллекцию
            if (flag)
                db.TrainingBases.Add(trainingBase);
            else
            {
                trainingBase.Id = Id;
                // изменить запись в коллекции
                db.Entry(trainingBase).State = EntityState.Modified;
                db.TrainingBases.Update(trainingBase);
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
                db.TrainingBases.Remove(trainingBase);
            else
                db.Entry(trainingBase).State = EntityState.Detached;
        }
    }

}
