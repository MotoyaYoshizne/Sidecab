﻿
using System.ComponentModel;
using System.Collections.Generic;

namespace Sidecab.Presenter
{
    public class Core : Base
    {
        public Settings Settings { get; private set; }
        public Selector<Root> RootSelector { get; private set; }


        //======================================================================
        public Core(Model.Core model)
        {
            this.model = model;
            this.model.RootListChanged += RefreshRoot;

            Settings = new Settings(this.model.Settings);
            RefreshRoot();
        }

        //======================================================================
        ~Core()
        {
            this.model.RootListChanged -= RefreshRoot;
        }

        //======================================================================
        public void SetRootDirectory(Directory directory)
        {
            this.model.SetRootDirectory(directory.ExposeModel());
            this.RootSelector.Current = this.RootSelector.List[this.RootSelector.List.Count - 1];
        }


        //======================================================================
        private void OnRootChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Selector<Root>.Current))
            {
                RootSelector.Current.CollectSubdirectories();
            }
        }

        //======================================================================
        private void RefreshRoot()
        {
            var rootList = new List<Root>(this.model.RootList.Count);
            foreach (var r in this.model.RootList) { rootList.Add(new Root(r)); }

            this.RootSelector = new Selector<Root>(rootList);
            this.RootSelector.PropertyChanged += OnRootChanged;

            RaisePropertyChanged(nameof(RootSelector));
        }


        private Model.Core model;
    }
}
