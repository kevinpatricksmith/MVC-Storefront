using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data
{
    public class ShippingMethod
    {
        public int ID { get; set; }
        public string Carrier { get; set; }
        public string ServiceName { get; set; }
        public decimal RatePerPound { get; set; }
        public string EstimatedDelivery { get; set; }
        public int DaysToDeliver { get; set; }

        public string Display {
            get {

                string shippingFormat = "{0}:{1} {2}";
                return string.Format(shippingFormat, Carrier, ServiceName, Cost.ToString("C"));

            }
        }

        //this is a placeholder field for the Shipping Service
        //to fill in
        public decimal Cost { get; set; }
        
        public ShippingMethod() { }
        

        public ShippingMethod(string carrier,string serviceName, 
            decimal ratePerPound, string estimatedDelivery, int daysToDeliver)
        {
            this.ID = 0;
            this.Carrier = carrier;
            this.ServiceName = serviceName;
            this.RatePerPound = ratePerPound;
            this.EstimatedDelivery = estimatedDelivery;
            this.Cost = 0;
            this.DaysToDeliver = daysToDeliver;
        }


        public override string ToString() {

            return Display;
        }


    }

    

}
