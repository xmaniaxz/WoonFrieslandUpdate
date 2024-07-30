using Microsoft.Toolkit.Uwp.Notifications;
using System.Text.RegularExpressions;
using System.Threading;
namespace WoonFrieslandUpdate
{
    public class ScriptInfo
    {
        public int StartLine;
        public int EndLine;

        public List<Container> containers = new List<Container>();

        public async void GetData(string responseBody)
        {
            SplitLines(responseBody);
            FilterMessage();
        }

        private async void FilterMessage()
        {
            for (int i = 0; i < containers.Count; i++)
            {
                Container container = containers[i];
                container.Address = Regex.Replace(container.Address, "<\\/span>|<span>", string.Empty);
                container.City = Regex.Replace(container.City, "<\\/strong>|<strong>", string.Empty);
                container.Price = Regex.Replace(container.Price, "\\&euro;", "€");
            }
        }

        private string RemoveAllWhitelines(string input)
        {
            string output = Regex.Replace(input, @"\s+", " ");
            return output;
        }

        private void SplitLines(string Input)
        {
            // grab all lines
            var Splitlines = Input.Split('\n');
            for (int i = 0; i < Splitlines.Length; i++)
            {
                if (Splitlines[i].Contains("dwellings"))
                {
                    StartLine = i;
                }
                else if (Splitlines[i].Contains("map"))
                {
                    EndLine = i;
                }
            }

            //Now grab only the nessecary lines

            for (int i = StartLine; i < EndLine; i++)
            {
                if (Splitlines[i].Contains("dwelling-info"))
                {
                    containers.Add(new Container(RemoveAllWhitelines(Splitlines[i + 2]), RemoveAllWhitelines(Splitlines[i + 1]), RemoveAllWhitelines(Splitlines[i + 5])));
                }
            }
        }
    }


    public class Container
    {
        public string Address;
        public string City;
        public string Price;

        public Container(string address, string city, string price)
        {
            Address = address;
            City = city;
            Price = price;
        }
    }
}