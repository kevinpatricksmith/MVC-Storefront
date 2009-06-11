using System;
using System.IO;
using System.Net;
using Commerce.Data;

namespace Commerce.Services {
    public class USAddressValidationService:IAddressValidationService{

        public Address VerifyAddress(Address address) {
            
            //using RPC.Geocoder - this is a free service
            Address result = address;

            //http://rpc.geocoder.us/demo.cgi?address=1600+Pennsylvania+Ave%2C+Washington+DC

            string addressArgs;

            if (!String.IsNullOrEmpty(address.Zip)) {
                addressArgs = string.Format("{0},{1}", address.Street1, address.Zip);

            } else {
                addressArgs = string.Format("{0},{1},{2}", address.Street1, address.City, address.StateOrProvince);
            }
            //do the web request and get the reply
            string url = string.Format("{0}?{1}", "http://rpc.geocoder.us/service/csv?address=", addressArgs);
            url = url.Replace(" ", "+").Replace(",", "%2C");


            string webPage;
            WebRequest request = WebRequest.Create(url);
            using (Stream stream = request.GetResponse().GetResponseStream()) {
                StreamReader sr = new StreamReader(stream);
                webPage = sr.ReadToEnd();
                sr.Close();
            }

            //the reply is in CSV Format, like this:
            //38.898748,-77.037684,1600 Pennsylvania Ave NW,Washington,DC,20502

            //if there is no match, then it will send back something like
            //2) oops sorry we can't find

            string[] resultCSV = webPage.Split(',');
            if (resultCSV.Length > 1) {
                result.Latitude = resultCSV[0];
                result.Longitude = resultCSV[1];
                result.Street1 = resultCSV[2];
                result.City = resultCSV[3];
                result.StateOrProvince = resultCSV[4];
                result.Zip = resultCSV[5];
            } else {
                throw new InvalidDataException("Cannot find this address :" + address);

            }
            return result;
        }

    }
}
