using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace BexFileRead
{
   

    [XmlRoot(ElementName = "ArrayOfReference")]
    public class ArrayOfReference
    {
        [XmlElement(ElementName = "Reference")]
        public List<Reference> Reference { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
    }

}
