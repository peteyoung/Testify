using System.Diagnostics;
using System.Collections.Generic;
using NUnit.Framework;

namespace Spikes
{
    [TestFixture]
    public class GeneralSpike
    {
        [Test]
        public void Exclusive_or_operator_truth_table()
        {
            Debug.WriteLine("true ^ true = " + (true ^ true));
            Debug.WriteLine("true ^ false = " + (true ^ false));
            Debug.WriteLine("false ^ true = " + (false ^ true));
            Debug.WriteLine("false ^ false = " + (false ^ false));
        }

        [Test]
        public void List_indexer_set()
        {
            var list = new List<object> { new object() };
            Debug.WriteLine(string.Format("list size: {0}", list.Count));
            list[0] = new object();
            Debug.WriteLine(string.Format("list size: {0}", list.Count));
        }
    }
}
