using System;
using System.Collections.Generic;


namespace Liquid.NET.Tests.Helpers
{
    public class TestFileSystem : IFileSystem
    {
        private readonly IDictionary<string, string> _lookupTable;

        public TestFileSystem(IDictionary<String, String> lookupTable)
        {
            _lookupTable = lookupTable;
        }

        public string Include(ITemplateContext ctx, string key)
        {
            if (_lookupTable.ContainsKey(key))
            {
                return _lookupTable[key];
            }
            else
            {
                return ""; // TODO: check what this should return when not found.
            }
        }
    }
}
