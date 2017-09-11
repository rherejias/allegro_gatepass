using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace GatePass.Extensions
{
    public static class ExtensionMethods
    {
        public static string ToLiteral(string input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }

        public static string unescape(string code)
        {
            var str = Regex.Replace(code, @"\+", "\x20");
            str = Regex.Replace(str, @"%([a-fA-F0-9][a-fA-F0-9])", new MatchEvaluator((mach) => {
                var _tmp = mach.Groups[0].Value;
                var _hexC = mach.Groups[1].Value;
                var _hexV = int.Parse(_hexC, System.Globalization.NumberStyles.HexNumber);
                return ((char) _hexV).ToString();
            }));

            return str;
        }

        public static string Unescape(this string txt)
        {
            if (string.IsNullOrEmpty(txt)) { return txt; }
            StringBuilder retval = new StringBuilder(txt.Length);
            for (int ix = 0; ix < txt.Length;)
            {
                int jx = txt.IndexOf('\\', ix);
                if (jx < 0 || jx == txt.Length - 1) jx = txt.Length;
                retval.Append(txt, ix, jx - ix);
                if (jx >= txt.Length) break;
                switch (txt[jx + 1])
                {
                    case 'n': retval.Append('\n'); break;  // Line feed
                    case 'r': retval.Append('\r'); break;  // Carriage return
                    case 't': retval.Append('\t'); break;  // Tab
                    case '\\': retval.Append(@"\"); break; // Don't escape
                    default:                                 // Unrecognized, copy as-is
                        retval.Append(@"\").Append(txt[jx + 1]); break;
                }
                ix = jx + 2;
            }
            return retval.ToString();
        }

        public static StringBuilder UcWords(this string theString)
        {
            StringBuilder output = new StringBuilder();
            string[] pieces = theString.Split(' ');
            foreach (string piece in pieces)
            {
                char[] theChars = piece.ToCharArray();
                theChars[0] = char.ToUpper(theChars[0]);
                output.Append(' ');
                output.Append(new string(theChars));
            }

            return output;

        }
    }
}