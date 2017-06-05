using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Yixin.Atom.Core
{
    public static class Setting
    {
        public static T GetValue<T>(string name)
        {
            var setting = ApplicationData.Current.LocalSettings;
            return (T)setting.Values[name];
        }
        public static void SetValue<T>(string name, T value)
        {
            var setting = ApplicationData.Current.LocalSettings;
            setting.Values[name] = value;
        }
        public static int UserId
        {
            get { return GetValue<int>("userid"); }
            set { SetValue("userid", value); }
        }
    }
}
