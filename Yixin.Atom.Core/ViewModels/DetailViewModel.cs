using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yixin.Atom.Core.Models;

namespace Yixin.Atom.Core.ViewModels
{
    public class DetailViewModel : ViewModelBase
    {
        private DataBase _db = new DataBase();
        public DataBase Db
        {
            get { return _db; }
        }
        private int _year;
        public int Year
        {
            get { return _year; }
            set { _year = value; OnPropertyChanged(); }
        }
        private int _month;
        public int Month
        {
            get { return _month; }
            set { _month = value; OnPropertyChanged(); }
        }
        private int _day;
        public int Day
        {
            get { return _day; }
            set { _day = value; OnPropertyChanged(); }
        }
        private DateType _dateType = DateType.Year;
        public DateType DateType
        {
            get { return _dateType; }
            set { _dateType = value; OnPropertyChanged(); }
        }
        private ObservableCollection<GraphModel> _atomData = new ObservableCollection<GraphModel>();
        public ObservableCollection<GraphModel> AtomData
        {
            get { return _atomData; }
            set { _atomData = value; OnPropertyChanged(); }
        }
        private ObservableCollection<PieModel> _pm = new ObservableCollection<PieModel>();
        private ObservableCollection<PieModel> _rain = new ObservableCollection<PieModel>();
        private ObservableCollection<PieModel> _soil = new ObservableCollection<PieModel>();
        public ObservableCollection<PieModel> PmData
        {
            get { return _pm; }
            set { _pm = value; OnPropertyChanged(); }
        }
        public ObservableCollection<PieModel> RainData
        {
            get { return _rain; }
            set { _rain = value; OnPropertyChanged(); }
        }
        public ObservableCollection<PieModel> SoilData
        {
            get { return _soil; }
            set { _soil = value; OnPropertyChanged(); }
        }
        private string _top;
        private string _ava;
        private string _dowm;
        public string _cloumn;
        public string Top
        {
            get { return _top; }
            set { _top = value; OnPropertyChanged(); }
        }
        public string Ava
        {
            get { return _ava; }
            set { _ava = value; OnPropertyChanged(); }
        }
        public string Down
        {
            get { return _dowm; }
            set { _dowm = value; OnPropertyChanged(); }
        }
        public string Column
        {
            get { return _cloumn; }
            set { _cloumn = value; OnPropertyChanged(); }
        }
        public DetailViewModel()
        {

        }

