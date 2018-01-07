using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace BexFileRead
{
    
    [XmlRoot(ElementName = "CatalogGuids")]
    public class CatalogGuids
    {
        [XmlElement(ElementName = "CatalogObject")]
        public List<string> CatalogObject { get; set; }
    }

    [XmlRoot(ElementName = "PackageContentInformation")]
    public class PackageContentInformation
    {
        [XmlElement(ElementName = "CatalogGuids")]
        public CatalogGuids CatalogGuids { get; set; }
        [XmlElement(ElementName = "BizAgiBizAgiversion")]
        public string BizAgiBizAgiversion { get; set; }
        [XmlElement(ElementName = "Time")]
        public string Time { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
    }

}
