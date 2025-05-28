

using PropertyChanged;
using SepidMelk.MVVM.Models;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;


namespace SepidMelk.MVVM.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class RegisterPageViewModel
    {
        public string fullName { get; set; }
        public string phone { get; set; }
        public int quantity { get; set; }
        public string numberString { get; set; }
        public string deviceInfo { get; set; } = "1";
        public bool isUserEnable { get; set; } = false;
        public string Count { get; set; }
        public string Search { get; set; }

        public string entryNum { get; set; }

        public ObservableCollection<UserData> UsersData { get; set; }


        public ObservableCollection<NumberData> numbersData { get; set; }

        HttpClient HttpClient;

        string baseUrl = "https://mauiapi.sepidmelk.ir/api";
  

        JsonSerializerOptions JsonSerializerOptions;

        public RegisterPageViewModel()
        {
            HttpClient = new HttpClient();
            HttpClient.Timeout = TimeSpan.FromMinutes(1);

            JsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            numbersData = new ObservableCollection<NumberData>();
        }


        public ICommand BtnCommand => new Command(async () =>
        {
            AddUser();
        });

        public ICommand BtnAddNumberCommand => new Command(async () =>
        {
            if (string.IsNullOrEmpty(entryNum))
            {
                _ = App.Current.MainPage.DisplayAlert("Alert", "لطفا شماره را وارد کنید", "بستن");
                return;
            }
            else
            {
                NumberData numberData = new NumberData
                {
                    num = Convert.ToInt32(GetEnglishNumber(entryNum.Trim()))

                }; 

                numbersData.Add(numberData);
                entryNum = string.Empty;
                numberString = String.Join("-", numbersData.Select(x => x.num));


            }
        });

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

        public async Task AddUser()
        {
            var url = $"{baseUrl}/users/register";

            UserData userData = new UserData
            {

                fullName = fullName.Trim(),
                phone = GetEnglishNumber(phone.Trim()),
                quantity = numbersData.Count,
                numberString = numberString.Trim(),
                deviceInfo = deviceInfo.Trim(),
                isUserEnable = isUserEnable 

            };



            var json = JsonSerializer.Serialize<UserData>(userData, JsonSerializerOptions);

            StringContent content = new StringContent(json, encoding: Encoding.UTF8, "Application/Json");



            try
            {
                var response = await HttpClient.PostAsync(url, content);
                var content2 = await response.Content.ReadAsStringAsync();
                //  var auth = response.Headers.GetValues("Authorization").FirstOrDefault();
                if (response.IsSuccessStatusCode)
                {

                    _ = App.Current.MainPage.DisplayAlert("Alert", "کاربر ثبت گردید", "بستن");
                }
                else {
                    _ = App.Current.MainPage.DisplayAlert("Alert", "کاربر قبلا ثبت کرده است", "بستن");
                }


            }
            catch (Exception ex)
            {
                _ = App.Current.MainPage.DisplayAlert("Alert", ex.Message, "ok");

            }
        }
    }
}
