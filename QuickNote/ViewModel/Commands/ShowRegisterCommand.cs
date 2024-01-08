using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuickNote.ViewModel.Commands
{
    public class ShowRegisterCommand : ICommand
    {
        public LoginVM loginVM;
        public event EventHandler? CanExecuteChanged;

        public ShowRegisterCommand(LoginVM vm)
        {
            loginVM = vm;
        }
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            loginVM.SwitchView();
        }
    }
}
