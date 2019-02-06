using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
namespace ToolpathLib
{
    public class ConstantDistancePathBuilder : ModelPathBuilder, IModelPathBuilder
    {
        bool isFiveAxis;
        public ModelPath Build(ToolPath inputPath, double increment)
        {
            try
            {
                isFiveAxis = false;
                if (inputPath.Count == 0)
                    throw new InvalidOperationException("inputpath length must be > 0");
                if (increment <= 0)
                    throw new InvalidOperationException("path increment must be > 0");

                return parsePath(inputPath, increment);

            }
            catch(Exception)
            {
                throw;
            }
            
        }
        private ModelPath parsePath(ToolPath inputPath, double increment)
        {
            try
            {
                inputPath[0].JetVector = GeometryLib.Vector3.ZAxis;
                BoundingBox ext = getBoundingBox(inputPath);
                BoundingBox jetOnBox = getJetOnBoundingBox(inputPath);

                ModelPath mp = new ModelPath(ext, jetOnBox);
                mp.MeshSize = increment;
                for (int i = 1; i < inputPath.Count; i++)
                {

                    if (inputPath[i] is LinePathEntity)
                    {
                        mp.AddRange(parseLine(increment, inputPath[i], inputPath[i - 1]));
                    }
                    if (inputPath[i] is ArcPathEntity)
                    {
                        mp.AddRange(parseArc(increment, inputPath[i], inputPath[i - 1].Position));
                    }
                }
                mp.IsFiveAxis = isFiveAxis;
                return mp;
            }
            catch (Exception)
            {

                throw;
            }
           

        }
        private List<ModelPathEntity> parseLine(double increment, PathEntity p2, PathEntity p1)
        {
            try
            {
                List<ModelPathEntity> path = new List<ModelPathEntity>();
                double segLength = p2.Position.DistanceTo(p1.Position);
                int parseCount = (int)Math.Round(segLength / increment);
                if (parseCount == 0) parseCount = 1;
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
                double dx = p2.Position.X - p1.Position.X;
                double dy = p2.Position.Y - p1.Position.Y;
                double dz = p2.Position.Z - p1.Position.Z;
                double db = p2.Position.Bdeg - p1.Position.Bdeg;
                double dc = p2.Position.Cdeg - p1.Position.Cdeg;
                double dvx = p2.JetVector.X - p1.JetVector.X;
                double dvy = p2.JetVector.Y - p1.JetVector.Y;
                double dvz = p2.JetVector.Z - p1.JetVector.Z;
                if (Math.Abs(dc) > .5 || Math.Abs(db) > .5)
                {
                    isFiveAxis = true;
                }
                double pathsegF = p2.Feedrate.Value ;

                for (int j = 0; j < parseCount; j++)
                {
                    ModelPathEntity pathSeg = new ModelPathEntity(p2);

                    //calc new position
                    pathSeg.Position = new CNCLib.MachinePosition(CNCLib.MachineGeometry.XYZBC);
                    pathSeg.Position.X = p1.Position.X + j * dx / parseCount;
                    pathSeg.Position.Y = p1.Position.Y + j * dy / parseCount;
                    pathSeg.Position.Z = p1.Position.Z + j * dz / parseCount;
                    pathSeg.Position.Bdeg = p1.Position.Bdeg + j * db / parseCount;
                    pathSeg.Position.Cdeg = p1.Position.Cdeg + j * dc / parseCount;



                    //calc new jet vector
                    double vx = p1.JetVector.X + j * dvx / parseCount;
                    double vy = p1.JetVector.Y + j * dvy / parseCount;
                    double vz = p1.JetVector.Z + j * dvz / parseCount;
                    pathSeg.JetVector = new GeometryLib.Vector3(vx, vy, vz);
                    if (p2.Feedrate.Inverted)
                    {
                        pathSeg.Feedrate.Value = pathsegF;
                    }
                    else
                    {
                        pathSeg.Feedrate.Value = p2.Feedrate.Value;
                    }
                    if (p2.JetOn == true)
                    {
                        pathSeg.JetOn = true;
                    }
                    else
                    {
                        pathSeg.JetOn = false;
                    }
                    path.Add(pathSeg);

                }
                return path;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        private List<ModelPathEntity> parseArc(double increment, PathEntity entity, CNCLib.MachinePosition startPoint)
        {
            try
            {
                List<ModelPathEntity> path = new List<ModelPathEntity>();
                ArcPathEntity arc = entity as ArcPathEntity;
                Vector3 vEnd = new GeometryLib.Vector3(arc.Position.X, arc.Position.Y, arc.Position.Z);
                Vector3 vStart = new Vector3(startPoint.X, startPoint.Y, startPoint.Z);
                double segLength = Math.Abs(arc.SweepAngle * arc.Radius);
                int parseCount = (int)Math.Round(segLength / increment);
                if (parseCount == 0) parseCount = 1;
                double dA = arc.SweepAngle / parseCount;

                for (int j = 0; j < parseCount; j++)
                {

                    ModelPathEntity pathSeg = new ModelPathEntity(arc);

                    pathSeg.Position = getNewArcEndpoint(arc, j * dA);
                    if (entity.JetOn == true)
                    {
                        pathSeg.JetOn = true;
                    }
                    else
                    {
                        pathSeg.JetOn = false;
                    }
                    path.Add(pathSeg);
                }
                return path;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        private List<ModelPathEntity> parseDelay(double increment, PathEntity entity, DelayPathEntity delayEnt)
        {
            try
            {
                List<ModelPathEntity> path = new List<ModelPathEntity>();
                int parseCount = (int)Math.Round(delayEnt.Delay / increment);
                for (int i = 0; i < parseCount; i++)
                {
                    path.Add(new ModelPathEntity(entity));

                }
                return path;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }
}
