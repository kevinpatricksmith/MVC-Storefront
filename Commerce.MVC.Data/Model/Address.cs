using System;
using System.IdentityModel.Claims;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.TokenProcessor;

namespace Commerce.Data
{
    public class Address
    {

        public int ID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateOrProvince { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public bool IsDefault { get; set; }
        public string FullName {
            get
            {
                return FirstName + " " + LastName;
            }
        
        }

        public Address(string infoCardToken)
        {
            Token token=null;
            token = new Token(infoCardToken);

            this.FirstName = token.Claims[ClaimTypes.GivenName];
            this.LastName = token.Claims[ClaimTypes.Surname];
            this.Email = token.Claims[ClaimTypes.Email];
            this.Street1 = token.Claims[ClaimTypes.StreetAddress];
            this.City = token.Claims[ClaimTypes.Locality];
            this.StateOrProvince = token.Claims[ClaimTypes.StateOrProvince];
            this.Country = token.Claims[ClaimTypes.Country];
            this.Zip = token.Claims[ClaimTypes.PostalCode];
            
            this.UserName = token.UniqueID;


        }


        public Address()
        {
            ID = 0;
        }

        public Address(string userName, string first, string last, string email, string street1, string street2, string city, string state, string zip, string country)
        {
            this.UserName = userName;
            this.FirstName = first;
            this.LastName = last;
            this.Email = email;
            this.Street1 = street1;
            this.Street2 = street2;
            this.City = city;
            this.StateOrProvince = state;
            this.Zip = zip;
            this.Country = country;
            ID = 0;
        }

        /// <summary>
        /// ToString() override, which formats an Address in a Postal-service, readable way
        /// </summary>
        /// <returns>System.String</returns>       
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} {1}\r\n",this.FirstName, this.LastName);
            sb.AppendLine(this.Street1);
            if (!String.IsNullOrEmpty(this.Street2))
                sb.AppendLine(this.Street2);

            sb.AppendLine(this.City + ", " + this.StateOrProvince + " " + this.Zip + ", " + this.Country);
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            bool result = false;
            try
            {
                Address compareTo = (Address)obj;
                result = this.Street1.Equals(compareTo.Street1, StringComparison.CurrentCultureIgnoreCase) &&
                    this.City.Equals(compareTo.City, StringComparison.CurrentCultureIgnoreCase);


            }
            catch
            {
                result = base.Equals(obj);
            }
            return result;

        }

    }
}
