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
        private readonly DialogService _dialogService = DialogService.Instance;

        private T _selected;
        private T _origSelected;

        public T Selected
        {
            get => _selected;
            set => SafeThreading.JoinableTaskFactory.RunAsync(async () =>
            {
                try
                {
                    await SelectAsync(value);
                }
                catch (Exception ex)
                {
                    _dialogService.ShowError(ex);
                }
            });
        }

        public ObservableCollection<T> Items { get; } = new ObservableCollection<T>();

        public ObservableCollection<T> SelectedItems { get; } = new ObservableCollection<T>();

        public MultiSelectableGroup(Func<bool> isSelectionEnabled, Func<Task> onSelected = null)
        {
            _isSelectionEnabled = isSelectionEnabled;
            _onSelected = onSelected;
        }

        private async Task<bool> SelectAsync(T value)
        {
            if (value != null)
            {
                _origSelected = _selected;
                if (value != _selected)
                {
                    _selected = value;
                }

                if (_isSelectionEnabled())
                {
                    var selectedItems = Items.Where(x => x.IsSelected);
                    if (selectedItems.Count() == 1 && selectedItems.First() == value)
                    {
                        return false;
                    }

                    if (_selected.IsSelected == true)
                    {
                        _selected.IsSelected = false;
                        SelectedItems.Remove(_selected);
                    }
                    else
                    {
                        _selected.IsSelected = true;
                        SelectedItems.Add(_selected);
                    }

                    OnPropertyChanged(nameof(Selected));
                    if (_onSelected != null)
                    {
                        await _onSelected?.Invoke();
                    }

                    return true;
                }
                else
                {
                    DispatcherService.BeginInvoke(() =>
                    {
                        _selected = _origSelected;
                        OnPropertyChanged(nameof(Selected));
                    });
                }
            }

            return false;
        }
    }
}
