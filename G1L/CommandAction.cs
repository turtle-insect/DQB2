using System;
using System.Windows.Input;

namespace G1L
{
	internal class CommandAction : ICommand
	{
#pragma warning disable CS0067
		public event EventHandler? CanExecuteChanged;
#pragma warning restore CS0067
		private Action mExecute;

		public CommandAction(Action action) => mExecute = action;
		public bool CanExecute(object? parameter) => true;
		public void Execute(object? parameter) => mExecute();
	}
}
