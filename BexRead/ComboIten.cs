using Bex.Domain;

namespace BexFileRead
{
    public class ComboIten
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public CatalogObject Catalogo { get; set; }
        public int Catalogos { get; internal set; }

        public override string ToString() { return this.Name; }
    }
}