using System;
using System.Diagnostics;
using NUnit.Framework;
//using TestifyTDD;

namespace Spikes.DI
{
    [TestFixture]
    public class AppDomainSpike
    {
        [Test]
        public void List_all_assemblies_loaded_into_AppDomain()
        {
            var appDomain = AppDomain.CurrentDomain;
            
            foreach(var assembly in appDomain.GetAssemblies())
            {
//                Debug.WriteLine(
//                    string.Format("Assembly: {0}", assembly.FullName));
                var assemblyName = assembly.GetName();
                Debug.WriteLine(assemblyName.Name);
            }
        }
    }
}
