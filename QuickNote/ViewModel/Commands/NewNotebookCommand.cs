﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuickNote.ViewModel.Commands
{
    public class NewNotebookCommand : ICommand
    {
        public NotesVM VM;

        public event EventHandler? CanExecuteChanged;

        public NewNotebookCommand(NotesVM vm)
        {
            VM = vm;
        }
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            VM.CreateNoteBook();
            //ToDo
        }
    }
}
