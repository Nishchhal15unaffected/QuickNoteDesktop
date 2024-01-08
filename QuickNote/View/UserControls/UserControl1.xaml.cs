using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuickNote.Model;

namespace QuickNote.View.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public Notebook Notebook
        {
            get { return (Notebook)GetValue(NotebookProperty); }
            set { SetValue(NotebookProperty,value); }   
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotebookProperty =
            DependencyProperty.Register("Notebook", typeof(Notebook), typeof(UserControl1), new PropertyMetadata(null, SetValue));

        private static void SetValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d != null)
            {
                UserControl1 noteBookUserControl = d as UserControl1;
                if (noteBookUserControl != null)
                {
                    noteBookUserControl.DataContext = noteBookUserControl.Notebook;
                }
            }
        } 
        public UserControl1()
        {
            InitializeComponent(); 
        }
    }
}
