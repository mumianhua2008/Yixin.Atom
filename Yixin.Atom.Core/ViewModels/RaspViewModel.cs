using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yixin.Atom.Core.ViewModels
{
   public class RaspViewModel:ViewModelBase
    {
        private double dhtT;
        private double humi;
        private double bmpT;
        private double press;
        private int soil;
        private int pm;
        private int rain;
        private string time;
        public double DhtTemp
        {
            get { return dhtT; }
            set { dhtT = value;OnPropertyChanged(); }
        }
        public double Humi
        {
            get { return humi; }
            set { humi = value;OnPropertyChanged(); }
        }
        public double BmpTemp
        {
            get { return bmpT; }
            set { bmpT = value;OnPropertyChanged(); }
        }
        public double Press
        {
            get { return press; }
            set { press = value;OnPropertyChanged(); }
        }
        public int Pm
        {
            get { return pm; }
            set { pm = value;OnPropertyChanged(); }
        }
        public int Soil
        {
            get { return soil; }
            set { soil = value;OnPropertyChanged(); }
        }
        public int Rain
        {
            get { return rain; }
            set { rain = value;OnPropertyChanged(); }
        }
        public string Time
        {
            get { return time; }
            set { time = value;OnPropertyChanged(); }
        }
    }
}
