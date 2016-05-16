using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElasticSearchBulkLoaderDataGenerator
{
    public class CreditCardTransaction
    {

        public string MaskedCardNumber {
            get {
                return MaskCreditCard(Faker.CreditCard.CreditCardType());
            }
        }

        public string Name
        {
            get
            {
                return Faker.Name.FullName();
            }
        }

        public decimal Amount
        {
            get
            {
                return (decimal)Faker.Number.RandomNumber(0,10000) + (decimal)Faker.Number.RandomNumber(1, 100)/100;
            }
        }

        public string Address
        {
            get
            {
                return Faker.Number.RandomNumber(100, 500) + " " +  Faker.Address.StreetName() + Faker.Address.StreetSuffix();
            }
        }

        public string City
        {
            get
            {
                return Faker.Address.USCity();
            }
        }

        public string State
        {
            get
            {
                return Faker.Address.StateAbbreviation();
            }
        }


        public string IP
        {
            get
            {
                return Faker.Internet.IPv4();
            }
        }

        public string TerminalID
        {
            get
            {
                return RandomString(10); //actually want to get a few dupes on this
            }
        }

        public string MerchantID
        {
            get
            {
                return RandomString(10); //actually want to get a few dupes on this
            }
        }


        public string TimestampUTC
        {
            get
            {
                return Faker.Date.Between(DateTime.Now.AddDays(-30), DateTime.Now).ToUniversalTime().ToString("s"); //stortable datetime
            }
        }

        public static string MaskCreditCard(string value)
        {
            const string PATTERN = @"\b(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|" +
              @"6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|" +
              @"[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})\b";

            var replace = Regex.Replace(value, PATTERN, new MatchEvaluator(match =>
            {
                var num = match.ToString();
                return num.Substring(0, 6) + new string('*', num.Length - 10) +
                  num.Substring(num.Length - 4);
            }));

            return replace;
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
