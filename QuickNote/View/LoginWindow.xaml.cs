using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using QuickNote.ViewModel;

namespace QuickNote.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        LoginVM viewModel;
        public LoginWindow()
        {
            InitializeComponent();
            viewModel = Resources["vm"] as LoginVM;
            viewModel.Authenticate += ViewModel_Authenticate;
        }

        private void ViewModel_Authenticate(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
