using System.Xml.Serialization;

namespace BexFileRead
{
    [XmlRoot(ElementName = "DocumentTemplate")]
    public class DocumentTemplate
    {
        [XmlElement(ElementName = "Document")]
        public string Document { get; set; }
        [XmlElement(ElementName = "TemplateName")]
        public string TemplateName { get; set; }
        [XmlElement(ElementName = "Format")]
        public string Format { get; set; }
        [XmlElement(ElementName = "UploadDate")]
        public string UploadDate { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
    }
}