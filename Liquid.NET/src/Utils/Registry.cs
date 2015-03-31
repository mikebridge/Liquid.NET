using System;
using System.Collections.Generic;

namespace Liquid.NET.Utils
{
    /// <summary>
    /// Register types which implement T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Registry<T>
    {
        readonly IDictionary<String, Type> _dictionary = new Dictionary<String, Type>();

        public void Register<TSubtype>(String key)
            where TSubtype : T
        {
            if (_dictionary.ContainsKey(key))
            {
                Deregister(key);
            }
            _dictionary.Add(key, typeof(TSubtype));
        }

        public void Deregister(string filterName)
        {
            if (_dictionary.ContainsKey(filterName))
            {
                _dictionary.Remove(filterName);
            }
        }

        public Type Find(String key)
        {
            return HasKey(key) ? _dictionary[key] : null;
        }

        public bool HasKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }
    }
}
