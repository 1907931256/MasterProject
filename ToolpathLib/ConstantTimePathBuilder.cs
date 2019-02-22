using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
namespace ToolpathLib
{
    public class ConstantTimePathBuilder : ModelPathBuilder, IModelPathBuilder
    {
        bool isFiveAxis;
        double averageSpacing;
        double averageFeedrate;
        int pointCount;
        public ModelPath Build(ToolPath inputPath, double increment)
        {
            averageSpacing = 0;
            pointCount = 0;
            if (inputPath.Count > 0 && increment > 0)
                return parsePath(inputPath, increment);

            return new ModelPath();
        }
        ModelPath parsePath(ToolPath inputPath, double timeIncrement)
        {

            ModelPath mp = new ModelPath(getBoundingBox(inputPath),getJetOnBoundingBox(inputPath));
            
            double cumulativeTime = inputPath[inputPath.Count - 1].CumulativeTime;

            int totalTimeInc = (int)Math.Round(cumulativeTime / timeIncrement);
            double currentTime = 0;
            int j = 1;
            while (currentTime <= cumulativeTime)
            {
                while (currentTime >= inputPath[j - 1].CumulativeTime && currentTime < inputPath[j].CumulativeTime)
                {
                    mp.Add(interpolate(inputPath[j - 1], inputPath[j], currentTime, timeIncrement));
                    currentTime += timeIncrement;
                }
                j++;

            }
            mp.IsFiveAxis = isFiveAxis;
            return mp;
        }
        private ModelPathEntity interpolate(PathEntity p1, PathEntity p2, double currentTime, double timeInc)
        {
            ModelPathEntity mpe = new ModelPathEntity(p1);
            if (p2 is LinePathEntity)
            {
                mpe = interpolateLine(p1, p2, currentTime);
            }
            if (p2 is ArcPathEntity)
            {
                mpe = interpolateArc(p1, p2, currentTime);
            }
            if (p2 is DelayPathEntity)
            {
                mpe = interpolateDelay(p1, p2, currentTime);
            }
            mpe.CumulativeTime = currentTime;
            mpe.TravelTime = timeInc;
            return mpe;
        }
        private ModelPathEntity interpolateLine(PathEntity p1, PathEntity p2, double currentTime)
        {
            ModelPathEntity mpe = new ModelPathEntity(p2);
            double dt = currentTime - p1.CumulativeTime;
            if (p1.Type == BlockType.FiveAxis)
            {
                isFiveAxis = true;
            }
            else
            {
                p1.JetVector = GeometryLib.Vector3.ZAxis;
            }
            if (p2.Type == BlockType.FiveAxis)
            {
                isFiveAxis = true;
            }
            else
            {
                p2.JetVector = GeometryLib.Vector3.ZAxis;
            }
            
            mpe.Position = interpolatePosition(p1, p2, currentTime);
            mpe.JetVector = interpolateVector(p1, p2, currentTime);

            return mpe;
        }
        private CNCLib.XYZBCMachPosition interpolatePosition(PathEntity p1, PathEntity p2, double currentTime)
        {
            double dx = p2.Position.X - p1.Position.X;
            double dy = p2.Position.Y - p1.Position.Y;
            double dz = p2.Position.Z - p1.Position.Z;
            double db = p2.Position.Bdeg - p1.Position.Bdeg;
            double dc = p2.Position.Cdeg - p1.Position.Cdeg;
            double t = interpolateTime(p1, currentTime);
            var Position = new CNCLib.XYZBCMachPosition();
            Position.X  = p1.Position.X + t * dx;
            Position.Y  = p1.Position.Y + t * dy;
            Position.Z = p1.Position.Z + t * dz;
            Position.Bdeg = p1.Position.Bdeg + t * db;
            Position.Cdeg = p1.Position.Cdeg + t * dc;
            if (Math.Abs(dc) > .5 || Math.Abs(db) > .5)
            {
                isFiveAxis = true;
            }
            return Position;
        }
        private Vector3 interpolateVector(PathEntity p1, PathEntity p2, double currentTime)
        {
            double dvx = p2.JetVector.X - p1.JetVector.X;
            double dvy = p2.JetVector.Y - p1.JetVector.Y;
            double dvz = p2.JetVector.Z - p1.JetVector.Z;
            double t = interpolateTime(p1, currentTime);
            double vx = p1.JetVector.X + t * dvx;
            double vy = p1.JetVector.Y + t * dvy;
            double vz = p1.JetVector.Z + t * dvz;
            return new Vector3(vx, vy, vz);
        }
        private double interpolateTime(PathEntity p1, double currentTime)
        {
            return currentTime - p1.CumulativeTime;
        }
        private ModelPathEntity interpolateArc(PathEntity p1, PathEntity p2, double currentTime)
        {
            ModelPathEntity mpe = new ModelPathEntity(p2);
            ArcPathEntity arc = p2 as ArcPathEntity;
            double dt = currentTime - p1.CumulativeTime;
            mpe.Position = getNewArcEndpoint(arc, dt);
            return mpe;
        }
        private ModelPathEntity interpolateDelay(PathEntity p1, PathEntity p2, double currentTime)
        {
            ModelPathEntity mpe = new ModelPathEntity(p1);

            return mpe;
        }
    }
}
