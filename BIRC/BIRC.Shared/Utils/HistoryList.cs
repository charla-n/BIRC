using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BIRC.Shared.Utils
{
    public class HistoryList : IList<string>
    {
        private static int MAX_SIZE = 50;
        private List<string> list;
        private int index;

        public HistoryList()
        {
            index = -1;
            list = new List<string>();
        }

        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string UpHistory()
        {
            index++;
            if (index > (MAX_SIZE - 1))
                index = MAX_SIZE - 1;
            if (index == list.Count())
                index--;
            return list.ElementAtOrDefault(index);
        }

        public string DownHistory()
        {
            index--;
            if (index < -1)
                index = -1;
            return list.ElementAtOrDefault(index);
        }

        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int IndexOf(string item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, string item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Add(string item)
        {
            if (list.Count == MAX_SIZE)
                list.RemoveAt(MAX_SIZE - 1);
            index = -1;
            list.Add(item);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(string item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<string> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
