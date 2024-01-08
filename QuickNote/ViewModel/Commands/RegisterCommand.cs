using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using QuickNote.Model;

namespace QuickNote.ViewModel.Commands
{
    public class RegisterCommand : ICommand
    {
        public LoginVM VM { get; set; }
        public event EventHandler? CanExecuteChanged;

        public RegisterCommand(LoginVM vm) 
        {
            VM = vm;
        }
        public bool CanExecute(object? parameter)
        {
            //User user = parameter as User;
            //if(user == null || String.IsNullOrEmpty(user.UserName) || String.IsNullOrEmpty(user.Password) || String.IsNullOrEmpty(user.ConfirmPassword)
            //    || user.Password != user.ConfirmPassword)
            //{
            //    return false;
            //}
            return true;
        }

        public void Execute(object? parameter)
        {
            VM.Register();
        }
    }
}
