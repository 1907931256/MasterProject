using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolpathLib;
using GeometryLib;

namespace AbMachModel
{
    /// <summary>
    /// collection of jet radial locations and jet centerline locations 
    /// </summary>
    public class XSecJetPath
    {
        XSecJet jet;
        XSecPathList path;
        double meshSize;
        double nomF;
        Ray2[,] jetArray;
        int pathCount;
        int jetCount;
        public int PathCount { get { return pathCount; } }
        public int JetCount { get { return jetCount; } }

        public Ray2 GetJetRay(int pathIndex, int jetIndex)
        {
            return jetArray[pathIndex, jetIndex];
        }
        /// <summary>
        /// returns Ray[pathIndex,jetLocationIndex]
        /// </summary>
        /// <returns></returns>
        Ray2[,] GetJetArray()
        {
            jetCount = (int)(Math.Ceiling(jet.Radius * 2 / meshSize));
            pathCount = path.Count;

            Ray2[,] jetArr = new Ray2[path.Count, jetCount];
            for (int pathIndex = 0; pathIndex < pathCount; pathIndex++)
            {
                double endX = path[pathIndex].CrossLoc + jet.Radius;
                for (int jetLocIndex = 0; jetLocIndex < jetCount; jetLocIndex++)
                {
                    double x = (path[pathIndex].CrossLoc - jet.Radius) + (meshSize * jetLocIndex);
                    var origin = new Vector2(x, 0);
                    double angleDeg = 90;
                    double angRad = Math.PI * (angleDeg / 180.0);
                    var direction = new Vector2(Math.Cos(angRad), Math.Sin(angRad));
                    double jetX = x - path[pathIndex].CrossLoc;
                    double mrr = jet.GetMrr(jetX) * nomF / path[pathIndex].Feedrate;
                    var jetRay = new Ray2(origin, direction, mrr);
                    jetArr[pathIndex, jetLocIndex] = jetRay;
                }
            }
            return jetArr;
        }
        public XSecJetPath(XSecJet xSecJet, XSecPathList xSecPathList, double meshSize, double nominalFeedrate)
        {
            path = xSecPathList;


            jet = xSecJet;

            this.meshSize = meshSize;
            nomF = nominalFeedrate;
            jetArray = GetJetArray();
        }
    }
}
