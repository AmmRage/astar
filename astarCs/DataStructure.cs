using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace astarCs
{
    public class NumList : IList<AsPoint>
    {
        private readonly SortedList<int, AsPoint> _sortedList;

        public int[] hashSearchArray;

        public NumList()
        {
            this._sortedList = new SortedList<int, AsPoint>();
        }

        public NumList(int capacity)
        {
            this._sortedList = new SortedList<int, AsPoint>();
            this.hashSearchArray = new int[capacity];
        }

        public IEnumerator<AsPoint> GetEnumerator()
        {
            return this._sortedList.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(AsPoint item)
        {
            if (!this._sortedList.ContainsKey(item.HashCode))
                this._sortedList.Add(item.HashCode, item);
        }

        public void Clear()
        {
            this._sortedList.Clear();
        }

        public bool Contains(AsPoint item)
        {
            return this._sortedList.ContainsKey(item.HashCode);
        }

        public void CopyTo(AsPoint[] array, int arrayIndex)
        {
            this._sortedList.Values.CopyTo(array, arrayIndex);
        }

        public bool Remove(AsPoint item)
        {
            return this._sortedList.Remove(item.HashCode);
        }

        public int Count {
            get
            {
                return this._sortedList.Count;
            } 
        }
        public bool IsReadOnly { get; private set; }
        public int IndexOf(AsPoint item)
        {
            return 0;
        }

        public void Insert(int index, AsPoint item)
        {
            Add(item);
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public AsPoint this[int index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void AddRange(IEnumerable<AsPoint> points)
        {
            foreach (var t in points)
                if (t != null)
                    Add(t);
        }
    }

    public class NumHashlist : IList<AsPoint>
    {
        private AsPoint[] _hashSearchArray;
        private int _count;

        public NumHashlist(int capacity)
        {
            this._hashSearchArray = new AsPoint[capacity];
            this._count = 0;
        }

        public IEnumerator<AsPoint> GetEnumerator()
        {
            return this._hashSearchArray.Where(p => p != null).GetEnumerator();
        }

        public void Add(AsPoint item)
        {
            if (this._hashSearchArray[item.HashCode] == null)
            {
                this._hashSearchArray[item.HashCode] = item;
                this._count += 1;
            }

        }

        public void Clear()
        {
            this._hashSearchArray = new AsPoint[this._hashSearchArray.Length];
            this._count = 0;
        }

        public bool Contains(AsPoint item)
        {
            return this._hashSearchArray[item.HashCode] != null;
        }

        public void CopyTo(AsPoint[] array, int arrayIndex)
        {
            this._hashSearchArray.TakeWhile(p => p!=null).ToArray().CopyTo(array, arrayIndex);
        }

        public bool Remove(AsPoint item)
        {
            if (this._hashSearchArray[item.HashCode] != null)
            {
                this._hashSearchArray[item.HashCode] = null;
                this._count -= 1;
                return true;
            }
            else
                return false;
        }

        public int Count
        {
            get
            {
                return this._count;
            }
        }
        public bool IsReadOnly { get; private set; }
        public int IndexOf(AsPoint item)
        {
            return this._hashSearchArray.TakeWhile(p => p != null).ToList().IndexOf(item);
        }

        public void Insert(int index, AsPoint item)
        {
            Add(item);
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public AsPoint this[int index]
        {
            get { throw new NotImplementedException(); }
            set {  }
        }

        public void AddRange(IEnumerable<AsPoint> points)
        {
            foreach (var t in points)
                if (t != null)
                    Add(t);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}