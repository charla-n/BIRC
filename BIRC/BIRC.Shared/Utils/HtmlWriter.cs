﻿using System;
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

        public static string WriteStartTag()
        {
            return "<div>";
        }

        public static string WriteHour()
        {
            return "<span style=\"" + TIME_STYLE + "\">[" + DateTime.Now.ToString("HH:mm:ss") + "]</span> ";
        }

        public static string Write(string body)
        {
            return WriteStartTag() + WriteHour() + WriteBody(body) + WriteEndTag();
        }

        public static string WriteBody(string body)
        {
            return "<span style=\"" + BODY_STYLE + "\">" + body + "</span>";
        }

        public static string WriteBody(string from, string body)
        {
            return "@<span style=\"" + FROM_STYLE + "\"></span>" + WriteBody(body);
        }

        public static string WriteError(string body)
        {
            return WriteStartTag() + WriteHour() + "<span style=\"" + ERROR_STYLE + "\">" + body + "</span>" + WriteEndTag();
        }

        public static string WriteEndTag()
        {
            return "</div>";
        }
    }
}