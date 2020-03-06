namespace Publisher
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;

    class CircularBuffer<T> : ICollection<T>, ICollection where T : class
    {
        int capacity;
        int size;
        int head;
        int tail;
        T[] buffer;
        object syncRoot;
        object bufferLock = new object();

        public CircularBuffer(int capacity, bool allowOverflow = false)
        {
            if (capacity < 0)
            {
                throw new ArgumentException("Capacity can not be less than zero", nameof(capacity));
            }

            this.capacity = capacity;
            size = 0;
            head = 0;
            tail = 0;
            buffer = new T[capacity];
            AllowOverflow = allowOverflow;
        }

        bool AllowOverflow
        {
            get;
            set;
        }

        public int Capacity
        {
            get { return capacity; }
            set
            {
                if (value == capacity)
                    return;

                if (value < size)
                    throw new ArgumentOutOfRangeException(nameof(value), "Cannot reduce capacity below current size");

                var dst = new T[value];
                if (size > 0)
                    CopyTo(dst);
                buffer = dst;

                capacity = value;
            }
        }

        public int Size => size;

        public bool Contains(T item)
        {
            var bufferIndex = head;
            var comparer = EqualityComparer<T>.Default;
            for (var i = 0; i < size; i++, bufferIndex++)
            {
                if (bufferIndex == capacity)
                    bufferIndex = 0;

                if (item == null && buffer[bufferIndex] == null)
                    return true;

                if ((buffer[bufferIndex] != null) &&
                    comparer.Equals(buffer[bufferIndex], item))
                    return true;
            }

            return false;
        }

        public void Clear()
        {
            size = 0;
            head = 0;
            tail = 0;
        }

        public int Put(T[] src)
        {
            return Put(src, 0, src.Length);
        }

        public int Put(T[] src, int offset, int count)
        {
            if (!AllowOverflow && count > capacity - size)
                throw new InvalidOperationException("Overflow is not allowed");

            var srcIndex = offset;
            for (var i = 0; i < count; i++, tail++, srcIndex++)
            {
                if (tail == capacity)
                    tail = 0;
                buffer[tail] = src[srcIndex];
            }
            size = Math.Min(size + count, capacity);
            return count;
        }

        public void Put(T item)
        {
            if (!AllowOverflow && size == capacity)
                throw new InvalidOperationException("Overflow is not allowed");

            lock (bufferLock)
            {
                buffer[tail] = item;
                if (++tail == capacity)
                    tail = 0;
                size++;
            }
        }

        public void Skip(int count)
        {
            head += count;
            if (head >= capacity)
            {
                head -= capacity;
            }
        }

        public T[] Get(int count)
        {
            var dst = new T[count];
            Get(dst);
            return dst;
        }

        public int Get(T[] dst)
        {
            return Get(dst, 0, dst.Length);
        }

        public int Get(T[] dst, int offset, int count)
        {
            if (size == 0)
                throw new InvalidOperationException("Buffer is empty");

            var realCount = Math.Min(count, size);
            var dstIndex = offset;
            for (var i = 0; i < realCount; i++, head++, dstIndex++)
            {
                if (head == capacity)
                    head = 0;
                dst[dstIndex] = buffer[head];
            }
            return realCount;
        }

        public T Get()
        {
            if (size == 0)
            {
                throw new InvalidOperationException("Buffer is empty");
            }

            lock (bufferLock)
            {
                var item = buffer[head];
                if (++head == capacity)
                    head = 0;

                return item;
            }
        }

        public void CopyTo(T[] array)
        {
            CopyTo(array, 0);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(0, array, arrayIndex, size);
        }

        void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            if (count > size)
                throw new ArgumentOutOfRangeException("count", "Message read count is larger than size");

            var bufferIndex = head;
            for (var i = 0; i < count; i++, bufferIndex++, arrayIndex++)
            {
                if (bufferIndex == capacity)
                    bufferIndex = 0;
                array[arrayIndex] = buffer[bufferIndex];
            }
        }

        IEnumerator<T> GetEnumerator()
        {
            var bufferIndex = head;
            for (var i = 0; i < size; i++, bufferIndex++)
            {
                if (bufferIndex == capacity)
                    bufferIndex = 0;

                yield return buffer[bufferIndex];
            }
        }

        public T[] GetBuffer()
        {
            return buffer;
        }

        public T[] ToArray()
        {
            var dst = new T[size];
            CopyTo(dst);
            return dst;
        }

        int ICollection<T>.Count => Size;

        bool ICollection<T>.IsReadOnly => false;

        void ICollection<T>.Add(T item)
        {
            Put(item);
        }

        bool ICollection<T>.Remove(T item)
        {
            if (size == 0)
            {
                return false;
            }

            Get();
            return true;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        int ICollection.Count => Size;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot
        {
            get
            {
                if (syncRoot == null)
                {
                    Interlocked.CompareExchange(ref syncRoot, new object(), null);
                }
                return syncRoot;
            }
        }

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            CopyTo((T[])array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}