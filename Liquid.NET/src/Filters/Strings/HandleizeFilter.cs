using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/filters/string-filters#handle
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class HandleizeFilter : FilterExpression<IExpressionConstant, StringValue>
    {

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, IExpressionConstant liquidExpression)
        {
            var strArray = ValueCaster.RenderAsString(liquidExpression).Split();
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidExpression, x => Slug.Create(true, strArray)));
        }


        public static class Slug
        {
            public static string Create(bool toLower, params string[] values)
            {
                return Create(toLower, String.Join("-", values));
            }

            /// <summary>
            /// Creates a slug.
            /// References:
            /// http://stackoverflow.com/questions/25259/how-does-stack-overflow-generate-its-seo-friendly-urls
            /// 
            /// http://www.unicode.org/reports/tr15/tr15-34.html
            /// http://meta.stackexchange.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696
            /// http://stackoverflow.com/questions/25259/how-do-you-include-a-webpage-title-as-part-of-a-webpage-url/25486#25486
            /// http://stackoverflow.com/questions/3769457/how-can-i-remove-accents-on-a-string
            /// </summary>
            public static string Create(bool toLower, string value)
            {
                if (value == null)
                {
                    return "";
                }
                var normalised = value.Normalize(NormalizationForm.FormKD);

                const int maxlen = 80;
                var len = normalised.Length;
                var prevDash = false;
                var sb = new StringBuilder(len);

                for (int i = 0; i < len; i++)
                {
                    var c = normalised[i];
                    if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                    {
                        if (prevDash)
                        {
                            sb.Append('-');
                            prevDash = false;
                        }
                        sb.Append(c);
                    }
                    else if (c >= 'A' && c <= 'Z')
                    {
                        if (prevDash)
                        {
                            sb.Append('-');
                            prevDash = false;
                        }
                        // Tricky way to convert to lowercase
                        if (toLower)
                            sb.Append((char)(c | 32));
                        else
                            sb.Append(c);
                    }
                    else if (c == ' ' || c == ',' || c == '.' || c == '/' || c == '\\' || c == '-' || c == '_' || c == '=')
                    {
                        if (!prevDash && sb.Length > 0)
                        {
                            prevDash = true;
                        }
                    }
                    else
                    {
                        var swap = ConvertEdgeCases(c, toLower);

                        if (swap != null)
                        {
                            if (prevDash)
                            {
                                sb.Append('-');
                                prevDash = false;
                            }
                            sb.Append(swap);
                        }
                    }

                    if (sb.Length == maxlen)
                        break;
                }
                return sb.ToString();
            }

            static string ConvertEdgeCases(char c, bool toLower)
            {
                string swap = null;
                switch (c)
                {
                    case 'ı':
                        swap = "i";
                        break;
                    case 'ł':
                        swap = "l";
                        break;
                    case 'Ł':
                        swap = toLower ? "l" : "L";
                        break;
                    case 'đ':
                        swap = "d";
                        break;
                    case 'ß':
                        swap = "ss";
                        break;
                    case 'ø':
                        swap = "o";
                        break;
                    case 'Þ':
                        swap = "th";
                        break;
                }
                return swap;
            }
        }

    }
}
