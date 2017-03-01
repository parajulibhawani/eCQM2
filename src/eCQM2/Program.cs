
using eCQM2.Models;
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
            var db = new eMeasureDbContext();
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

                for (var i = 2; i < 3; i++)
                {
                    var htmlUrl = htmlFiles[2].Descendants("a")?.ElementAt(0).ChildAttributes("href")?.FirstOrDefault().Value;
                    await Task.Delay(2000);
                    var htmlHttpClient = new HttpClient();
                    var newHtml2 = await htmlHttpClient.GetStringAsync(htmlUrl);
                    
                    var xmlDoc = XDocument.Parse(newHtml2);
                    var measure = new eMeasure();
                    //measure details node
                    measure.title = xmlDoc.Root?.XPathSelectElement("/measure/measureDetails/title")?.Value;
                    measure.eMeasureNumber = xmlDoc.Root?.XPathSelectElement("/measure/measureDetails/emeasureid")?.Value;
                    measure.version = xmlDoc.Root?.XPathSelectElement("/measure/measureDetails/version")?.Value;
                    measure.guidance = xmlDoc.Root?.XPathSelectElement("/measure/measureDetails/guidance")?.Value;
                    measure.clinicalRecommendation = xmlDoc.Root?.XPathSelectElement("/measure/measureDetails/recommendations")?.Value;
                    measure.rationale = xmlDoc.Root?.XPathSelectElement("/measure/measureDetails/rationale")?.Value;
                    var references = xmlDoc.Root?.XPathSelectElements("/measure/measureDetails/references/reference")?.ToList();

                    foreach (var refer in references)
                    {
                        var reference = new Reference();

                        reference.reference = refer?.Value;
                    }
                    //measure grouping
                    var measureGroupingNodes = xmlDoc.Root?.XPathSelectElements("/measure/measureGrouping/group/clause")?.ToList();
                    foreach(var measureGroupingNode in measureGroupingNodes)
                    {
                        var newClause = new Clause();
                        newClause.clauseName = measureGroupingNode?.Attribute("displayName")?.Value;
                        var logicalOps = measureGroupingNode?.Descendants()?.Where(n=>n.Name.LocalName =="logicalOp")?.ToList();
                        var subTreeRefs = measureGroupingNode?.Descendants()?.Where(n => n.Name.LocalName == "subTreeRef")?.ToList();
                        
                        foreach(var subTreeRef in subTreeRefs)
                        {
                            var subTreeName = subTreeRef?.Attribute("displayName")?.Value;
                        }
                    }
                    //sub tree lookup
                    var subTreeGrouping = xmlDoc.Root?.XPathSelectElements("/measure/subTreeLookUp/subTree")?.ToList();
                    foreach(var subTree in subTreeGrouping)
                      {
                        var subTreeName = subTree?.Attribute("displayName")?.Value;
                        var subTreeElements = subTree?.Elements()?.ToList();
                        for(var j = 0; j < subTreeElements.Count; j++)
                        {
                            var subTreeElementType = subTreeElements[j]?.Name.LocalName;
                            var subTreeElementName = subTreeElements[j]?.Attribute("displayName")?.Value;
                            var elementRefs = subTreeElements[j]?.Descendants()?.ToList();
                            foreach(var elementRef in elementRefs)
                            {
                                if(elementRef?.Name.LocalName == "elementRef")
                                {
                                    var elementRefName = elementRef?.Attribute("displayName")?.Value;
                                    var attributeValue = elementRef?.Element("attribute")?.Attribute("name")?.Value;
                                }
                            }
                        }
                    }
                    //element lookup
                    var elementLookUpList = xmlDoc.Root?.XPathSelectElements("/measure/elementLookUp/qdm")?.ToList();
                    foreach(var qdm in elementLookUpList)
                    {
                        var newQdm = new QDM();
                        newQdm.qdmTitle = qdm?.Attribute("name")?.Value;
                        newQdm.qdmDatatype = qdm?.Attribute("datatype")?.Value;
                        newQdm.valueSet = qdm?.Attribute("oid")?.Value;
                    }
                }
            }
        }
    }
}
