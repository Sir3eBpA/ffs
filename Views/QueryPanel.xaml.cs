using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FFS.Views
{
    /// <summary>
    /// Interaction logic for QueryPanel.xaml
    /// </summary>
    public partial class QueryPanel : UserControl
    {
        private const int FileColumnWidth = 50;
        private const int FullNameColumnWidth = 200;

        public QueryPanel()
        {
            InitializeComponent();
        }

        private void FilesView_OnFileIconColumnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Lock files view column to constant size
            if (e.NewSize.Width <= FileColumnWidth || e.NewSize.Width >= FileColumnWidth)
            {
                e.Handled = true;
                ((GridViewColumnHeader)sender).Column.Width = FileColumnWidth;
            }
        }

        private void FileView_OnFullNameColumnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width <= FullNameColumnWidth)
            {
                e.Handled = true;
                ((GridViewColumnHeader)sender).Column.Width = FullNameColumnWidth;
            }
        }
    }
}
