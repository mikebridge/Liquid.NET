using System;


namespace Liquid.NET
{
    public interface IFileSystem
    {
        String Include(ITemplateContext templateContext, String key);
    }
}
