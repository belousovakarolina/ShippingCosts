using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingCosts
{
    public static class InputOutput
    {
        /// <summary>
        /// Reads data from a specified file and returns a list of shipments
        /// </summary>
        /// <param name="path">specified file path</param>
        /// <param name="availableSizes">list of available parcel sizes. Needed for data validation</param>
        /// <param name="availableCouriers">list of available couriers. Needed for data validation</param>
        /// <returns>A list of shipments</returns>
        public static List<Shipment> ReadData(string path, List<string> availableSizes, 
            List<string> availableCouriers)
        {
            List<Shipment> shipments = new();
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                string[] words = line.Split(' ');
                if (words.Length == 3)
                {
                    bool tryDate = DateTime.TryParse(words[0], out DateTime date);
                    if (tryDate) //date format was correct
                    {
                        string size = words[1];
                        if (availableSizes.Contains(size)) //size was correct
                        {
                            string courier = words[2];
                            if (availableCouriers.Contains(courier))
                            {
                                Shipment shipment = new(date, size, courier);
                                shipments.Add(shipment);
                            }
                            else
                            {
                                //Console.WriteLine("Provided courier was incorrect. Courier provided: " + words[2]);
                                Shipment shipment = new(line);
                                shipments.Add(shipment);
                            }

                        }
                        else //size was incorrect
                        {
                            //Console.WriteLine("Provided size was incorrect. Size provided: " + words[1]);
                            Shipment shipment = new(line);
                            shipments.Add(shipment);
                        }
                    }
                    else //date format was incorrect
                    {
                        //Console.WriteLine("Provided date was incorrect. Date provided: " + words[0]);
                        Shipment shipment = new(line);
                        shipments.Add(shipment);
                    }
                }
                else //number of input words was incorrect
                {
                    //Console.WriteLine("Provided data was incorrect. Data provided: " + line);
                    Shipment shipment = new(line);
                    shipments.Add(shipment);
                }

            }
            return shipments;
        }
        /// <summary>
        /// Prints data of all shipments to the console
        /// </summary>
        /// <param name="shipments">The shipments of which the data will be printed</param>
        public static void PrintData(List<Shipment> shipments)
        {
            foreach (Shipment shipment in shipments)
            {
                Console.WriteLine(shipment.ToString());
            }
        }
    }
}
