using System;
using System.Collections.Generic;

namespace Liquid.NET.Filters
{
    public class FilterRegistry
    {
        readonly IDictionary<String, Type> _filterDictionary = new Dictionary<String, Type>();
        
        public void Register<T>(String filterName)
            where T : IFilterExpression
        {
            if (_filterDictionary.ContainsKey(filterName))
            {
                Deregister(filterName);
            }
            _filterDictionary.Add(filterName, typeof(T));
        }

        public void Deregister(string filterName)
        {
            if (_filterDictionary.ContainsKey(filterName))
            {
                _filterDictionary.Remove(filterName);
            }
        }

        public Type Find(String filterName)
        {
            return HasKey(filterName) ? _filterDictionary[filterName] : null;
        }

        public bool HasKey(string filterName)
        {
            return _filterDictionary.ContainsKey(filterName);
        }
    }
}
