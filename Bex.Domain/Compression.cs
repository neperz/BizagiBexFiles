
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bex.Domain
{

    public static class Compression
    {
        public static byte[] CompressBinaryBuffers(Compression.CNameBufferValue[] apFileBuffers)
        {
            return Compression.CompressBinaryBuffers(apFileBuffers, null);
        }

        public static byte[] CompressBinaryBuffers(Compression.CNameBufferValue[] apFileBuffers, string password)
        {
            MemoryStream memoryStream = new MemoryStream();
            Compression.SaveZipStreamFromBuffer(memoryStream, apFileBuffers, password);
            return memoryStream.ToArray();
        }

        public static void CompressFile(string sZipFileName, string sFileNameBuffer)
        {
            Compression.CompressFileList(sZipFileName, new string[] { sFileNameBuffer });
        }

        public static void CompressFileList(string sZipFileName, string[] asFileNameBuffer)
        {
            CNamePathPair[] cNamePathPairArray = new CNamePathPair[(int)asFileNameBuffer.Length];
            for (int i = 0; i < (int)asFileNameBuffer.Length; i++)
            {
                CNamePathPair cNamePathPair = new CNamePathPair(i.ToString(), asFileNameBuffer[i]);
                cNamePathPairArray[i] = cNamePathPair;
            }
            Compression.SaveZipFile(sZipFileName, cNamePathPairArray);
        }

        public static byte[] CompressString(string str, Encoding encoding, string password)
        {
            return Compression.CompressStringArray(new string[] { str }, encoding, password);
        }

        public static byte[] CompressString(string aStr, Encoding aEncoding)
        {
            return Compression.CompressStringArray(new string[] { aStr }, aEncoding);
        }

        public static byte[] CompressStringArray(string[] asArray, Encoding aEncoding)
        {
            return Compression.CompressStringArray(asArray, aEncoding, null);
        }

        public static byte[] CompressStringArray(string[] asArray, Encoding aEncoding, string password)
        {
            Compression.CNameStringValue[] cNameStringValueArray = new Compression.CNameStringValue[(int)asArray.Length];
            for (int i = 0; i < (int)asArray.Length; i++)
            {
                Compression.CNameStringValue cNameStringValue = new Compression.CNameStringValue(i.ToString(), asArray[i]);
                cNameStringValueArray[i] = cNameStringValue;
            }
            return Compression.CompressStringBuffers(cNameStringValueArray, aEncoding, password);
        }

        public static byte[] CompressStringBuffers(Compression.CNameStringValue[] apStrings, Encoding aEncoding)
        {
            return Compression.CompressStringBuffers(apStrings, aEncoding, null);
        }

        public static byte[] CompressStringBuffers(Compression.CNameStringValue[] apStrings, Encoding aEncoding, string password)
        {
            Compression.CNameBufferValue[] cNameBufferValueArray = new Compression.CNameBufferValue[(int)apStrings.Length];
            for (int i = 0; i < (int)apStrings.Length; i++)
            {
                Compression.CNameBufferValue cNameBufferValue = new Compression.CNameBufferValue(apStrings[i].Name, aEncoding.GetBytes(apStrings[i].StringValue));
                cNameBufferValueArray[i] = cNameBufferValue;
            }
            return Compression.CompressBinaryBuffers(cNameBufferValueArray, password);
        }

        public static byte[] CompressStringBuffers(StringDictionaryByName apStrings, Encoding aEncoding)
        {
            int num = 0;
            Compression.CNameBufferValue[] cNameBufferValueArray = new Compression.CNameBufferValue[apStrings.Count];
            foreach (string key in apStrings.Keys)
            {
                Compression.CNameBufferValue cNameBufferValue = new Compression.CNameBufferValue(key, aEncoding.GetBytes(apStrings[key]));
                cNameBufferValueArray[num] = cNameBufferValue;
                num++;
            }
            return Compression.CompressBinaryBuffers(cNameBufferValueArray);
        }

        public static Compression.CNameBufferValue[] DecompressBinaryBuffers(byte[] aInputZipBuffer)
        {
            return Compression.ReadZipStreamToBuffer(new MemoryStream(aInputZipBuffer));
        }

        public static string DecompressFile(string sZipFileName, string sBufferDir)
        {
            return Compression.DecompressFileList(sZipFileName, sBufferDir)[0];
        }

        public static string[] DecompressFileList(string sZipFileName, string sBufferDir)
        {
            CNamePathPair[] directory = Compression.UnzipFileToDirectory(sZipFileName, sBufferDir);
            string[] strArrays = new string[(int)directory.Length];
            for (int i = 0; i < (int)directory.Length; i++)
            {
                strArrays[Convert.ToInt32(directory[i].Name)] = directory[i].Path;
            }
            return strArrays;
        }

        public static string DecompressString(byte[] aInputZipBuffer, Encoding aEncoding)
        {
            return Compression.DecompressStringArray(aInputZipBuffer, aEncoding)[0];
        }

        public static string[] DecompressStringArray(byte[] aInputZipBuffer, Encoding aEncoding)
        {
            Compression.CNameStringValue[] cNameStringValueArray = Compression.DecompressStringBuffers(aInputZipBuffer, aEncoding);
            string[] strArrays = new string[(int)cNameStringValueArray.Length];
            for (int i = 0; i < (int)cNameStringValueArray.Length; i++)
            {
                strArrays[Convert.ToInt32(cNameStringValueArray[i].Name)] = cNameStringValueArray[i].StringValue;
            }
            return strArrays;
        }

        public static Compression.CNameStringValue[] DecompressStringBuffers(byte[] aInputZipBuffer, Encoding aEncoding)
        {
            Compression.CNameBufferValue[] cNameBufferValueArray = Compression.DecompressBinaryBuffers(aInputZipBuffer);
            Compression.CNameStringValue[] cNameStringValueArray = new Compression.CNameStringValue[(int)cNameBufferValueArray.Length];
            for (int i = 0; i < (int)cNameBufferValueArray.Length; i++)
            {
                Compression.CNameStringValue cNameStringValue = new Compression.CNameStringValue(cNameBufferValueArray[i].Name, aEncoding.GetString(cNameBufferValueArray[i].BufferValue));
                cNameStringValueArray[i] = cNameStringValue;
            }
            return cNameStringValueArray;
        }

        public static void DeleteDirectoryFiles(string sDirectory, string sSearchPattern)
        {
            if (Directory.Exists(sDirectory))
            {
                string[] files = Directory.GetFiles(sDirectory, sSearchPattern);
                for (int i = 0; i < (int)files.Length; i++)
                {
                    string str = files[i];
                    try
                    {
                        File.Delete(str);
                    }
                    catch
                    {
                    }
                }
            }
        }

        public static void EnsureDirectory(string sDirectory)
        {
            if (!Directory.Exists(sDirectory))
            {
                Directory.CreateDirectory(sDirectory);
            }
        }

        public static byte[] ReadFileToByteBuffer(string aFileName)
        {
            FileStream fileStream = new FileStream(aFileName, FileMode.Open);
            int num = Convert.ToInt32(fileStream.Length);
            byte[] numArray = new byte[num];
            fileStream.Read(numArray, 0, num);
            fileStream.Close();
            return numArray;
        }

        public static string ReadFileToString(string aFileName, Encoding oEncoding)
        {
            string end = null;
            FileStream fileStream = null;
            StreamReader streamReader = null;
            try
            {
                try
                {
                    fileStream = new FileStream(aFileName, FileMode.Open);
                    streamReader = new StreamReader(fileStream, oEncoding);
                    end = streamReader.ReadToEnd();
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    throw new Exception(string.Concat("Error while read the file '", aFileName, "'. ", exception.Message), exception);
                }
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
                streamReader = null;
                if (fileStream != null)
                {
                    fileStream.Close();
                }
                fileStream = null;
            }
            return end;
        }

        public static string ReadFileToString(string aFileName)
        {
            return Compression.ReadFileToString(aFileName, Encoding.Default);
        }

        public static StringDictionaryByName ReadZipBufferToStringDict(byte[] aoBuffer, Encoding aEncoding)
        {
            Compression.CNameBufferValue[] cNameBufferValueArray = Compression.DecompressBinaryBuffers(aoBuffer);
            StringDictionaryByName stringDictionaryByName = new StringDictionaryByName();
            Compression.CNameBufferValue[] cNameBufferValueArray1 = cNameBufferValueArray;
            for (int i = 0; i < (int)cNameBufferValueArray1.Length; i++)
            {
                Compression.CNameBufferValue cNameBufferValue = cNameBufferValueArray1[i];
                stringDictionaryByName.Add(cNameBufferValue.Name, aEncoding.GetString(cNameBufferValue.BufferValue));
            }
            return stringDictionaryByName;
        }

        public static Compression.CNameBufferValue[] ReadZipFileToBuffer(string sFileName)
        {
            return Compression.ReadZipStreamToBuffer(File.OpenRead(sFileName));
        }

        public static StringDictionaryByName ReadZipFileToStringDict(string sFileName, Encoding aEncoding)
        {
            Compression.CNameBufferValue[] buffer = Compression.ReadZipStreamToBuffer(File.OpenRead(sFileName));
            StringDictionaryByName stringDictionaryByName = new StringDictionaryByName();
            Compression.CNameBufferValue[] cNameBufferValueArray = buffer;
            for (int i = 0; i < (int)cNameBufferValueArray.Length; i++)
            {
                Compression.CNameBufferValue cNameBufferValue = cNameBufferValueArray[i];
                stringDictionaryByName.Add(cNameBufferValue.Name, aEncoding.GetString(cNameBufferValue.BufferValue));
            }
            return stringDictionaryByName;
        }

        public static Compression.CNameBufferValue[] ReadZipStreamToBuffer(Stream aInputStream)
        {
            MemoryStream memoryStream;
            ZipInputStream zipInputStream = new ZipInputStream(aInputStream);
            List<Compression.CNameBufferValue> cNameBufferValues = new List<Compression.CNameBufferValue>();
            while (true)
            {
                ZipEntry nextEntry = zipInputStream.GetNextEntry();
                ZipEntry zipEntry = nextEntry;
                if (nextEntry == null)
                {
                    break;
                }
                if (Path.GetFileName(zipEntry.Name).Length != 0)
                {
                    memoryStream = (zipEntry.Size <= (long)0 ? new MemoryStream() : new MemoryStream(Convert.ToInt32(zipEntry.Size)));
                    int num = 2048;
                    byte[] numArray = new byte[2048];
                    while (true)
                    {
                        num = zipInputStream.Read(numArray, 0, (int)numArray.Length);
                        if (num <= 0)
                        {
                            break;
                        }
                        memoryStream.Write(numArray, 0, num);
                    }
                    memoryStream.Close();
                    cNameBufferValues.Add(new Compression.CNameBufferValue(zipEntry.Name, memoryStream.ToArray()));
                }
            }
            zipInputStream.Close();
            return cNameBufferValues.ToArray();
        }

        public static void SaveZipFile(string sFileName, string sDirPathToCompress)
        {
            string[] files = Directory.GetFiles(sDirPathToCompress);
            CNamePathPair[] cNamePathPair = new CNamePathPair[(int)files.Length];
            for (int i = 0; i < (int)files.Length; i++)
            {
                cNamePathPair[i] = new CNamePathPair(Path.GetFileName(files[i]), files[i]);
            }
            Compression.SaveZipFile(sFileName, cNamePathPair);
        }

        public static void SaveZipFile(string sZipFileName, params CNamePathPair[] apFileNames)
        {
            int num;
            using (ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(sZipFileName)))
            {
                zipOutputStream.SetLevel(9);
                byte[] numArray = new byte[4096];
                CNamePathPair[] cNamePathPairArray = apFileNames;
                for (int i = 0; i < (int)cNamePathPairArray.Length; i++)
                {
                    CNamePathPair cNamePathPair = cNamePathPairArray[i];
                    ZipEntry zipEntry = new ZipEntry(cNamePathPair.Name)
                    {
                        DateTime = DateTime.Now
                    };
                    zipOutputStream.PutNextEntry(zipEntry);
                    using (FileStream fileStream = File.OpenRead(cNamePathPair.Path))
                    {
                        do
                        {
                            num = fileStream.Read(numArray, 0, (int)numArray.Length);
                            zipOutputStream.Write(numArray, 0, num);
                        }
                        while (num > 0);
                    }
                }
                zipOutputStream.Finish();
                zipOutputStream.Close();
            }
        }

        public static void SaveZipFile(string sZipFileName, string sProtectPassword, params CNamePathPair[] apFileNames)
        {
            int num;
            using (ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(sZipFileName)))
            {
                if (sProtectPassword != null && sProtectPassword.Length > 0)
                {
                    zipOutputStream.Password = sProtectPassword;
                }
                zipOutputStream.SetLevel(9);
                byte[] numArray = new byte[4096];
                CNamePathPair[] cNamePathPairArray = apFileNames;
                for (int i = 0; i < (int)cNamePathPairArray.Length; i++)
                {
                    CNamePathPair cNamePathPair = cNamePathPairArray[i];
                    ZipEntry zipEntry = new ZipEntry(cNamePathPair.Name)
                    {
                        DateTime = DateTime.Now
                    };
                    zipOutputStream.PutNextEntry(zipEntry);
                    using (FileStream fileStream = File.OpenRead(cNamePathPair.Path))
                    {
                        do
                        {
                            num = fileStream.Read(numArray, 0, (int)numArray.Length);
                            zipOutputStream.Write(numArray, 0, num);
                        }
                        while (num > 0);
                    }
                }
                zipOutputStream.Finish();
                zipOutputStream.Close();
            }
        }

        public static void SaveZipFile(string sFileName, StringDictionaryByName apStrings, Encoding aEncoding)
        {
            int num = 0;
            Compression.CNameBufferValue[] cNameBufferValueArray = new Compression.CNameBufferValue[apStrings.Count];
            foreach (string key in apStrings.Keys)
            {
                Compression.CNameBufferValue cNameBufferValue = new Compression.CNameBufferValue(key, aEncoding.GetBytes(apStrings[key]));
                cNameBufferValueArray[num] = cNameBufferValue;
                num++;
            }
            Compression.SaveZipFileFromBuffer(sFileName, cNameBufferValueArray);
        }

        public static void SaveZipFileFromBuffer(string sFileName, Compression.CNameBufferValue[] apFileBuffers)
        {
            Compression.SaveZipStreamFromBuffer(File.Create(sFileName), apFileBuffers);
        }

        public static void SaveZipStreamFromBuffer(Stream aOutputStream, Compression.CNameBufferValue[] apFileBuffers)
        {
            Compression.SaveZipStreamFromBuffer(aOutputStream, apFileBuffers, null);
        }

        public static void SaveZipStreamFromBuffer(Stream aOutputStream, Compression.CNameBufferValue[] apFileBuffers, string password)
        {
            Crc32 crc32 = new Crc32();
            ZipOutputStream zipOutputStream = new ZipOutputStream(aOutputStream);
            zipOutputStream.SetLevel(7);
            if (!string.IsNullOrEmpty(password))
            {
                zipOutputStream.Password = password;
            }
            Compression.CNameBufferValue[] cNameBufferValueArray = apFileBuffers;
            for (int i = 0; i < (int)cNameBufferValueArray.Length; i++)
            {
                Compression.CNameBufferValue cNameBufferValue = cNameBufferValueArray[i];
                byte[] bufferValue = cNameBufferValue.BufferValue;
                ZipEntry zipEntry = new ZipEntry(cNameBufferValue.Name)
                {
                    DateTime = DateTime.Now,
                    Size = (long)((int)bufferValue.Length)
                };
                crc32.Reset();
                crc32.Update(bufferValue);
                zipEntry.Crc = crc32.Value;
                zipOutputStream.PutNextEntry(zipEntry);
                zipOutputStream.Write(bufferValue, 0, (int)bufferValue.Length);
            }
            zipOutputStream.Finish();
            zipOutputStream.Close();
        }

        public static CNamePathPair[] UnzipFileToDirectory(string sZipFileName, string sUnzipDirectory)
        {
            if (!sUnzipDirectory.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                char directorySeparatorChar = Path.DirectorySeparatorChar;
                sUnzipDirectory = string.Concat(sUnzipDirectory, directorySeparatorChar.ToString());
            }
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

        public static CNamePathPair[] UnzipFileToDirectory(string sZipFileName, string sProtectPassword, string sUnzipDirectory)
        {
            if (!sUnzipDirectory.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                char directorySeparatorChar = Path.DirectorySeparatorChar;
                sUnzipDirectory = string.Concat(sUnzipDirectory, directorySeparatorChar.ToString());
            }
            CNamePathPairList cNamePathPairList = new CNamePathPairList();
            using (ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(sZipFileName)))
            {
                if (sProtectPassword != null && sProtectPassword.Length > 0)
                {
                    zipInputStream.Password = sProtectPassword;
                }
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

        public static void WriteByteBufferToFile(string aFileName, byte[] aBuffer, Encoding oEncoding)
        {
            FileStream fileStream = null;
            fileStream = (!File.Exists(aFileName) ? new FileStream(aFileName, FileMode.OpenOrCreate) : new FileStream(aFileName, FileMode.Truncate));
            BinaryWriter binaryWriter = new BinaryWriter(fileStream, oEncoding);
            binaryWriter.Write(aBuffer);
            binaryWriter.Close();
        }

        public static void WriteByteBufferToFile(string aFileName, byte[] aBuffer)
        {
            Compression.WriteByteBufferToFile(aFileName, aBuffer, Encoding.Default);
        }

        public static void WriteStringToFile(string aFileName, string aBuffer, Encoding oEncoding)
        {
            FileStream fileStream = null;
            fileStream = (!File.Exists(aFileName) ? new FileStream(aFileName, FileMode.OpenOrCreate) : new FileStream(aFileName, FileMode.Truncate));
            StreamWriter streamWriter = new StreamWriter(fileStream, oEncoding);
            streamWriter.Write(aBuffer);
            streamWriter.Close();
        }

        public static void WriteStringToFile(string aFileName, string aBuffer)
        {
            Compression.WriteStringToFile(aFileName, aBuffer, Encoding.Default);
        }

        public class CNameBufferValue
        {
            public string Name;

            public byte[] BufferValue;

            public CNameBufferValue()
            {
            }

            public CNameBufferValue(string sName, byte[] abBufferValue)
            {
                this.Name = sName;
                this.BufferValue = abBufferValue;
            }
        }

        public class CNameStringValue
        {
            public string Name;

            public string StringValue;

            public CNameStringValue()
            {
            }

            public CNameStringValue(string sName, string sStringValue)
            {
                this.Name = sName;
                this.StringValue = sStringValue;
            }
        }
    }
}