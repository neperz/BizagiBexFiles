using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace BexFileRead
{

   

    [XmlRoot(ElementName = "CatalogObject")]
    public class CatalogObject
    {
        [XmlElement(ElementName = "Id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "Content")]
        public string Content { get; set; }
        [XmlElement(ElementName = "DeployOnParent")]
        public string DeployOnParent { get; set; }
        [XmlElement(ElementName = "ModifiedByUser")]
        public string ModifiedByUser { get; set; }
        [XmlElement(ElementName = "Version")]
        public string Version { get; set; }
        [XmlElement(ElementName = "MetadataState")]
        public string MetadataState { get; set; }
        [XmlElement(ElementName = "Root")]
        public string Root { get; set; }
        [XmlElement(ElementName = "ChangeSetId")]
        public string ChangeSetId { get; set; }
        [XmlElement(ElementName = "Deleted")]
        public string Deleted { get; set; }
        [XmlElement(ElementName = "ContentFormat")]
        public string ContentFormat { get; set; }
        [XmlElement(ElementName = "HasOverride")]
        public string HasOverride { get; set; }
        [XmlElement(ElementName = "IsSystem")]
        public string IsSystem { get; set; }
        [XmlElement(ElementName = "IsInProduction")]
        public string IsInProduction { get; set; }
        [XmlElement(ElementName = "IsReadyForRemoving")]
        public string IsReadyForRemoving { get; set; }
        [XmlElement(ElementName = "Children")]
        public string Children { get; set; }
        [XmlElement(ElementName = "CatalogReferences")]
        public string CatalogReferences { get; set; }
        [XmlElement(ElementName = "References")]
        public References References { get; set; }
        [XmlElement(ElementName = "Tags")]
        public Tags Tags { get; set; }
        [XmlElement(ElementName = "Indexes")]
        public Indexes Indexes { get; set; }
        [XmlElement(ElementName = "ParentId")]
        public string ParentId { get; set; }
        [XmlElement(ElementName = "LocalizableProperties")]
        public LocalizableProperties LocalizableProperties { get; set; }
        [XmlIgnore]
        public Dictionary<string, object> Dicionario { get;  set; }
    }

    [XmlRoot(ElementName = "Reference")]
    public class Reference
    {
        [XmlElement(ElementName = "Pointer")]
        public string Pointer { get; set; }
        [XmlElement(ElementName = "Referrer")]
        public string Referrer { get; set; }
        [XmlElement(ElementName = "Target")]
        public string Target { get; set; }
        [XmlElement(ElementName = "Root")]
        public string Root { get; set; }
        [XmlElement(ElementName = "Deleted")]
        public string Deleted { get; set; }
    }

    [XmlRoot(ElementName = "References")]
    public class References
    {
        [XmlElement(ElementName = "Reference")]
        public List<Reference> Reference { get; set; }
    }

    [XmlRoot(ElementName = "IndexValue")]
    public class IndexValue
    {
        [XmlElement(ElementName = "Root")]
        public string Root { get; set; }
        [XmlElement(ElementName = "ObjectId")]
        public string ObjectId { get; set; }
        [XmlElement(ElementName = "TagType")]
        public string TagType { get; set; }
        [XmlElement(ElementName = "IndexName")]
        public string IndexName { get; set; }
        [XmlElement(ElementName = "Value")]
        public string Value { get; set; }
        [XmlElement(ElementName = "Deleted")]
        public string Deleted { get; set; }
    }

    [XmlRoot(ElementName = "Tags")]
    public class Tags
    {
        [XmlElement(ElementName = "IndexValue")]
        public List<IndexValue> IndexValue { get; set; }
    }

    [XmlRoot(ElementName = "IndexObject")]
    public class IndexObject
    {
        [XmlElement(ElementName = "IndexName")]
        public string IndexName { get; set; }
        [XmlElement(ElementName = "SourceGuid")]
        public string SourceGuid { get; set; }
        [XmlElement(ElementName = "TargetGuid")]
        public string TargetGuid { get; set; }
        [XmlElement(ElementName = "Root")]
        public string Root { get; set; }
        [XmlElement(ElementName = "Deleted")]
        public string Deleted { get; set; }
    }

    [XmlRoot(ElementName = "Indexes")]
    public class Indexes
    {
        [XmlElement(ElementName = "IndexObject")]
        public List<IndexObject> IndexObject { get; set; }
    }

    [XmlRoot(ElementName = "ParentId")]
    public class ParentId
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "LocalizableProperty")]
    public class LocalizableProperty
    {
        [XmlElement(ElementName = "ObjectId")]
        public string ObjectId { get; set; }
        [XmlElement(ElementName = "URI")]
        public string URI { get; set; }
        [XmlElement(ElementName = "Content")]
        public string Content { get; set; }
    }

    [XmlRoot(ElementName = "LocalizableProperties")]
    public class LocalizableProperties
    {
        [XmlElement(ElementName = "LocalizableProperty")]
        public List<LocalizableProperty> LocalizableProperty { get; set; }
    }

    [XmlRoot(ElementName = "ArrayOfCatalogObject")]
    public class ArrayOfCatalogObject
    {
        [XmlElement(ElementName = "CatalogObject")]
        public List<CatalogObject> CatalogObject { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
    }

}
