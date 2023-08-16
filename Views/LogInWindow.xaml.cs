using System;
using System.Windows;

namespace Wpf_Kurvovaya_BD.Views;

/// <summary>
/// Логика взаимодействия для LogInWindow.xaml
/// </summary>
public partial class LogInWindow : Window
{
    public LogInWindow()
    {
        InitializeComponent();
    }

    // команда проверки введённых логина и пароля
    private void Enter_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // если логин и пароль верны то закрываем окно 
            // иначе выдаём ошибку ввода
            DialogResult = (TbxLogin.Text == "admin" && TbxPassword.Password == "12345") ?
                            true :
                            throw new ArgumentException("Не верно введён логин и/или пароль");
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
