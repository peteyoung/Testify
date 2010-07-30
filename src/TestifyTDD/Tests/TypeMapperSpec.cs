using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NBehave.Spec.NUnit;
using TestifyTDD;

namespace Tests
{
    [TestFixture]
    public class TypeMapperSpec
    {
        private TypeMapper _mapper;
        
        [SetUp]
        public void SetUp()
        {
            _mapper = new TypeMapper();

            _mapper.Map(typeof (IList), typeof (ArrayList));
            _mapper.Map(typeof (IList<>), typeof (List<>));
        }

        [Test]
        public void Mapping_from_concrete_type_should_result_in_exception()
        {
            Assert.Throws<ArgumentException>(
                () => _mapper.Map(typeof (String), typeof (String)));
        }

        [Test]
        public void Non_generic_interface_should_map_to_non_generic_class()
        {
            // Arrange & Act
            var typeToBuild = _mapper.Resolve(typeof (IList));

            // Assert
            typeToBuild.ShouldNotBeNull();
            typeToBuild.ShouldEqual(typeof (ArrayList));
        }

        [Test]
        public void Generic_interface_should_map_to_generic_class_with_same_generic_arguments()
        {
            // Arrange & Act
            var typeToBuild = _mapper.Resolve(typeof (IList<Int32>));

            // Assert
            typeToBuild.ShouldNotBeNull();
            typeToBuild.ShouldEqual(typeof (List<Int32>));
        }

        [Test]
        public void Resolving_non_generic_concrete_type_should_pass_through_type()
        {
            // Arrange & Act
            var typeToBuild = _mapper.Resolve(typeof(SortedList));

            // Assert
            typeToBuild.ShouldNotBeNull();
            typeToBuild.ShouldEqual(typeof(SortedList));
        }

        [Test]
        public void Resolving_generic_concrete_type_should_pass_through_type()
        {
            // Arrange & Act
            var typeToBuild = _mapper.Resolve(typeof(SortedList<string, int>));

            // Assert
            typeToBuild.ShouldNotBeNull();
            typeToBuild.ShouldEqual(typeof(SortedList<string, int>));
        }
    }
}
