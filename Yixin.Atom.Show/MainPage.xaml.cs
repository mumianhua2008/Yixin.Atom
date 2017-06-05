using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using uPLibrary.Networking.M2Mqtt;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Yixin.Atom.Core;
using Yixin.Atom.Core.Models;
using Yixin.Atom.Core.ViewModels;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Yixin.Atom.Show
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainViewModel _model = new MainViewModel();
        public MainViewModel Model
        {
            get { return _model; }
            set { _model = value; }
        }
        public static MainPage Current { get; set; }
        public Mqtt _mqtt;
        public MainPage()
        {
            this.InitializeComponent();
            Current = this;
            Setting.UserId = 1;
            var msgserver = new MessageServer();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            InsertData();
        }
        private void InsertData()
        {
            var db = new DataBase();
            //var data = db.Table<DataModel>().ToList();
            //var list = data.Where(p => p.Time.Month == 2);
            //foreach (var item in list)
            //    Debug.WriteLine(item.Temp);
            db.Table<DataModel>().Delete(p => 1 == 1);
            var time = new DateTime(2016, 1, 1, 0, 0, 0);
            Random R = new Random();
            while (time.CompareTo(DateTime.Now) < 0)
            {
                for (int i = 0; i < 24; i++)
                {
                    var data = new ModelBase()
                    {
                        Pm25 = R.Next(0, 2),
                        Rain = R.Next(0, 2),
                        Soil = R.Next(0, 2),
                        Humi = (double)R.Next(400, 900) / 1000.0,
                        Temp = (double)R.Next(50, 300) / 10.0,
                        Press = R.NextDouble() + 97.5
                    };
                    db.Insert(new DataModel(data, time));
                    time = time.AddHours(1);
                    Debug.WriteLine(time + "---" + db.Table<DataModel>().Count().ToString());
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DetailPage));
        }
    }
}
