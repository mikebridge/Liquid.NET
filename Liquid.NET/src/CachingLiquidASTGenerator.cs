using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Liquid.NET
{
    /// <summary>
    /// A *global* cache for templates.
    /// </summary>
    public class CachingLiquidASTGenerator : ILiquidASTGenerator
    {
        private readonly ILiquidASTGenerator _generator;
        private readonly TimeSpan _slidingExpiration;

        public CachingLiquidASTGenerator(ILiquidASTGenerator generator, int slidingExpirationSeconds = 300)
        {
             _slidingExpiration = TimeSpan.FromSeconds(slidingExpirationSeconds);
            _generator = generator;
        }

        private String CacheKey(String str)
        {
            return str;
        }

        public LiquidASTGenerationResult Generate(string template)
        {
            var errors = new List<LiquidError>();
            return LiquidASTGenerationResult.Create(Generate(template, errors.Add), errors);
        }

        public LiquidAST Generate(string template, Action<LiquidError> errorAccumulator)
        {
            // We need to pass the errors back to the parent, but we
            // also need to avoid saving in the cache if an error is generated.
            errorAccumulator = errorAccumulator ?? (err => { });
            IList<LiquidError> errors = new List<LiquidError>();

            Action<LiquidError> decoratedAccumulator = err =>
            {
                errors.Add(err);
                errorAccumulator(err);
            };

            String hash = CacheKey(template);
            ObjectCache cache = MemoryCache.Default;
            var liquidAST = cache[hash] as LiquidAST;
            if (liquidAST == null)
            {                
                CacheItemPolicy policy = new CacheItemPolicy
                {
                    SlidingExpiration = _slidingExpiration
                };
                // TODO: If there are errors, don't caceh the template
                liquidAST = _generator.Generate(template, decoratedAccumulator);
                
                if (!errors.Any() && liquidAST != null)
                {
                    cache.Set(hash, liquidAST, policy);
                }
            }
            return liquidAST;
        }
    }
}
