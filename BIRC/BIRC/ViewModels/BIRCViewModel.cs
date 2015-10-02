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
            RetrieveList();
            AddConnectionCmd = new RelayCommand(AddConnectionAction, () => true);
        }

        public async void RetrieveList()
        {
            await ConnectionFile.Instance().ReadImpl();
            OnPropertyChanged("ByServers");
        }

        public IEnumerable<object> ByServers
        {
            get
            {
                return ConnectionFile.Instance().Connections.GroupBy(x => x.Group)
                    .Select(x => new
                    {
                        Group = x.Key,
                        Items = x.ToList()
                    });
            }
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
