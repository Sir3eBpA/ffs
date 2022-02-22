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
    public partial class QueryPanel : Page
    {
        public QueryPanel()
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
