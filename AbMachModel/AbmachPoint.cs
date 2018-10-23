using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurfaceModel;
using GeometryLib;
namespace AbMachModel
{
    public class Abmach2DPoint
    {
        public double Depth { get; set; }
        public double TargetDepth { get; set; }
        public double DepthTolerance { get; set; }
        public double StartDepth { get; set; }
        public Vector3 Normal { get; set; }
        public bool JetHit { get; set; }
        public double MinDepth { get { return Math.Abs(TargetDepth) - Math.Abs(DepthTolerance); } }
        public double MaxDepth { get { return Math.Abs(TargetDepth) + Math.Abs(DepthTolerance); } }
        public Abmach2DPoint()
        {
            Depth = 0;
            TargetDepth = 0;
            StartDepth = 0;
            Normal = new Vector3(0, 0, 1);
            JetHit = false;
        }
    }
   
    public class AbmachPoint : SurfacePoint
    {
        
        public double Depth { get { return (originalPosition - Position).Length; } }
        public double TargetDepth { get { return (originalPosition - targetPosition).Length; } }
        public Vector3 TargetPosition { get { return targetPosition; } }
        public bool JetHit { get; set; }
        public Vector3 OriginalPosition { get { return originalPosition; } set { originalPosition = value; } }

        private Vector3 targetPosition;
        private Vector3 originalPosition;

        public AbmachPoint()
        {

            targetPosition = new Vector3();
            originalPosition = new Vector3();

        }
        public AbmachPoint(Vector3 pt) : base(pt)
        {
            targetPosition = new Vector3();
            originalPosition = new Vector3();
            JetHit = false;
        }
        public AbmachPoint(AbmachPoint pt) : base(pt.Position, pt.Normal)
        {
            targetPosition = new Vector3(pt.TargetPosition);
            originalPosition = new Vector3(pt.OriginalPosition);
            JetHit = pt.JetHit;

        }
        public AbmachPoint Clone()
        {
            AbmachPoint pt = new AbmachPoint(this);
            return pt;
        }

        public void SetTargetDepth(double depth, Vector3 direction)
        {
            targetPosition = OriginalPosition - Math.Abs(depth) * direction;
        }
    }

}
