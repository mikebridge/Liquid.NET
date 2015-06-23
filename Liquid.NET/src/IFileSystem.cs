using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.NET
{
    public interface IFileSystem
    {
        /// <summary>
        /// Return 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        String Include(String key);
    }
}
