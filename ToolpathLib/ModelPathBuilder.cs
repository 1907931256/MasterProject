using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
namespace ToolpathLib
{
    public  class ModelPathBuilder
    {     
        public BoundingBox getBoundingBox(ToolPath inputPath)
        {
            var pointList = new List<Vector3>();

            foreach (PathEntity ent in inputPath)
            {
                pointList.Add(new Vector3(ent.Position.X, ent.Position.Y, ent.Position.Z));
            }

            BoundingBox ext = BoundingBoxBuilder.FromPtArray(pointList.ToArray());
            return ext;
        }
        public BoundingBox getJetOnBoundingBox(ToolPath inputPath)
        {
            var pointList = new List<Vector3>();

            foreach (PathEntity ent in inputPath)
            {
                if(ent.JetOn)
                    pointList.Add(new Vector3(ent.Position.X, ent.Position.Y, ent.Position.Z));
            }

            BoundingBox ext = BoundingBoxBuilder.FromPtArray(pointList.ToArray());
            return ext;
        }
        
        public CNCLib.MachinePosition getNewArcEndpoint(ArcPathEntity arc, double newSweepAngle)
        {
            double coord1 = 0;
            double coord2 = 0;
            var mp = arc.Position.Clone() as CNCLib.MachinePosition;
            switch (arc.Type)
            {
                case BlockType.CCWArc:
                    coord1 = arc.Radius * Math.Cos(arc.StartAngleRad + newSweepAngle);
                    coord2 = arc.Radius * Math.Sin(arc.StartAngleRad + newSweepAngle);
                    break;
                case BlockType.CWArc:
                    coord1 = arc.Radius * Math.Cos(arc.StartAngleRad - newSweepAngle);
                    coord2 = arc.Radius * Math.Sin(arc.StartAngleRad - newSweepAngle);
                    break;
            }
            switch (arc.ArcPlane)
            {
                case ArcPlane.XY:
                    mp.X = coord1 + arc.CenterPoint.X;
                    mp.Y = coord2 + arc.CenterPoint.Y;
                    mp.Z = arc.Position.Z;
                    break;
                case ArcPlane.XZ:
                    mp.X = coord1 + arc.CenterPoint.X;
                    mp.Z = coord2 + arc.CenterPoint.Z;
                    mp.Y = arc.Position.Y;
                    break;
                case ArcPlane.YZ:
                    mp.Y = coord1 + arc.CenterPoint.Y;
                    mp.Z = coord2 + arc.CenterPoint.Z;
                    mp.X = arc.Position.X;
                    break;
            }
            return mp;
        }
    }
}
