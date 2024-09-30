using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace STGDAT
{
    class CommandAction : ICommand
	{
#pragma warning disable CS0067
		public event EventHandler? CanExecuteChanged;
#pragma warning restore CS0067

		private readonly Action<object?> mAction;

		public CommandAction(Action<object?> action) => mAction = action;

		public bool CanExecute(object? parameter) => true;

		public void Execute(object? parameter) => mAction(parameter);
	}
}
