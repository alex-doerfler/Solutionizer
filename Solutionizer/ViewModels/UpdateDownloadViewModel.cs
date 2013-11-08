﻿using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Input;
using NLog;
using Solutionizer.Framework;
using Solutionizer.Infrastructure;

namespace Solutionizer.ViewModels {
    public class UpdateDownloadViewModel : DialogViewModel<bool>, IOnLoadedHandler {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly UpdateManager _updateManager;
        private readonly ReleaseInfo _releaseInfo;
        private readonly ICommand _cancelCommand;
        private int _progress;
        private bool _isPreparingDownload;

        public UpdateDownloadViewModel(UpdateManager updateManager, ReleaseInfo releaseInfo) {
            _updateManager = updateManager;
            _releaseInfo = releaseInfo;

            _cancelCommand = new RelayCommand(() => {
                _log.Debug("Cancelling download");
                _cancellationTokenSource.Cancel();
            });
        }

        public void OnLoaded() {
            Download();
        }

        private async void Download() {
            string filename;
            try {
                _log.Debug("Downloading update");
                filename = await _updateManager.DownloadReleaseAsync(
                    _releaseInfo, 
                    progress => {
                        Progress = progress;
                        IsPreparingDownload = false;
                    },
                    _cancellationTokenSource.Token);
            } catch (WebException ex) {
                if (ex.Status != WebExceptionStatus.RequestCanceled) {
                    _log.ErrorException("Error downloading release from " + _releaseInfo.DownloadUrl, ex);
                }
                filename = null;
            }
            if (filename != null && File.Exists(filename)) {
                _log.Debug("Downloading succeeded, spawning");
                Process.Start(filename);
                Close(true);
            } else {
                _log.Debug("Download failed or cancelled");
                Close(false);
            }
        }

        public int Progress {
            get { return _progress; }
            set {
                if (value != _progress) {
                    _progress = value;
                    NotifyOfPropertyChange(() => Progress);
                }
            }
        }

        public bool IsPreparingDownload {
            get { return _isPreparingDownload; }
            set {
                if (_isPreparingDownload != value) {
                    _isPreparingDownload = value;
                    NotifyOfPropertyChange(() => IsPreparingDownload);
                }
            }
        }

        public ICommand CancelCommand {
            get { return _cancelCommand; }
        }
    }
}