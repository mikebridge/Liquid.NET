using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{
    // TODO: Test this whole class for null values, esp. nulls in dictionaries.
    public class Reflector
    {
        /// <summary>
        /// Put the properties of an object into a dictionary.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="capitalizationStrategy"></param>
        /// <returns></returns>
        public IExpressionConstant GenerateExpressionConstant(Object obj /*, CapitalizationStrategy capitalizationStrategy*/)
        {
            // TODO: reflect on dictionary, array
            // http://stackoverflow.com/questions/9115413/is-there-an-easy-way-to-convert-object-properties-to-a-dictionarystring-string

            var type = obj.GetType();
            //Console.WriteLine("TYype is " + type);
            var dict = obj as IDictionary;
            if (dict != null) // TODO: handle any collection
            {
                return DictionaryToDictionaryValue(dict);
            }

            // reflect Object
            return ObjectToDictionaryValue(obj);
        }

        private IExpressionConstant DictionaryToDictionaryValue(IDictionary dict)
        {
            
            var newDict = dict.Keys.Cast<object>()
                             .ToDictionary(key => key.ToString(), 
                                           key => ConvertToConstant(dict[key].GetType(), dict[key]));
            //foreach (var key in newDict.Keys)
            //{
                //Console.WriteLine("   " + key + "=" + newDict[key].Value);
            //}
            return new DictionaryValue(newDict);
        }

        private IExpressionConstant ObjectToDictionaryValue(object obj)
        {
            var dictionary = obj.GetType()
                .GetProperties()
                .Select(pi => new {pi.Name, Value = ConvertToConstant(pi.PropertyType, pi.GetValue(obj, null))})
                .Union(
                    obj.GetType()
                        .GetFields()
                        .Select(fi => new {fi.Name, Value = ConvertToConstant(fi.FieldType, fi.GetValue(obj))})
                )
                .ToDictionary(ks => ks.Name, vs => vs.Value);
            //Console.WriteLine("== dictionary ==");
            //Console.WriteLine(String.Join("\r\n", dictionary.Keys.Select(x => x + " = " + dictionary[x].Value)));
            return new DictionaryValue(dictionary);
        }

        private Option<IExpressionConstant> ConvertToConstant(Type type, Object obj)
        {
            //Console.WriteLine("Converting " + obj + " to a constant type ");
            if (obj == null)
            {
                return new None<IExpressionConstant>();
            }
            if (type == typeof(String))
            {
                return new Some<IExpressionConstant>(new StringValue((String)obj));
            }
            if (type == typeof(int) || type == typeof(decimal))
            {
                // TODO: long, etc.
                return new Some<IExpressionConstant>(NumericValue.Create((decimal)obj));
            }
            if (type == typeof (bool))
            {
                return new Some<IExpressionConstant>(new BooleanValue((bool) obj));
            }

            

            // todo: recurse
            
            //Console.WriteLine("OBJ IS " + obj + " type is "+type);
            throw new NotImplementedException();
            //return GenerateExpressionConstant(obj);
            //return ExpressionConstant.CreateError<StringValue>("ConvertToCOnstant not implemented yet");

        }


    }
}
