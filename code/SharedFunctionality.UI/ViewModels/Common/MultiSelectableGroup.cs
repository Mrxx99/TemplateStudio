// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.UI.ViewModels.Common;

namespace SharedFunctionality.UI.ViewModels.Common
{
    public class MultiSelectableGroup<T> : Observable
        where T : Selectable
    {
        private readonly Func<bool> _isSelectionEnabled;
        private readonly Func<Task> _onSelected;
        private RelayCommand<T> _removeCommand;

        public RelayCommand<T> RemoveCommand => _removeCommand ?? (_removeCommand = new RelayCommand<T>(a => Remove(a)));

        public ObservableCollection<T> Items { get; } = new ObservableCollection<T>();

        public ObservableCollection<T> SelectedItems { get; } = new ObservableCollection<T>();

        public MultiSelectableGroup(Func<bool> isSelectionEnabled, Func<Task> onSelected = null)
        {
            _isSelectionEnabled = isSelectionEnabled;
            _onSelected = onSelected;
        }

        public async Task<bool> SelectAsync(T value)
        {
            if (value != null)
            {
                if (_isSelectionEnabled())
                {
                    var selectedItems = Items.Where(x => x.IsSelected);
                    if (selectedItems.Count() == 1 && selectedItems.First() == value)
                    {
                        return false;
                    }

                    if (value.IsSelected == true)
                    {
                        value.IsSelected = false;
                        SelectedItems.Remove(value);
                    }
                    else
                    {
                        value.IsSelected = true;
                        SelectedItems.Add(value);
                    }

                    if (_onSelected != null)
                    {
                        await _onSelected?.Invoke();
                    }

                    return true;
                }
            }

            return false;
        }

        private void Remove(T value)
        {
            SelectedItems.Remove(value);
            value.IsSelected = false;

        }
    }
}
