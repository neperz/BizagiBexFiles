using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace BexFileRead
{
   
    [XmlRoot(ElementName = "element", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class Element
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "minOccurs")]
        public string MinOccurs { get; set; }
    }

    [XmlRoot(ElementName = "sequence", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class Sequence
    {
        [XmlElement(ElementName = "element", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public List<Element> Element { get; set; }
    }

    [XmlRoot(ElementName = "complexType", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class ComplexType
    {
        [XmlElement(ElementName = "sequence", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public Sequence Sequence { get; set; }
    }

    [XmlRoot(ElementName = "choice", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class Choice
    {
        [XmlElement(ElementName = "element", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public Element Element { get; set; }
        [XmlAttribute(AttributeName = "minOccurs")]
        public string MinOccurs { get; set; }
        [XmlAttribute(AttributeName = "maxOccurs")]
        public string MaxOccurs { get; set; }
    }

    [XmlRoot(ElementName = "schema", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class Schema
    {
        [XmlElement(ElementName = "element", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public Element Element { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "xs", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xs { get; set; }
        [XmlAttribute(AttributeName = "msdata", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Msdata { get; set; }
    }

    [XmlRoot(ElementName = "BIZAGIINFO")]
    public class BIZAGIINFO
    {
        [XmlElement(ElementName = "BAInfo")]
        public string BAInfo { get; set; }
        [XmlElement(ElementName = "BAValue")]
        public string BAValue { get; set; }
    }

    [XmlRoot(ElementName = "NewDataSet")]
    public class NewDataSet
    {
        [XmlElement(ElementName = "schema", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public Schema Schema { get; set; }
        [XmlElement(ElementName = "BIZAGIINFO")]
        public List<BIZAGIINFO> BIZAGIINFO { get; set; }
    }

}
