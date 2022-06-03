using HukSleva.ViewModels.BookController;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HukSleva.Services
{
    public class BookViewModelsCollectionService : ICollection<BookViewModel>
    {
        
        LinkedList<BookViewModel> books = new LinkedList<BookViewModel>();
        public LinkedListNode<BookViewModel> First => books.First;
        public LinkedListNode<BookViewModel> Last => books.Last;
        public int Count => books.Count;

        public bool IsReadOnly => false;

        public void Add(BookViewModel item)
        {
            if (Contains(item) == true)
            {
                return;
            }

            books.AddLast(item);
        }

        public void Clear()
        {
            books.Clear();  
        }

        public bool Contains(BookViewModel item)
        {
            var f =  books.First;
            var next = f;
            while (next != null)
            {
                var v = f.Value;
                if (v.Id == item.Id)
                {
                    return true;
                }
                next = next.Next;
            }
            return false;
        }
        public void CopyTo(BookViewModel[] array, int arrayIndex)
        {
            if (array == null || arrayIndex < 0 || arrayIndex >= Count)
            {
                return;
            }
            try
            {
                books.CopyTo(array, arrayIndex);
            }
            catch
            {

            }
        }

        public IEnumerator<BookViewModel> GetEnumerator()
        {
            return books.GetEnumerator();
        }

        public bool Remove(BookViewModel item)
        {
            return books.Remove(item);
        }
        public void RemoveLast()
        {
            if (books.Last == null)
            {
                return;
            }

            books.RemoveLast();
        }
        public void RemoveFirst()
        {
            if (books.First == null)
            {
                return;
            }

            books.RemoveFirst();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return books.GetEnumerator();
        }
        public void AddRange(params BookViewModel[] item)
        {
            for (int i = 0; i < item.Length; i++)
            {
                Add(item[i]);
            }
        }
    }
}
