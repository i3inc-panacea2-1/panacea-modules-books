using Panacea.Modules.Books.Models;
using Panacea.Modules.Books.Views;
using Panacea.Core;
using Panacea.Mvvm;
using System.Collections.Generic;
using Panacea.Controls;
using Panacea.Modularity.UiManager;
using Panacea.Modularity.Media;
using Panacea.Modularity.MediaPlayerContainer;
using System.Windows.Input;
using Panacea.Modularity.Media.Channels;
using System.Text;

namespace Panacea.Modules.Books.ViewModels
{
    [View(typeof(AudioChaptersPresenter))]
    public class AudioChaptersPresenterViewModel : PopupViewModelBase<object>
    {
        private readonly PanaceaServices _core;
        List<AudioBookChapter> _audioChapters;
        public List<AudioBookChapter> AudioChapters
        {
            get { return _audioChapters; }
            set
            {
                _audioChapters = value;
                OnPropertyChanged();
            }
        }

        // List<AudioBookChapter> convertedChapters = urls.Select(du => new AudioBookChapter { url = du.Url }).ToList();
        public AudioChaptersPresenterViewModel(PanaceaServices core, List<AudioBookChapter> audioChapters)
        {
            _core = core;
            _audioChapters = audioChapters;
            PlayCommand = new RelayCommand((arg) =>
            {
                taskCompletionSource.SetResult(null);
                if (_core.TryGetUiManager(out IUiManager uiManager) && _core.TryGetMediaPlayerContainer(out IMediaPlayerContainer mediaPlayerContainer)) {
                    var chapter = arg as AudioBookChapter;
                    var url = _core.HttpClient.RelativeToAbsoluteUri(chapter.Url);
                    mediaPlayerContainer.Play(
                        new MediaRequest(new IptvMedia { URL = url, Name = chapter.Title })
                        {
                            FullscreenMode = FullscreenMode.NoFullscreen,
                            MediaPlayerPosition = MediaPlayerPosition.Notification,
                            ShowVideo = false,
                            ShowControls = true
                        });
                } else
                {
                    string msg = "";
                    if (uiManager == null)
                    {
                        msg = "ui manager not available";
                    }
                    else
                    {
                        msg = "media player container not loaded";
                    }
                    _core.Logger.Warn(this, msg);
                }
            });
        }

        public ICommand PlayCommand { get; protected set; }
    }
}
