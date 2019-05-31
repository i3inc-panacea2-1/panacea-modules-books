using Panacea.Controls;
using Panacea.Core;
using Panacea.Models;
using Panacea.Modules.Books.Models;
using Panacea.Modules.Books.Views;
using Panacea.Mvvm;
using System.Linq;
using System.Windows.Input;

namespace Panacea.Modules.Books.ViewModels
{
    [View(typeof(BookList))]
    public class BookListViewModel : ViewModelBase
    {
        private readonly PanaceaServices _core;

        private BooksPlugin _plugin;

        public BooksProvider Provider { get; private set; }

        public BookListViewModel(PanaceaServices core, BooksPlugin plugin)
        {
            _plugin = plugin;
            _core = core;
            Provider = plugin.Provider;
            ItemClickCommand = new RelayCommand(async arg =>
            {
                if (plugin == null) return;
                if ((arg as Book).DataUrl.Any((du) => du.DataType == "file"))
                {
                    await plugin.ReadBookAsync(arg as Book);
                }
                else if ((arg as Book).DataUrl.Any((du) => du.DataType == "audio"))
                {
                    await plugin.ListenToBookAsync(arg as Book);
                }
                else if ((arg as Book).DataUrl.Any((du) => du.DataType == "url"))
                {
                    await plugin.GoToBookAsync(arg as Book);
                }
            });
            InfoClickCommand = new RelayCommand((arg) =>
            {
                if (plugin == null) return;
                plugin.OpenItem(arg as Book);
            });

            //TODO: WhenFavorites
            IsFavoriteCommand = new RelayCommand((arg) => { _core.Logger.Debug(this, "TODO: FAVORITES"); });
            //IsFavoriteCommand = new RelayCommand((arg) =>
            //{
            //}, (arg) =>
            //{
            //    var book = arg as Book;
            //    if (plugin.Favorites == null) return false;
            //    return plugin.Favorites.Any(mm => mm.Id == book.Id);
            //});
            FavoriteCommand = new RelayCommand((arg) => { _core.Logger.Debug(this, "TODO: FAVORITES"); });
            //FavoriteCommand = new RelayCommand((args) =>
            //{
            //    var book = args as Book;
            //    if (book == null) return;
            //    try
            //    {
            //        var pluginName = "Books";
            //        if (userManager.User.ID == null)
            //        {
            //            window.RequestLogin(new Translator(pluginName).Translate("You need an account to add favorites"),
            //                () => { window.ThemeManager.Navigate(new BooksList()); }, null);
            //            return;
            //        }
            //        if (plugin.Favorites.Any(mm => mm.Id == book.Id))
            //        {
            //            _server.FavoriteRemove("Books", book.Id);
            //            plugin.Favorites.Remove(plugin.Favorites.First(mm => mm.Id == book.Id));
            //            window.ThemeManager.Toast(new Translator("Books").Translate("This book has been removed from your favorites"));
            //            //todo (sender as Button).Background = new SolidColorBrush((Application.Current.Resources["ColorInformation"] as SolidColorBrush).Color);
            //        }
            //        else
            //        {
            //            _server.FavoriteNotify("Books", book.Id);
            //            plugin.Favorites.Add(book);
            //            window.ThemeManager.Toast(new Translator("Books").Translate("This book has been added to your favorites"));
            //            //todo (sender as Button).Background = new SolidColorBrush((Application.Current.Resources["ColorError"] as SolidColorBrush).Color);
            //        }
            //        _webSocket.Emit("set_cookie",
            //            new { pluginName = "Radio", user = userManager.User.ID, data = plugin.Favorites });
            //        OnPropertyChanged("IsFavoriteCommand");
            //    }
            //    catch
            //    {
            //    }
            //});
        }

        public ICommand ItemClickCommand { get; private set; }
        public ICommand InfoClickCommand { get; protected set; }
        public ICommand FavoriteCommand { get; protected set; }
        public ICommand IsFavoriteCommand { get; protected set; }
    }
}
