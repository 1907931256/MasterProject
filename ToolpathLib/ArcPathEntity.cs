﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
namespace ToolpathLib
{
   
    public class ArcPathEntity:PathEntity5Axis
    {

        public ArcSpecType ArcType { get; set; }
        public ArcPlane ArcPlane { get; set; }
        public bool FullArcFlag { get; set; }
        public double Icoordinate { get; set; }
        public double Jcoordinate { get; set; }
        public double Kcoordinate { get; set; }

        public double SweepAngle { get; set; }
        public double StartAngleRad { get; set; }
        public double Radius { get; set; }
        public Vector3 CenterPoint { get; set; }


        internal ArcPathEntity(BlockType type)
        {
            
            CenterPoint = new Vector3();
            ArcType = ArcSpecType.IJKRelative;
            ArcPlane = ToolpathLib.ArcPlane.XY;
            base.Type = type;
        }       
    }
}
