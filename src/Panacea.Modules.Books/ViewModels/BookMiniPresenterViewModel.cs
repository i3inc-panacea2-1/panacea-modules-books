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

            CloseCommand = new RelayCommand(Close);
        }
        protected void Close(object arg)
        {
            IsOpen = false;
            _plugin.ListenToBook(Book);
            taskCompletionSource.SetResult(null);
        }
        protected void ListenToBook(object arg)
        {
            IsOpen = false;
            _plugin.ListenToBook(Book);
            taskCompletionSource.SetResult(null);
        }
        protected void ReadBook(object arg)
        {
            IsOpen = false;
            _plugin.ReadBook(Book);
            taskCompletionSource.SetResult(null);
        }
        protected void GoToBook(object arg)
        {
            IsOpen = false;
            _plugin.GoToBook(Book);
            taskCompletionSource.SetResult(null);
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

        bool _isOpen = true;
        public bool IsOpen
        {
            get => _isOpen;
            protected set
            {
                _isOpen = value;
                OnPropertyChanged();
            }
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