        public void DrawMonth(int year)
        {
            AtomData.Clear();
            SoilData.Clear();
            SoilData.Add(new PieModel { title = "", value = 0 });
            RainData.Clear();
            PmData.Clear();
            var data = Db.Table<DataModel>().ToList().Where(p => p.Time.Year == year);
            for (int i = 1; i <= 12; i++)
            {
                var temp = data.Where(p => p.Time.Month == i);
                if (temp.Count() > 0)
                    AtomData.Add(new GraphModel { Line = temp.Max(p => p.Temp), Line2 = temp.Average(p => p.Temp), Line3 = temp.Min(p => p.Temp) });

            }
            var count = data.Count();
            var soil = (double)data.Count(p => p.Soil == 0) / count;
            var rain = (double)data.Count(p => p.Rain == 0) / count;
            var pm = (double)data.Count(p => p.Pm25 == 0) / count;
            SoilData.Add(new PieModel { title = "干燥", value = soil });
            SoilData.Add(new PieModel { title = "湿润", value = 1 - soil });
            RainData.Add(new PieModel { title = "有雨", value = 1 - rain });
            RainData.Add(new PieModel { title = "无雨", value = rain });
            PmData.Add(new PieModel { title = "超标", value = 1 - pm });
            PmData.Add(new PieModel { title = "良好", value = pm });
        }
        public void DrawDay(int year, int month)
        {
            SoilData.Clear();
            RainData.Clear();
            PmData.Clear();
            AtomData.Clear();
            var data = Db.Table<DataModel>().ToList().Where(p => p.Time.Year == year && p.Time.Month == month);
            for (int i = 1; i <= data.Last().Time.Day; i++)
            {
                var temp = data.Where(p => p.Time.Day == i);
                AtomData.Add(new GraphModel { Line = temp.Max(p => p.Temp), Line2 = temp.Average(p => p.Temp), Line3 = temp.Min(p => p.Temp) });

            }
            var count = data.Count();
            var soil = (double)data.Count(p => p.Soil == 0) / count;
            var rain = (double)data.Count(p => p.Rain == 0) / count;
            var pm = (double)data.Count(p => p.Pm25 == 0) / count;
            SoilData.Add(new PieModel { title = "干燥", value = soil });
            SoilData.Add(new PieModel { title = "湿润", value = 1 - soil });
            RainData.Add(new PieModel { title = "有雨", value = 1 - rain });
            RainData.Add(new PieModel { title = "无雨", value = rain });
            PmData.Add(new PieModel { title = "超标", value = 1 - pm });
            PmData.Add(new PieModel { title = "良好", value = pm });
        }
        public void DrawHour(int year, int month, int day)
        {
            SoilData.Clear();
            RainData.Clear();
            PmData.Clear();
            AtomData.Clear();
            SoilData.Add(new PieModel { title = "", value = 0 });
            var list = Db.Table<DataModel>().ToList();
            var data = list.Where(p => p.Time.Year == year && p.Time.Month == month && p.Time.Day == day);
            for (int i = 0; i < 24; i++)
            {
                var temp = data.First(p => p.Time.Hour == i);
                AtomData.Add(new GraphModel { Line = temp.Temp });

            }
            var count = data.Count();
            var soil = (double)data.Count(p => p.Soil == 0) / count;
            var rain = (double)data.Count(p => p.Rain == 0) / count;
            var pm = (double)data.Count(p => p.Pm25 == 0) / count;
            SoilData.Add(new PieModel { title = "干燥", value = soil });
            SoilData.Add(new PieModel { title = "湿润", value = 1 - soil });
            RainData.Add(new PieModel { title = "有雨", value = 1 - rain });
            RainData.Add(new PieModel { title = "无雨", value = rain });
            PmData.Add(new PieModel { title = "超标", value = 1 - pm });
            PmData.Add(new PieModel { title = "良好", value = pm });
        }
        public void ByHour()
        {
            AtomData.Clear();
            for (int i = 0; i < 24; i++)
            {
                var data = Db.Table<DataModel>().Where(p => p.Time.Hour == i);
                AtomData.Add(new GraphModel { Line = data.Max(p => p.Temp), Line2 = data.Average(p => p.Temp), Line3 = data.Min(p => p.Temp) });
            }

        }
        public void ByDay()
        {
            AtomData.Clear();
            for (int i = 1; i <= 31; i++)
            {
                var data = Db.Table<DataModel>().Where(p => p.Time.Day == i);
                AtomData.Add(new GraphModel { Line = data.Max(p => p.Temp), Line2 = data.Average(p => p.Temp), Line3 = data.Min(p => p.Temp) });
            }
        }
        public void ByMonth()
        {
            AtomData.Clear();
            for (int i = 1; i <= 12; i++)
            {
                var data = Db.Table<DataModel>().Where(p => p.Time.Month == i);
                AtomData.Add(new GraphModel { Line = data.Max(p => p.Temp), Line2 = data.Average(p => p.Temp), Line3 = data.Min(p => p.Temp) });
            }
        }
    }
    public class GraphModel
    {
        public double Line { get; set; }
        public double Line2 { get; set; }
        public double Line3 { get; set; }
    }
    public class PieModel
    {
        public string title { get; set; }
        public double value { get; set; }
    }
    public enum DateType
    {
        Year = 0,
        Month = 1,
        Day = 2
    }
}
