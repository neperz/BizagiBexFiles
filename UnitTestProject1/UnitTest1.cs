using System;
using dnlib.DotNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {


        [TestMethod]
        public void DEVE_RETORNAR_MINUTOS()
        {
            var time = "01:30";
            var vTime = time.Split(':');

            var hora = int.Parse(vTime[0]);
            var hMinuto = hora * 60;
            var minuto = int.Parse( vTime[1]);
            var totalMinutos = hMinuto + minuto;


        }
        [System.Runtime.InteropServices.DllImport("Imagehlp.dll ")]
        private static extern bool ImageRemoveCertificate(IntPtr handle, int index);
        [TestMethod]
        public void TestMethod1()
        {
            ModuleDefMD module = ModuleDefMD.Load(@"C:\Bizagi\Projects\AuxiliarBPMS\WebApplication\bin\BizAgi.Commons.dll");

            AssemblyDef asm = module.Assembly;
            Console.WriteLine("Assembly: {0}", asm);
            module.Write(@"D:\Projetos\Bizagi\BizAgi.Commons.dll");


            UnsignFile(@"C:\Bizagi\Projects\AuxiliarBPMS\WebApplication\bin\BizAgi.Commons.dll");
        }

        private void UnsignFile(string file)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite))
            {
                var removed = ImageRemoveCertificate(fs.SafeFileHandle.DangerousGetHandle(), 0);
                fs.Close();
                Console.Write(removed);
            }
        }
    }
}
