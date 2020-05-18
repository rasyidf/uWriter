using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace uWriter.ViewModels
{
    class ZenViewModel : BindableBase
    {
        private DelegateCommand<object> _zenModeCommand;
        public DelegateCommand<object> ZenModeCommand =>
            _zenModeCommand ?? (_zenModeCommand = new DelegateCommand<object>(ExecuteZenModeCommand));

        void ExecuteZenModeCommand(object parameter)
        {
            
        }
    }
}
