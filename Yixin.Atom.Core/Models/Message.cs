using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Yixin.Atom.Core.Models
{
    [DataContract]
    public class MessageBase
    {
        [DataMember]
        public int UserId { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        [DataMember]
        public int DataType { get; set; }
        [DataMember]
        public DateTime StartTime { get; set; }
        [DataMember]
        public DateTime EndTime { get; set; }

    }
    [DataContract]
    public class Message : MessageBase
    {
        /// <summary>
        /// 气温变化
        /// </summary>
        [DataMember]
        public double TempValue { get; set; }
        [DataMember]
        public int RainValue { get; set; }
        [DataMember]
        public int SmokeValue { get; set; }
        [DataMember]
        public int SoilValue { get; set; }
        public ChangeType ChangeType { get; set; }
        [DataMember]
        public List<DataModel> Body { get; set; }

    }
    public enum ChangeType
    {
        Rain = 0,
        Soil = 1,
        Pm25 = 2,
        Temp = 4
    }
}
