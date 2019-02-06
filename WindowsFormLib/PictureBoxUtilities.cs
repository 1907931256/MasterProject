using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
namespace WinFormsLib
{
    public class PictureBoxUtilities
    {
        public static void SaveScreenShotasPicture(Control control)
        {
            try
            {
                Bitmap memoryImage;
                memoryImage = new Bitmap(control.Width, control.Height);
                Size s = new Size(memoryImage.Width, memoryImage.Height);
                // Create graphics 
                Graphics memoryGraphics = Graphics.FromImage(memoryImage);
                // Copy data from screen 
                var origin = control.PointToScreen(control.Location);
                memoryGraphics.CopyFromScreen(origin.X, origin.Y, 0, 0, s);
                var sfd = new SaveFileDialog();

                string filename = string.Format(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + @"\Screenshot.png");
                sfd.FileName = filename;
                sfd.Filter = "(*.png)|*.png";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    memoryImage.Save(sfd.FileName);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static PointF GetNearestPoint(Point mousePt, List<PointF> screenPoints)
        {
            try
            {
                double minDist2 = double.MaxValue;
                PointF minPt = new PointF();
                foreach (var p in screenPoints)
                {
                    var dist2 = Math.Pow(p.X - mousePt.X, 2) + Math.Pow(p.Y - mousePt.Y, 2);
                    if (dist2 < minDist2)
                    {
                        minDist2 = dist2;
                        minPt = new PointF(p.X, p.Y);
                    }
                }

                return minPt;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
