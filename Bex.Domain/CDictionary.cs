using System;
using System.Collections.Generic;

namespace Bex.Domain
{

    public class CDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public CDictionary()
        {
        }

        public CDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
        }

        public CDictionary(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        public CDictionary(int capacity) : base(capacity)
        {
        }

        public CDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer)
        {
        }

        public CDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer)
        {
        }

        public new TValue GetValueOrDefault(TKey key)
        {
            return this.GetValueOrDefault(key, default(TValue));
        }

        public TValue GetValueOrDefault(TKey key, TValue oDefaultValue)
        {
            TValue tValue;
            if (!base.TryGetValue(key, out tValue))
            {
                tValue = oDefaultValue;
            }
            return tValue;
        }
    }
}