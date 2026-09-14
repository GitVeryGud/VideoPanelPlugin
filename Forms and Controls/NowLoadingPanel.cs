using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class NowLoadingPanel : UserControl
    {
        public NowLoadingPanel()
        {
            InitializeComponent();
            Loading.LocationChanged += Loading_LocationChanged;
        }

        // Recenters the loading gif whenever the location changes because of being added to another control
        private void Loading_LocationChanged(object sender, EventArgs e)
        {
            recenter();
            Utilities.debugPrint("Loading icon recentered = " + Loading.Location.ToString());
        }

        public Point getLocation()
        {
            return Loading.Location;
        }

        public void recenter()
        {
            var X = (Width - Loading.Width) / 2;
            var Y = (Height - Loading.Height) / 2;
            Loading.Location = new Point(X, Y);
        }
    }
}
