﻿using System.Collections.Generic;
using System.Windows.Input;
using Solutionizer.Framework;

namespace Solutionizer.ViewModels {
    public class AboutViewModel : DialogViewModel, IWithTitle {
        private readonly ICommand _closeCommand;

        public AboutViewModel() {
            _closeCommand = new RelayCommand(Close);
        }

        public string Title {
            get { return "About Solutionizer"; }
        }

        public ICommand CloseCommand {
            get { return _closeCommand; }
        }

        public IEnumerable<CreditItem> CreditItems {
            get {
                return new[] {
                    new CreditItem { Name = "Autofac", Uri = "http://autofac.org/" },
                    new CreditItem { Name = "MahApps.Metro", Uri = "http://mahapps.com/MahApps.Metro/" },
                    new CreditItem { Name = "Json.NET", Uri = "http://james.newtonking.com/projects/json-net.aspx" },
                    new CreditItem { Name = "NLog", Uri = "http://nlog-project.org/" },
                    new CreditItem { Name = "Ookii Dialogs WPF library", Uri = "http://www.ookii.org/software/dialogs/" },
                    new CreditItem { Name = "RestSharp", Uri = "http://restsharp.org/" },
                };
            }
        }
    }

    public class CreditItem {
        public string Name { get; set; }
        public string Uri { get; set; }
    }
}