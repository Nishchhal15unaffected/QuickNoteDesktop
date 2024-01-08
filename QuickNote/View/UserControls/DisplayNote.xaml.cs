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
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuickNote.Model;

namespace QuickNote.View.UserControls
{
    /// <summary>
    /// Interaction logic for DisplayNote.xaml
    /// </summary>
    public partial class DisplayNote : UserControl
    {
        public Note Note
        {
            get { return (Note)GetValue(NoteProperty); }
            set { SetValue(NoteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoteProperty =
            DependencyProperty.Register("Note", typeof(Note), typeof(DisplayNote), new PropertyMetadata(null, SetValue));

        private static void SetValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null)
            {
                DisplayNote note = d as DisplayNote;
                if (note != null)
                {
                    note.DataContext = note.Note;
                }
            }
        }
        public DisplayNote()
        {
            InitializeComponent();
        }
    }
}
