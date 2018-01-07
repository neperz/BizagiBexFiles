using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace BexFileRead
{
  

    [XmlRoot(ElementName = "ArrayOfIndexObject")]
    public class ArrayOfIndexObject
    {
        [XmlElement(ElementName = "IndexObject")]
        public List<IndexObject> IndexObject { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
    }

}
