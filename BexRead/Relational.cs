using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace BexFileRead
{
   

    [XmlRoot(ElementName = "selector", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class Selector
    {
        [XmlAttribute(AttributeName = "xpath")]
        public string Xpath { get; set; }
    }

    [XmlRoot(ElementName = "field", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class Field
    {
        [XmlAttribute(AttributeName = "xpath")]
        public string Xpath { get; set; }
    }

    [XmlRoot(ElementName = "unique", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class Unique
    {
        [XmlElement(ElementName = "selector", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public Selector Selector { get; set; }
        [XmlElement(ElementName = "field", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public Field Field { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "PrimaryKey", Namespace = "urn:schemas-microsoft-com:xml-msdata")]
        public string PrimaryKey { get; set; }
    }

  

    [XmlRoot(ElementName = "BADPLYCONFIG")]
    public class BADPLYCONFIG
    {
        [XmlElement(ElementName = "idBaDplyConfig")]
        public string IdBaDplyConfig { get; set; }
        [XmlElement(ElementName = "baDplyConfigFile")]
        public string BaDplyConfigFile { get; set; }
        [XmlElement(ElementName = "guidBADplyConfig")]
        public string GuidBADplyConfig { get; set; }
        [XmlElement(ElementName = "dplyBADplyConfig")]
        public string DplyBADplyConfig { get; set; }
        [XmlElement(ElementName = "dcFileVersion")]
        public string DcFileVersion { get; set; }
        [XmlElement(ElementName = "dcMapType")]
        public string DcMapType { get; set; }
    }

  

}
