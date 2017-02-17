
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;


namespace eCQM2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Crawler();
            Console.ReadLine();

        }
        private static async void Crawler()
        {
            var url = "https://ecqi.healthit.gov/ep/ecqms-2017-performance-period";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var tds = htmlDocument.DocumentNode.Descendants("td")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("views-field views-field-title active")).ToList();

            foreach (var td in tds)
            {
                var uri = "https://ecqi.healthit.gov" + td.Descendants("a")?.FirstOrDefault()?.ChildAttributes("href")?.FirstOrDefault().Value;
                await Task.Delay(5000);
                var newHttpClient = new HttpClient();
                var newHtml = await newHttpClient.GetStringAsync(uri);
                var newHtmlDocument = new HtmlDocument();
                newHtmlDocument.LoadHtml(newHtml);
                var htmlFiles = newHtmlDocument.DocumentNode.Descendants("span")
                    .Where(n => n.GetAttributeValue("class", "")
                    .Equals("file")).ToArray();

                for (var i = 1; i < 3; i++)
                {
                    var htmlUrl = htmlFiles[2].Descendants("a")?.ElementAt(0).ChildAttributes("href")?.FirstOrDefault().Value;
                    await Task.Delay(2000);
                    var htmlHttpClient = new HttpClient();
                    var newHtml2 = await htmlHttpClient.GetStringAsync(htmlUrl);
                    
                    var xmlDoc = XDocument.Parse(newHtml2);

                    //measure details node
                    var title = xmlDoc.Root?.XPathSelectElement("/measure/measureDetails/title")?.Value;
                    var eMeasureId = xmlDoc.Root?.XPathSelectElement("/measure/measureDetails/emeasureid")?.Value;
                    var version = xmlDoc.Root?.XPathSelectElement("/measure/measureDetails/version")?.Value;
                    var guidance = xmlDoc.Root?.XPathSelectElement("/measure/measureDetails/guidance")?.Value;
                    var clinicalRecommendation = xmlDoc.Root?.XPathSelectElement("/measure/measureDetails/recommendations")?.Value;
                    var rationale = xmlDoc.Root?.XPathSelectElement("/measure/measureDetails/rationale")?.Value;
                    var references = xmlDoc.Root?.XPathSelectElements("/measure/measureDetails/references/reference")?.ToList();

                    //measure grouping
                    var measureGroupingNodes = xmlDoc.Root?.XPathSelectElements("/measure/measureGrouping/group/clause")?.ToList();
                    //sub tree lookup
                    var subTreeGrouping = xmlDoc.Root?.XPathSelectElements("/measure/subTreeLookUp/subTree")?.ToList();
 
                    //element lookup
                    var elementLookUpList = xmlDoc.Root?.XPathSelectElements("/measure/elementLookUp/qdm")?.ToList();
                }
            }
        }
    }
}
