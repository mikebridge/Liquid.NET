using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.NET.Symbols
{
    public interface IASTNode
    {
        void Accept(IASTVisitor visitor);
    }
}
