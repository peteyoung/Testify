using System;
using System.Collections;
using System.Collections.Generic;
using TestifyTDD.DITool;

namespace TestifyTDD
{
    public class CollectionMappingInitializer : IInitializer
    {
        public void InitializeTypeMapper(ITypeMapper typeMapper)
        {
            typeMapper.Map<IList, ArrayList>();
            typeMapper.Map<IDictionary, Hashtable>();

            typeMapper.Map(typeof(IList<>), typeof(List<>));
            typeMapper.Map(typeof(IDictionary<,>), typeof(Dictionary<,>));
        }
    }
}
