using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.IO;

namespace App2
{
    public static class LocationDB
    {
        static SQLiteConnection conn;

        public static void Init()
        {
            conn = new SQLiteConnection(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/db");
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

        public static Location GetLocation(int id)
        {
            return conn.Get<Location>(id);
        }

        public static List<Location> GetLocations()
        {
            return conn.Table<Location>().ToList();
        }

        public static int DeleteItem(Location item)
        {
            return conn.Delete(item);
        }
    }
}
