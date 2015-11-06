using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Filters.Strings
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class StripHtmlFilter : FilterExpression<LiquidString, LiquidString>
    {

        public override LiquidExpressionResult ApplyTo(ITemplateContext ctx, LiquidString liquidLiquidStringExpression)
        {
            //input.to_s.gsub(/<script.*?<\/script>/m, empty).gsub(/<!--.*?-->/m, empty).gsub(/<style.*?<\/style>/m, empty).gsub(/<.*?>/m, empty)
            return LiquidExpressionResult.Success(StringUtils.Eval(liquidLiquidStringExpression, UnHtml));
        }


        // http://stackoverflow.com/questions/19523913/remove-html-tags-from-string-including-nbsp-in-c-sharp#answer-19524290

        private static readonly Regex Tags = new Regex(@"<[^>]+?>", RegexOptions.Multiline | RegexOptions.Compiled);

        //add characters that are should not be removed to this regex
        private static readonly Regex NotOkCharacter = new Regex(@"[^\w;&#@.:/\\?=|%!() -]", RegexOptions.Compiled);

        public static String UnHtml(String html)
        {
            html = HttpUtility.UrlDecode(html);
            html = HttpUtility.HtmlDecode(html);

            html = RemoveTag(html, "<!--", "-->");
            html = RemoveTag(html, "<script", "</script>");
            html = RemoveTag(html, "<style", "</style>");

            //replace matches of these regexes with space
            html = Tags.Replace(html, " ");
            html = NotOkCharacter.Replace(html, " ");
            html = SingleSpacedTrim(html);

            return html;
        }

        private static String RemoveTag(String html, String startTag, String endTag)
        {
            Boolean again;
            do
            {
                again = false;
                var startTagPos = html.IndexOf(startTag, 0, StringComparison.CurrentCultureIgnoreCase);
                if (startTagPos < 0)
                {
                    continue;
                }
                var endTagPos = html.IndexOf(endTag, startTagPos + 1, StringComparison.CurrentCultureIgnoreCase);
                if (endTagPos <= startTagPos)
                {
                    continue;
                }
                html = html.Remove(startTagPos, endTagPos - startTagPos + endTag.Length);
                again = true;
            } while (again);
            return html;
        }

        private static String SingleSpacedTrim(String inString)
        {
            StringBuilder sb = new StringBuilder();
            var inBlanks = false;
            foreach (var c in inString)
            {
                switch (c)
                {
                    case '\r':
                    case '\n':
                    case '\t':
                    case ' ':
                        if (!inBlanks)
                        {
                            inBlanks = true;
                            sb.Append(' ');
                        }
                        continue;
                    default:
                        inBlanks = false;
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString().Trim();
        }

    }
}
