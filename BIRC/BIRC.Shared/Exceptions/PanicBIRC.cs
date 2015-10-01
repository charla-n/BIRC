using System;
using System.Collections.Generic;
using System.Text;

namespace BIRC.Shared.Exceptions
{
    public class PanicBIRC : Exception
    {
        public PanicBIRC(string msg) : base(msg)
        { }
    }
}
