using SepidMelk.MVVM.ViewModel;
using SepidMelk.MVVM.Views;
using System.Text.Json;

namespace SepidMelk
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
         
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPageView());
        }

        protected override void OnSleep()
        {
           
            if (Current?.MainPage?.BindingContext is MainPageViewModel vm)
            {
                vm.SaveData();
            }
            base.OnSleep();
        }
    }
}