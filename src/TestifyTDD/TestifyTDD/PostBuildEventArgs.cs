using System;

namespace TestifyTDD
{
    public class PostBuildEventArgs<TDOMAIN> : EventArgs
    {
        public TDOMAIN BuiltObject { get; set; }

        public PostBuildEventArgs(TDOMAIN domainObj)
        {
            BuiltObject = domainObj;
        }
    }
}