using System;
using System.IO;
using System.Linq;
using System.Text;

namespace TestCountriesApp
{
    public class TestCountriesApp
    {
        public static void Main(string[] args)
        {
            //See https://www.nationsonline.org/oneworld/countries_of_the_world.htm
            // for list of countries used.
            string[] countries = File.ReadAllLines("Countries.txt");
            
            //Create a file with all of the countries and their scores
            //Scores.txt
            StringBuilder countryScores = new StringBuilder();

            //Go through each country in the list
            //and see what their maxium score could be
            foreach(string country in countries)
            {
                //Create a list that remembers which countries
                // have already been "traveled" to
                List<string> countriesTravelledTo = new List<string>();

                //Add the country to the list of countries
                //travelled to
                countriesTravelledTo.Add(country);

                //Create a method that will retrieve the country's score
                // Func(country name, countries travelled to, countries)
                GetCountryScore(country, countriesTravelledTo, countries);

                //Add the country and their score to the file
                countryScores.Append($"{country}, {countriesTravelledTo.Count()}\n");
            }

            File.WriteAllText("Scores.txt", countryScores.ToString());
        }

        private static void GetCountryScore(string _countryName, List<string> _countriesTravelledTo, string[] _countries)
        {
            char countryNameLastChar = _countryName[_countryName.Length - 1].ToString().ToUpper().ToCharArray()[0];
            string nextCountry =
                _countries.Where(
                    cname => !_countriesTravelledTo.Contains(cname) && cname.StartsWith(countryNameLastChar)
                    ).FirstOrDefault();

            if (string.IsNullOrEmpty(nextCountry))
            {
                return;
            }
            else
            {
                _countriesTravelledTo.Add(nextCountry);
                GetCountryScore(nextCountry, _countriesTravelledTo, _countries);
            }
        }
    }
}
