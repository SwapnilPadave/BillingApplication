using System.Xml.Linq;

namespace BA.Utility.XmlHelper
{
    public static class XmlReaderHelper
    {
        public static string ToXml<T>(List<T> data, string rootName, string itemName)
        {
            XElement xml = new XElement(rootName,
                data.Select(item =>
                {
                    XElement element = new XElement(itemName);

                    foreach (var prop in typeof(T).GetProperties())
                    {
                        element.Add(new XElement(prop.Name, prop.GetValue(item)));
                    }

                    return element;
                })
            );

            return xml.ToString();
        }
    }

}
