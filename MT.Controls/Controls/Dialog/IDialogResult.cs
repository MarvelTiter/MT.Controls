namespace MT.Controls
{
    public enum DialogFeedback
    {
        Cancel,
        Confirm,
        Exception,
        None,
    }
    public interface IDialogResult
    {
        object Result { get; set; }
        DialogFeedback DialogAction { get; set; }
    }
    public class DialogResult : IDialogResult
    {
        public object Result { get; set; }
        public DialogFeedback DialogAction { get; set; }
    }
}
