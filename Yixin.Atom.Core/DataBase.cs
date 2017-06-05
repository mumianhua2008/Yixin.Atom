using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Yixin.Atom.Core.Models;

namespace Yixin.Atom.Core
{
    public class DataBase : SQLiteConnection
    {
        public static string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "atmo.db");
        public DataBase() : base(new SQLitePlatformWinRT(), path)
        {
            CreateTable<DataModel>();
        }
    }
}
