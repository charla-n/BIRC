using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BIRC.Shared.Utils
{
    public class HistoryList : IList<string>
    {
        private static int MAX_SIZE = 20;
        private List<string> list;
        private int index;

        public HistoryList()
        {
            index = -1;
            list = new List<string>(MAX_SIZE);
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
            index--;
            if (index < 0)
                index = 0;
            string res = list.ElementAtOrDefault(index);
            if (res == null)
                return string.Empty;
            return res;
        }

        public string DownHistory()
        {
            index++;
            if (index > list.Count)
                index = list.Count;
            string res = list.ElementAtOrDefault(index);
            if (res == null)
                return string.Empty;
            return res;
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
                list.RemoveAt(0);
            list.Add(item);
            index = list.Count;
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
