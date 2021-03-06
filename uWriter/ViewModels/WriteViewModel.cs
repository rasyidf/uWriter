﻿using System;
using Prism.Commands;
using Prism.Mvvm;
using uWriter.Views;

namespace uWriter.ViewModels
{
    public class WriteViewModel : BindableBase
    {
        public WriteViewModel()
        {
        }
        
        
        private DelegateCommand<object> _zenModeCommand;
        public DelegateCommand<object> ZenModeCommand =>
            _zenModeCommand ?? (_zenModeCommand = new DelegateCommand<object>(ExecuteZenModeCommand));

        void ExecuteZenModeCommand(object parameter)
        {
            var zen = new ZenWindow();
            zen.Show();

        }
    }
}

