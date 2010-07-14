using System.Diagnostics;
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
    }
}
