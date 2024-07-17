using System;
using System.Collections.Generic;
using System.Linq;

namespace IziHardGames.Unsafe
{
    public unsafe ref struct StackListManaged<T> where T : class
    {
        public StackListManaged<T>* pointerToPrevious;
        public readonly T* pointerToValue;
        public int index;
#if DEBUG
        private bool isAsignedAddress = false;
#endif
        public StackListManaged(int index, T* pointerToValue)
        {
            this.index = index;
            this.pointerToValue = pointerToValue;
        }

        public void SetAddressAtStack(StackListManaged<T>* pointer)
        {
#if DEBUG
            isAsignedAddress = true;
#endif
            pointerToPrevious = pointer;
        }

        public bool Contains(T compare)
        {
#if DEBUG
            if (!isAsignedAddress) throw new InvalidOperationException("Pointer is not asigned");
#endif
            for (int i = index; i >= 0; i--)
            {
                if (GetValue().Equals(compare)) return true;
            }

            return false;
        }

        public StackListManaged<T>* First(T compare)
        {
#if DEBUG
            if (!isAsignedAddress) throw new InvalidOperationException("Pointer is not asigned");
#endif
            for (int i = index; i >= 0; i--)
            {
                if (GetValue().Equals(compare)) return pointerToPrevious;
            }

            throw new ArgumentOutOfRangeException();
        }

        public StackListManaged<T> Add(T* value)
        {
            return new StackListManaged<T>(index + 1, value);
        }

        public T GetValue()
        {
            return *pointerToValue;
        }

#if DEBUG
        private unsafe static void WorkAround()
        {
            List<Type> items = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
            Type first = items.First();
            StackListManaged<Type> head = new StackListManaged<Type>(0, &first);
            CallRecursive(items.First(), head);
        }

        private static unsafe void CallRecursive(Type value, StackListManaged<Type> item)
        {
            var props = value.GetProperties();
        }
#endif
    }
}