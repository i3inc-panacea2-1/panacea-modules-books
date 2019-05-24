using Panacea.Modules.Books.Models;
using Panacea.ContentControls;
using Panacea.Core;

namespace Panacea.Modules.Books
{
    public class BooksProvider : HospitalServerLazyItemProvider<Book>
    {
        public BooksProvider(PanaceaServices core)
            : base(core, "e-books/get_categories_only/", "e-books/get_category_limited/{0}/{1}/{2}/", "e-books/find_books/{0}/{1}/{2}/{3}/", 10)
        {

        }
    }
}
