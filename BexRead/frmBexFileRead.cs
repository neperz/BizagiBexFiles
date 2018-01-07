using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace BexFileRead
{
    public partial class frmBexFileRead : Form
    {
        public ArrayOfCatalogObject DataCatalog { get; private set; }
        public ArrayOfIndexValue DataCatalogIndex { get; private set; }

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
            var Catalog__Objects = lista.Where(d => d.Name == "Catalog__Objects.xml").FirstOrDefault();
            var Catalog__Indexes = lista.Where(d => d.Name == "Catalog__Indexes.xml").FirstOrDefault();
            loadXml(Catalog__Indexes.Name, Catalog__Indexes.Path);
            loadXml(Catalog__Objects.Name, Catalog__Objects.Path);
            //  throw new NotImplementedException();
        }

        public void FilterByGuid(string guid)
        {
            List<CatalogObject> lista = this.DataCatalog.CatalogObject.Where(r => r.Id == guid).ToList();

            Filter(lista);
        }
        public void FilterByParent(string guid)
        {
            var toFind = string.Format("\"finalEntity\":\"{0}\"", guid);
            List<CatalogObject> lista = this.DataCatalog.CatalogObject.Where(r => r.Content.Contains(toFind)).ToList();

            Filter(lista);
        }
        public void FilterDisableds()
        {
            txtContent.Text = "";
            var toFind = "\"disable\":true";
            List<CatalogObject> lista = this.DataCatalog.CatalogObject.Where(r => r.Content.Contains(toFind)).ToList();
            foreach (var d in lista)
            {
                txtContent.Text += "'" + d.Id + "',";
            }
            Filter(lista);
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
            foreach (var cont in lista)
            {
                if (!checkBox1.Checked)
                    if (cont.Type != "EntityValue") continue;
                ListViewItem item1 = new ListViewItem();
                item1.Text = (cont.Id);
                item1.SubItems.Add(cont.Type);
                var ultPart = cont.Content.Split(':').Last().Replace("}}", "");
                item1.SubItems.Add(ultPart);
                item1.Tag = cont;
                listView1.Items.Add(item1);
            }
            listView1.Refresh();
        }

        public void  loadXml (string name, string path)
        {
            if (name== "Catalog__Objects.xml")
            {
                
                var xmlCO = File.ReadAllText(path);
                CatalogObject  catOb = new CatalogObject();
                XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfCatalogObject));
                StringReader rdr = new StringReader(xmlCO);
                ArrayOfCatalogObject resultingMessage = (ArrayOfCatalogObject)serializer.Deserialize(rdr);
                this.DataCatalog = resultingMessage;
                propertyGrid1.SelectedObjects = resultingMessage.CatalogObject.ToArray();

                Filter(resultingMessage.CatalogObject);
            }
            if (name== "Catalog__Indexes.xml")
            {
                var xmlCO = File.ReadAllText(path);
                ArrayOfIndexValue cIndex = new ArrayOfIndexValue();
                XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfIndexValue));
                StringReader rdr = new StringReader(xmlCO);
                cIndex = (ArrayOfIndexValue)serializer.Deserialize(rdr);
                this.DataCatalogIndex = cIndex;

            }
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

            }
            catch (Exception)
            {

              //  throw;
            }

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
                    var obj = this.DataCatalogIndex.IndexValueIn.Where(x => x.ObjectId == xname).FirstOrDefault();
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


  }

