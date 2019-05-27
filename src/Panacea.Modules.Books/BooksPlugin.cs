﻿using Panacea.Core;
using Panacea.Modularity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Panacea.Models;
using Panacea.Modularity.UiManager;
using Panacea.Modules.Books.ViewModels;
using Panacea.Modules.Books.Models;
using System;
using System.Linq;
using Panacea.Modularity.MediaPlayerContainer;
using System.Windows.Input;
using Panacea.Modularity.Media.Channels;
using Panacea.Modularity.Billing;
using Panacea.Modularity.WebBrowsing;
using Panacea.Modules.Books.Views;
using Panacea.Core.Extensions;
using System.Web;
using System.Windows;
using Panacea.Multilinguality;

namespace Panacea.Modules.Books
{
    public class BooksPlugin : ICallablePlugin //TODO: ,IHasFavorites in modularity?
    {
        readonly Translator _translator;
        readonly PanaceaServices _core;
        readonly BooksProvider _provider;
        public BooksProvider Provider
        {
            get => _provider;
        }

        public BooksPlugin(PanaceaServices core)
        {
            _core = core;
            _translator = new Translator("Books");
            _provider = new BooksProvider(core);
        }
        public List<ServerItem> Favorites { get; set; }

        public Task BeginInit()
        {
            return Task.CompletedTask;
        }

        public Task EndInit()
        {
            return Task.CompletedTask;
        }

        public void ListenToBook(Book book)
        {
            var urls = book.DataUrl.Where(du => du.DataType == "audio").ToList();
            if (urls.Count == 1)
            {
                var url = _core.HttpClient.RelativeToAbsoluteUri(urls[0].Url);
                if (_core.TryGetMediaPlayerContainer(out IMediaPlayerContainer _mediaContainer))
                {
                    _mediaContainer.Play(new MediaRequest(new IptvMedia() { URL = url, Name = book.Name })
                    {
                        FullscreenMode = FullscreenMode.NoFullscreen,
                        ShowVideo = false,
                        AllowPip = false,
                        MediaPlayerPosition = MediaPlayerPosition.Notification
                    });
                }
                else
                {
                    _core.Logger.Warn(this, "media player container not loaded");
                }
            }
            else
            {
                List<AudioBookChapter> convertedChapters = urls.Where(u => u.Url != null).Select(du => new AudioBookChapter { Url = du.Url }).ToList();
                var acp = new AudioChaptersPresenterViewModel(_core, convertedChapters);
                if (_core.TryGetUiManager(out IUiManager _uiManager))
                {
                    _uiManager.ShowPopup(acp, "", PopupType.None);
                }
                else
                {
                    _core.Logger.Warn(this, "ui manager not loaded");
                }
            }
        }

        async public void ReadBook(Book book)
        {
            if (_core.TryGetBilling(out IBillingManager _billing)) {
                if (!_billing.IsPluginFree("Books"))
                {
                    string msg = "This Book requires service.";
                    //TODO TRANSLATOR: var t = _translator.Translate(msg);
                    var s = await _billing.GetServiceForItemAsync(msg, "Books", book);
                    if (s == null)
                    {
                        return;
                    }
                }
            }
            if (book != null)
            {
                //TODO: WS: _websocket.PopularNotify("Books", "Book", book.Id);
                var durl = book.DataUrl.FirstOrDefault(d => d?.DataType == "file");
                if (durl == null || string.IsNullOrEmpty(durl.Url)) return;
                var url = _core.HttpClient.RelativeToAbsoluteUri(durl.Url);
                OpenItem(book, url);
            }
        }

        public void GoToBook(Book book)
        {
            if(_core.TryGetWebBrowser(out IWebBrowserPlugin _webBrowser)){
                _webBrowser.OpenUnmanaged(book.DataUrl.Where(du => du.DataType == "url").First().Url);
            } else
            {
                _core.Logger.Warn(this, "web browser not loaded");
            }
        }

        private BookMiniPresenterViewModel _previouspresenter;
        public void OpenItem(ServerItem item)
        {
            if(_core.TryGetUiManager(out IUiManager _uiManager))
            {
                _uiManager.HideKeyboard();
                //_uiManager.HidePopup(_previouspresenter);
                var bmp = new BookMiniPresenterViewModel(_core, this, item as Book);
                _previouspresenter = bmp;
                _uiManager.ShowPopup(bmp, "", PopupType.None);
            }
        }

        public void OpenItem(ServerItem item, string url)
        {
            var extension = GetExtension(url);
            if (_core.TryGetWebBrowser(out IWebBrowserPlugin _webBrowser))
            {
                switch (extension)
                {
                    case "epub":
                        _webBrowser.OpenUnmanaged(_core.HttpClient.RelativeToAbsoluteUri("/static/epub/index.html?c=") + _core.HttpClient.RelativeToAbsoluteUri(url));
                        break;
                    case "html":
                    case "htm":

                        break;
                    case "pdf":
                        _webBrowser.OpenUnmanaged(_core.HttpClient.RelativeToAbsoluteUri("/static/pdf/web/viewer.html?file=") + _core.HttpClient.RelativeToAbsoluteUri(url));
                        break;
                }
            }
        }

        private string GetExtension(string url)
        {
            var splits = url.Split('.');
            return splits[splits.Length - 1];
        }

        public Task Shutdown()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            return;
        }

        public void Call()
        {
            if (_core.TryGetUiManager(out IUiManager ui))
            {
                var bookListViewModel = new BookListViewModel(_core, this);
                ui.Navigate(bookListViewModel);
            }
        }
    }
}
