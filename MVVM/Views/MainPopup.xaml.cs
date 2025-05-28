namespace SepidMelk.MVVM.Views;
using CommunityToolkit.Maui.Views;

public partial class MainPopup : Popup
{
    public MainPopup(string url)
    {
        InitializeComponent();
        lbl.Text = url;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}