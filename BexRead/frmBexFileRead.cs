﻿using Bex.Domain;
using ICSharpCode.SharpZipLib.Zip;
using Ookii.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
        private string strUsername;
        private string strPassword;
        private string strServer;
        private string strDataBase;

        public ArrayOfCatalogObject DataCatalog { get; private set; }
        public ArrayOfIndexValue DataCatalogIndex { get; private set; }
        public List<CatalogObject> Tabelas { get; private set; }
        internal List<TipoObjeto> Types { get; private set; }
        public NewDataSet BizagiInfo { get; private set; }
        public ArrayOfReference CatalogReferences { get; private set; }
        public AdvancedOptions AdvancedOptions { get; private set; }
        public NewDataSet BADPLYConfig { get; private set; }
        public PackageContentInformation PackageContentInformation { get; private set; }
        public string ActualId { get; private set; }

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
        public void FilterByAttributeByParent(string guid)
        {
            // var toFind = string.Format("\"finalEntity\":\"{0}\"", guid).ToLower();
            List<CatalogObject> lista = this.DataCatalog.CatalogObject.Where(r => r.ParentId.ToLower() == guid.ToLower()).ToList();

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
            //chID.Width
            listView1.Columns.Add(chID);
            ColumnHeader ch = new ColumnHeader();
            ch.Text = "Type";
            listView1.Columns.Add(ch);

            ColumnHeader CHname = new ColumnHeader();
            CHname.Text = "Nome";
            listView1.Columns.Add(CHname);


            // ColumnHeader che = new ColumnHeader();
            //  che.Text = "Content";
            //            listView1.Columns.Add(che);
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<string> cabecalhos = new List<string>();
            foreach (var cont in lista)
            {
                try
                {
                    if (cont.Type == "EntityValue")
                    {
                        Dictionary<string, object> dic = js.Deserialize<Dictionary<string, object>>(cont.Content);
                        cont.Dicionario = dic;

                        if (dic.ContainsKey("fields"))
                        {
                            var tup = dic["fields"];
                            Console.Write(tup);
                            foreach (var idc in (Dictionary<string, object>)tup)
                            {
                                var nome = GetObjectByGuid(idc.Key.ToLower()); if (!cabecalhos.Any(s => s == nome.Value)) cabecalhos.Add(nome.Value);
                            }
                        }
                    }
                }
                catch (Exception) { }
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
                item1.SubItems.Add(cont.Name);

                // var ultPart = cont.Content.Split(':').Last().Replace("}}", "");
                //  item1.SubItems.Add(ultPart);



                try
                {
                    if (cont.Dicionario != null)
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
                                    if (item.Index <= 2) continue;
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
                }
                catch (Exception)
                {

                    // throw;
                }


                item1.Tag = cont;
                listView1.Items.Add(item1);
            }
            listView1.Refresh();
            lblCont.Text = lista.Count().ToString();
        }

        public void loadXml(string name, string path)
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
                CatalogObject catOb = new CatalogObject();
                XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfCatalogObject));
                StringReader rdr = new StringReader(xmlCO);
                ArrayOfCatalogObject resultingMessage = (ArrayOfCatalogObject)serializer.Deserialize(rdr);
                this.DataCatalog = resultingMessage;
                propertyGrid1.SelectedObjects = resultingMessage.CatalogObject.ToArray();
                this.Tabelas = new List<CatalogObject>();
                this.Types = new List<TipoObjeto>();
                this.Types = FillTypes(resultingMessage.CatalogObject).OrderBy(d => d.Tipo).ToList();
                cmbType.Items.Clear();
                foreach (var tb in this.Types)
                {
                    cmbType.Items.Add(new ComboIten { Name = tb.Tipo, Value = tb.Tipo, Catalogos = tb.Objetos });
                }
                this.Tabelas = FillTabelas(resultingMessage.CatalogObject);

                //Filter(resultingMessage.CatalogObject);
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
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        private void SaveDataToSQL(string id, string data64)
        {
            var jsonText = new MemoryStream(Encoding.Unicode.GetBytes(data64));
            
            var data = jsonText.ToArray();
            string sConn = @"server=" + this.strServer + "; database=" + this.strDataBase + ";user id=" + this.strUsername + ";password=" + this.strPassword + ";";
            using (var conn = new SqlConnection(sConn))
            using (var cmd = new SqlCommand("UPDATE BABIZAGICATALOG SET objContent=@JasonData where guidObject = @Id", conn))
            {
                conn.Open();
                var param = new SqlParameter("@JasonData", SqlDbType.Binary)
                {
                    Value = data
                };
                var paramId = new SqlParameter("@Id", SqlDbType.UniqueIdentifier)
                {
                    Value =Guid.Parse( id)
                };
                cmd.Parameters.Add(param);
                cmd.Parameters.Add(paramId);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                    MessageBox.Show("Registro atualizado com sucesso!");
            }
        }
        public void loadSQL(string name, string path)
        {
            string sConn = @"server="+ this.strServer +"; database="+ this.strDataBase + ";user id="+ this.strUsername + ";password="+ this.strPassword +";";
            SqlConnection objConn = new SqlConnection(sConn);
            objConn.Open();

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
                //var xmlCO = File.ReadAllText(path);
                //XmlSerializer serializer = new XmlSerializer(typeof(PackageContentInformation));
                //StringReader rdr = new StringReader(xmlCO);
                //PackageContentInformation packageContentInformation = (PackageContentInformation)serializer.Deserialize(rdr);
                //this.PackageContentInformation = packageContentInformation;

                //propertyGrid1.SelectedObject = packageContentInformation;
                //txtContent.Text = xmlCO;
            }
            if (name.ToLower() == "relational.xml")
            {
                string sTSQL = "SELECT * from BADPLYCONFIG";
                SqlCommand objCmd = new SqlCommand(sTSQL, objConn);
                objCmd.CommandType = CommandType.Text;
                SqlDataReader dr = objCmd.ExecuteReader();
                dr.Read();
                var dFile = dr["baDplyConfigFile"];
                var m_oDefaultEncodingForXML = new UTF8Encoding(false, true);
                //var texto = Encoding.Unicode.GetString((byte[])dFile);
                var texto = Compression.DecompressString((byte[])dFile, m_oDefaultEncodingForXML);
                var textoFormatado = "";
                if (texto.StartsWith("<?xml") || texto.Contains("xmlns="))
                {
                    textoFormatado = texto;
                }
                else
                    textoFormatado = Newtonsoft.Json.Linq.JToken.Parse(texto).ToString();

                NewDataSet bADPLYCONFIG = new NewDataSet();
                bADPLYCONFIG.BADPLYCONFIG = new List<BADPLYCONFIG>();

                BADPLYCONFIG bi = new BADPLYCONFIG();

                bi.BaDplyConfigFile = textoFormatado;
                bi.DcFileVersion = ((byte)dr["dcFileVersion"]).ToString();
                bi.DcMapType = ((byte)dr["dcMapType"]).ToString();
                bi.DplyBADplyConfig = ((byte)dr["dplyBADplyConfig"]).ToString();
                bi.GuidBADplyConfig = ((System.Guid)dr["guidBADplyConfig"]).ToString();
                bi.IdBaDplyConfig = ((int)dr["idBaDplyConfig"]).ToString();

                bADPLYCONFIG.BADPLYCONFIG.Add(bi);

                this.BADPLYConfig = bADPLYCONFIG;
                var jdata = Newtonsoft.Json.JsonConvert.SerializeObject(bADPLYCONFIG);
                txtContent.Text = Newtonsoft.Json.Linq.JToken.Parse(jdata).ToString();
            }

            if (name.ToLower() == "options.xml")
            {
                //var xmlCO = File.ReadAllText(path);
                //XmlSerializer serializer = new XmlSerializer(typeof(AdvancedOptions));
                //StringReader rdr = new StringReader(xmlCO);
                //AdvancedOptions advancedOptions = (AdvancedOptions)serializer.Deserialize(rdr);
                //this.AdvancedOptions = advancedOptions;
                //var dic = new Dictionary<string, string>();
                //var option = advancedOptions.AdvancedOption.FirstOrDefault();
                //if (option != null)
                //    propertyGrid1.SelectedObject = option;
                //txtContent.Text = xmlCO;
            }
            if (name.ToLower() == "bizagiinfo.xml")
            {
                //select * from BIZAGIINFO
                string sTSQL = "SELECT * from BIZAGIINFO";
                SqlCommand objCmd = new SqlCommand(sTSQL, objConn);
                objCmd.CommandType = CommandType.Text;
                SqlDataReader dr = objCmd.ExecuteReader();
                NewDataSet bizagiInfo = new NewDataSet();
                bizagiInfo.BIZAGIINFO = new List<BIZAGIINFO>();
                while (dr.Read())
                {
                    BIZAGIINFO bi = new BIZAGIINFO
                    {
                        BAInfo = (string)dr["BAInfo"],
                        BAValue = (string)dr["BAValue"]
                    };
                    bizagiInfo.BIZAGIINFO.Add(bi);
                }
                this.BizagiInfo = bizagiInfo;
                var dic = new Dictionary<string, string>();
                foreach (var b in bizagiInfo.BIZAGIINFO)
                {
                    dic.Add(b.BAInfo, b.BAValue);
                }
                propertyGrid1.SelectedObject = new DictionaryPropertyGridAdapter(dic);
                txtContent.Text = "";
            }
            if (name.ToLower() == "catalog__references.xml")
            {
               // var xmlCO = File.ReadAllText(path);
                string sTSQL = "SELECT * from BACATALOGREFERENCE";
                SqlCommand objCmd = new SqlCommand(sTSQL, objConn);
                objCmd.CommandType = CommandType.Text;
                SqlDataReader dr = objCmd.ExecuteReader();

                ArrayOfReference catalogReferences = new ArrayOfReference();
                CatalogReferences.Reference = new List<Reference>();                
                while (dr.Read())
                {
                    Reference bi = new Reference();
                    bi.Deleted = ((byte)dr["Deleted"]).ToString();
                    bi.Pointer = ((System.Guid)dr["guidPointer"]).ToString();
                    bi.Referrer = ((System.Guid)dr["guidObjectRef"]).ToString();
                    bi.Root = ((System.Guid)dr["rootObject"]).ToString();
                    bi.Target = ((System.Guid)dr["guidObjectTarget"]).ToString();                   

                    CatalogReferences.Reference.Add(bi);
                }

                this.CatalogReferences = catalogReferences;

                FilterReferences(catalogReferences.Reference);

                //txtContent.Text = xmlCO;
            }
            if (name.ToLower() == "catalog__objects.xml")
            {
                //string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

                string sTSQL = "SELECT * from vwBA_Catalog_BABIZAGICATALOG";
                SqlCommand objCmd = new SqlCommand(sTSQL, objConn);
                objCmd.CommandType = CommandType.Text;
                SqlDataReader dr = objCmd.ExecuteReader();

              
                CatalogObject catOb = new CatalogObject();

                ArrayOfCatalogObject resultingMessage = new ArrayOfCatalogObject();
                resultingMessage.CatalogObject = new List<CatalogObject>();
                while (dr.Read())
                {
                    CatalogObject bi = new CatalogObject();
                    //bi.CatalogReferences = ((byte)dr["Deleted"]).ToString();
                    bi.ChangeSetId = dr["changeSetId"] == DBNull.Value ? "" : ((int)dr["changeSetId"]).ToString();
                    bi.Type = dr["objTypeName"] == DBNull.Value ? "" : ((string)dr["objTypeName"]);
                    var byCont = (byte[])dr["objContent"];
                    bi.Content = Convert.ToBase64String(byCont, 0, byCont.Length);
                    try
                    {
                        var texto = Encoding.Unicode.GetString(byCont);
                        var textoFormatado = "";
                        if (texto.StartsWith("<?xml"))
                        {
                            textoFormatado = texto;
                        }
                        else
                            textoFormatado = Newtonsoft.Json.Linq.JToken.Parse(texto).ToString();
                        bi.Content = textoFormatado;
                    }
                    catch (Exception)
                    {

                     //   throw;
                    }


                    bi.ParentId = dr["guidObjectParent"] == DBNull.Value ? "" : ((System.Guid)dr["guidObjectParent"]).ToString();
                    bi.DeployOnParent = dr["DeployOnParent"] == DBNull.Value ? "false" : ((bool)dr["DeployOnParent"]).ToString();// ((byte)dr["DeployOnParent"]).ToString();
                    bi.HasOverride = dr["isOverride"] == DBNull.Value ? "false" : ((string)dr["isOverride"]);
                    bi.ContentFormat = ((byte)dr["contentFormat"]).ToString();
                    bi.Deleted = dr["Deleted"] == DBNull.Value ? "false" : ((bool)dr["Deleted"]).ToString();
                    bi.DeployOnParent = dr["deployOnParent"] == DBNull.Value ? "false" : ((bool)dr["deployOnParent"]).ToString(); ;// ((byte)dr["deployOnParent"]).ToString();
                    bi.Dicionario = new Dictionary<string, object>();
                    bi.Id = ((System.Guid)dr["guidObject"]).ToString();
                    bi.Indexes = new Indexes();
                    bi.ModifiedByUser = (string)dr["modifiedByUser"];
                    bi.Name = (string)dr["objName"];
                    bi.ParentId = dr["guidObjectParent"] == DBNull.Value ? "":((System.Guid)dr["guidObjectParent"]).ToString();
                    bi.References = new References();
                    bi.Root = dr["rootObject"] == DBNull.Value ? "" : ((System.Guid)dr["rootObject"]).ToString();
                    bi.Tags = new Tags();
                    
                    
                    bi.Version= ((int)dr["mtdVersion"]).ToString();
                    resultingMessage.CatalogObject.Add(bi);
                }

                this.DataCatalog = resultingMessage;
                propertyGrid1.SelectedObjects = resultingMessage.CatalogObject.ToArray();
                this.Tabelas = new List<CatalogObject>();
                this.Types = new List<TipoObjeto>();
                this.Types = FillTypes(resultingMessage.CatalogObject).OrderBy(d => d.Tipo).ToList();
                cmbType.Items.Clear();
                foreach (var tb in this.Types)
                {
                    cmbType.Items.Add(new ComboIten { Name = tb.Tipo, Value = tb.Tipo, Catalogos = tb.Objetos });
                }
                this.Tabelas = FillTabelas(resultingMessage.CatalogObject);

                Filter(resultingMessage.CatalogObject);
            }
            if (name.ToLower() == "catalog__indexes.xml")
            {
                string sTSQL = "SELECT * from BATAGVALUE";
                SqlCommand objCmd = new SqlCommand(sTSQL, objConn);
                objCmd.CommandType = CommandType.Text;
                SqlDataReader dr = objCmd.ExecuteReader();

                ArrayOfIndexValue cIndex = new ArrayOfIndexValue();
                cIndex.IndexValueIn = new List<IndexValue>();
                while (dr.Read())
                {
                    IndexValue bi = new IndexValue();
                    bi.Deleted = dr["Deleted"] == DBNull.Value?"false": ((bool)dr["Deleted"]).ToString();
                    bi.IndexName = ((string)dr["tagName"]);
                    bi.ObjectId = ((System.Guid)dr["taggedObject"]).ToString();
                    bi.Root = ((System.Guid)dr["rootObject"]).ToString();
                    bi.TagType = ((System.Guid)dr["tagType"]).ToString();
                    bi.Value = (string)dr["value"];
                    cIndex.IndexValueIn.Add(bi);
                }

                this.DataCatalogIndex = cIndex;

            }
            objConn.Close();
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

        public CNamePathPair[] UnzipFileToDirectory(string sZipFileName, string sUnzipDirectory)
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
            if (name.isSQL)
                loadSQL(name.Name, name.Value);
            else
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
                this.ActualId = selectedOb.Id;
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
                            var name = obj.Value;
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
                // MessageBox.Show("JSON data is not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        finalNode.Tag = item;
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
                var xname = parentIdAr[2].Replace("\"", "").Replace("\",\"disable\"", "").Replace(",disable", "");
                if (this.DataCatalogIndex != null)
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
            //Process.Start("notepad.exe", fileName);
            System.Diagnostics.Process.Start("notepad.exe", e.Link.LinkData.ToString());
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
            var filterObject = this.DataCatalog.CatalogObject.Where(c => c.Type == obj.Value).ToList().OrderBy(g => g.Name).ToList();
            foreach (var tb in filterObject)
            {
                cmbTabelas.Items.Add(new ComboIten { Name = tb.Name, Value = tb.Id.ToLower(), Catalogo = tb });
            }
            Filter(filterObject);
        }

        private void frmBexFileRead_Load(object sender, EventArgs e)
        {

        }

        private void jsonExplorer_MouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            Console.WriteLine("Clicked: " + e.Node.Text);
        }

        private void jsonExplorer_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void jsonExplorer_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private string GetExtFromBase64(string base64)
        {
            var data = base64.Substring(0, 5);
            switch (data.ToUpper())
            {
                case "IVBOR":
                case "/9J/4":
                    return ".png";
                case "AAAAF":
                    return ".mp4";
                case "JVBER":
                    return ".pdf";
                case "UESDB":
                    return ".zip";
                case "TVQQA":
                    return ".dll";
                default:
                    return ".bin";
            }
        }

        private void jsonExplorer_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (jsonExplorer.SelectedNode.Parent.Text == "file")
            {
                KeyValuePair<string, object> item = (KeyValuePair<string, object>)jsonExplorer.SelectedNode.Tag;
                string ext = GetExtFromBase64((string)item.Value);
                var outdir = Path.GetTempPath() + "\\LeitorDeBex\\";
                var file = Path.GetTempFileName() + ext;
                //var fFile = outdir + file;
                File.WriteAllBytes(file, Convert.FromBase64String((string)item.Value));
                if (ext == ".dll")
                    MessageBox.Show("Dll salva em " + file);
                else
                {
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(file);
                    psi.UseShellExecute = true;
                    System.Diagnostics.Process.Start(psi);
                }
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel lnk = new LinkLabel();
            lnk = (LinkLabel)sender;
            lnk.Links[lnk.Links.IndexOf(e.Link)].Visited = true;
            var entidadeGuid = (ComboIten)cmbTabelas.SelectedItem;
            FilterByAttributeByParent(entidadeGuid.Value);
            //Filter()
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var sel = cmbTabelas.SelectedItem;
            var obj = (ComboIten)sel;
            txtGuid.Text = obj.Value;
            FilterByParent(obj.Value);
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            DocumentTemplate d = new DocumentTemplate();
            var selectedOb = (CatalogObject)listView1.SelectedItems[0].Tag;
            if (selectedOb.Type == "DocumentTemplate")
            {
                string cont = selectedOb.Content;
                XmlSerializer serializer = new XmlSerializer(typeof(DocumentTemplate));
                StringReader rdr = new StringReader(cont);
                d = (DocumentTemplate)serializer.Deserialize(rdr);
                var ext = d.Format;

                var file = Path.GetTempFileName() + "." + ext;
                File.WriteAllBytes(file, Convert.FromBase64String(d.Document));
                if (ext == ".dll")
                    MessageBox.Show("Dll salva em " + file);
                else
                {
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(file);
                    psi.UseShellExecute = true;
                    System.Diagnostics.Process.Start(psi);
                }
            }
            //   propertyGrid1.SelectedObject = selectedOb;
            // txtContent.Text = selectedOb.Content;

        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }
        private void loadSQLs(Dictionary<string, string> array)
        {
            var lista = array.ToList();
            var Catalog__Objects = lista.Where(d => d.Key.ToLower() == "catalog__objects.xml").FirstOrDefault();
            var Catalog__Indexes = lista.Where(d => d.Key.ToLower() == "catalog__indexes.xml").FirstOrDefault();
            loadSQL(Catalog__Indexes.Key, Catalog__Indexes.Value);
            loadSQL(Catalog__Objects.Key, Catalog__Objects.Value);
            //  throw new NotImplementedException();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //try
            //{


                CredentialDialog crDiag = new CredentialDialog();
                crDiag.Content = "Suas informações são necessárias para conectar ao banco SQL.";
                crDiag.MainInstruction = "Por favor, entre com o formato: servidor:banco@usuario e senha";
                crDiag.Target = "Leitor de Catálogo Bizagi";
                crDiag.UseApplicationInstanceCredentialCache = true;

                if (crDiag.ShowDialog() == DialogResult.OK)
                {
                    var udata = crDiag.UserName.Split('@');
                    var sData = udata[0].Split(':');
                    this.strUsername = udata[1].Trim();
                    this.strPassword = crDiag.Password;
                    this.strServer = sData[0].Trim();
                    this.strDataBase = sData[1].Trim();
                    Dictionary<string, string> filesxml = new Dictionary<string, string>();
                    filesxml.Add("BizagiInfo.xml", "");
                    filesxml.Add("Catalog__Indexes.xml", "");
                    filesxml.Add("Catalog__IndexObjects.xml", "");
                    filesxml.Add("Catalog__Localization.xml", "");
                    filesxml.Add("Catalog__Objects.xml", "");
                    filesxml.Add("Catalog__References.xml", "");
                    filesxml.Add("Options.xml", "");
                    filesxml.Add("PackageInfo.xml", "");
                    filesxml.Add("Relational.xml", "");

                    loadSQLs(filesxml);
                    propertyGrid1.SelectedObject = filesxml;
                    comboBox1.Items.Clear();
                    foreach (var i in filesxml)
                    {
                        comboBox1.Items.Add(new ComboIten { Name = i.Key, Value = i.Value , isSQL=true});
                    }
                }
            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.Message);
            //}
        }

        private void btnSalva_Click(object sender, EventArgs e)
        {
            var c64 = (txtContent.Text);
            var id = this.ActualId;
            SaveDataToSQL(id, c64);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var f = new Util.frmProtect();
            f.ShowDialog();
        }
    }





}

