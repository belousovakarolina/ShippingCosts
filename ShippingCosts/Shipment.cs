using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingCosts
{
    public class Shipment
    {
        public DateTime Date { get; set; }
        public string? Size { get; set; }
        public string? Courier { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public bool Ignored { get; set; }
        public string? Data { get; set; } //for simply storing data if it is incorrect
                                          //(will use it for printing)
        public Shipment(DateTime date, string size, string courier)
        {
            Date = date;
            Size = size;
            Courier = courier;
            Ignored = false;
        }
        /// <summary>
        /// Constructor for creating a shipment with faulty data
        /// </summary>
        /// <param name="data">The faulty string of data</param>
        public Shipment(string data)
        {
            Ignored = true;
            Data = data;
        }
        /// <summary>
        /// Formats the data of a parcel in a needed way
        /// </summary>
        /// <returns>A string with all data of a shipment</returns>
        public override string ToString()
        {
            if (Ignored)
                return string.Format(Data + " Ignored");
            else if (Decimal.Equals(Discount, 0m))
                return string.Format(Date.ToString("yyyy-MM-dd") + " " + Size + " " + Courier + " " + Price.ToString("F") + " -");
            return string.Format(Date.ToString("yyyy-MM-dd") + " " + Size + " " + Courier + " " + 
                Price.ToString("F") + " " + Discount.ToString("F"));
        }
    }
}
