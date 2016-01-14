using System;
using System.Collections.Generic;
using System.Text;

namespace BIRC.Shared.Utils
{
    public static class HtmlWriter
    {
        private static string TIME_STYLE = "color: #C0C0C0; font-size: 0.9em";
        private static string BODY_STYLE = "font-size: 0.9em";
        private static string ERROR_STYLE = "color: red; font-size: 0.9em";
        private static string FROM_STYLE = "color: #600000; font-size: 0.9em";
        private static string SOURCE_STYLE = "font-weight: bold; color: red";
        private static string SOURCE_STYLE_SELF = "font-weight: bold; color: blue";

        public static string WriteStartTag()
        {
            return "<table><tr>";
        }

        public static string WriteHour()
        {
            return "<td valign=\"top\" style=\"" + TIME_STYLE + "\">[" + DateTime.Now.ToString("HH:mm:ss") + "]</td> ";
        }

        public static string Write(string body)
        {
            return WriteStartTag() + WriteHour() + WriteBody(System.Net.WebUtility.HtmlEncode(body)) + WriteEndTag();
        }

        public static string WriteFrom(string body, string from, bool self)
        {
            return WriteStartTag() + WriteHour() + WriteBodyWithSource(System.Net.WebUtility.HtmlEncode(body), from, self) 
                + WriteEndTag();
        }

        public static string WriteBodyWithSource(string body, string src, bool self)
        {
            return "<td style=\"" + BODY_STYLE + "\">&lt;<span style=\"" 
                + (self == true ? SOURCE_STYLE_SELF : SOURCE_STYLE) + "\">" + src + "</span>&gt;&nbsp;"
                + body + "</td>";
        }

        public static string WriteBody(string body)
        {
            return "<td style=\"" + BODY_STYLE + "\">" + body + "</td>";
        }

        public static string WriteBody(string from, string body)
        {
            return "@<td style=\"" + FROM_STYLE + "\"></td>" + WriteBody(body);
        }

        public static string WriteError(string body)
        {
            return WriteStartTag() + WriteHour() + "<td style=\"" + ERROR_STYLE + "\">" + body + "</td>" + WriteEndTag();
        }

        public static string WriteEndTag()
        {
            return "</tr></table>";
        }
    }
}
