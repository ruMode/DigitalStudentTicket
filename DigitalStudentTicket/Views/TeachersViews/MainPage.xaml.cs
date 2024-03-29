﻿using DigitalStudentTicket.Entities;
using DigitalStudentTicket.Models;
using DigitalStudentTicket.Views;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DigitalStudentTicket
{
    public partial class MainPage : ContentPage //страница с отображением расписания для преподавательвателей
    {
        public static ScannerPageView scannerPage = new ScannerPageView(); //создаем сканер
        public string CurrentDate { get; set; } = $"Сегодня: {DateTime.Now.ToShortDateString()}г. ({DateTime.Now.DayOfWeek})"; //отображаем текущую дату
        public static string TeacherCode { get; set; } //поле для записи кода преподаватель

        public static ObservableCollection<SheduleItems> _shedule = new ObservableCollection<SheduleItems>(); //список пара
        public static LessonData _lessonData = new LessonData(); //храним данные о паре
        public static LessonData studentsData = new LessonData(); 
        private bool _isSheduleExist { get; set; } //проверка уже загруженного расписания, чтобы каждый раз при OnAppearing() не посылать запрос на сервер
        public MainPage()
        {
            InitializeComponent();
            _isSheduleExist = false;
        }

     
        protected override async void OnAppearing()
        {
            currentDateLabel.Text = CurrentDate;

            _lessonData.date = DateTime.Now.ToShortDateString(); // сразу заполняем текущую дату пары
            _lessonData.teacher_code = TeacherCode; //и код учителя

            //проверяем есть ли загруженное расписание
            if (!_isSheduleExist) { sheduleCV.ItemsSource = GetShedule(TeacherCode).Result; _isSheduleExist = true; }
            else return;
            sheduleCV.SelectedItem = null;
            
            if (App.Database.GetSavedData() != null)
            { 
                if ( await DisplayAlert("Внимание!", "У вас есть неотправленные данные. \nОтправить?", "Yes", "No"))
                {
                    await Task.Run(SendData);
                }
                else App.Database.DeleteSavedData();
            }
            
        }

        private void SendData()
        {
            MainThread.BeginInvokeOnMainThread(async () => 
            {
                try
                {
                    var client = new RestClient("http://192.168.5.68:80/kamtk/hs/SetDataLesson");
                    var request = new RestRequest() { Method = Method.Post };
                    request.AddHeader("Authorization", "Basic 0JDQtNC80LjQvTo=");
                    request.AddHeader("Content-Type", "text/plain");
                    var rqstContent = App.Database.GetSavedData().JSONData;
                    request.AddParameter("application/json", rqstContent, ParameterType.RequestBody);
                    
                    RestResponse response = client.Execute(request);
                    if (response.IsSuccessful)
                    {
                        App.Database.DeleteSavedData();
                        await DisplayAlert("Подтверждение", "Результаты успешно отправлены!", "Назад");

                    }
                    else await DisplayAlert("Ошибка", response.ErrorMessage, "Назад");


                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ошибка", ex.Message, "Ок");
                    throw;
                }


            });

        }
        private async void sheduleCV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((e.CurrentSelection[0] as SheduleItems).para)
            {
                case "1":
                    if (DateTime.Now.Hour >= 15 && DateTime.Now.Minute >= 5)
                        await DisplayAlert("Ошибка!", "Пара уже закончилась!. \n Отметки посещаемости недоступны", "Ок");
                    else await SaveLessonData(0); //записываем данные кликнутой пары
                    break;
                    
                case "2":
                    if (DateTime.Now.Hour >= 11 && DateTime.Now.Minute >= 50)
                        await DisplayAlert("Ошибка!", "Пара уже закончилась!. \n Отметки посещаемости недоступны", "Ок");
                    else await SaveLessonData(1);
                    break;

                case "3":
                    if (DateTime.Now.Hour >= 11 && DateTime.Now.Minute >= 55)
                        await DisplayAlert("Ошибка!", "Пара уже закончилась. \n Отметки посещаемости недоступны", "Ок");
                    else await SaveLessonData(2);
                    break;

                case "4":
                    if (DateTime.Now.Hour >= 15 && DateTime.Now.Minute >= 40)
                        await DisplayAlert("Ошибка!", "Пара уже закончилась. \n Отметки посещаемости недоступны", "Ок");
                    else await SaveLessonData(3);
                    break;

                case "5":
                    if (DateTime.Now.Hour >= 16 && DateTime.Now.Minute >= 50)
                        await DisplayAlert("Ошибка!", "Пара уже закончилась. \n Отметки посещаемости недоступны", "Ок");
                    else await SaveLessonData(4);
                    break;

                default: break;
                    
            }
        }

        private async Task SaveLessonData(int num)
        {
            _lessonData.index_predmet = _shedule[num].code_predmet;
            _lessonData.name_predmet = _shedule[num].name_predmet;
            _lessonData.para = num+1;
            _lessonData.theme = "";
            _lessonData.group_name = _shedule[num].group_name;
            _lessonData.code_group = _shedule[num].group_code;

            GetStudents();
            await Navigation.PushAsync(scannerPage);
        }

        private void GetStudents()
        {
            var date = $"{DateTime.Now:dd.MM.yyyy}";
            var listStudents = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Students.Students_extended>>(
                GetStundentInfoByGroupCode(_lessonData.code_group, _lessonData.index_predmet, TeacherCode, date, _lessonData.para));

            for (int i = 0; i < listStudents.Count; i++)
            {
                studentsData.Code_student_J.Add(listStudents[i].code_student);
                studentsData.studentSave.Add(listStudents[i].name_student);
            }
        }

        private void logoutBtn_Clicked(object sender, EventArgs e)
        {
            Preferences.Clear();
            Shell.Current.GoToAsync("///loginPage");
        }


        private async Task<ObservableCollection<SheduleItems>> GetShedule(string teacherCode)
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(5);
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://kamtk.ru/BaseKPK/hs/El_zurnal11?code_teacher={teacherCode}&date_lessons=2");
            request.Headers.Add("Authorization", "Basic 0KHQsNC50YI6"); //заголовки базовой авторизации

             try
            {   
                var response = client.SendAsync(request).Result;

                var respContent = response.EnsureSuccessStatusCode().Content.ReadAsStringAsync().Result;
           
                //конвертируем полученный JSON в объекты класса SheduleItems (непосредственно само расписание)
                var shedule = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SheduleItems>>(respContent);
                _shedule.Clear();
                foreach (var item in shedule)
                    _shedule.Add(item);
                return _shedule;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "ok");
                throw;
            }
        }

        
        private async void UpdateSheduleBnt_Clicked(object sender, EventArgs e)
        {
            if (await GetShedule(TeacherCode) != null)
            {
                await DisplayAlert("Обновление расписания", "Расписание успешно обновлено!", "Ок");
            }
            else await DisplayAlert("Обновление расписания", "Не удалось обновить расписание :( \n Повторите попытку...", "Ок");

        }
        private string GetStundentInfoByGroupCode(string GC, string IP, string TC, string date, int para)
        {
            //    запрос
            var client = new RestClient("http://kamtk.ru/BaseKPK/hs/El_zurnal6");
            var request = new RestRequest() { Method = Method.Post };
            request.AddHeader("Authorization", "Basic 0KHQsNC50YI6");
            request.AddHeader("Content-Type", "text/plain");

            string rqstBody = $"{{" +
                                $"\"code_group\": \"{GC}\", \n" +
                                $"\"index_predmet\": \"{IP}\", \n" +
                                $"\"code_teacher\": \"{TC}\", \n" +
                                $"\"date\": \"{date}\", \n" +
                                $"\"para\": \"{para}\"" +
                            $"}}";
            request.AddParameter("text/plain", rqstBody, ParameterType.RequestBody);
            try
            {
                RestResponse response = client.Execute(request);

                var editedResponse = response.Content.Substring(response.Content.IndexOf("]") + 16);
                editedResponse = editedResponse.Remove(editedResponse.IndexOf("]") + 1);

                return editedResponse;

            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "ok");
                throw;
            }
        }





















        private void admin_Clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("///dbTables");
        }

    }
}
