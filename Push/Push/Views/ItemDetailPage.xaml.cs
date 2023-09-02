using System.ComponentModel;
using Xamarin.Forms;
using Push.ViewModels;

namespace Push.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
