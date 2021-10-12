using System;

namespace MT.Controls
{
    internal struct DialogRegister
    {
        public Type ViewType { get; set; }
        public Type ViewModelType { get; set; }
    }
    public interface IDialogActivity
    {
        void OnDialogOpening(IDialogParameter parameter);
        void OnDialogClosing(IDialogParameter parameter);
        object GetDialogResult();
    }
}
