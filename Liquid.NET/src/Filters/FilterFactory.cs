using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters
{
    public class FilterFactory
    {
        private readonly FilterRegistry _registry;

        public FilterFactory(FilterRegistry registry)
        {
            _registry = registry;
        }


        /// <summary>
        /// TODO: THe IExpressionDescription args should be eval-ed before
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="filterArgs"></param>
        /// <returns></returns>
        public static T InstantiateFilter<T>(String name, IList<Option<IExpressionConstant>> filterArgs)
            where T: IFilterExpression
        {
            return (T) InstantiateFilter(name, typeof(T), filterArgs);
        }

        // TODO: CHange to IExpressionConstants---we don't want to eval them here.
        public static IFilterExpression InstantiateFilter(String name, Type filterType, IEnumerable<Option<IExpressionConstant>> filterArgs)
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
                throw new Exception("The \""+filterType+"\" class for " + name + " has more than one constructor.  Please contact the developer to fix this.");
            }
            //filterArgs.Select(x => x.HasValue ? x.Value : new NilValue());
            var filter = InstantiateFilter(filterType, CreateArguments(filterArgs, constructors[0]));
            filter.Name = name;
            return filter;
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
            int i = 0;
            var filterList = filterArgs.ToList();

            foreach (var argType in argConstructor.GetParameters()
                                                    .Select(parameter => parameter.ParameterType))
            {
                Console.WriteLine("There are " + filterList.Count + " args in the filter.");
                if (i < filterList.Count)
                {
                    if (filterList[i].HasValue)
                    {
                        Console.WriteLine("COMPARING " + filterList[i].Value.GetType() + " TO " + argType);
                        if (argType == typeof (ExpressionConstant) || argType == typeof (IExpressionConstant)) // most generic type
                        {
                            Console.WriteLine("Skipping ExpressionConstant...");
                            result.Add(filterList[i].Value);
                            continue;
                        }
                        CastParameter(filterList[i], argType).WhenSuccess( // more specific type
                            prm => result.Add(prm.Value)
                        ).WhenError(
                            prm =>
                            {
                                Console.WriteLine("THIS IS NULL");
                                result.Add(null);// This shouldnt' be hit
                            }
                        );

                    }
                    else
                    {
                        result.Add(null);
                    }

//                    if (filterList[i].HasValue)
//                    {
//                        Console.WriteLine("COMPARING " + filterList[i].Value.GetType() + " TO " + argType);
//                        if (argType == typeof (ExpressionConstant) || argType == typeof (IExpressionConstant))
//                        {
//                            Console.WriteLine("Skipping ExpressionConstant...");
//                            result.Add(filterList[i].Value);
//                            continue;
//                        }
//                    }
//                    else
//                    {
//                        result.Add(null);
//                    }
//
//                    //result.Add(filterList[i].GetType() == parmType // if it's the same type
//                    if (argType.IsInstanceOfType(filterList[i]))
//                    {
//                        result.Add(filterList[i]);
//                    }
//                    else
//                    {
//                        CastParameter(filterList[i], argType).WhenSuccess(
//                            prm => result.Add(prm.Value)
//                        ).WhenError(
//                            prm => result.Add(null) // This shouldnt' be hit
//                        );
//                    }
//                    result.Add(argType.IsInstanceOfType(filterList[i])
//                        ? filterList[i] // then it's ok
//                        : CastParameter(filterList[i], argType)); // else cast it 
                }
                else
                {
                    result.Add(null);
//                    // no value provided, so use the default                    
//                    Console.WriteLine("Passing null--- no value");
//                    Console.WriteLine("THERE ARE "+argType.GetConstructors().Count());
//                    // construct a default value type
//                    var constructorInfo = argType.GetConstructors()[0];
//                    var parameterInfos = constructorInfo.GetParameters();
//
//
//                    var defaultValue = parameterInfos.Select(p => GetDefault(p.ParameterType)).ToArray(); //.Select(x => x == System.DBNull? ToArray();
//                    Console.WriteLine("Default is " + defaultValue.GetType());
//                    //var defaultarg = (IExpressionConstant)Activator.CreateInstance(argType, null);
//                    var defaultarg = (IExpressionConstant)constructorInfo.Invoke(defaultValue);
//                    //defaultarg.IsUndefined = true;
//                    result.Add(defaultarg);
//                    //result.Add(CreateUndefinedForType(parmType, defaultParams));
                    //result.Add(null);
                }
                i++;
            }
            return result;
        }

        public static object GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

//        public static IExpressionConstant CreateUndefinedForType(Type valueType, object[] args)
//        {
//            // TODO: Introspect the argument for the Value type and get the default.
//            Console.WriteLine("passing " + args.Count() + " args");
//            var result = (IExpressionConstant)Activator.CreateInstance(valueType, args);
//            result.IsUndefined = true;
//            return result;
//        }

        private static LiquidExpressionResult CastParameter(Option<IExpressionConstant> filterList, Type parmType)
        {
            
            MethodInfo method = typeof (ValueCaster).GetMethod("Cast");
            if (filterList.HasValue)
            {
                MethodInfo generic = method.MakeGenericMethod(filterList.Value.GetType(), parmType);
                //return (IExpressionConstant) generic.Invoke(null, new object[] {filterList.Value});
                return (LiquidExpressionResult) generic.Invoke(null, new object[] { filterList.Value });
            }
            else
            {
                MethodInfo generic = method.MakeGenericMethod(filterList.Value.GetType(), parmType);
                //return (IExpressionConstant) generic.Invoke(null, null);
                return (LiquidExpressionResult)generic.Invoke(null, null);
                
            }
            // If the filters use Options, this should be used instead....
            //MethodInfo generic = method.MakeGenericMethod(filterList.GetType(), parmType);
            //return (Option<IExpressionConstant>) generic.Invoke(null, new object[] { filterList });
        }

        public static IFilterExpression CreateCastExpression(Type sourceType, Type resultType)
        {

            Type genericClass = typeof(CastFilter<,>);
            // MakeGenericType is badly named
            //Console.WriteLine("Creating Converter from "+sourceType+" to "+resultType);
            Type constructedClass = genericClass.MakeGenericType(sourceType, resultType);
            return (IFilterExpression)Activator.CreateInstance(constructedClass);
        }


    }
}
