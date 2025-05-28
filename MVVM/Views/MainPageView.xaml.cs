using CommunityToolkit.Maui.Views;
using SepidMelk.MVVM.Models;
using SepidMelk.MVVM.ViewModel;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace SepidMelk.MVVM.Views;

public partial class MainPageView : ContentPage
{
    string pick;
 
    public MainPageView()
    {
        InitializeComponent();
       // BindingContext = new MainPageViewModel();
    }



    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MainPageViewModel vm)
        {
            vm.LoadData();
            if (vm.isUserLogined)
            {
                entryPhone.IsEnabled = false;
            }
        }
    

    }

    private void picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        pick = picker.SelectedItem as string;
    }

    private string GetEnglishNumber(string persianNumber)
    {
        string englishNumber = "";
        foreach (char ch in persianNumber)
        {
            englishNumber += char.GetNumericValue(ch);
        }

        if (englishNumber == "-1")
        {
            return persianNumber;
        }
        else
        {
            return englishNumber;
        }

    }


    private async void Button_Clicked(object sender, EventArgs e)
    {
        IsBusy = true;
        try
        {
            var vm = (MainPageViewModel)BindingContext;

            if (entryPhone.Text == null || entryPhone.Text == "")
            {
                await DisplayAlert("", "ابتدا وارد اکانت خود شوید سپس دوباره امتحان کنید", "بستن", FlowDirection.RightToLeft);
                return;
            }
            if (picker.SelectedIndex == -1)
            {
                await DisplayAlert("", "لطفا یک دسته بندی را فیلتر کنید سپس دوباره امتحان کنید", "بستن", FlowDirection.RightToLeft);
                return;
            }
            if (entrySearch.Text == null || entrySearch.Text == "")
            {
                await DisplayAlert("", "فیلد جستجو خالی است", "بستن", FlowDirection.RightToLeft);

                list.ItemsSource = vm.melksData;
                vm.Count = list.ItemsSource.OfType<Object>().Count().ToString();
                return;
            }

            string tx = GetEnglishNumber(entrySearch.Text);

            IEnumerable<MelkData> filtered = vm.melksData;

            // عملیات فیلتر را روی ترد جداگانه انجام دهید تا
            // UI
            // قفل نشود
            filtered = await Task.Run(() =>
            {
                switch (pick)
                {
                    case "عنوان_ملک":
                        return vm.melksData.Where(x => x.عنوان_ملک != null && x.عنوان_ملک.Contains(entrySearch.Text));
                    case "آدرس":
                        return vm.melksData.Where(x => x.آدرس != null && x.آدرس.Contains(entrySearch.Text));
                    case "اجاره":
                        return vm.melksData.Where(x => x.اجاره != null && x.اجاره.Contains(tx));
                    case "تاریخ_شمسی":
                        return vm.melksData.Where(x => x.تاریخ_شمسی != null && x.تاریخ_شمسی == entrySearch.Text);
                    case "تلفن":
                        return vm.melksData.Where(x => x.تلفن != null && x.تلفن.Contains(tx));
                    case "سال_ساخت":
                        return vm.melksData.Where(x => x.سال_ساخت != null && x.سال_ساخت.Contains(tx));
                    case "شرح":
                        return vm.melksData.Where(x => x.شرح != null && x.شرح.Contains(entrySearch.Text));
                    case "شرح_ملک":
                        return vm.melksData.Where(x => x.شرح_ملک != null && x.شرح_ملک.Contains(entrySearch.Text));
                    case "شماره":
                        return vm.melksData.Where(x => x.شماره.ToString().Contains(tx));
                    case "طبقه":
                        return vm.melksData.Where(x => x.طبقه != null && x.طبقه.Contains(tx));
                    case "قیمت":
                        return vm.melksData.Where(x => x.قیمت != null && x.قیمت.Contains(tx));
                    case "لینک":
                        return vm.melksData.Where(x => x.لینک != null && x.لینک.Contains(entrySearch.Text));
                    case "مالک":
                        return vm.melksData.Where(x => x.مالک != null && x.مالک.Contains(entrySearch.Text));
                    case "متراژ":
                        return vm.melksData.Where(x => x.متراژ != null && x.متراژ.Contains(tx));
                    case "محل":
                        return vm.melksData.Where(x => x.محل != null && x.محل.Contains(entrySearch.Text));
                    case "نوع":
                        return vm.melksData.Where(x => x.نوع != null && x.نوع.Contains(entrySearch.Text));
                    case "ودیعه":
                        return vm.melksData.Where(x => x.ودیعه != null && x.ودیعه.Contains(tx));
                    default:
                        return vm.melksData;
                }
            });

            list.ItemsSource = filtered.ToList();
            vm.Count = list.ItemsSource.OfType<Object>().Count().ToString();

            entryPhone.Unfocus();
    }
        finally
        {
            IsBusy = false;
        }
    }






    private async void Button_Clicked_New(object sender, EventArgs e)
    {
        IsBusy = true;
        try
        {
            entrySearch.Text = string.Empty;
            var vm = (MainPageViewModel)BindingContext;
            await vm.LoginUser();

            if (vm.isUserLogined)
            {
                entryPhone.IsEnabled = false;
                await vm.RefreshDataAsync2();
            }

        }
        finally
        {
            IsBusy = false;
        }
    }

  
    private async void Button_Clicked_SignUp(object sender, EventArgs e)
    {
        var vm = (MainPageViewModel)BindingContext;
        if (entryPhone.Text.Length < 11) return;

        IsBusy = true;
        try
        {
        string tx = GetEnglishNumber(entryPhone.Text);
        vm.phone = tx;
        await vm.LoginUser();

        if (vm.isUserLogined)
        {
            entryPhone.IsEnabled = false;
            await vm.RefreshDataAsync();
        }

        }
        finally
        {
            IsBusy = false;
        }
    }
}