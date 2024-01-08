using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using QuickNote.ViewModel;
using QuickNote.ViewModel.Helper;

namespace QuickNote.View
{
    /// <summary>
    /// Interaction logic for NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        NotesVM viewModel;
        public NotesWindow()
        {
            InitializeComponent();
            viewModel = Resources["vm"] as NotesVM;
            viewModel.SelectedNoteChanged += ViewModel_SelectedNoteChanged;
            var fontFaimilies = Fonts.SystemFontFamilies.OrderBy(x => x.Source);
            fontFaimilyComboBox.ItemsSource = fontFaimilies;

            List<double> fontSize = new List<double>() {8,9,10,11,12,14,16,28,48 };
            fontSizeComboBox.ItemsSource = fontSize;
        }
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (String.IsNullOrEmpty(App.UserId))
            {
                LoginWindow loginWindow= new LoginWindow();
                loginWindow.ShowDialog();
                viewModel.GetNoteBooks();
            }
        }
        private void ViewModel_SelectedNoteChanged(object? sender, EventArgs e)
        {
            richTextBoxContent.Document.Blocks.Clear();
            if (viewModel.SelectedNote != null)
            {
                if (!string.IsNullOrEmpty(viewModel.SelectedNote.FileLocation))
                {
                    using (FileStream fileStream = new FileStream(viewModel.SelectedNote.FileLocation, FileMode.Open, FileAccess.ReadWrite))
                    {
                        var contents = new TextRange(richTextBoxContent.Document.ContentStart, richTextBoxContent.Document.ContentEnd);
                        contents.Load(fileStream, DataFormats.Rtf);

                    }
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void richTextBoxContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            int amountOfText = (new TextRange(richTextBoxContent.Document.ContentStart,richTextBoxContent.Document.ContentEnd)).Text.Length;
            statusTextBlock.Text = $"Document Length {amountOfText} charecters";
        }

        
        private void boldButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonChecked = (sender as ToggleButton).IsChecked ?? false;
            if (isButtonChecked)
            {
                richTextBoxContent.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            }
            else
            {
                richTextBoxContent.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
            }

        }

        private void richTextBoxContent_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedWeight = richTextBoxContent.Selection.GetPropertyValue(Inline.FontWeightProperty);
            boldButton.IsChecked = (selectedWeight != DependencyProperty.UnsetValue) && selectedWeight.Equals(FontWeights.Bold);

            var selectedStyle = richTextBoxContent.Selection.GetPropertyValue(Inline.FontStyleProperty);
            italicButton.IsChecked = (selectedStyle != DependencyProperty.UnsetValue) && selectedStyle.Equals(FontStyles.Italic);

            var selectedTextDecoration = richTextBoxContent.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            italicButton.IsChecked = (selectedTextDecoration != DependencyProperty.UnsetValue) && selectedTextDecoration.Equals(TextDecorations.Underline);

            fontFaimilyComboBox.SelectedItem = richTextBoxContent.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            fontSizeComboBox.Text = richTextBoxContent.Selection.GetPropertyValue(Inline.FontSizeProperty).ToString();
        }

        private void italicButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonChecked = (sender as ToggleButton).IsChecked ?? false;
            if (isButtonChecked)
            {
                richTextBoxContent.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontStyles.Italic);
            }
            else
            {
                richTextBoxContent.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
            }
        }

        private void underlineButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonChecked = (sender as ToggleButton).IsChecked ?? false;
            if (isButtonChecked)
            {
                richTextBoxContent.Selection.ApplyPropertyValue(Inline.FontWeightProperty, TextDecorations.Underline);
            }
            else
            {
                TextDecorationCollection textDecorations;
                (richTextBoxContent.Selection.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection).TryRemove(TextDecorations.Underline, out textDecorations);
                richTextBoxContent.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, textDecorations);
            }
        }

        private void fontFaimilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fontFaimilyComboBox.SelectedItem != null)
            {
                richTextBoxContent.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, fontFaimilyComboBox.SelectedItem);
            }
        }
        private void fontSizeComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fontSizeComboBox.Text != null)
            {
                richTextBoxContent.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSizeComboBox.Text);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var rtfFile = System.IO.Path.Combine(Environment.CurrentDirectory, $"{viewModel.SelectedNote.Id}.rtf");
            viewModel.SelectedNote.FileLocation= rtfFile;
            DatabaseHelper.Update(viewModel.SelectedNote);

            using(FileStream fileStream = new FileStream(rtfFile, FileMode.Create))
            {
                var contents = new TextRange(richTextBoxContent.Document.ContentStart, richTextBoxContent.Document.ContentEnd);
                contents.Save(fileStream, DataFormats.Rtf);
            }
        }
    }
}
