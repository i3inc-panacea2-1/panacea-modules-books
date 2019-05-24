using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Panacea.Modules.Books.Models
{
    [DataContract]
    public class BooksGetFullResponse
    {
        [DataMember(Name = "Books")]
        public BooksObject Books { get; set; }
    }

    [DataContract]
    public class BooksObject
    {
        [DataMember(Name = "bookCategories")]
        public List<BookCategory> Categories { get; set; }
    }
}
