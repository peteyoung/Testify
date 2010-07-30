using System;
using System.Collections.Generic;

namespace TestifyTDD.DITool
{
    public static class DIT
    {
        [ThreadStatic] private static Dictionary<Type, Type> _typeMappings;

        public static T GetInstance<T>() where T : new()
        {
            // lazy initialize the type mappings
            if (_typeMappings == null)
                InitializeTypeMappings();

            return new T();
        }

        private static void InitializeTypeMappings()
        {
            
        }
    }
}
