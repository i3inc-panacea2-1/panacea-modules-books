using Panacea.Controls;
using Panacea.Core;
using Panacea.Modularity.UiManager;
using Panacea.Modularity.WebBrowsing;
using Panacea.Modules.Books.Models;
using Panacea.Modules.Books.Views;
using Panacea.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;

namespace Panacea.Modules.Books.ViewModels
{
    [View(typeof(BookMiniPresenter))]
    public class BookMiniPresenterViewModel : PopupViewModelBase<object>
    {
        private readonly PanaceaServices _core;
        BooksPlugin _plugin;
        Book _book;
        public Book Book
        {
            get => _book;
            set
            {
                _book = value;
                OnPropertyChanged();
            }
        }
        public BookMiniPresenterViewModel(
            PanaceaServices core,
            BooksPlugin plugin,
            Book book)
        {
            _core = core;
            _plugin = plugin;
            if (book != null)
            {
                Book = book;
                SetupCommands();
                //UnloadedCommand = new RelayCommand(args => sendEmptyCommands());
            }
        }
        protected void SetupCommands()
        {
            var canRead = CanRead();
            ReadBookCommand = new RelayCommand(ReadBook, (arg) => canRead);

            var canGo = CanGo();
            GoToBookCommand = new RelayCommand(GoToBook, (arg) => canGo);

            var canListen = CanListen();
            ListenToBookCommand = new RelayCommand(ListenToBook, (arg) => canListen);
        }
        async void ListenToBook(object arg)
        {
            SetResult(null);
            await _plugin.ListenToBookAsync(Book);
        }
        async void ReadBook(object arg)
        {
            SetResult(null);
            await _plugin.ReadBookAsync(Book);
        }
        async void GoToBook(object arg)
        {
            SetResult(null);
            await _plugin.GoToBookAsync(Book);
        }
        protected bool CanGo()
        {
            if (Book == null) return false;
            return Book.DataUrl.Any((du) => du.DataType == "url");
        }
        protected bool CanRead()
        {
            if (Book == null) return false;
            return Book.DataUrl.Any((du) => du.DataType == "file");
        }

        protected bool CanListen()
        {
            if (Book == null) return false;
            return Book.DataUrl.Any((du) => du.DataType == "audio");
        }

        ICommand _readBookCommand;
        public ICommand ReadBookCommand
        {
            get => _readBookCommand;
            protected set
            {
                _readBookCommand = value;
                OnPropertyChanged();
            }
        }
        ICommand _goToBookCommand;
        public ICommand GoToBookCommand
        {
            get => _goToBookCommand;
            protected set
            {
                _goToBookCommand = value;
                OnPropertyChanged();
            }
        }

        ICommand _listenToBookCommand;
        public ICommand ListenToBookCommand
        {
            get => _listenToBookCommand;
            protected set
            {
                _listenToBookCommand = value;
                OnPropertyChanged();
            }
        }
        ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get => _closeCommand;
            protected set
            {
                _closeCommand = value;
                OnPropertyChanged();
            }
        }
    }
}
