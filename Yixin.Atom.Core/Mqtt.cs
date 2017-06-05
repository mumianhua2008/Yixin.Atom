using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Yixin.Atom.Core
{
    public class Mqtt : MqttClient
    {

        public Mqtt(string host) : base(host, 61613, false, MqttSslProtocols.None)
        {
            Connect(Guid.NewGuid().ToString(), "admin", "password");
            Subscribe(new string[] { "atmo" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
           // PostData("Hello World!");
        } 
        public void RequestData(string msg)
        {
            Publish("atmo", Encoding.UTF8.GetBytes(msg));
        }
        public void PostData(string msg)
        {
            Publish("atmo", Encoding.UTF8.GetBytes(msg));
        }
    }
}
