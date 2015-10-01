using System;
using System.Collections.Generic;
using System.Text;

namespace BIRC.Shared.Exceptions
{
    public class ErrorBIRC : Exception
    {
        public ErrorBIRC(string msg) : base(msg)
        { }
    }
}
