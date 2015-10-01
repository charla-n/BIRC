using BIRC.Shared.Files;
using BIRC.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BIRC.ViewModels
{
    public class BIRCViewModel : ViewModelBase
    {
        private RelayCommand AddConnectionCmd;

        public BIRCViewModel()
        {
            AddConnectionCmd = new RelayCommand(AddConnectionAction, () => true);
        }

        public ICommand AddConnection {
            get
            {
                return AddConnectionCmd;
            }
        }

        private void AddConnectionAction()
        {
            var frame = (Frame)Window.Current.Content;

            frame.Navigate(typeof(AddServerPage));
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
