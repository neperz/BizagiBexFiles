using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace BexFileRead
{
    public partial class frmBexFileRead : Form
    {
        public ArrayOfCatalogObject DataCatalog { get; private set; }
        public ArrayOfIndexValue DataCatalogIndex { get; private set; }
        public List<CatalogObject> Tabelas { get; private set; }
        internal List<TipoObjeto> Types { get; private set; }
        public NewDataSet BizagiInfo { get; private set; }
        public ArrayOfReference CatalogReferences { get; private set; }
        public AdvancedOptions AdvancedOptions { get; private set; }
        public NewDataSet BADPLYConfig { get; private set; }
        public PackageContentInformation PackageContentInformation { get; private set; }

        public frmBexFileRead()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Bex File";
            theDialog.Filter = "BEX files|*.bex";
            //theDialog.InitialDirectory = @"C:\";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                
                if (File.Exists(theDialog.FileName))
                {
                    var outdir = Path.GetTempPath() + "\\LeitorDeBex\\";
                    var array = UnzipFileToDirectory(theDialog.FileName, outdir);
                    LoadXmls(array);
                    propertyGrid1.SelectedObject = array;
                    comboBox1.Items.Clear();
                    foreach (var i in array)
                    {
                        comboBox1.Items.Add(new ComboIten { Name = i.Name, Value = i.Path });
                    }
                }
                else
                {
                    MessageBox.Show("Cadê o arquivo??");
                }
            }

         //   var sBackup = ReadBackupFromFile(txtBexFile.Text);
            //propertyGrid1.SelectedObject = sBackup;

        }

        private void LoadXmls(CNamePathPair[] array)
        {
            var lista = array.ToList();
            var Catalog__Objects = lista.Where(d => d.Name.ToLower() == "catalog__objects.xml").FirstOrDefault();
            var Catalog__Indexes = lista.Where(d => d.Name.ToLower() == "catalog__indexes.xml").FirstOrDefault();
            loadXml(Catalog__Indexes.Name, Catalog__Indexes.Path);
            loadXml(Catalog__Objects.Name, Catalog__Objects.Path);
            //  throw new NotImplementedException();
        }

        public void FilterByGuid(string guid)
        {
            List<CatalogObject> lista = this.DataCatalog.CatalogObject.Where(r => r.Id.ToLower() == guid.ToLower()).ToList();

            Filter(lista);
        }
        public void FilterByParent(string guid)
        {
            var toFind = string.Format("\"finalEntity\":\"{0}\"", guid).ToLower();
            List<CatalogObject> lista = this.DataCatalog.CatalogObject.Where(r => r.Content.ToLower().Contains(toFind)).ToList();

            Filter(lista);
        }
        public void FilterDisableds()
        {
            txtContent.Text = "";
            var toFind = "\"disable\":true";
            List<CatalogObject> lista = this.DataCatalog.CatalogObject.Where(r => r.Content.ToLower().Contains(toFind.ToLower())).ToList();
            foreach (var d in lista)
            {
                txtContent.Text += "'" + d.Id + "',";
            }
            Filter(lista);
        }

        private void FilterReferences(List<Reference> lista)
        {
            /*

    <Pointer>51e6530f-6860-4cb1-9a1d-97f34c0d5a2f</Pointer>
    <Referrer>d17202b8-b0ab-44c5-8404-004c35b9ae1c</Referrer>
    <Target>0634f672-a510-49ca-bff4-f608f3c40d2a</Target>
    <Root>4ac1e130-ed13-411f-90d0-1577fc5d3eb0</Root>
    <Deleted>false</Deleted>             
             */
            listView1.Columns.Clear();
            listView1.Items.Clear();
            ColumnHeader chID = new ColumnHeader();
            chID.Text = "Pointer";
            listView1.Columns.Add(chID);
            ColumnHeader ch = new ColumnHeader();
            ch.Text = "Referrer";
            listView1.Columns.Add(ch);
            ColumnHeader che = new ColumnHeader();
            che.Text = "Target";
            listView1.Columns.Add(che);

            ColumnHeader cheRoot = new ColumnHeader();
            cheRoot.Text = "Root";
            listView1.Columns.Add(cheRoot);

            ColumnHeader cheDeleted = new ColumnHeader();
            cheDeleted.Text = "Deleted";
            listView1.Columns.Add(cheDeleted);
            foreach (var cont in lista)
            {
                //if (!checkBox1.Checked)
                //    if (cont.Type != "EntityValue") continue;
                ListViewItem item1 = new ListViewItem();
                item1.Text = GetObjectByGuid(cont.Pointer).IndexName;
                item1.SubItems.Add(GetObjectByGuid(cont.Referrer).IndexName);
                item1.SubItems.Add(GetObjectByGuid(cont.Target).IndexName);
                item1.SubItems.Add(GetObjectByGuid(cont.Root).IndexName);
                item1.SubItems.Add(cont.Deleted);
                            
                item1.Tag = cont;
                listView1.Items.Add(item1);
            }
            listView1.Refresh();
        }

        private void Filter(List<CatalogObject> lista)
        {
            listView1.Columns.Clear();
            listView1.Items.Clear();
            ColumnHeader chID = new ColumnHeader();
            chID.Text = "ID";
            listView1.Columns.Add(chID);
            ColumnHeader ch = new ColumnHeader();
            ch.Text = "Type";
            listView1.Columns.Add(ch);
            ColumnHeader che = new ColumnHeader();
            che.Text = "Content";
            listView1.Columns.Add(che);
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<string> cabecalhos = new List<string>();
            foreach (var cont in lista)
            {
                try
                {
                    Dictionary<string, object> dic = js.Deserialize<Dictionary<string, object>>(cont.Content);
                    cont.Dicionario = dic;
                    if (dic.ContainsKey("fields"))
                    {
                        var tup = dic["fields"];
                        Console.Write(tup);
                        foreach (var idc in (Dictionary<string, object>)tup)
                        {
                            var nome = GetObjectByGuid(idc.Key.ToLower());  if (!cabecalhos.Any(s => s == nome.Value))  cabecalhos.Add(nome.Value);                          
                        }
                    }
                }
                catch (Exception)  { }
            }
            foreach (var h in cabecalhos)
            {
                ColumnHeader hin = new ColumnHeader();
                hin.Text = h;
                listView1.Columns.Add(hin);
            }
                foreach (var cont in lista)
            {
                //if (!checkBox1.Checked)
                //    if (cont.Type != "EntityValue") continue;
                ListViewItem item1 = new ListViewItem();
                item1.Text = (cont.Id);
                item1.SubItems.Add(cont.Type);
                 
                   // var ultPart = cont.Content.Split(':').Last().Replace("}}", "");
                  //  item1.SubItems.Add(ultPart);
                
                  

                try
                {
                    Dictionary<string, object> dic = cont.Dicionario;// js.Deserialize<Dictionary<string, object>>(cont.Content);
                    if (dic.ContainsKey("fields"))
                    {
                        var tup = dic["fields"];
                        Console.Write(tup);
                        foreach (var idc in (Dictionary<string, object>)tup)
                        {
                            foreach (ColumnHeader item in listView1.Columns)
                            {
                                if (item.Index <= 1) continue;
                                var nome = GetObjectByGuid(idc.Key.ToLower());
                                if (item.Text == nome.Value)
                                {
                                    item1.SubItems.Add(idc.Value.ToString());
                                  //  break;
                                }
                                //else
                                    //item1.SubItems.Add("");
                                //if (!cabecalhos.Any(s => s == nome.Value))
                                //    cabecalhos.Add(nome.Value);
                                //linha += (nome.Value + ":" + idc.Value + "; ");
                            }
                        }

                    }
                }
                catch (Exception)
                {

                   // throw;
                }

             
                item1.Tag = cont;
                listView1.Items.Add(item1);
            }
            listView1.Refresh();
        }

        public void  loadXml (string name, string path)
        {
            ///BizagiInfo.xml
            ///Catalog__Indexes.xml
            ///Catalog__IndexObjects.xml
            ///Catalog__Localization.xml
            ///Catalog__Objects.xml
            ///Catalog__References.xml
            ///Options.xml
            ///PackageInfo.xml
            ///Relational.xml
            ///
            if (name.ToLower() == "packageinfo.xml")
            {
                var xmlCO = File.ReadAllText(path);
                XmlSerializer serializer = new XmlSerializer(typeof(PackageContentInformation));
                StringReader rdr = new StringReader(xmlCO);
                PackageContentInformation packageContentInformation = (PackageContentInformation)serializer.Deserialize(rdr);
                this.PackageContentInformation = packageContentInformation;
              
                    propertyGrid1.SelectedObject = packageContentInformation;
                txtContent.Text = xmlCO;
            }
            if (name.ToLower() == "relational.xml")
            {
                var xmlCO = File.ReadAllText(path);
                XmlSerializer serializer = new XmlSerializer(typeof(NewDataSet));
                StringReader rdr = new StringReader(xmlCO);
                NewDataSet bADPLYCONFIG = (NewDataSet)serializer.Deserialize(rdr);
                this.BADPLYConfig = bADPLYCONFIG;
                //var dic = new Dictionary<string, string>();
                //foreach (var b in bADPLYCONFIG.BADPLYCONFIG)
                //{
                //    dic.Add(b., b.BAValue);
                //}
               // propertyGrid1.SelectedObject = new DictionaryPropertyGridAdapter(dic);
                txtContent.Text = xmlCO;
            }

            if (name.ToLower() == "options.xml")
            {
                var xmlCO = File.ReadAllText(path);
                XmlSerializer serializer = new XmlSerializer(typeof(AdvancedOptions));
                StringReader rdr = new StringReader(xmlCO);
                AdvancedOptions advancedOptions = (AdvancedOptions)serializer.Deserialize(rdr);
                this.AdvancedOptions = advancedOptions;
                var dic = new Dictionary<string, string>();
                var option = advancedOptions.AdvancedOption.FirstOrDefault();
                if (option != null)
                    propertyGrid1.SelectedObject = option;
                txtContent.Text = xmlCO;
            }
            if (name.ToLower() == "bizagiinfo.xml")
            {
                var xmlCO = File.ReadAllText(path);                
                XmlSerializer serializer = new XmlSerializer(typeof(NewDataSet));
                StringReader rdr = new StringReader(xmlCO);
                NewDataSet bizagiInfo = (NewDataSet)serializer.Deserialize(rdr);
                this.BizagiInfo = bizagiInfo;
                var dic = new Dictionary<string, string>();
                foreach (var b in bizagiInfo.BIZAGIINFO)
                {
                    dic.Add(b.BAInfo, b.BAValue);
                }
                propertyGrid1.SelectedObject = new DictionaryPropertyGridAdapter(dic);
                txtContent.Text = xmlCO;
            }
            if (name.ToLower() == "catalog__references.xml")
            {
                var xmlCO = File.ReadAllText(path);
                XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfReference));
                StringReader rdr = new StringReader(xmlCO);
                ArrayOfReference catalogReferences = (ArrayOfReference)serializer.Deserialize(rdr);
                this.CatalogReferences = catalogReferences;

                FilterReferences(catalogReferences.Reference);
                
                txtContent.Text = xmlCO;
            }
            if (name.ToLower() == "catalog__objects.xml")
            {                
                var xmlCO = File.ReadAllText(path);
                CatalogObject  catOb = new CatalogObject();
                XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfCatalogObject));
                StringReader rdr = new StringReader(xmlCO);
                ArrayOfCatalogObject resultingMessage = (ArrayOfCatalogObject)serializer.Deserialize(rdr);
                this.DataCatalog = resultingMessage;
                propertyGrid1.SelectedObjects = resultingMessage.CatalogObject.ToArray();
                this.Tabelas = new List<CatalogObject>();
                this.Types = new List<TipoObjeto>();
                this.Types = FillTypes(resultingMessage.CatalogObject).OrderBy(d=>d.Tipo).ToList();
                cmbType.Items.Clear();
                foreach (var tb in this.Types)
                {
                    cmbType.Items.Add(new ComboIten { Name = tb.Tipo, Value = tb.Tipo,   Catalogos= tb.Objetos });
                }
                this.Tabelas = FillTabelas(resultingMessage.CatalogObject);
               
                Filter(resultingMessage.CatalogObject);
            }
            if (name.ToLower() == "catalog__indexes.xml")
            {
                var xmlCO = File.ReadAllText(path);
                ArrayOfIndexValue cIndex = new ArrayOfIndexValue();
                XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfIndexValue));
                StringReader rdr = new StringReader(xmlCO);
                cIndex = (ArrayOfIndexValue)serializer.Deserialize(rdr);
                this.DataCatalogIndex = cIndex;

            }

        }

        private List<TipoObjeto> FillTypes(List<CatalogObject> catalogObject)
        {
            var results = from p in catalogObject
                          group p.Type by p.Type into g
                          select new TipoObjeto { Tipo = g.Key, Objetos = g.Count() };
            return results.ToList();
        }

        private List<CatalogObject> FillTabelas(List<CatalogObject> catalogObject)
        {
            List<CatalogObject> tabelas = catalogObject.Where(t => t.Type == "Entity").ToList();
            return tabelas;
        }

        public  CNamePathPair[] UnzipFileToDirectory(string sZipFileName, string sUnzipDirectory)
        {
            CNamePathPairList cNamePathPairList = new CNamePathPairList();
            using (ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(sZipFileName)))
            {
                while (true)
                {
                    ZipEntry nextEntry = zipInputStream.GetNextEntry();
                    ZipEntry zipEntry = nextEntry;
                    if (nextEntry == null)
                    {
                        break;
                    }
                    string str = string.Concat(sUnzipDirectory, zipEntry.Name);
                    string fileName = Path.GetFileName(zipEntry.Name);

                    cNamePathPairList.Add(new CNamePathPair(zipEntry.Name, str));
                    Compression.EnsureDirectory(Path.GetDirectoryName(str));
                    if (fileName != string.Empty)
                    {
                        using (FileStream fileStream = File.Create(str))
                        {
                            int num = 2048;
                            byte[] numArray = new byte[2048];
                            while (true)
                            {
                                num = zipInputStream.Read(numArray, 0, (int)numArray.Length);
                                if (num <= 0)
                                {
                                    break;
                                }
                                fileStream.Write(numArray, 0, num);
                            }
                        }
                    }
                }
            }
            CNamePathPair[] array = cNamePathPairList.ToArray();
            cNamePathPairList.Clear();
           
            return array;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sel = comboBox1.SelectedItem;
            var name = (ComboIten)sel;
            loadXml(name.Name, name.Value);
            linkLabel1.Links.Clear();
            linkLabel1.Text = name.Name;
            linkLabel1.Links.Add(0, name.Name.Length, name.Value);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selectedOb = (CatalogObject)listView1.SelectedItems[0].Tag;
                propertyGrid1.SelectedObject = selectedOb;
                txtContent.Text = selectedOb.Content;
                string ObjectName = GetObjectName(selectedOb.Content);
                lblObject.Text = ObjectName;
                var toDeSerializy = selectedOb.Content;
                var guids = FindAllGuids(toDeSerializy);
                foreach (var g in guids)
                {
                    toDeSerializy = toDeSerializy.Replace(g, g.ToLower());
                }
                guids = FindAllGuids(toDeSerializy);
                if (this.DataCatalogIndex != null)
                {
                    foreach (var g in guids)
                    {
                        var obj = this.DataCatalogIndex.IndexValueIn.Where(x => x.ObjectId.ToLower() == g.ToLower()).FirstOrDefault();
                        if (obj != null)
                        {
                         var   name = obj.Value;
                            toDeSerializy = toDeSerializy.Replace(g, name);
                        }
                    }
                }
                Deserialize(toDeSerializy);
                jsonExplorer.ExpandAll();
            }
            catch (Exception)
            {

              //  throw;
            }

        }

        private void Deserialize(string json)
        {
            jsonExplorer.Nodes.Clear();
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Dictionary<string, object> dic = js.Deserialize<Dictionary<string, object>>(json);

                TreeNode rootNode = new TreeNode("Root");
                jsonExplorer.Nodes.Add(rootNode);
                BuildTree(dic, rootNode);
            }
            catch (ArgumentException argE)
            {
                MessageBox.Show("JSON data is not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void BuildTree(Dictionary<string, object> dictionary, TreeNode node)
        {
            foreach (KeyValuePair<string, object> item in dictionary)
            {
                TreeNode parentNode = new TreeNode(item.Key);
                node.Nodes.Add(parentNode);

                try
                {
                    dictionary = (Dictionary<string, object>)item.Value;
                    BuildTree(dictionary, parentNode);
                }
                catch (InvalidCastException dicE)
                {
                    try
                    {
                        ArrayList list = (ArrayList)item.Value;
                        foreach (string value in list)
                        {
                            TreeNode finalNode = new TreeNode(value);
                            finalNode.ForeColor = Color.Blue;
                            parentNode.Nodes.Add(finalNode);
                        }

                    }
                    catch (InvalidCastException ex)
                    {
                        TreeNode finalNode = new TreeNode(item.Value.ToString());
                        finalNode.ForeColor = Color.Blue;
                        parentNode.Nodes.Add(finalNode);
                    }
                }
            }
        }

        public IndexValue GetObjectByGuid(string id)
        {
            IndexValue tb = new IndexValue();
            if (this.DataCatalogIndex != null)
            {
                var obj = this.DataCatalogIndex.IndexValueIn.Where(x => x.ObjectId == id).FirstOrDefault();
                if (obj != null)
                {
                    tb = obj;
                }
                else
                    tb.IndexName = id;
            }
            return tb;
        }

        public IndexValue GetTabela(string content)
        {
            IndexValue tb = new IndexValue();
            if (content.Contains("finalEntity"))
            {
                var parentIdAr = content.Split(':');
                var xname = parentIdAr[2].Replace("\"", "").Replace("\",\"disable\"", "").Replace(",disable", "");
                if (this.DataCatalogIndex != null)
                {
                    var obj = this.DataCatalogIndex.IndexValueIn.Where(x => x.ObjectId == xname).FirstOrDefault();
                    if (obj != null)
                    {
                        tb = obj;                      
                    }
                }
            }
            return tb;
        }

        private string GetObjectName(string content)
        {
            var name = "";
            if (content.Contains("finalEntity"))
            {
                var parentIdAr = content.Split(':');
                var xname = parentIdAr[2].Replace("\"","").Replace("\",\"disable\"","").Replace(",disable","");
                if (this.DataCatalogIndex!=null)
                {
                    var obj = this.DataCatalogIndex.IndexValueIn.Where(x => x.ObjectId.ToLower() == xname.ToLower()).FirstOrDefault();
                    if (obj != null)
                        name = obj.Value;
                }

            }
            return name;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FilterByGuid(txtGuid.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FilterByParent(txtGuid.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FilterDisableds();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel lnk = new LinkLabel();
            lnk = (LinkLabel)sender;
            lnk.Links[lnk.Links.IndexOf(e.Link)].Visited = true;
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void jsonExplorer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode.ToString() == "C")
            {
                if (jsonExplorer.SelectedNode != null)
                {
                    e.Handled = true;
                    this.KeyPreview = true;

                    //copy node label to clipboard
                    Clipboard.SetText(jsonExplorer.SelectedNode.Text);
                }
            }
        }

        public List<String> FindAllGuids(string texto)
        {
            List<String> lista = new List<string>();
            var reg = "[a-fA-F0-9]{8}-([a-fA-F0-9]{4}-){3}[a-fA-F0-9]{12}";
            foreach (Match m in Regex.Matches(texto, reg))
            {
                lista.Add(m.Value);
            }
            return lista;
        }

        private void cmbTabelas_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sel = cmbTabelas.SelectedItem;
            var obj = (ComboIten)sel;
            txtGuid.Text = obj.Value;
            FilterByParent(obj.Value);
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sel = cmbType.SelectedItem;
            var obj = (ComboIten)sel;
            cmbTabelas.Items.Clear();
            var filterObject = this.DataCatalog.CatalogObject.Where(c => c.Type == obj.Value).ToList();
            foreach (var tb in filterObject)
            {
                cmbTabelas.Items.Add(new ComboIten { Name = tb.Name, Value = tb.Id.ToLower(), Catalogo = tb });
            }
            Filter(filterObject);
        }
    }




    public class CNamePathPair
    {
        public string Name;

        public string Path;

        public CNamePathPair()
        {
        }

        public CNamePathPair(string sName, string sPath)
        {
            this.Name = sName;
            this.Path = sPath;
        }
    }


    public class CNamePathPairList : List<CNamePathPair>
    {
        public CNamePathPairList()
        {
        }

        public CNamePathPairList(IEnumerable<CNamePathPair> osItems) : base(osItems)
        {
        }
    }

    class DictionaryPropertyGridAdapter : ICustomTypeDescriptor
    {
        IDictionary _dictionary;

        public DictionaryPropertyGridAdapter(IDictionary d)
        {
            _dictionary = d;
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return _dictionary;
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        PropertyDescriptorCollection
            System.ComponentModel.ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(new Attribute[0]);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            ArrayList properties = new ArrayList();
            foreach (DictionaryEntry e in _dictionary)
            {
                properties.Add(new DictionaryPropertyDescriptor(_dictionary, e.Key));
            }

            PropertyDescriptor[] props =
                (PropertyDescriptor[])properties.ToArray(typeof(PropertyDescriptor));

            return new PropertyDescriptorCollection(props);
        }
    }

    class DictionaryPropertyDescriptor : PropertyDescriptor
    {
        IDictionary _dictionary;
        object _key;

        internal DictionaryPropertyDescriptor(IDictionary d, object key)
            : base(key.ToString(), null)
        {
            _dictionary = d;
            _key = key;
        }

        public override Type PropertyType
        {
            get { return _dictionary[_key].GetType(); }
        }

        public override void SetValue(object component, object value)
        {
            _dictionary[_key] = value;
        }

        public override object GetValue(object component)
        {
            return _dictionary[_key];
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type ComponentType
        {
            get { return null; }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }

}

