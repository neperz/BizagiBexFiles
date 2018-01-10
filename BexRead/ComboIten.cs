namespace BexFileRead
{
    public class ComboIten
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public CatalogObject Catalogo { get; set; }
        public override string ToString() { return this.Name; }
    }
}