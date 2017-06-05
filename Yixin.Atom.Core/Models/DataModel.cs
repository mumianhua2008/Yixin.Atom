using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Yixin.Atom.Core.Models
{
    [DataContract]
    public class ModelBase
    {
        [DataMember]
        public double Temp { get; set; }
        [DataMember]
        public double Humi { get; set; }
        [DataMember]
        public double Press { get; set; }
        [DataMember]
        public int Rain { get; set; }
        [DataMember]
        public int Pm25 { get; set; }
        [DataMember]
        public int Soil { get; set; }
    }
    [Table("DataModel"), DataContract]
    public class DataModel : ModelBase
    {
        [PrimaryKey, AutoIncrement, DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime Time { get; set; }
        public DataModel(ModelBase data)
        {
            Temp = data.Temp;
            Press = data.Press;
            Humi = data.Humi;
            Rain = data.Rain;
            Soil = data.Soil;
            Pm25 = data.Pm25;
            Time = DateTime.Now;
        }
        public DataModel(ModelBase data, DateTime time)
        {
            Temp = data.Temp;
            Press = data.Press;
            Humi = data.Humi;
            Rain = data.Rain;
            Soil = data.Soil;
            Pm25 = data.Pm25;
            Time = time;
        }
        public DataModel() { }
    }
}
