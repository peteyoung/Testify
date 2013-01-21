using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NBehave.Spec.NUnit;
using TestifyTDD.DITool;

namespace Tests.DITool
{
    [TestFixture]
    public class InitializerTypeListSpec
    {
        [Test]
        public void Object_initializer_should_add_initializers_to_collection()
        {
            // Arrange 
            var initializerStubType = typeof(InitializerStub);
            var initializerDummyType = typeof(InitializerDummy);
            
            // Act
            var initializerTypeList = new InitializerTypeList
                                          {
                                              initializerStubType,
                                              initializerDummyType,
                                              initializerStubType,
                                              initializerDummyType
                                          };

            // Assert
            initializerTypeList.Count.ShouldEqual(4);
            initializerTypeList[0].ShouldEqual(initializerStubType);
            initializerTypeList[1].ShouldEqual(initializerDummyType);
            initializerTypeList[2].ShouldEqual(initializerStubType);
            initializerTypeList[3].ShouldEqual(initializerDummyType);
        }

        [Test]
        public void Object_initializer_should_throw_ArgumentException_if_passed_an_interface()
        {
            Assert.Throws<ArgumentException>(
                () => 
                    new InitializerTypeList
                        {
                            typeof (IInitializer)
                        });
        }

        [Test]
        public void Object_initializer_should_throw_ArgumentException_if_passed_an_abstract_class()
        {
            Assert.Throws<ArgumentException>(
                () =>
                    new InitializerTypeList
                        {
                            typeof (AbstractInitializer)
                        });
        }

        [Test]
        public void Add_should_add_initializer_to_collection()
        {
            // Arrange
            var initializerType = typeof(InitializerStub);
            var initializerTypeList = new InitializerTypeList();

            // Act
            initializerTypeList.Add(initializerType);

            // Assert
            initializerTypeList.Count.ShouldEqual(1);
            initializerTypeList[0].ShouldEqual(initializerType);
        }

        [Test]
        public void Add_should_throw_ArgumentException_if_passed_an_interface()
        {
            // Arrange
            var initializerType = typeof(IInitializer);
            var initializerTypeList = new InitializerTypeList();

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => initializerTypeList.Add(initializerType));
        }

