using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.NET.Tests.Ruby
{
    public class TestFileSystem : IFileSystem
    {
        public string Include(string key)
        {
            String result = key;
            switch (key)
            {
                case "product":
                    result = "Product: {{ product.title }} ";
                    break;
                case  "locale_variables":
                    result = "Locale: {{echo1}} {{echo2}}";
                    break;
                case "variant":
                    result = "Variant: {{ variant.title }}";
                    break;
                case "nested_template":
                    result = "{% include 'header' %} {% include 'body' %} {% include 'footer' %}";
                    break;
                case "body":
                    result = "body {% include 'body_detail' %}";
                    break;
                case  "nested_product_template":
                    result = "Product: {{ nested_product_template.title }} {%include 'details'%} ";
                    break;
                case  "recursively_nested_template":
                    result = "-{% include 'recursively_nested_template' %}";
                    break;
                case  "pick_a_source":
                    result = "from TestFileSystem";
                    break;
                case "assignments":
                    result = "{% assign foo = 'bar' %}";
                    break;


            }

            return result;
        }
    }
}
