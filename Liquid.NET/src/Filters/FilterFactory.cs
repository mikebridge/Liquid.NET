using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.XPath;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Filters.Math;
using Liquid.NET.Filters.Strings;

namespace Liquid.NET.Filters
{
    public class FilterFactory
    {
        private readonly FilterRegistry _registry;

        public FilterFactory(FilterRegistry registry)
        {
            _registry = registry;
        }

        public static void AddDefaultFilters(FilterRegistry registry)
        {
            registry.Register<UpCaseFilter>("upcase");
            registry.Register<RemoveFilter>("remove");
            registry.Register<PlusFilter>("plus");
            registry.Register<LookupFilter>("lookup");
            //registry.Register<MinusFilter>("subtract");
        }

        /// <summary>
        /// TODO: THe IExpressionDescription args should be eval-ed before
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="filterArgs"></param>
        /// <returns></returns>
        public static T InstantiateFilter<T>(String name, IList<IExpressionConstant> filterArgs)
            where T: IFilterExpression
        {
            return (T) InstantiateFilter(name, typeof(T), filterArgs);
        }

        // TODO: CHange to IExpressionConstants---we don't want to eval them here.
        public static IFilterExpression InstantiateFilter(String name, Type filterType, IEnumerable<IExpressionConstant> filterArgs)
        {
           
            if (filterType == null)
            {
                // Todo: Figure out how to handle this without an exception
                throw new ArgumentException("Unable to create filter " + name);
            }
            if (!typeof (IFilterExpression).IsAssignableFrom(filterType))
            {
                throw new ArgumentException("Filter \"" + name + "\" refers to " + filterType + ", which does not implement IFilterExpression.");
            }
            var constructors = filterType.GetConstructors();

            if (constructors.Count() != 1)
            {
                // for the time being, ensure just one constructor.
                throw new Exception("The \""+filterType+"\" class for " + name + " has more than one constructor.  Please contact the devloper to fix this.");
            }
            return InstantiateFilter(filterType, CreateArguments(filterArgs, constructors));
           
        }

        private static IFilterExpression InstantiateFilter(Type filterType, IList<object> args)
        {
            return !args.Any()
                ? (IFilterExpression) Activator.CreateInstance(filterType)
                : (IFilterExpression) Activator.CreateInstance(filterType, args.ToArray());
        }

        private static IList<object> CreateArguments(IEnumerable<IExpressionConstant> filterArgs, ConstructorInfo[] constructors)
        {
            IList<Object> result = new List<object>();
            int i = 0;
            var filterList = filterArgs.ToList();

            foreach (var parmType in constructors[0].GetParameters()
                                                    .Select(parameter => parameter.ParameterType))
            {
                Console.WriteLine("There are " + filterList.Count + " args in the filter.");
                if (i < filterList.Count)
                {
                    Console.WriteLine("COMPARING " + filterList[i].GetType() + " TO " + parmType);
                    if (parmType == typeof (ExpressionConstant) || parmType == typeof(IExpressionConstant))
                    {
                        Console.WriteLine("Skipping...");
                        result.Add(filterList[i]);
                        continue;
                    }
               /*     Console.WriteLine(filterList[i].GetType() == parmType);
                    Console.WriteLine(filterList[i].GetType().IsAssignableFrom(parmType));
                    Console.WriteLine(parmType.IsAssignableFrom(filterList[i].GetType()));
                    Console.WriteLine(parmType.IsInstanceOfType(filterList[i]));
                    Console.WriteLine(filterList[i].GetType().IsInstanceOfType(parmType));*/
                    // TODO: don't know why isassignabel from doesn't work.
                    //result.Add(filterList[i].GetType() == parmType // if it's the same type
                    result.Add(parmType.IsInstanceOfType(filterList[i])
                    //result.Add(parmType.IsInstanceOfType(filterList[i])    
                        ? filterList[i] // then it's ok
                        : CastParameter(filterList[i], parmType)); // else cast it 
                }
                else
                {
                    // no value provided, so use the default
                    // TODO: pass the wrapped null parameter, not just a bare null. (?)
                    //Console.WriteLine("Passing null--- no value");
                    result.Add(null);
                }
                i++;
            }
            return result;
        }

        private static IExpressionConstant CastParameter(IExpressionConstant filterList, Type parmType)
        {
            MethodInfo method = typeof (ValueCaster).GetMethod("Cast");
            MethodInfo generic = method.MakeGenericMethod(filterList.GetType(), parmType);
            return (IExpressionConstant) generic.Invoke(null, new object[] {filterList});

            //Console.WriteLine("Adding CASTED value " + result.Value);
        }

        //public static CastFilter<IObjectExpression, IObjectExpression> CreateCastFilter(Type sourceType, Type resultType)
        public static IFilterExpression CreateCastExpression(Type sourceType, Type resultType)
            //where TSource : IObjectExpression
            //where TResult : IObjectExpression

        {

            Type genericClass = typeof(CastFilter<,>);
            // MakeGenericType is badly named
            //Console.WriteLine("Creating Converter from "+sourceType+" to "+resultType);
            Type constructedClass = genericClass.MakeGenericType(sourceType, resultType);
            return (IFilterExpression)Activator.CreateInstance(constructedClass);
        }


    }
}