        [Test]
        public void Add_should_throw_ArgumentException_if_passed_an_abstract_class()
        {
            // Arrange
            var initializerType = typeof(AbstractInitializer);
            var initializerTypeList = new InitializerTypeList();

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => initializerTypeList.Add(initializerType));
        }

        [Test]
        public void AddRange_should_add_initializers_to_collection()
        {
            // Arrange
            var initializerTypeList = new InitializerTypeList
                                          {
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy)
                                          };

            var moreInitializers = new InitializerTypeList
                                       {
                                           typeof (InitializerStub),
                                           typeof (InitializerStub)
                                       };

            // Act
            initializerTypeList.AddRange(moreInitializers);

            // Assert
            initializerTypeList.Count.ShouldEqual(5);
            initializerTypeList[0].ShouldEqual(typeof(InitializerDummy));
            initializerTypeList[1].ShouldEqual(typeof(InitializerDummy));
            initializerTypeList[2].ShouldEqual(typeof(InitializerDummy));
            initializerTypeList[3].ShouldEqual(typeof(InitializerStub));
            initializerTypeList[4].ShouldEqual(typeof(InitializerStub));
        }

        [Test]
        public void AddRange_should_add_initializer_types_to_collection_from_list()
        {
            // Arrange
            var initializerTypeList = new InitializerTypeList
                                          {
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy)
                                          };

            var moreInitializers = new List<Type>
                                       {
                                           typeof (InitializerStub),
                                           typeof (InitializerStub)
                                       };

            // Act
            initializerTypeList.AddRange(moreInitializers);

            // Assert
            initializerTypeList.Count.ShouldEqual(5);
            initializerTypeList[0].ShouldEqual(typeof(InitializerDummy));
            initializerTypeList[1].ShouldEqual(typeof(InitializerDummy));
            initializerTypeList[2].ShouldEqual(typeof(InitializerDummy));
            initializerTypeList[3].ShouldEqual(typeof(InitializerStub));
            initializerTypeList[4].ShouldEqual(typeof(InitializerStub));
        }

        [Test]
        public void AddRange_should_throw_ArgumentException_if_passed_an_interface()
        {
            // Arrange
            var initializerType = typeof(IInitializer);
            var initializerTypeList = new InitializerTypeList
                                          {
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy)
                                          };

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => initializerTypeList.Add(initializerType));
        }

        [Test]
        public void AddRange_should_throw_ArgumentException_if_passed_an_abstract_class()
        {
            // Arrange
            var initializerType = typeof(AbstractInitializer);
            var initializerTypeList = new InitializerTypeList
                                          {
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy)
                                          };

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => initializerTypeList.Add(initializerType));
        }

        [Test]
        public void Insert_should_add_initializer_to_collection()
        {
            // Arrange
            var initializerType = typeof(InitializerStub);
            var initializerTypeList = new InitializerTypeList
                                          {
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy)
                                          };

            // Act
            initializerTypeList.Insert(1, initializerType);

            // Assert
            initializerTypeList.Count.ShouldEqual(4);
            initializerTypeList[1].ShouldEqual(initializerType);
        }

        [Test]
        public void Insert_should_throw_ArgumentException_if_passed_an_interface()
        {
            // Arrange
            var initializerType = typeof(IInitializer);
            var initializerTypeList = new InitializerTypeList
                                          {
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy)
                                          };

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => initializerTypeList.Insert(1, initializerType));
        }

        [Test]
        public void Insert_should_throw_ArgumentException_if_passed_an_abstract_class()
        {
            // Arrange
            var initializerType = typeof(AbstractInitializer);
            var initializerTypeList = new InitializerTypeList
                                          {
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy)
                                          };

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => initializerTypeList.Insert(1, initializerType));
        }

        [Test]
        public void Indexer_set_should_add_initializer_to_collection()
        {
            // Arrange
            var initializerType = typeof(InitializerStub);
            var initializerTypeList = new InitializerTypeList
                                          {
                                              typeof (InitializerDummy)
                                          };

            // Act
            initializerTypeList[0] = initializerType;

            // Assert
            initializerTypeList.Count.ShouldEqual(1);
            initializerTypeList[0].ShouldEqual(initializerType);
        }

        [Test]
        public void Indexer_get_should_add_initializer_to_collection()
        {
            // Arrange
            var initializerType = typeof(InitializerStub);
            var initializerTypeList = new InitializerTypeList
                                          {
                                              initializerType
                                          };

            // Act
            var foundInitializerType = initializerTypeList[0];

            // Assert
            foundInitializerType.ShouldEqual(initializerType);
        }

        [Test]
        public void Indexer_set_should_throw_ArgumentException_if_passed_an_interface()
        {
            // Arrange
            var initializerType = typeof(IInitializer);
            var initializerTypeList = new InitializerTypeList();

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => initializerTypeList[0] = initializerType);
        }

        [Test]
        public void Indexer_set_should_throw_ArgumentException_if_passed_an_abstract_class()
        {
            // Arrange
            var initializerType = typeof(AbstractInitializer);
            var initializerTypeList = new InitializerTypeList();

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => initializerTypeList[0] = initializerType);
        }

        [Test]
        public void RemoveAt_should_remove_item_at_index()
        {
            // Arrange
            var initializerTypeList = new InitializerTypeList
                                          {
                                              typeof (InitializerDummy),
                                              typeof (InitializerStub),
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy)
                                          };

            // Act
            initializerTypeList.RemoveAt(1);

            // Assert
            initializerTypeList.Count.ShouldEqual(3);

            foreach (var item in initializerTypeList)
                item.ShouldNotEqual(typeof(InitializerStub));
        }

        [Test]
        public void Contains_should_return_true_if_collection_contains_type()
        {
            // Arrange
            var initializerTypeList = new InitializerTypeList
                                          {
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy)
                                          };

            // Act
            var doesContain = initializerTypeList.Contains(typeof(InitializerDummy));

            // Assert
            doesContain.ShouldBeTrue();
        }

        [Test]
        public void Contains_should_return_false_if_collection_doesnt_contains_type()
        {
            // Arrange
            var initializerTypeList = new InitializerTypeList
                                          {
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy)
                                          };

            // Act
            var doesContain = initializerTypeList.Contains(typeof(InitializerStub));

            // Assert
            doesContain.ShouldBeFalse();
        }

        [Test]
        public void CopyTo_should_copy_elements_from_collection_to_array()
        {
            // Arrange
            var copyToMe = new Type[6];
            var initializerTypeList = new InitializerTypeList
                                          {
                                              typeof (InitializerDummy),
                                              typeof (InitializerStub),
                                              typeof (InitializerStub),
                                              typeof (InitializerDummy)
                                          };

            // Act
            initializerTypeList.CopyTo(copyToMe, 1);

            // Assert
            copyToMe[0].ShouldBeNull();
            copyToMe[1].ShouldEqual(typeof(InitializerDummy));
            copyToMe[2].ShouldEqual(typeof(InitializerStub));
            copyToMe[3].ShouldEqual(typeof(InitializerStub));
            copyToMe[4].ShouldEqual(typeof(InitializerDummy));
            copyToMe[5].ShouldBeNull();
        }

        [Test]
        public void Remove_should_remove_first_element_of_a_type()
        {
            // Arrange
            var initializerTypeList = new InitializerTypeList
                                          {
                                              typeof (InitializerDummy),
                                              typeof (InitializerStub),
                                              typeof (InitializerDummy),
                                              typeof (InitializerStub)
                                          };

            // Act
            initializerTypeList.Remove(typeof (InitializerStub));

            // Assert
            initializerTypeList.Count.ShouldEqual(3);
            initializerTypeList[0].ShouldEqual(typeof(InitializerDummy));
            initializerTypeList[1].ShouldEqual(typeof(InitializerDummy));
            initializerTypeList[2].ShouldEqual(typeof(InitializerStub));
        }

        [Test]
        public void IsReadOnly_should_return_false()
        {
            new InitializerTypeList().IsReadOnly.ShouldBeFalse();
        }

        [Test]
        public void IndexOf_should_return_index_of_first_occurence_of_type()
        {
            // Arrange
            var initializerTypeList = new InitializerTypeList
                                          {
                                              typeof (InitializerDummy),
                                              typeof (InitializerStub),
                                              typeof (InitializerDummy),
                                              typeof (InitializerStub)
                                          };

            // Act
            var index = initializerTypeList.IndexOf(typeof(InitializerStub));

            // Assert
            index.ShouldEqual(1);
        }

        [Test]
        public void Generic_GetEnumerator_should_return_enumerator_of_type_Type()
        {
            // Arrange
            var initializerTypeList = new InitializerTypeList
                                          {
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy)
                                          };

            // Act
            var enumerator = initializerTypeList.GetEnumerator();

            // Assert
            enumerator.GetType().ShouldEqual(typeof (List<Type>.Enumerator));

            var itemCount = 0;
            while (enumerator.MoveNext())
            {
                enumerator.Current.ShouldEqual(typeof(InitializerDummy));
                itemCount++;
            }

            itemCount.ShouldEqual(4);
        }

        [Test]
        public void GetEnumerator_should_return_enumerator()
        {
            // Arrange
            var initializerTypeList = new InitializerTypeList
                                          {
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy),
                                              typeof (InitializerDummy)
                                          };

            // Act
            var enumerator = ((IEnumerable)initializerTypeList).GetEnumerator();

            // Assert
            enumerator.GetType().ShouldEqual(typeof(List<Type>.Enumerator));

            var itemCount = 0;
            while (enumerator.MoveNext())
            {
                enumerator.Current.ShouldEqual(typeof (InitializerDummy));
                itemCount++;
            }

            itemCount.ShouldEqual(4);
        }
    }

    internal abstract class AbstractInitializer : IInitializer
    {
        public abstract void InitializeTypeMapper(ITypeMapper typeMapper);
    }

    internal class InitializerDummy : AbstractInitializer
    {
        public override void InitializeTypeMapper(ITypeMapper typeMapper){}
    }
}
