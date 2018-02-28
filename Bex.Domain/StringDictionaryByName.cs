using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Bex.Domain
{

    public class StringDictionaryByName : CDictionary<string, string>
    {
        public StringDictionaryByName()
        {
        }

        public StringDictionaryByName(IDictionary<string, string> dictionary) : base(dictionary)
        {
        }

        public StringDictionaryByName(IEqualityComparer<string> comparer) : base(comparer)
        {
        }

        public StringDictionaryByName(int capacity) : base(capacity)
        {
        }

        public StringDictionaryByName(IDictionary<string, string> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer)
        {
        }

        public StringDictionaryByName(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer)
        {
        }

        public static StringDictionaryByName ComposeStringDict(string[] asDictsPrefix, StringDictionaryByName[] aoDicts)
        {
            StringDictionaryByName stringDictionaryByName = new StringDictionaryByName();
            int num = 0;
            int length = (int)aoDicts.Length;
            while (num < length)
            {
                string str = asDictsPrefix[num];
                StringDictionaryByName stringDictionaryByName1 = aoDicts[num];
                foreach (string key in stringDictionaryByName1.Keys)
                {
                    stringDictionaryByName.Add(string.Format("{0}__{1}", str, key), stringDictionaryByName1[key]);
                }
                num++;
            }
            return stringDictionaryByName;
        }

        public static Dictionary<string, StringDictionaryByName> DeComposeStringDict(string[] asDictsPrefix, StringDictionaryByName oDict)
        {
            Dictionary<string, StringDictionaryByName> strs = new Dictionary<string, StringDictionaryByName>((int)asDictsPrefix.Length);
            for (int i = 0; i < (int)asDictsPrefix.Length; i++)
            {
                strs[asDictsPrefix[i]] = new StringDictionaryByName();
            }
            string str = "__";
            foreach (string key in oDict.Keys)
            {
                int num = key.IndexOf(str);
                Trace.Assert(num > 0, string.Format("Cannot Read composed String Dictionary, Key '{0}' is invalid.", key));
                string str1 = key.Substring(0, num);
                string str2 = key.Substring(num + 2);
                strs[str1].Add(str2, oDict[key]);
            }
            return strs;
        }
    }
}