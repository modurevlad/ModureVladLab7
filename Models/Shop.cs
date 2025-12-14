using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModureVladLab7.Models
{
    public class Shop
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string ShopName { get; set; }

        public string Address { get; set; }

        public string ShopDetails
        {
            get
            {
                return ShopName + Environment.NewLine + Address;
            }
        }

        [OneToMany]
        public List<ShopList> ShopLists { get; set; }

    }
}
