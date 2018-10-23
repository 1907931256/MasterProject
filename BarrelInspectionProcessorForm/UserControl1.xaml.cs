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
using System.Windows.Media.Media3D;

namespace BarrelInspectionProcessorForm
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        // The camera's current location.
        public double CameraPhi = Math.PI / 6.0;       // 30 degrees
        public double CameraTheta = Math.PI / 6.0;     // 30 degrees
        public double CameraR = 4.0;
        public PerspectiveCamera TheCamera;
        //public OrthographicCamera TheCamera;
        // The change in CameraPhi when you press the up and down arrows.
        private const double CameraDPhi = 0.01;

        // The change in CameraTheta when you press the left and right arrows.
        private const double CameraDTheta = 0.01;

        // The change in CameraR when you press + or -.
        private const double CameraDR = .5;
        public UserControl1()
        {
            InitializeComponent();
        }

        public void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    CameraPhi += CameraDPhi;
                    if (CameraPhi > Math.PI / 2.0) CameraPhi = Math.PI / 2.0;
                    break;
                case Key.Down:
                    CameraPhi -= CameraDPhi;
                    if (CameraPhi < -Math.PI / 2.0) CameraPhi = -Math.PI / 2.0;
                    break;
                case Key.Left:
                    CameraTheta += CameraDTheta;
                    break;
                case Key.Right:
                    CameraTheta -= CameraDTheta;
                    break;
                case Key.Add:
                case Key.OemPlus:
                    CameraR -= CameraDR;
                    if (CameraR < CameraDR) CameraR = CameraDR;
                    break;
                case Key.Subtract:
                case Key.OemMinus:
                    CameraR += CameraDR;
                    break;
            }
        
        PositionCamera();
    }

    // Position the camera.
    public void PositionCamera()
    {
        // Calculate the camera's position in Cartesian coordinates.
        double y = CameraR * Math.Sin(CameraPhi);
        double hyp = CameraR * Math.Cos(CameraPhi);
        double x = hyp * Math.Cos(CameraTheta);
        double z = hyp * Math.Sin(CameraTheta);
        TheCamera.Position = new Point3D(x, y, z);

        // Look toward the origin.
        TheCamera.LookDirection = new Vector3D(-x, -y, -z);

        // Set the Up direction.
        TheCamera.UpDirection = new Vector3D(0, 1, 0);

        // Console.WriteLine("Camera.Position: (" + x + ", " + y + ", " + z + ")");
    }

        //private void MainViewport_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    var x = 1;
        //    x += 1;
        //}

        //private void MainViewport_KeyDown(object sender, KeyEventArgs e)
        //{
        //    switch (e.Key)
        //    {
        //        case Key.Up:
        //            CameraPhi += CameraDPhi;
        //            if (CameraPhi > Math.PI / 2.0) CameraPhi = Math.PI / 2.0;
        //            break;
        //        case Key.Down:
        //            CameraPhi -= CameraDPhi;
        //            if (CameraPhi < -Math.PI / 2.0) CameraPhi = -Math.PI / 2.0;
        //            break;
        //        case Key.Left:
        //            CameraTheta += CameraDTheta;
        //            break;
        //        case Key.Right:
        //            CameraTheta -= CameraDTheta;
        //            break;
        //        case Key.Add:
        //        case Key.OemPlus:
        //            CameraR -= CameraDR;
        //            if (CameraR < CameraDR) CameraR = CameraDR;
        //            break;
        //        case Key.Subtract:
        //        case Key.OemMinus:
        //            CameraR += CameraDR;
        //            break;
        //    }

        //    PositionCamera();
        //}
        bool mouseDown;
        Point mouseDownPt;
        Point mouseUpPt;
        private void MainViewport_MouseMove(object sender, MouseEventArgs e)
        {
            var mousePt = e.GetPosition(MainViewport);
            if(mouseDown)
            {
                var dx = mousePt.X - mouseDownPt.X;
                var dy = mousePt.Y - mouseDownPt.Y;
                if(dy>0)
                {
                    CameraPhi += CameraDPhi;
                    if (CameraPhi > Math.PI / 2.0)
                    {
                        CameraPhi = Math.PI / 2.0;
                    }
                        
                }
                else
                {
                    CameraPhi -= CameraDPhi;
                    if (CameraPhi < -Math.PI / 2.0)
                    {
                        CameraPhi = -Math.PI / 2.0;
                    }
                }
                if(dx>0)
                {
                    CameraTheta += CameraDTheta;
                }
                else
                {
                    CameraTheta -= CameraDTheta;
                }
                PositionCamera();
            }
        }
        private void MainViewport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseDown = true;
            mouseDownPt = e.GetPosition(MainViewport);
        }

        private void MainViewport_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
            mouseUpPt = e.GetPosition(MainViewport);
        }

        private void MainViewport_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var roller = e.Delta;
            if(roller>0)
            {
                CameraR -= CameraDR;
                if (CameraR < CameraDR) CameraR = CameraDR;
            }
            else
            {
                CameraR += CameraDR;
            }
            PositionCamera();
        }

        private void MainViewport_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void MainViewport_MouseEnter(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}
