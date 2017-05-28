using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using PCLStorage;

namespace App2
{
    public static class LocationDB
    {
        static SQLiteConnection conn;

        static LocationDB()
        {
            conn = new SQLiteConnection(FileSystem.Current.LocalStorage.Path + "/db");
            conn.CreateTable<Location>();
        }

        public static int SaveItem(Location item)
        {
            if(item.Id == 0)
            {
                return conn.Insert(item);
            }
            else
            {
                return conn.Update(item);
            }
        }

        public static List<Location> GetLocationList()
        {
            return conn.Table<Location>().ToList();
        }

        public static int DeleteItem(Location item)
        {
            return conn.Delete(item);
        }
    }
}
