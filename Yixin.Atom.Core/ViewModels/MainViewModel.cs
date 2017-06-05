using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yixin.Atom.Core.Models;

namespace Yixin.Atom.Core.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private double temp;
        private double humi;
        private double press;
        private string soil;
        private string pm25;
        private string rain;
        private string date;
        private string time;
        private string waring;
        /// <summary>
        /// 温度属性
        /// </summary>
        public double Temp
        {
            get { return temp; }
            set { temp = value; OnPropertyChanged(nameof(Temp)); }
        }
        public double Humi
        {
            get { return humi; }
            set { humi = value; OnPropertyChanged(nameof(Humi)); }
        }
        public double Press
        {
            get { return press; }
            set { press = value; OnPropertyChanged(nameof(Press)); }
        }
        public string Soil
        {
            get { return soil; }
            set { soil = value; OnPropertyChanged(nameof(Soil)); }
        }
        public string Pm25
        {
            get { return pm25; }
            set { pm25 = value; OnPropertyChanged(nameof(Pm25)); }
        }
        public string Rain
        {
            get { return rain; }
            set { rain = value; OnPropertyChanged(nameof(Rain)); }
        }
        public string Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged(nameof(Date)); }
        }
        public string Time
        {
            get { return time; }
            set { time = value; OnPropertyChanged(nameof(Time)); }
        }
        public string Waring
        {
            get { return waring; }
            set { waring = value; OnPropertyChanged(nameof(Waring)); }
        }

        public MainViewModel()
        {
            var now = DateTime.Now;
            Date = now.Date.ToString("yy/MM") + now.ToString("ddd") + " " + SolarToChineseLunisolarDate(now);
            Humi = 55;
            Pm25 = "良好";
            Press = 99.3;
            Rain = "未下雨";
            Soil = "干燥";
            Temp = 15;
            Time = now.ToString("HH:mm");
            Waring = "警告！土壤过于干燥!";
        }
        public string GetDate(DateTime date)
        {
           return date.ToString("yy/MM") + date.ToString("ddd") + " " + SolarToChineseLunisolarDate(date);
        }
        public string GetTime(DateTime time)
        {
            return time.ToString("HH:mm");
        }
        public void SetData(DataModel data)
        {
            Date = data.Time.Date.ToString("yy/MM") + data.Time.ToString("ddd") + " " + SolarToChineseLunisolarDate(data.Time);
            Humi = data.Humi;
            Pm25 = data.Pm25 == 0 ? "良好" : "浓度过高";
            Press = ((data.Press)/(double)10);
            Rain = data.Rain == 0 ? "未下雨" : "有雨";
            Soil = data.Soil == 0 ? "良好" : "干燥";
            Temp = data.Temp;
            Time = data.Time.ToString("HH:mm:ss");
            Waring = data.Pm25 == 1 ? "警告！烟雾浓度过高！" : data.Rain == 1 ? "警告！开始下雨了！" : data.Soil == 0 ? "警告！土壤过于干燥!" : "";
        }

        public static string SolarToChineseLunisolarDate(DateTime solarDateTime)
        {
            System.Globalization.ChineseLunisolarCalendar cal = new System.Globalization.ChineseLunisolarCalendar();

            int year = cal.GetYear(solarDateTime);
            int month = cal.GetMonth(solarDateTime);
            int day = cal.GetDayOfMonth(solarDateTime);
            int leapMonth = cal.GetLeapMonth(year, 1);
            return string.Format("农历{3}{4}月{5}{6}"
                                , "甲乙丙丁戊己庚辛壬癸"[(year - 4) % 10]
                                , "子丑寅卯辰巳午未申酉戌亥"[(year - 4) % 12]
                                , "鼠牛虎兔龙蛇马羊猴鸡狗猪"[(year - 4) % 12]
                                , month == leapMonth ? "闰" : ""
                                , "无正二三四五六七八九十冬腊"[leapMonth > 0 && leapMonth <= month ? month - 1 : month]
                                , "初十廿三"[day / 10]
                                , "日一二三四五六七八九"[day % 10]
                                );
        }
    }
}
