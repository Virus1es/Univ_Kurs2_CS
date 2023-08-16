using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wpf_Kurvovaya_BD.Controllers;

namespace Wpf_Kurvovaya_BD.Views;

/// <summary>
/// Логика взаимодействия для AddEditWindow.xaml
/// </summary>
public partial class AddEditWindow : Window
{
    PostgresContext db = new PostgresContext();
    List<OurClubs> ourClubs = QuarytiesController.ShowTableOurClubs();
    List<Positions> positions = QuarytiesController.ShowTablePositions();
    bool flag = true;
    int Id = 0;
    Bitmap bitmap = null!;

    // вызов окна как окна добавления новой записи
    public AddEditWindow()
    {
        InitializeComponent();
    }

    // вызов окна как окна изменения записи
    public AddEditWindow(Gamers gamer)
    {
        InitializeComponent();

        // обнавление информации окна
        Title = "Редактирование иформации о игроке";
        TblInfo.Text = "Редактирование иформации о игроке(Таблица 'Наши игроки')";

        flag = false;

        // назначение полям нужных значений
        Id = gamer.Id;
        TbxFirstParam.Text = gamer.Surname;
        TbxSecondParam.Text = gamer.Name;
        TbxThirdParam.Text = gamer.Patronymic;
        CbxFourthParam.SelectedItem = positions.First(p => gamer.IdPosition == p.Id).Position;
        bitmap = new Bitmap(new MemoryStream(gamer.Photo));
        DateFivethParam.Text = gamer.Birthday.ToString();
        CbxSixthParam.SelectedItem = ourClubs.First(c => gamer.IdClub == c.Id).Club;
        TbxSeventhParam.Text = $"{gamer.YearFact}";
        TbxNinethParam.Text = gamer.Comments;
        TbxTenthParam.Text = $"{gamer.Cost}";
    }

    // действия при загрузке окна
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // заполнение комбобоксов информацией
        CbxFourthParam.ItemsSource = positions.Select(p => p.Position);
        CbxSixthParam.ItemsSource = ourClubs.Select(c => c.Club);
    }

    private void LoadImage_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog
        {
            Multiselect = false,
            Title = "Загрузка фотографии футболиста",
            Filter = "Image Files (*.BMP; *.JPG; *.JPEG; *.PNG) | *.BMP; *.JPG; *.JPEG; *.PNG",
            FilterIndex = 0,
        };
        if (ofd.ShowDialog() == true)
            bitmap = new Bitmap(ofd.FileName);
    }


    // нажатие на кнопку ОК
    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        Gamers gamer = null!;

        try
        {
            gamer = new Gamers
            {
                Surname = TbxFirstParam.Text,
                Name = TbxSecondParam.Text,
                Patronymic = TbxThirdParam.Text,
                IdPosition = positions.First(p => CbxFourthParam.Text == p.Position).Id,
                Birthday = DateOnly.FromDateTime((DateTime)DateFivethParam?.SelectedDate),
                Photo = (byte[])new ImageConverter().ConvertTo(bitmap, typeof(byte[])),
                IdClub = ourClubs.First(c => CbxSixthParam.Text == c.Club).Id,
                YearFact = int.Parse(TbxSeventhParam.Text),
                Comments = TbxNinethParam.Text,
                Cost = long.Parse(TbxTenthParam.Text)
            };

            // вставить запись в коллекцию
            if (flag)
                db.Gamers.Add(gamer);
            else
            {
                gamer.Id = Id;
                // изменить запись в коллекции
                db.Entry(gamer).State = EntityState.Modified;
                db.Gamers.Update(gamer);
            }
            // сохранить в базу данных
            db.SaveChanges();

            DialogResult = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            if (flag)
                db.Gamers.Remove(gamer);
            else
                db.Entry(gamer).State = EntityState.Detached;
        }
    }


    // разрешаем только нажатия цифр для TextBox'ов
    private void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (Key.D0 <= e.Key && e.Key <= Key.D9) return;
        e.Handled = true;
    }
}
