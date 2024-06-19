using CommunityToolkit.Mvvm.ComponentModel;

namespace SQLGen.ViewModels;

public abstract partial class SelectableElement : ObservableObject
{
    [ObservableProperty]
    private bool _isSelected;

    public Type SelfType => this.GetType();
}
