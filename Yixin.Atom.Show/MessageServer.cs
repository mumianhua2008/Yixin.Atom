using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Yixin.Atom.Core;
using Yixin.Atom.Core.Models;
using Yixin.Atom.Core.ViewModels;

namespace Yixin.Atom.Show
{
    public class MessageServer
    {
        public static MessageServer Current { get; set; }
        Mqtt _mqtt;
        public MessageServer()
        {
            Current = this;
            _mqtt = new Mqtt("192.168.31.123");
            _mqtt.MqttMsgPublishReceived += _mqtt_MqttMsgPublishReceived;
        }

      async  private void  _mqtt_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            var str = Encoding.UTF8.GetString(e.Message);
            var msg = JsonConvert.DeserializeObject<Message>(str);
            if (msg.UserId != Setting.GetValue<int>("userid"))
            {
                if (msg.DataType == 0 && MainPage.Current != null) //当前数据
                {
                    var model= msg.Body.FirstOrDefault();
                    if (model != null)
                    {
                      await  MainPage.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,() =>
                        {
                           MainPage.Current.Model.SetData(model);
                        });
                        if(MainPage.Current.Model.Waring.Length>1)
                        ShowToastNotification(MainPage.Current.Model.Waring);

                    }
                   
                }
                else if (msg.DataType == 1) //突发情况
                {
                    switch (msg.ChangeType)
                    {
                        case ChangeType.Pm25:
                            if (MainPage.Current != null)
                            {
                                MainPage.Current.Model.Pm25 = msg.SmokeValue == 0 ? "良好" : "浓度过高";
                                MainPage.Current.Model.Waring = msg.SmokeValue == 1 ? "当前空气质量糟糕，注意防护！" : "";
                            }
                            if (msg.SmokeValue == 1)
                                ShowToastNotification("当前空气质量糟糕，注意防护！");
                            break;
                        case ChangeType.Rain:
                            if (MainPage.Current != null)
                            {
                                MainPage.Current.Model.Rain = msg.RainValue == 0 ? "未下雨" : "有雨";
                                MainPage.Current.Model.Waring = msg.RainValue == 1 ? "当前正在下雨，请做好防雨措施！" : "";
                            }
                            if (msg.RainValue == 1)
                            {
                                ShowToastNotification("当前正在下雨，请做好防雨措施！");
                            }
                            break;
                        case ChangeType.Soil:
                            if (MainPage.Current != null)
                            {
                                MainPage.Current.Model.Soil = msg.SoilValue == 0 ? "良好" : "干燥";
                                MainPage.Current.Model.Waring = msg.SoilValue == 1 ? "当前土壤干燥，请处理！" : "";
                            } 
                                ShowToastNotification(MainPage.Current.Model.Waring);
                            break;
                        case ChangeType.Temp:
                            if (MainPage.Current != null)
                            {
                                MainPage.Current.Model.Temp += msg.TempValue;
                                MainPage.Current.Model.Waring = msg.TempValue < 0 ? "降温" : msg.TempValue > 0 ? "高温" : "";
                            }
                            if (msg.TempValue < 0)
                                ShowToastNotification("气温骤降，注意保暖！");
                            else if (msg.TempValue > 0)
                                ShowToastNotification("高温，注意防暑!");
                            break;
                    }
                }
                else if (msg.DataType == 2 && DetailPage.Current != null) //历史数据
                {
                    foreach (var item in msg.Body)
                    {
                        //DetailPage.Current.Model.ModelList.Add(new MainViewModel(item));
                    }
                }
            }
        }

        public void RequestData(int dataType, DateTime start, DateTime end)
        {
            var msg = new MessageBase()
            {
                DataType = dataType,
                UserId = Setting.GetValue<int>("userid"),
                StartTime = start,
                EndTime = end
            };
            _mqtt.PostData(JsonConvert.SerializeObject(msg));
        }
        public static void ShowToastNotification(string text)
        {
            // 1. create element
            ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            // 2. provide text
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(text));

            // 3. provide image
            //XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
            //((XmlElement)toastImageAttributes[0]).SetAttribute("src", $"ms-appx:///assets/{assetsImageFileName}");
            //((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "logo");

            // 4. duration
            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "short");

            // 5. audio
            //XmlElement audio = toastXml.CreateElement("audio");
            //audio.SetAttribute("src", $"ms-winsoundevent:Notification.{audioName.ToString().Replace("_", ".")}");
            //toastNode.AppendChild(audio);

            // 6. app launch parameter
            //((XmlElement)toastNode).SetAttribute("launch", "{\"type\":\"toast\",\"param1\":\"12345\",\"param2\":\"67890\"}");

            // 7. send toast
            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
