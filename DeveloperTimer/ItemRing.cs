using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DeveloperTimer
{
    public class ItemRing<T> : IList<T>
    {
        private readonly List<T> items;
        private int currentIndex;

        public ItemRing()
        {
            items = new List<T>();
        }

        public ItemRing(IEnumerable<T> items)
        {
            this.items = items.ToList();
        }

        public IEnumerable<T> Items
        {
            get { return items; }
        }

        public T Current
        {
            get
            {
                return !items.Any() 
                    ? default(T) 
                    : items[currentIndex];
            }
        }

        public T Next
        {
            get
            {
                return !items.Any() 
                    ? default(T) 
                    : items[IncrementIndex(currentIndex)];
            }
        }

        public void Increment()
        {
            currentIndex = IncrementIndex(currentIndex);
        }

        private int IncrementIndex(int startPtr)
        {
            return startPtr >= (Count - 1)
                       ? 0
                       : startPtr + 1;
        }

        private int DecrementIndex(int startPtr)
        {
            return startPtr <= 0
                       ? Count - 1
                       : startPtr - 1;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            items.Add(item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);

            if (index < currentIndex)
            {
                currentIndex--;
            }
            else if(index >= (Count - 1))
            {
                currentIndex = 0;
            }

            return items.Remove(item);
        }

        public int Count { get { return items.Count; } }
        public bool IsReadOnly { get { return ((IList<T>) items).IsReadOnly; } }

        public int CurrentIndex
        {
            get { return currentIndex; }
        }

        public int IndexOf(T item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Remove(this[index]);
        }

        public T this[int index]
        {
            get { return items[index]; }
            set { items[index] = value; }
        }

        public void MoveDown(T item)
        {
            var startIndex = IndexOf(item);
            var goalIndex = IncrementIndex(startIndex);

            var holding = this[goalIndex];

            this[goalIndex] = item;
            this[startIndex] = holding;
        }

        public void MoveUp(T item)
        {
            var startIndex = IndexOf(item);
            var goalIndex = DecrementIndex(startIndex);

            var holding = this[goalIndex];

            this[goalIndex] = item;
            this[startIndex] = holding;
        }
    }
}