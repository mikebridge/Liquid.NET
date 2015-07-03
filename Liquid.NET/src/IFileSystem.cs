using System;


namespace Liquid.NET
{
    public interface IFileSystem
    {
        /// <summary>
        /// Return 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        String Include(ITemplateContext templateContext, String key);
    }
}
