using Newtonsoft.Json; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Yixin.Atom.Core;
using Yixin.Atom.Core.Models;

namespace Yixin.Atom.Rasp
{
    public class SensorData
    {
        const int Pin_Pm25 = 17;
        const int Pin_Soil = 27;
        const int Pin_Rain = 22;
        const int Pin_Dht = 4;
        int[] DhtData = new int[4];

        private GpioPin PinPm25, PinSoil, PinRain, PinDht;
        private Bmp180Sensor _bmp180;
        private DHT11Sensor _dht;

        private DataBase _db;
        private GpioController gpio;

        public ModelBase Data { get; set; }

        public Mqtt _mqtt;
        private DispatcherTimer timer_;

        public SensorData()
        {
            _db = new DataBase();
            Data = new ModelBase();
            _mqtt = new Mqtt("192.168.31.123");
            Setting.UserId = 0;
            InitSensors();
            _mqtt.MqttMsgPublishReceived += _mqtt_MqttMsgPublishReceived;
            UpdatedataAsync();
        }

        private void _mqtt_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            var msg = JsonConvert.DeserializeObject<Message>(Encoding.UTF8.GetString(e.Message));
            if (msg.UserId != Setting.GetValue<int>("userid"))
            {
                var data = new Message()
                {
                    DataType = msg.DataType,
                    UserId = Setting.GetValue<int>("userid"),
                    StartTime = msg.StartTime,
                    EndTime = msg.EndTime
                };
                if (msg.DataType == 0) //当前数据
                {
                    data.Body = new List<DataModel>() { new DataModel(Data) };
                }
                else if (msg.DataType == 2) //历史数据
                {
                    data.Body = _db.Table<DataModel>().Where(p => DateTime.Compare(msg.StartTime, p.Time) < 0 && DateTime.Compare(msg.EndTime, p.Time) > 0).ToList();
                }
                _mqtt.PostData(JsonConvert.SerializeObject(data));
            }
        }

        public async void InitSensors()
        {
            gpio = GpioController.GetDefault();

            PinDht = gpio.OpenPin(Pin_Dht, GpioSharingMode.Exclusive);
            PinPm25 = gpio.OpenPin(Pin_Pm25, GpioSharingMode.Exclusive);
            PinRain = gpio.OpenPin(Pin_Rain, GpioSharingMode.Exclusive);
            PinSoil = gpio.OpenPin(Pin_Soil, GpioSharingMode.Exclusive);

            PinPm25.SetDriveMode(GpioPinDriveMode.Input);
            PinRain.SetDriveMode(GpioPinDriveMode.Input);
            PinSoil.SetDriveMode(GpioPinDriveMode.Input);

            PinPm25.ValueChanged += PinPm25_ValueChanged;
            PinRain.ValueChanged += PinRain_ValueChanged;
            PinSoil.ValueChanged += PinSoil_ValueChanged;
             
            Data.Pm25 = PinPm25.Read() == GpioPinValue.High ? 1 : 0;
            Data.Rain = PinRain.Read() == GpioPinValue.High ? 1 : 0;
            Data.Soil = PinSoil.Read() == GpioPinValue.High ? 1 : 0;
           
        }

        private void PinSoil_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            Data.Soil = PinSoil.Read() == GpioPinValue.High ? 1 : 0;
        }

        private void PinRain_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            Data.Rain = PinRain.Read() == GpioPinValue.High ? 1 : 0;
            PostValueChange(ChangeType.Rain);
        }

        private void PinPm25_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            Data.Pm25 = PinPm25.Read() == GpioPinValue.High ? 1 : 0;
        }
        private void PostValueChange(ChangeType type, double temp = 0)
        {
            var data = new Message()
            {
                DataType = 3,
                ChangeType = type,
                RainValue = GetRain(),
                SoilValue = GetSoil(),
                SmokeValue = GetPm25(),
                TempValue = temp
            };
            _mqtt.PostData(JsonConvert.SerializeObject(data));
        }

        public async Task<(double Temp, double Press)> GetTempAndPressAsync()
        {
            if (_bmp180 == null )
            {
                _bmp180 = new Bmp180Sensor();
            }
            Bmp180SensorData bmp;
                if (!_bmp180.IsInitialized)
                {
                   await _bmp180.InitializeAsync();
                   bmp = await _bmp180.GetSensorDataAsync(Bmp180AccuracyMode.UltraHighResolution); 
                }
                else
                {
                    bmp = await _bmp180.GetSensorDataAsync(Bmp180AccuracyMode.UltraHighResolution);
                }
                Data.Press = bmp.Pressure;
                Data.Temp = bmp.Temperature;
                MainPage.Current.Model.BmpTemp = bmp.Temperature;
                MainPage.Current.Model.Press = bmp.Pressure;
               
                return (Data.Temp, Data.Press);
             
        }
        public double GetHumiAsync()
        {
            //int[] data = new int[4];
            if (_dht == null)
            {
                _dht = new DHT11Sensor(PinDht, new PrecisionCronometer());
                _dht.initialize();
                _dht.read();
                _dht.getData(DhtData);
            }
            else
            {
                _dht.read();
                _dht.getData(DhtData);
            } 
            double.TryParse(DhtData[0] + "." + DhtData[1], out double humi);
            double.TryParse(DhtData[2] + "." + DhtData[3],out double temp);
            Data.Humi = humi;
            MainPage.Current.Model.Humi = humi;
            MainPage.Current.Model.DhtTemp = temp;
            return humi;
        }
        
        public int GetSoil()
        {
            return Data.Soil = MainPage.Current.Model.Soil = PinSoil.Read() ==  GpioPinValue.High ? 1 : 0;
        }
        public int GetPm25()
        {
            return Data.Pm25 = MainPage.Current.Model.Pm= PinPm25.Read() == GpioPinValue.High ? 1 : 0;
        }
        public int GetRain()
        {
            return Data.Rain = MainPage.Current.Model.Rain= PinRain.Read() == GpioPinValue.High ? 1 : 0; ;
        }
        public async void GetNowDataAsync()
        {
            await GetTempAndPressAsync();
            GetHumiAsync();
            GetSoil();
            GetRain();
            GetPm25();
           // return (Data.Temp, Data.Press, Data.Humi, Data.Soil, Data.Rain, Data.Pm25);
        }
        public void UpdatedataAsync()
        {
            timer_ = new DispatcherTimer();
            timer_.Interval = TimeSpan.FromSeconds(3);
            timer_.Tick += Timer__Tick;
            timer_.Start();
        }

        private void Timer__Tick(object sender, object e)
        {
            GetNowDataAsync();
            MainPage.Current.Model.Time = DateTime.Now.ToString();
            var msg = new Message() { Body = new List<DataModel> { new DataModel(Data) } , UserId=Setting.UserId };
            _mqtt.PostData(JsonConvert.SerializeObject(msg));
        }

       
    }
}
