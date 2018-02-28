using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Bex.Domain
{
   
 
    [XmlRoot(ElementName = "ArrayOfIndexValue")]
    public class ArrayOfIndexValue
    {
        [XmlElement(ElementName = "IndexValue")]
        public List<IndexValue> IndexValueIn { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
    }

}
