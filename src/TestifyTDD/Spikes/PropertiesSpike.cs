using System;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NBehave.Spec.NUnit;

namespace Spikes
{
    [TestFixture]
    public class PropertiesSpike
    {
        [Test]
        public void Get_all_properties_of_a_class()
        {
            // Arrange
            var cwmpType = typeof(ClassWithManyProperties);

            // Act
//            var properties = cwmpType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var properties = cwmpType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            // Assert
            properties.ShouldNotBeNull();
            properties.Length.ShouldEqual(8);

            for (var idx = 0; idx < 7; idx++)
                properties[idx].Name.ShouldEqual("Property_" + (idx));
        }

        [Test]
        public void Get_all_properties_of_a_class_with_public_setters()
        {
            // Arrange
            var cwmpType = typeof(ClassWithManyProperties);

            // Act
            var allPropertiesArray = cwmpType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var allProperties = new System.Collections.Generic.List<PropertyInfo>(allPropertiesArray);
            var properties = allProperties.Where(p => p.GetSetMethod(true) != null && p.GetSetMethod(true).IsPublic).ToList();

            // Assert
            properties.ShouldNotBeNull();
            properties.Count().ShouldEqual(6);

            for (var idx = 0; idx < 6; idx++)
            {
                var i = idx;
                var property = properties.Where(p => p.Name == "Property_" + i).ToList();
                property.Count().ShouldEqual(1);
            }
        }

        [Test]
        public void Get_property_info_using_lambda()
        {
            // Arrange & Act
            var helper = new PropertyHelperSpike<ClassWithManyProperties>();

            var pi1 = helper.GetPropertyInfo(c => c.Property_5);
            //var pi2 = helper.GetPropertyInfo(c => c.Latitude * 10);
            //var foo = helper.GetPropertyInfo(c => c.GetNeighboringAddress());

            // Assert
            pi1.ShouldNotBeNull();
            pi1.Name.ShouldEqual("Property_5");
        }

        [Test]
        public void Does_getting_PropertyInfo_return_the_same_instance()
        {
            // Arrange
            var cwmpType = typeof(ClassWithManyProperties);
            var helper = new PropertyHelperSpike<ClassWithManyProperties>();

            // Act
            var propertyInfo_1 = cwmpType.GetProperty("Property_2", BindingFlags.Instance | BindingFlags.Public);
            var propertyInfo_2 = helper.GetPropertyInfo(c => c.Property_2);

            // Assert
            propertyInfo_1.ShouldBeTheSameAs(propertyInfo_2);
            propertyInfo_1.ShouldEqual(propertyInfo_2);
            propertyInfo_1.GetHashCode().ShouldEqual(propertyInfo_2.GetHashCode());
        }

        [Test]
        public void Can_set_property_value()
        {
            // Arrange
            var longVal = 12345L;

            var helper = new PropertyHelperSpike<ClassWithManyProperties>();
            var propertyInfo = helper.GetPropertyInfo(c => c.Property_2);
            var setter = helper.GetValueSetter(propertyInfo);

            var instance = new ClassWithManyProperties();

            // Act
            setter(instance, longVal);

            // Assert
            instance.Property_2.ShouldEqual(longVal);
        }

        [Test]
        public void Can_set_private_setter_property_value()
        {
            // Arrange
            var intVal = 123;

            var helper = new PropertyHelperSpike<ClassWithManyProperties>();
            var propertyInfo = helper.GetPropertyInfo(c => c.Property_6);
            var setter = helper.GetValueSetter(propertyInfo);

            var instance = new ClassWithManyProperties();

            // Act
            setter(instance, intVal);

            // Assert
            instance.Property_6.ShouldEqual(intVal);
        }

        [Test]
        public void Cant_set_property_value_with_wrong_argument_type()
        {
            // Arrange
            var stringVal = "I'm a string.";

            var helper = new PropertyHelperSpike<ClassWithManyProperties>();
            var propertyInfo = helper.GetPropertyInfo(c => c.Property_2);
            var setter = helper.GetValueSetter(propertyInfo);

            var instance = new ClassWithManyProperties();

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => setter(instance, stringVal));
        }


        internal class ClassWithManyProperties
        {
            private string _asecret;
            private string PrivateProperty { get; set; }
            protected string ProtectedProperty { get; set; }
            public string Property_0 { get; set; }
            public int Property_1 { get; set; }
            public long Property_2 { get; set; }
            public bool Property_3 { get; set; }
            public string Property_4 { get; set; }
            public decimal Property_5 { get; set; }
            public int Property_6 { get; private set; }
            public string Property_7 { get { return "I have no setter"; } }
            public string OohAMethod() { return "Ooh, a method!"; }
        }
    }

    internal class PropertyHelperSpike<T>
    {
        // http://stackoverflow.com/questions/491429/how-to-get-the-propertyinfo-of-a-specific-property
        public PropertyInfo GetPropertyInfo<TRETURNS>(
            Expression<Func<T, TRETURNS>> selector)
        {
            Expression body = selector;

            if (body is LambdaExpression)
                body = ((LambdaExpression)body).Body;

            if (body.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException(
                    string.Format("selector must be a property. Please specify a property of type {0}", typeof(T)));
            
            return (PropertyInfo)((MemberExpression)body).Member;
        }

        // http://weblogs.asp.net/marianor/archive/2009/04/10/using-expression-trees-to-get-property-getter-and-setters.aspx
        public Func<T, object> GetValueGetter(PropertyInfo propertyInfo)
        {
            if (typeof(T) != propertyInfo.DeclaringType)
                throw new ArgumentException();

//            var returnType = propertyInfo.PropertyType;

            var instance = Expression.Parameter(propertyInfo.DeclaringType, "i");
            var property = Expression.Property(instance, propertyInfo);
            var convert = Expression.TypeAs(property, typeof(object));

            var func = (Func<T, object>) Expression.Lambda(convert, instance).Compile();
            return func;
        }

        // http://weblogs.asp.net/marianor/archive/2009/04/10/using-expression-trees-to-get-property-getter-and-setters.aspx
        public Action<T, object> GetValueSetter(PropertyInfo propertyInfo)
        {
            if (typeof(T) != propertyInfo.DeclaringType)
                throw new ArgumentException();

            var instance = Expression.Parameter(propertyInfo.DeclaringType, "i");
            var argument = Expression.Parameter(typeof(object), "a");

            var setterCall = Expression.Call(
                instance,
                propertyInfo.GetSetMethod(true),
                Expression.Convert(argument, propertyInfo.PropertyType));

            var action = (Action<T, object>) Expression.Lambda(setterCall, instance, argument).Compile();
            return action;
        }
    }
}