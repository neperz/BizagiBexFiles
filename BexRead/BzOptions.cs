using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace BexFileRead
{
    
    [XmlRoot(ElementName = "AdvancedOption")]
    public class AdvancedOption
    {
        [XmlElement(ElementName = "Id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "DisplayOrder")]
        public string DisplayOrder { get; set; }
        [XmlElement(ElementName = "Enabled")]
        public string Enabled { get; set; }
        [XmlElement(ElementName = "Visible")]
        public string Visible { get; set; }
        [XmlElement(ElementName = "EAdvancedOptionType")]
        public string EAdvancedOptionType { get; set; }
        [XmlElement(ElementName = "SelectedValue")]
        public List<string> SelectedValue { get; set; }
        [XmlElement(ElementName = "DefaultValue")]
        public List<string> DefaultValue { get; set; }
        [XmlElement(ElementName = "Enumeration")]
        public string Enumeration { get; set; }
        [XmlElement(ElementName = "Assembly")]
        public string Assembly { get; set; }
    }

    [XmlRoot(ElementName = "AdvancedOptions")]
    public class AdvancedOptions
    {
        [XmlElement(ElementName = "AdvancedOption")]
        public List<AdvancedOption> AdvancedOption { get; set; }
    }

}
