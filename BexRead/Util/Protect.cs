using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;

namespace BexFileRead.Util
{


    [Serializable]
	public abstract class Protection : NameValueCollection
	{
		private static string m_sAPP_BASE_PATH;

		protected readonly static string m_sCryptKey;

		protected readonly static string m_sCryptPrefix;

	 
		protected Hashtable htSections;

		public static string APP_BASE_PATH
		{
			get
			{
				if (Protection.m_sAPP_BASE_PATH == null)
				{
					Protection.m_sAPP_BASE_PATH = string.Concat(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.RelativeSearchPath);
					if (Protection.m_sAPP_BASE_PATH[Protection.m_sAPP_BASE_PATH.Length - 1] != Path.DirectorySeparatorChar)
					{
						string mSAPPBASEPATH = Protection.m_sAPP_BASE_PATH;
						char directorySeparatorChar = Path.DirectorySeparatorChar;
						Protection.m_sAPP_BASE_PATH = string.Concat(mSAPPBASEPATH, directorySeparatorChar.ToString());
					}
				}
				return Protection.m_sAPP_BASE_PATH;
			}
		}

  

		static Protection()
		{
			Protection.m_sAPP_BASE_PATH = null;
			Protection.m_sCryptKey = "B20E89B5-74AA-4DB3-ABB4-6A605537A918";
			Protection.m_sCryptPrefix = "CRYPT.1:";
		}

		public Protection()
		{
		}

 
		public abstract void AddHashTableParams(Hashtable htParameters);

		public static string DecryptString(string sKey, string sValue)
		{
			string str;
			try
			{
				str = Protection.DecryptString(sValue);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				throw new Exception(string.Format("Unable to get key setting '{0}'. {1}", sKey, exception.Message), exception);
			}
			return str;
		}

		public static string DecryptString(string sValue)
		{
			if (!Protection.IsEncrypted(sValue))
			{
				return sValue;
			}
			return (new Cryptography()).DecryptData(Protection.m_sCryptKey, sValue.Substring(Protection.m_sCryptPrefix.Length));
		}

		public static string EncryptString(string sValue)
		{
			string str;
			try
			{
				Cryptography cryptography = new Cryptography();
				str = string.Concat(Protection.m_sCryptPrefix, cryptography.EncryptData(Protection.m_sCryptKey, sValue));
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				throw new Exception(string.Format("Unable to encrypt text. {0}", exception.Message), exception);
			}
			return str;
		}

		public static bool IsEncrypted(string sValue)
		{
			if (sValue.Length < Protection.m_sCryptPrefix.Length)
			{
				return false;
			}
			return sValue.Substring(0, Protection.m_sCryptPrefix.Length) == Protection.m_sCryptPrefix;
		}
	}
}