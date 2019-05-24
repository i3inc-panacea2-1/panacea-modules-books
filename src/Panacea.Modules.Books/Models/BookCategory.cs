using Panacea.Models;
using Panacea.Multilinguality;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Panacea.Modules.Books.Models
{
    public class BookCategory : ServerGroupItem
    {
        private List<BookItem> _books;

        [IsTranslatable]
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "books")]
        public List<BookItem> Books
        {
            get { return _books; }
            set
            {
                if (value != _books)
                {
                    _books = value;
                    OnPropertyChanged("Books");
                }
            }
        }
    }
    public class BookItem
    {
        public Book book { get; set; }
        public int priority { get; set; }
    }
}
