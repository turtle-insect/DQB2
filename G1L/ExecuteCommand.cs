using System;
using System.Windows.Input;

namespace G1L
{
	internal class ExecuteCommand : ICommand
	{
		public event EventHandler? CanExecuteChanged;
		private Action mExecute;

		public ExecuteCommand(Action action) => mExecute = action;
		public bool CanExecute(object? parameter) => true;
		public void Execute(object? parameter) => mExecute();
	}
}
