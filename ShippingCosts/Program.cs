using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingCosts;
class Program
{
    static void Main()
    {
        List<Tuple<string, string, decimal>> Prices = new()
        {              //Courier, Size, Price
            Tuple.Create("LP", "S", 1.5m),
            Tuple.Create("LP", "M", 4.9m),
            Tuple.Create("LP", "L", 6.9m),
            Tuple.Create("MR", "S", 2m),
            Tuple.Create("MR", "M", 3m),
            Tuple.Create("MR", "L", 4m)
        };

        List<string> AvailableCouriers = Prices.Select(d => d.Item1).ToList();
        List<string> AvailableSizes = Prices.Select(d => d.Item2).ToList();

        List<Shipment> AllShipments = InputOutput.ReadData(@"input.txt", AvailableSizes, AvailableCouriers);

        //It is assumed that shipments in the input file are written in order of date. If that is not the case,
        //the following line of code can be uncommented and thus put to use. However, if there are any shipments
        //with incorrect data, those will be written in the beginning of the list and thus may not obey the date rule
        //AllShipments.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
        AllShipments = Calculations.SetPrices(AllShipments, "S", 10m, 3, "LP", "L", AvailableCouriers, Prices);
        InputOutput.PrintData(AllShipments);
    }
}