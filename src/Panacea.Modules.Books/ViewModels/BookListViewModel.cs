using Panacea.Controls;
using Panacea.Core;
using Panacea.Models;
using Panacea.Modularity.Favorites;
using Panacea.Modules.Books.Models;
using Panacea.Modules.Books.Views;
using Panacea.Mvvm;
using System;
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
            SetupCommands();
        }
        void SetupCommands()
        {
            ItemClickCommand = new RelayCommand(async arg =>
            {
                if (_plugin == null) return;
                if ((arg as Book).DataUrl.Any((du) => du.DataType == "file"))
                {
                    await _plugin.ReadBookAsync(arg as Book);
                }
                else if ((arg as Book).DataUrl.Any((du) => du.DataType == "audio"))
                {
                    await _plugin.ListenToBookAsync(arg as Book);
                }
                else if ((arg as Book).DataUrl.Any((du) => du.DataType == "url"))
                {
                    await _plugin.GoToBookAsync(arg as Book);
                }
            });
            InfoClickCommand = new RelayCommand((arg) =>
            {
                if (_plugin == null) return;
                _plugin.OpenItem(arg as Book);
            });

            IsFavoriteCommand = new RelayCommand((arg) =>
            {
            }, (arg) =>
            {
                var book = arg as Book;
                if (_plugin.Favorites == null) return false;
                return _plugin.Favorites.Any(mm => mm.Id == book.Id);
            });

            FavoriteCommand = new AsyncCommand(async (args) =>
            {
                var book = args as Book;
                if (book == null) return;
                if (_core.TryGetFavoritesPlugin(out IFavoritesManager _favoritesManager))
                {
                    try
                    {
                        await _favoritesManager.AddOrRemoveFavoriteAsync("Books", book);
                        OnPropertyChanged(nameof(IsFavoriteCommand));
                    }
                    catch (Exception e)
                    {
                        _core.Logger.Error(this, e.Message);
                    }
                }
            });
        }
        public ICommand ItemClickCommand { get; private set; }
        public ICommand InfoClickCommand { get; protected set; }
        public AsyncCommand FavoriteCommand { get; protected set; }
        public ICommand IsFavoriteCommand { get; protected set; }
    }
}
