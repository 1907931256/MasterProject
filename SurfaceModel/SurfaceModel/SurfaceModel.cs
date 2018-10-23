using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
namespace SurfaceModel
{
    class Tri
    {
        uint v1;
        uint v2;
        uint v3;
        uint Index;
        List<uint> neighbors;

        public bool IsNeighbor(Tri tri)
        {
            bool result = false;
            if (tri.neighbors.Contains(Index))
            {
                result = true;
            }
            else
            {
                int score = 0;
                if (tri.v1 - v1 == 0)
                {
                    score++;
                }
                if (tri.v2 - v2 == 0)
                {
                    score++;
                }
                if (tri.v3 - v3 == 0)
                {
                    score++;
                }
                if (score >= 2)
                {
                    neighbors.Add(tri.Index);
                    tri.neighbors.Add(Index);
                    result= true;

                }
                else
                {
                    result= false;
                }
            }
            return result;
        }

    }
    //class SurfaceModel
    //{
    //    class ModelVertex:OctreePoint
    //    {
            
    //       public List<STLTriangle> Triangles;
    //       public ModelVertex(Vector3 position, int index)
    //           : base(position, index)
    //       {
    //           Triangles = new List<STLTriangle>();
    //       }
    //       public ModelVertex(ModelVertex vert)
    //           :base(vert.Position,vert.Index)
    //       {
    //           Triangles = vert.Triangles;
               
    //       }
    //    }
    //    double vertexEpsilon;
    //    Octree vertexOctree;
    //    Dictionary<uint, STLTriangle> triangleDictionary;
    //    Dictionary<uint, Vector3> vertexDictionary;
    //    int index;
    //    Extents modelExtents;
    //    public SurfaceModel(List<STLTriangle> triangles,double vertexEpsilon)
    //    {
    //        this.vertexEpsilon = vertexEpsilon;
    //        modelExtents = getExtents(triangles);
    //        vertexOctree = new Octree(modelExtents);
    //        triangleDictionary = new Dictionary<uint, STLTriangle>();
    //        foreach (STLTriangle tri in triangles)
    //        {
    //            foreach (Vector3 vertex in tri.Vert)
    //            {
    //                List<OctreePoint> neighborPts = new List<OctreePoint>();
    //                if (isUnique(vertex, ref neighborPts))
    //                {
    //                    ModelVertex vert = new ModelVertex(vertex,index);
    //                    vert.Triangles.Add(tri);
    //                    vertexOctree.Insert(vert);
    //                }
    //                else
    //                {

    //                    ModelVertex newVertex = neighborPts[0] as ModelVertex;
    //                    newVertex.Triangles.Add(tri);                       
    //                }
    //            }
    //            triangleDictionary.Add(tri.Index, tri);
    //        }
    //    }
    //    Extents getExtents(List<STLTriangle> triangles)
    //    {
    //        Extents extents = new Extents();
    //        if (triangles.Count > 0)
    //        {
    //            extents = triangles[0].Extents;
    //            foreach (STLTriangle tri in triangles)
    //            {
    //                extents.union(tri.Extents);
    //            }                
    //        }
    //        return extents;
    //    }
    //    bool isUnique(Vector3 vertex,ref List<OctreePoint> neighborPoints)
    //    {
    //        bool unique = true;
    //        Vector3 boxMin = new Vector3(vertex.X - vertexEpsilon, vertex.Y - vertexEpsilon, vertex.Z - vertexEpsilon);
    //        Vector3 boxMax = new Vector3(vertex.X + vertexEpsilon, vertex.Y + vertexEpsilon, vertex.Z + vertexEpsilon);
    //        vertexOctree.GetPointsInsideBox(boxMin,boxMax,ref neighborPoints);
    //        if (neighborPoints.Count > 0)
    //        {
    //            unique = false;
    //        }
            
    //        return unique;
    //    }
    //}
}
