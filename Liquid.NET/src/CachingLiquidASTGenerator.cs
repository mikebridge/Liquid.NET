using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.NET
{
    /// <summary>
    /// A *global* cache for templates.
    /// </summary>
    public class CachingLiquidASTGenerator : ILiquidASTGenerator
    {
        private readonly ILiquidASTGenerator _generator;
        private TimeSpan _slidingExpiration = TimeSpan.FromMinutes(5);

        public CachingLiquidASTGenerator(ILiquidASTGenerator generator)
        {
            _generator = generator;
        }

        public String CacheKey(String str)
        {
            return str;
        }

        public LiquidAST Generate(string template)
        {
            String hash = CacheKey(template);
            ObjectCache cache = MemoryCache.Default;
            var liquidAST = cache[hash] as LiquidAST;
            if (liquidAST == null)
            {                
                CacheItemPolicy policy = new CacheItemPolicy
                {
                    SlidingExpiration = _slidingExpiration
                };
                liquidAST =_generator.Generate(template);
                if (liquidAST != null)
                {
                    cache.Set(hash, liquidAST, policy);
                }
            }
            return liquidAST;
        }
    }
}
