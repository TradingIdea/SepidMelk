using CommunityToolkit.Maui.Views;

using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;
using PropertyChanged;
using SepidMelk.MVVM.Models;
using SepidMelk.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;




namespace SepidMelk.MVVM.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class MainPageViewModel
    {
        public string عنوان_ملک { get; set; }
        public string شرح_ملک { get; set; }
        public string متراژ { get; set; }
        public string آدرس { get; set; }
        public string ودیعه { get; set; }
        public string اجاره { get; set; }
        public string قیمت { get; set; }
        public string لینک { get; set; }
        public string محل { get; set; }
        public string لوکیشن { get; set; }
        public string تاریخ_شمسی { get; set; }
        public string تلفن { get; set; }
        public string سال_ساخت { get; set; }
        public string شرح { get; set; }
        public int شماره { get; set; }
        public string طبقه { get; set; }
        public string مالک { get; set; }
        public string نوع { get; set; }
        public string منطقه { get; set; }

        public string fullName { get; set; }
        public string pass { get; set; }
        public string phone { get; set; }
        public string quantity { get; set; }
        public string numberString { get; set; }
        public string Count { get; set; }
        public string Search { get; set; }

        public bool isUserInput { get; set; }

        public bool isUserLogined { get; set; } 

        public string packageName { get; set; } = "ir.divar";

      
        public ICommand OpenDivarCommand { get; }
        public ICommand SharhCommand { get; }
        public ICommand OnvanCommand { get; }
        public ICommand AddressCommand { get; }

        public ObservableCollection<UserData> usersData { get; set; }
        public ObservableCollection<MelkData> melksData { get; set; }

        HttpClient HttpClient;

        private MelkData selectedMelk;

        public MelkData SelectedMelk
        {
            get => selectedMelk;
            set => selectedMelk = value;
        }





        //string baseUrl = DeviceInfo.Platform == DevicePlatform.Android
        //    ? "http://10.0.2.2:5500/api"
        //    : "http://localhost:5500/api";

        string baseUrl = "https://mauiapi.sepidmelk.ir/api";



        JsonSerializerOptions JsonSerializerOptions;

        public MainPageViewModel()
        {


            HttpClient = new HttpClient();
            HttpClient.Timeout = TimeSpan.FromMinutes(1);

            JsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            melksData = new ObservableCollection<MelkData>();

            OpenDivarCommand = new Command<string>(async (package) =>
            {
                await openDivar(package);
            });

            SharhCommand = new Command<string>(async (package) =>
            {

                var popup = new MainPopup(package);
                Application.Current.MainPage.ShowPopup(popup);
            });

            OnvanCommand = new Command<string>(async (package) =>
            {
                var popup = new MainPopup(package);
                Application.Current.MainPage.ShowPopup(popup);
            });

            AddressCommand = new Command<string>(async (package) =>
            {
                var popup = new MainPopup(package);
                Application.Current.MainPage.ShowPopup(popup);
            });
          

        }


        public bool IsBusy { get; set; } 

      

        private async Task openDivar(string link)
        {

            string divarUrl = $"{link}?id={"ir.divar"}";

            try
            {
                if (await Launcher.Default.CanOpenAsync(divarUrl))
                {
                    await Launcher.Default.OpenAsync(divarUrl);
                }
                else
                {
                    // اگر دیوار نصب نبود، باز کردن در مرورگر
                    await Launcher.Default.OpenAsync(divarUrl);
                }

            }
            catch
            {
                _ = App.Current.MainPage.DisplayAlert("", "آگهی مورد نظر یافت نشد", "ok", FlowDirection.RightToLeft);
            }

        }


  


        public void SaveData()
        {
            Preferences.Set("EntryTextVML", isUserLogined);
            Preferences.Set("EntryTextVMP", phone);
            Preferences.Set("EntryTextVMC", Count);
            var json = JsonSerializer.Serialize(melksData, JsonSerializerOptions);
            Preferences.Set("MelkSave", json);
        }

        // بازیابی دادهها
        public void LoadData()
        {

            isUserLogined = Preferences.Get("EntryTextVML",false);
            phone = Preferences.Get("EntryTextVMP", string.Empty);
            Count = Preferences.Get("EntryTextVMC", string.Empty);
            if (Preferences.ContainsKey("MelkSave"))
            {
                var json = Preferences.Get("MelkSave", string.Empty);
                var data = JsonSerializer.Deserialize<ObservableCollection<MelkData>>(json, JsonSerializerOptions);
                if (melksData != null)
                {
                    melksData.Clear();
                    foreach (var item in data)
                    {
                        melksData.Add(item);
                    }
                }
                else
                {
                    melksData = data;
                }

            }


        }


        private async void UserPage()
        {
          

            await RefreshDataAsync();
         

            //عنوانملک = string.Empty;
            //شرحملک = string.Empty;
            //متراژ = 0;
            //آدرس = string.Empty;
            //ودیعه = 0;
            //اجاره = 0;
            //قیمت = string.Empty;
            //لینک = string.Empty;
            //محل = string.Empty;
            //لوکیشن = 0;
            //تاریخ_شمسی = 0;
            //تلفن = string.Empty;
            //سالساخت = 0;
            //شرح = string.Empty;
            //شماره = 0;
            //طبقه = 0;
            //مالک = string.Empty;
            //نوع = string.Empty;
            //phone = string.Empty;
            //quantity = 0;
            //fullName = string.Empty;
            //pass = string.Empty;



        }



        public async Task LoginUser()
        {
            
         

#if ANDROID

            var context = Android.App.Application.Context;
            var androidId = Android.Provider.Settings.Secure.GetString(
                context.ContentResolver,
                Android.Provider.Settings.Secure.AndroidId);

            var url = $"{baseUrl}/users/login";


            UserDataLogin userDataLogin = new UserDataLogin
            {


                phone = phone.Trim(),
                deviceInfo = androidId.Trim()

            };


            var json = JsonSerializer.Serialize<UserDataLogin>(userDataLogin, JsonSerializerOptions);

            StringContent content = new StringContent(json, encoding: Encoding.UTF8, "Application/Json");


     

            try
            {
                var response = await HttpClient.PostAsync(url, content);
                var content2 = await response.Content.ReadAsStringAsync();
                //string pattern = @"\d+";
                //Match match = Regex.Match(content2, pattern );
                //quantity = int.Parse(match.Value);
                string start = "\":\"";

                string middle = content2.Substring(content2.IndexOf("numberString") + "numberString".Length);
                string result = middle.Substring(middle.IndexOf(start) + start.Length);
                numberString = result.Substring(0, result.Length - 2);

                //var authToken = response.Headers.GetValues("Bearer").FirstOrDefault();
                //await SecureStorage.SetAsync("auth", authToken);
                //  "\":\"4-2\"}"


                if (response.IsSuccessStatusCode)
                {
                isUserLogined = true;
                
                    //UserPage();
                    

                }
                else if (((int)response.StatusCode) == 400)
                {
                    _ = App.Current.MainPage.DisplayAlert("", "شماره موبایل اشتباه است ", "بستن", FlowDirection.RightToLeft);
                }


            }
            catch (System.Exception ex)
            {
                _ = App.Current.MainPage.DisplayAlert("", "اشکال در نصب برنامه", "ok", FlowDirection.RightToLeft);

            }

#endif
       
        }



        public async Task<ObservableCollection<MelkData>> RefreshDataAsync()
        {
          
            
            var url = $"{baseUrl}/Tenders";
            url = url + "/" + numberString;
         
            Uri uri = new Uri(string.Format(url, string.Empty));
            try
            {


                var req = new HttpRequestMessage(HttpMethod.Get, uri);
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("auth"));
                var response = await HttpClient.SendAsync(req);


                if (response.IsSuccessStatusCode)
                {
                    using (var content = await response.Content.ReadAsStreamAsync())
                    {

                        melksData = await JsonSerializer.DeserializeAsync<ObservableCollection<MelkData>>(content);
                        Count = melksData.Count.ToString();
                        _ = App.Current.MainPage.DisplayAlert("", "برنامه با موفقیت نصب شد", "بستن", FlowDirection.RightToLeft);
                    }
                }
            }
            catch (System.Exception ex)
            {
                _ = App.Current.MainPage.DisplayAlert("", "اشکال در نصب برنامه", "ok", FlowDirection.RightToLeft);
            }
          
     
            return melksData;
         
           

        }


        public async Task<ObservableCollection<MelkData>> RefreshDataAsync2()
        {


            var url = $"{baseUrl}/Tenders";
            url = url + "/" + numberString;

            Uri uri = new Uri(string.Format(url, string.Empty));
            try
            {


                var req = new HttpRequestMessage(HttpMethod.Get, uri);
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("auth"));
                var response = await HttpClient.SendAsync(req);


                if (response.IsSuccessStatusCode)
                {
                    using (var content = await response.Content.ReadAsStreamAsync())
                    {

                        melksData = await JsonSerializer.DeserializeAsync<ObservableCollection<MelkData>>(content);
                        Count = melksData.Count.ToString();
                      //  _ = App.Current.MainPage.DisplayAlert("", "برنامه با موفقیت نصب شد", "بستن", FlowDirection.RightToLeft);
                    }
                }
            }
            catch (System.Exception ex)
            {
                _ = App.Current.MainPage.DisplayAlert("", "اشکال در دریافت داده ها", "ok", FlowDirection.RightToLeft);
            }


            return melksData;



        }

    }


}
