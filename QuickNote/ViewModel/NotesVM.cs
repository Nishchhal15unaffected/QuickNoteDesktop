using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using QuickNote.Model;
using QuickNote.ViewModel.Commands;
using QuickNote.ViewModel.Helper;

namespace QuickNote.ViewModel
{
    public class NotesVM :  INotifyPropertyChanged
    {
        private Notebook selectedNotebook;
        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set
            {
                selectedNotebook = value;
                OnPropertyChanged("SelectedNotebook");
                GetNotes();
            }
        }
        private Note selectedNote;
        public Note SelectedNote
        {
            get { return selectedNote; }
            set
            {
                selectedNote = value;
                OnPropertyChanged("SelectedNote");
                SelectedNoteChanged?.Invoke(this, new EventArgs());
            }
        }
        private Visibility isVisible;

        public Visibility IsVisible
        {
            get { return isVisible; }
            set 
            {
                isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }

        public ObservableCollection<Note> Notes { get; set; }
        public ObservableCollection<Notebook> Notebooks { get; set; }
        public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }
        public EditCommand EditCommand { get; set; }
        public EndEditing EndEditing { get; set; }
        public NotesVM()
        {
            NewNotebookCommand= new NewNotebookCommand(this);
            NewNoteCommand= new NewNoteCommand(this);
            EditCommand = new EditCommand(this);
            EndEditing = new EndEditing(this);
            Notebooks = new ObservableCollection<Notebook>(); 
            IsVisible = Visibility.Collapsed;
            Notes= new ObservableCollection<Note>();
            GetNoteBooks();

        }
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler SelectedNoteChanged;
        public async void CreateNoteBook()
        {
            Notebook newNotebook = new Notebook()
            {
                Name = "New NoteBook",
                UserId = App.UserId
            };
            await DatabaseHelper.Insert(newNotebook);
            GetNoteBooks();
        }

        public async void CreateNote(string notebookId)
        {
            Note newNote = new Note()
            {
                Notebook = notebookId,
                CreatedAt= DateTime.Now,
                UpdatedAt= DateTime.Now,
                Title= "New Note"
            };
            await DatabaseHelper.Insert(newNote);
            GetNotes();
        }
        public async void GetNoteBooks()
        {
            var noteBooks = (await DatabaseHelper.Read<Notebook>()).Where(notebook => notebook.UserId == App.UserId ).ToList();
            Notebooks.Clear();
            foreach (var noteBook in noteBooks)
            {
                Notebooks.Add(noteBook);
            }
        }
        public async void GetNotes()
        {
            if(SelectedNotebook != null)
            {
                var notes = (await DatabaseHelper.Read<Note>()).Where(n => n.Notebook == SelectedNotebook.Id).ToList();
                Notes.Clear();
                foreach (var note in notes)
                {
                    Notes.Add(note);
                }
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public void EditNotes()
        {
            IsVisible = Visibility.Visible;
        }

        public void StopEditNotes(Notebook notebook)
        {
            IsVisible = Visibility.Collapsed;
            DatabaseHelper.Update(notebook);
            GetNoteBooks();
        }

    }
}
