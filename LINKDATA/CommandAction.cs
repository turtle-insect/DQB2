﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LINKDATA
{
	internal class CommandAction : ICommand
	{
		private readonly Action<object?> mAction;
		public CommandAction(Action<object?> action) => mAction = action;
		public event EventHandler? CanExecuteChanged;
		public bool CanExecute(object? parameter) => true;
		public void Execute(object? parameter) => mAction(parameter);
	}
}
