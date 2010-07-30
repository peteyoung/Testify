using System.Reflection;

namespace TestifyTDD.PropertySetters
{
    public static class BuilderExtensionsForSetters
    {
        // TODO change the signature from T Build() to object Build(). Then get rid of this?
        public static object InvokeBuildMethod(this ITestDataBuilder builder)
        {
            var buildMethod = builder
                                .GetType()
                                .GetMethod("Build",
                                           BindingFlags.Instance | BindingFlags.Public);

            var value = buildMethod.Invoke(builder, null);

            return value;
        }
    }
}
