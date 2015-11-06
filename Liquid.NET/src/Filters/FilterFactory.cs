using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class FilterFactory
    {

        public static Try<IFilterExpression> InstantiateFilter(String name, Type filterType, IEnumerable<Option<IExpressionConstant>> filterArgs)
        {
           
            if (filterType == null)
            {               
                return new Failure<IFilterExpression>(new ApplicationException("Unable to create filter " + name));
            }
            if (!typeof (IFilterExpression).IsAssignableFrom(filterType))
            {
                return new Failure<IFilterExpression>(new ArgumentException("Filter \"" + name + "\" refers to " + filterType + ", which does not implement IFilterExpression."));
            }
            var constructors = filterType.GetConstructors();

            if (constructors.Length != 1)
            {
                // for the time being, ensure just one constructor.
                return new Failure<IFilterExpression>(new ApplicationException(("The \""+filterType+"\" class for " + name + " has more than one constructor.  Please contact the developer to fix this.")));              
            }
            var filter = InstantiateFilter(filterType, CreateArguments(filterArgs, constructors[0]));
            filter.Name = name;
            return new Success<IFilterExpression>(filter);
        }

        private static IFilterExpression InstantiateFilter(Type filterType, IList<object> args)
        {
            return !args.Any()
                ? (IFilterExpression) Activator.CreateInstance(filterType)
                : (IFilterExpression) Activator.CreateInstance(filterType, args.ToArray());
        }

        private static IList<object> CreateArguments(IEnumerable<Option<IExpressionConstant>> filterArgs, ConstructorInfo argConstructor)
        {
            IList<Object> result = new List<object>();
            int i = -1;
            var filterList = filterArgs.ToList();

            foreach (var argType in argConstructor.GetParameters()
                                                    .Select(parameter => parameter.ParameterType))
            {
                i++;
                //Console.WriteLine("There are " + filterList.Count + " args in the filter.");
                if (i < filterList.Count)
                {
                    if (filterList[i].HasValue)
                    {
                        //Console.WriteLine("COMPARING " + filterList[i].Value.GetType() + " TO " + argType);
                        if (argType == typeof (LiquidValue) || argType == typeof (IExpressionConstant)) // most generic type
                        {
                            //Console.WriteLine("Skipping LiquidValue...");
                            result.Add(filterList[i].Value);
                            continue;
                        }
                        CastParameter(filterList[i].Value, argType).WhenSuccess( // more specific type
                            prm => result.Add(prm.Value));

                    }
                    else
                    {
                        // missing args: Shopify liquid seems to pass "0" for requested numbers else nil.
                        result.Add(argType == typeof (LiquidNumeric) ? LiquidNumeric.Create(0) : null);
                    }

                }
                else
                {
                    result.Add(null);
                }
               
            }
            return result;
        }

        private static LiquidExpressionResult CastParameter(IExpressionConstant filterList, Type parmType)
        {

            MethodInfo method = typeof (ValueCaster).GetMethod("Cast");

            MethodInfo generic = method.MakeGenericMethod(filterList.GetType(), parmType);
            return (LiquidExpressionResult) generic.Invoke(null, new object[] {filterList});

        }


    }
}
