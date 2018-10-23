using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurfaceModel;
using DrawingIO;
namespace AbMachModel
{
    
    
  
   
    public class AbmachSurface:ModelSurface2D<AbmachVal>
    {
        Dictionary<AbmachValType, SurfaceInputType> surfaceStatusDictionary;
        public SurfaceInputType GetSurfaceStatus(AbmachValType type)
        {
            return surfaceStatusDictionary[type];
        }
        public void SetSurfaceStatus(AbmachValType surface, SurfaceInputType status)
        {
            surfaceStatusDictionary.Remove(surface);
            surfaceStatusDictionary.Add(surface, status);
        }
        public double GetDepth(int xi, int yi)
        {
            return GetValue(xi, yi).Depth;
        }
        public bool IsValley(int xI, int yI)
        {
            return GetValue(xI, xI).Model < GetValue(xI - 1, yI).Model && GetValue(xI, yI).Model < GetValue(xI + 1, yI).Model && GetValue(xI, yI).Model < GetValue(xI, yI - 1).Model && GetValue(xI, yI).Model < GetValue(xI, yI + 1).Model;
        }
        public bool IsPeak(int xI, int yI)
        {
            return GetValue(xI, xI).Model > GetValue(xI - 1, yI).Model && GetValue(xI, yI).Model > GetValue(xI + 1, yI).Model && GetValue(xI, yI).Model > GetValue(xI, yI - 1).Model && GetValue(xI, yI).Model > GetValue(xI, yI + 1).Model;
        }
        public void SmoothAt(int xI, int yI)
        {
            double newDepth = (GetValue(xI, yI).Model + GetValue(xI - 1, yI).Model + GetValue(xI + 1, yI).Model +
                    GetValue(xI, yI - 1).Model + GetValue(xI, yI + 1).Model) / 5;
            SetValue(AbmachValType.Model, newDepth, xI, yI);  
        }
        public void SetValue(AbmachValType type, AbmachVal value, int xI, int yI)
        {
           
            AbmachVal v = base.GetValue(xI, yI);
            switch (type)
            {
                case AbmachValType.MachIndex:
                    v.MachIndex = value.MachIndex;
                    break;
                case AbmachValType.Mask:
                    v.Mask = value.Mask;
                    break;
                case AbmachValType.Model:
                    v.Model = value.Model;
                    break;
                case AbmachValType.Start:
                    v.Start = value.Start;
                    break;
                case AbmachValType.Target:
                    v.Target = value.Target;
                    break;
                case AbmachValType.Temp:
                    v.Temp = value.Temp;
                    break;
            }
            base.SetValue(v, xI, yI);
        }

        public void SetValue(AbmachValType type, double value,double xPos,double yPos)
        {
            int xI = base.Xindex(xPos);
            int yI = base.Yindex(yPos);
            SetValue(type, value, xI, yI);
        }
        public void InitValues(double startDepth, double targetDepth)
        {
            AbmachVal v = new AbmachVal { MachIndex = 1, Model = 0, Mask = 1, Start = startDepth, Target = targetDepth, Temp = 0 };
            base.InitValue(v);
        }
        public void InitValue(AbmachValType type, double value)
        {

        }
        public void InsertAll( AbmachSurface surface)
        {
            for (int i = 0; i < xSize; i++)
            {
                int xIndex = surface.Xindex(Xposition(i));
                for (int j = 0; j < ySize; j++)
                {
                    int yIndex = surface.Yindex(Yposition(j));
                    values[i, j] = surface.GetValue(xIndex, yIndex);
                }
            }
        }
        public void Insert(AbmachValType type,AbmachSurface surface)
        {
            for (int i = 0; i < xSize; i++)
            {
                int xIndex = surface.Xindex(Xposition(i));
                for (int j = 0; j < ySize; j++)
                {
                    int yIndex = surface.Yindex(Yposition(j));
                    AbmachVal v = surface.GetValue(xIndex, yIndex);
                    
                    SetValue(type,v, xIndex, yIndex);

                }
            }
        }
        public Vector3 Normal(double xPosition, double yPosition)
        {
            int i = Xindex(xPosition);
            int j = Yindex(yPosition);
            return Normal(i, j);
        }
        public Vector3 Normal(int xIndex, int yIndex)
        {
            int i = yIndex;
            int j = yIndex;
            double zx1 = GetValue(i - 1, j).Model;
            double zx2 = GetValue(i + 1, j).Model;
            double zy1 = GetValue(i, j - 1).Model;
            double zy2 = GetValue(i, j + 1).Model;
            var vx = new Vector3(2 * meshSize, 0, zx2 - zx1);
            var vy = new Vector3(0, 2 * meshSize, zy2 - zy1);
            return vx.Cross(vy);
        }
        public AbmachSurface(double xMinIn, double yMinIn, double xMaxIn, double yMaxIn, double meshSizeIn)
            : base(xMinIn, yMinIn, xMaxIn, yMaxIn, meshSizeIn)
        {

        }
        public AbmachSurface(DrawingIO.Extents extents, double meshSizeIn)
            : base(extents, meshSizeIn)
        {

        }       
    }
}
