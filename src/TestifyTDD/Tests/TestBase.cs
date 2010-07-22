using System.Linq.Expressions;
using System.Reflection;
using Rhino.Mocks;

namespace Tests
{
    public class TestBase
    {
        internal static T M<T>(params object[] argumentsForConstructor) where T : class
        {
            return MockRepository.GenerateMock<T>(argumentsForConstructor);
        }

        internal static T S<T>(params object[] argumentsForConstructor) where T : class
        {
            return MockRepository.GenerateStub<T>(argumentsForConstructor);
        }
    }
}