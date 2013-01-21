using System;
using System.Collections;
using System.Collections.Generic;

namespace TestifyTDD.DITool
{
    public class InitializerTypeList : IList<Type>
    {
        private List<Type> _initializerList = new List<Type>();

        private void ValidateTypeIsConcrete(Type type)
        {
            if (!type.IsConcrete())
                throw new ArgumentException(
                    string.Format(
                        "Types added to InitializerTypeList must be concrete. {0} is either an interface or abstract.",
                        type.Name));
        }

        private void ValidateTypeIsIInitializer(Type type)
        {
            var initializerInterface = typeof (IInitializer).Name;

            if (type.GetInterface(initializerInterface) == null)
                throw new ArgumentException(
                    string.Format(
                        "Type {0} does not implement {1}.",
                        type.Name,
                        initializerInterface));
        }

        public void Add(Type type)
        {
            ValidateTypeIsConcrete(type);
            ValidateTypeIsIInitializer(type);

            _initializerList.Add(type);
        }

        public void AddRange(List<Type> types)
        {
            foreach (var type in types)
            {
                ValidateTypeIsConcrete(type);
                ValidateTypeIsIInitializer(type);
            }

            _initializerList.AddRange(types);
        }

        public void AddRange(InitializerTypeList initializers)
        {
            _initializerList.AddRange(initializers);
        }

        public void Insert(int index, Type type)
        {
            ValidateTypeIsConcrete(type);
            ValidateTypeIsIInitializer(type);

            _initializerList.Insert(index, type);
        }

        public Type this[int index]
        {
            get { return _initializerList[index]; }
            set
            {
                ValidateTypeIsConcrete(value);
                ValidateTypeIsIInitializer(value);

                _initializerList[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            _initializerList.RemoveAt(index);
        }

        public void Clear()
        {
            _initializerList.Clear();
        }

        public bool Contains(Type type)
        {
            return _initializerList.Contains(type);
        }

        public void CopyTo(Type[] array, int arrayIndex)
        {
            _initializerList.CopyTo(array, arrayIndex);
        }

        public bool Remove(Type type)
        {
            return _initializerList.Remove(type);
        }

        public int Count
        {
            get { return _initializerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(Type type)
        {
            return _initializerList.IndexOf(type);
        }

        public IEnumerator<Type> GetEnumerator()
        {
            return _initializerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
