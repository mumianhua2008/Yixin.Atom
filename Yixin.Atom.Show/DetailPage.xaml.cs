using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Yixin.Atom.Core.ViewModels;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Yixin.Atom.Show
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        public DetailViewModel Model { get; set; }
        public static DetailPage Current { get; set; }
        public DetailPage()
        {
            this.InitializeComponent();
            SelectType.SelectedIndex = 0;
            var db = new DataBase();
            //db.Table<DataModel>().ToList();
            Current = this;
            Model = new DetailViewModel();
            Random R = new Random();
            Model.Top = "最高";
            Model.Ava = "平均";
            Model.Down = "最低";
            //for(int i = 0; i < 30; i++)
            //{
            //    var max = R.Next(25, 30);
            //    var min = R.Next(18, 25);
            //    Model.AtomData.Add(new GraphModel { Line =max, Line2 = (max+min)/2, Line3 = min });
            //}
            //Model.SoilData.Add(new PieModel { title = "干燥", value = 0.3 });
            //Model.SoilData.Add(new PieModel { title = "湿润", value = 0.7 });
            //Model.RainData.Add(new PieModel { title = "下雨", value = 0.4 });
            //Model.RainData.Add(new PieModel { title = "未下雨", value = 0.6 });
            //Model.PmData.Add(new PieModel { title = "超标", value = 0.6 });
            //Model.PmData.Add(new PieModel { title = "良好", value = 0.4 });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SelectType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (SelectType.SelectedIndex)
            {
                case 0:
                    SelectYear.Visibility = Visibility.Visible;
                    SelectMonth.Visibility = SelectDay.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    SelectYear.Visibility = SelectMonth.Visibility = Visibility.Visible;
                    SelectDay.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    SelectYear.Visibility = SelectMonth.Visibility = Visibility.Collapsed;
                    SelectDay.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Model = new DetailViewModel();
            switch (SelectType.SelectedIndex)
            {
                case 0:
                    var year = SelectYear.SelectedIndex == 0 ? 2017 : 2016; 
                    Model.DrawMonth(year);
                    break;
                case 1:
                    var year1 = SelectYear.SelectedIndex == 0 ? 2017 : 2016;
                    Model.DrawDay(year1, SelectMonth.SelectedIndex + 1);
                    break;
                case 2:
                    var date = SelectDay.Date.Value.Date;
                    Model.DrawHour(date.Year, date.Month, date.Day);
                    break;

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
