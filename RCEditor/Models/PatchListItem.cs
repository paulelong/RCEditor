using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RCEditor.Models
{
    public class PatchListItem : INotifyPropertyChanged
    {        private string _name = string.Empty;
        private int _patchNumber;
        private char _variation = 'A';
        private string _filePath = string.Empty;
        private bool _isSelected;
        private bool _isVisible = true;

        public string Name 
        { 
            get => _name; 
            set 
            { 
                _name = value; 
                OnPropertyChanged(); 
            } 
        }
        
        public int PatchNumber 
        { 
            get => _patchNumber; 
            set 
            { 
                _patchNumber = value; 
                OnPropertyChanged(); 
            } 
        }
        
        public bool IsSelected 
        { 
            get => _isSelected; 
            set 
            { 
                _isSelected = value; 
                OnPropertyChanged(); 
            } 
        }
          public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }
        
        public char Variation
        {
            get => _variation;
            set
            {
                _variation = value;
                OnPropertyChanged();
            }
        }
        
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged();
            }
        }        
        
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}