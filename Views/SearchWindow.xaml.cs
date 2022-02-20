using System.Windows;
using System.Windows.Controls;
using AdonisUI.Controls;
using FFS.ViewModels;

namespace FFS.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SearchWindow : AdonisWindow
    {
        public SearchWindow()
        {
            InitializeComponent();
        }

        private void FilesView_OnFileIconColumnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Lock files view column to constant size
            if (e.NewSize.Width <= 50 || e.NewSize.Width >= 50)
            {
                e.Handled = true;
                ((GridViewColumnHeader)sender).Column.Width = 50;
            }
        }
    }
}
