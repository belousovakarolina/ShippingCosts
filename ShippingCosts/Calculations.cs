using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ShippingCosts
{
    public class Calculations
    {
        /// <summary>
        /// Gets a price of sending a parcel with specified parameters (courier, size)
        /// </summary>
        /// <param name="courier">The courier which sends the parcel</param>
        /// <param name="size">The size of the parcel</param>
        /// <param name="prices">A list of all prices</param>
        /// <returns>Price of sending specific parcel</returns>
        public static decimal GetPrice(string courier, string size, 
            List<Tuple<string, string, decimal>> prices)
        {
            foreach (Tuple<string, string, decimal> price in prices)
            {
                if (courier.Equals(price.Item1) && size.Equals(price.Item2))
                    return price.Item3;
            }
            return -1;
        }
        /// <summary>
        /// Gets a lowest price of sending a parcel of specified size between all couriers
        /// </summary>
        /// <param name="size">The specified size of a parcel</param>
        /// <param name="availableCouriers">The list of available couriers</param>
        /// <param name="prices">The list of all prices</param>
        /// <returns>A lowest price of sending a parcel with specified size</returns>
        public static decimal GetLowestPrice(string size, List<string> availableCouriers, 
            List<Tuple<string, string, decimal>> prices)
        {
            decimal price = decimal.MaxValue;
            foreach (string courier in availableCouriers)
            {
                decimal courierPrice = GetPrice(courier, size, prices);
                if (courierPrice < price && courierPrice != -1)
                    price = courierPrice;
            }
            return price;
        }
        /// <summary>
        /// Sets the prices for shipments according to the rules
        /// Rule No. 1: All "sizeLowestPrice" shipments should always match the lowest "sizeLowestPrice" package 
        /// price among the providers.
        /// Rule No. 2: The "whichShipmentFree" "shipmentFreeSize" shipment via "shipmentFreeCourier" should be free, 
        /// but only once a calendar month.
        /// Rule No. 3: Accumulated discounts cannot exceed "monthlyBudget" in a calendar month. If there are not enough 
        /// funds to fully cover a discount this calendar month, it should be covered partially.
        /// </summary>
        /// <param name="shipments">the list of all shipments</param>
        /// <param name="sizeLowestPrice">Shipments of this size should cost the smallest price (rule No. 1)</param>
        /// <param name="monthlyBudget">Budget for discounts (rule No. 3)</param>
        /// <param name="whichShipmentFree">Which shipment should be free (rule No. 2)</param>
        /// <param name="shipmentFreeCourier">Shipment with this courier might be free (rule No. 2)</param>
        /// <param name="shipmentFreeSize">Shipment with this size might be free (rule No. 2)</param>
        /// <param name="availableCouriers">List of available couriers</param>
        /// <param name="prices">The list of all prices</param>
        /// <returns>A list of all shipments with set prices</returns>
        public static List<Shipment> SetPrices(List<Shipment> shipments,
            string sizeLowestPrice, decimal monthlyBudget, int whichShipmentFree,
            string shipmentFreeCourier, string shipmentFreeSize, List<string> availableCouriers,
            List<Tuple<string, string, decimal>> prices)
        {
            decimal lowestPrice = GetLowestPrice(sizeLowestPrice, availableCouriers, prices);
            int currentMonth = -1;
            int freeShipmentLeft = -1;
            decimal budgetLeft = monthlyBudget;
            foreach (Shipment shipment in shipments)
            {
                if (shipment.Ignored)
                    continue; //skip this shipment

                decimal price = GetPrice(shipment.Courier, shipment.Size, prices);
                int shipmentMonth = shipment.Date.Month;

                if (currentMonth != shipmentMonth) //month has changed
                {
                    currentMonth = shipmentMonth;
                    freeShipmentLeft = whichShipmentFree;
                    budgetLeft = monthlyBudget;
                }

                if (shipment.Size.Equals(sizeLowestPrice) && price != lowestPrice && budgetLeft > 0)
                {//Rule No. 1: All "sizeLowestPrice" shipments should always match the lowest "sizeLowestPrice" package 
                 //price among the providers.
                    decimal discount = price - lowestPrice;
                    if (budgetLeft > discount)
                    {
                        shipment.Price = lowestPrice;
                        shipment.Discount = discount;
                        budgetLeft -= discount;
                    }
                    else
                    {
                        shipment.Discount = budgetLeft;
                        shipment.Price = price - budgetLeft;
                        budgetLeft = 0m;
                    }
                }
                else if (shipment.Size.Equals(shipmentFreeSize) && shipment.Courier.Equals(shipmentFreeCourier) 
                    && budgetLeft > 0)
                {//Rule No. 2: The "whichShipmentFree" "shipmentFreeSize" shipment via "shipmentFreeCourier" should be free, 
                 //but only once a calendar month.
                    if (freeShipmentLeft == 1) //this one is third, thus has to be free
                    {
                        if (budgetLeft > price)
                        {
                            shipment.Price = 0m;
                            shipment.Discount = price;
                            budgetLeft -= price;
                        }
                        else
                        {
                            shipment.Discount = budgetLeft;
                            shipment.Price = price - budgetLeft;
                            budgetLeft = 0m;
                        }
                    }
                    else //it is not third
                    {
                        shipment.Price = price;
                        shipment.Discount = 0m;
                    }
                    freeShipmentLeft--;
                }
                else //no discounts applied
                {
                    shipment.Price = price;
                    shipment.Discount = 0m;
                }
            }
            return shipments;
        }
    }
}
