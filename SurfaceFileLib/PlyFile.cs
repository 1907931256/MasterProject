using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using GeometryLib;
using SurfaceModel;
using System.Diagnostics;
using DataLib;

namespace SurfaceFileLib
{

    /// <summary>
    /// contains ply file header,vertices, faces, edges, materials
    /// </summary>
    public class PlyFile
    {
        public PlyHeader Header { get; set; }
        public PlyFormat Format { get; set; }
        public List<PlyVertex> Vertices { get { return _vertices; } }
        public List<PlyFace> Faces { get { return _faces; } }
        public List<PlyEdge> Edges { get { return _edges; } } 
        public List<PlyMaterial> Materials { get { return _materials; } }
        public Vector3[] FaceNorms { get; set; }
        public BoundingBox Extents { get { return extents; } }
        public static string Filter = "Ply files (*.ply)|*.ply|All files (*.*)|*.*";
        public static string Ext = ".ply";

        List<PlyVertex> _vertices;
        List<PlyFace> _faces;
        List<PlyEdge> _edges;
        List<PlyMaterial> _materials;
        StreamReader sr;
        string[] splitter = new string[1] { " " };
        string CurrentLine { get; set; }
        string NextLine(){ return CurrentLine = sr.ReadLine(); }
        string NextHeaderLine()
        {
            while (NextLine().StartsWith("comment"))
            { }
            return CurrentLine;
        }
        private void Init()
        {
            Header = new PlyHeader();
            _vertices = new List<PlyVertex>();
            _edges = new List<PlyEdge>();
            _faces = new List<PlyFace>();
            _materials = new List<PlyMaterial>();
            extents = new BoundingBox();
        }
        public List<Triangle> AsTriList()
        {
            List<Triangle> tris = new List<Triangle>();
            
            uint index = 0;
            foreach (PlyFace face in Faces)
            {
                if (face.Indices.Count == 3)
                {
                   tris.Add(new Triangle(Vertices[face.Indices[0]],Vertices[face.Indices[1]],Vertices[face.Indices[2]],Vertices[face.Indices[0]].Normal,index++));       
                    
                }
                if (face.Indices.Count == 4)
                {
                    tris.Add(new Triangle(Vertices[face.Indices[0]],Vertices[face.Indices[1]],Vertices[face.Indices[2]],Vertices[face.Indices[0]].Normal,index++));
                    tris.Add(new Triangle(Vertices[face.Indices[2]], Vertices[face.Indices[3]],Vertices[face.Indices[0]],Vertices[face.Indices[2]].Normal,index++)); 
                }
            }
            return tris;
        }
        int GetMinStripLength(CartGridData pointStripList)
        {
            int minStripLen = int.MaxValue;

            foreach (var strip in pointStripList)
            {
                if (strip.Count < minStripLen)
                {
                    minStripLen = strip.Count;
                }
            }
            return minStripLen;
        }
        public void BuildFromGrid2(CartGridData pointStripList)
        {
            int minStripLen = GetMinStripLength(pointStripList);
            int id = 0;
            var vertexGrid = new PlyVertex[pointStripList.Count, minStripLen];
            for (int i = 0; i < pointStripList.Count ; i++)
            {
                for (int j = 0; j < minStripLen ; j++)
                {                    
                    vertexGrid[i,j] = new PlyVertex(pointStripList[i][j],id++);
                    _vertices.Add(vertexGrid[i, j]);
                }               
            }
            for (int i = 0; i < pointStripList.Count-1; i++)
            {
                for (int j = 0; j < minStripLen - 1; j++)
                {
                    var indices1 = new List<int>() { vertexGrid[i, j].ID, vertexGrid[i, j+1].ID, vertexGrid[i+1, j ].ID };
                    _faces.Add(new PlyFace(indices1));
                    var indices2 = new List<int>() { vertexGrid[i, j+1].ID, vertexGrid[i + 1, j+1].ID, vertexGrid[i+1, j ].ID };
                    _faces.Add(new PlyFace(indices2));

                }
            }
        }
        public void BuildFromGrid(CartGridData pointStripList)
        {
            Debug.WriteLine("building ply file");

            //find minimum strip length


            int minStripLen = GetMinStripLength(pointStripList);
            //add triangles to list of faces
            
            var triList = new List<Triangle>();
            
            for (int i = 0; i < pointStripList.Count - 1; i++)
            {              
                for (int j = 0; j < minStripLen - 1; j++)
                {
                    
                    Triangle t1 = new Triangle(pointStripList[i][j], pointStripList[i][j + 1], pointStripList[i + 1][j]);
                    Triangle t2 = new Triangle(pointStripList[i][j + 1], pointStripList[i + 1][j + 1], pointStripList[i + 1][j]);

                    triList.Add(t1);
                    triList.Add(t2);
                }
               
            }
            int vertCount = 0;
            foreach(var tri in triList)
            {
                var indexList = new List<int>();
                foreach(var vt in tri.Vertices)
                {
                    var plyvert = new PlyVertex(vt,tri.Normal);
                    plyvert.ID = vertCount++;
                    indexList.Add(plyvert.ID);
                    _vertices.Add(plyvert);
                }
                var face = new PlyFace(indexList);
                _faces.Add(face);
            }
            
            
            Debug.WriteLine("triangles:{0} ", _faces.Count);
            Debug.WriteLine("Vertices:{0} ", _vertices.Count);
        }
        public void SaveAscii(string fileName)
        {
            if (ContentIsValid())
            {                
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    WriteHeader(sw);
                    WriteBody(sw);
                };
            }
        }
        public void Open(string fileName)
        {
            try
            {

                using (sr = new StreamReader(fileName))
                {
                    _containsNormals = false;
                    ReadHeader();
                    for (int i = 0; i < this.Header.Elements.Count; i++)
                    {
                        PlyElement element = Header.Elements.ElementAt(i);
                        AnalyzeElement(ref element);
                        List<string> section = new List<string>(element.Count);
                        for (int k = 0; k < element.Count; k++)
                            section.Add(NextLine());

                        ReadBody(section, element);

                        if (element.containsVertex && element.containsNormal)
                        {
                            _containsNormals = true;
                        }
                    }

                    if (!_containsNormals) CreateNormals();
                    GetExents();
                    
                };
            }
            catch(Exception ex)
            { 
                
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            
        }
        
        bool _containsVertices;
        bool _containsFaces;
        bool _containsEdges;
        bool _containsMaterials;
        bool _containsNormals;
        bool _containsEdgeColor;
        bool _containsVertexColor;
        bool _containsFaceColor;


        private bool ContentIsValid()
        {
            bool _contentIsValid = false;
            if (_vertices.Count > 0)
            {
                _containsVertices = true;
                _contentIsValid = true;
            }
            if (!_containsNormals)
            { 
                foreach (PlyVertex vertex in Vertices)
                {
                    _containsNormals = vertex.ContainsNormal;
                    _containsVertexColor = vertex.ContainsColor;
                    if (_containsVertexColor || _containsNormals) break;
                }
            }
            if (!_containsNormals)
            {
                CreateNormals();
            }
            if (_edges.Count > 0)
                _containsEdges = true;
            if (_faces.Count > 0)
                _containsFaces = true;
            if (_materials.Count > 0)
                _containsMaterials = true;

            return _contentIsValid;
        }
        
        private void CreateNormals()
        {
            if (_containsFaces)
            {
                FaceNorms = new Vector3[Faces.Count];
                for (int i = 0; i < Faces.Count; i++)
                {
                    Vector3 V01 = new Vector3( Vertices[Faces[i].Indices[0]]- Vertices[Faces[i].Indices[1]]);
                    Vector3 V21 = new Vector3( Vertices[Faces[i].Indices[1]] - Vertices[Faces[i].Indices[2]]);
                    Vector3 norm =V01.Cross(V21);

                    norm.Normalize();                    
                    FaceNorms[i]=norm;

                    foreach (int index in Faces[i].Indices)
                    {
                        Vertices[index].AddNormal( norm);                        
                    }
                }
                
            }
        }
        private BoundingBox extents;
        private void GetExents()
        {
            List<Vector3> vectors = new List<Vector3>();
            foreach(PlyVertex pv in Vertices)
            {
                Vector3 v = pv as Vector3;
                vectors.Add(v);
            }
            PointCloud pc = new PointCloud(vectors);

            extents = pc.BoundingBox;        
        }
        void AnalyzeElement(ref PlyElement element)
        {
            foreach (PlyProperty property in element.Properties)
            {
                    if ((property.Type == PlyPropertyType.nx)|
                        (property.Type==PlyPropertyType.ny)|
                        (property.Type==PlyPropertyType.nz))
                        element.containsNormal = true;
                    if ((property.Type == PlyPropertyType.x)|
                        (property.Type==PlyPropertyType.y)|
                        (property.Type==PlyPropertyType.z))
                        element.containsVertex=true;
                    if ((property.Type == PlyPropertyType.red) |
                        (property.Type == PlyPropertyType.blue) |
                        (property.Type == PlyPropertyType.green))
                        element.containsColor = true;                
            }
            if (element.Type == PlyElementType.vertex)
                _containsVertices = true;
            if (element.Type == PlyElementType.face)
                _containsFaces = true;
            if (element.Type == PlyElementType.edge)
                _containsEdges = true;
            if (element.Type == PlyElementType.material)
                _containsMaterials = true;

      }
        object Parse(string typeName, string value)
        {
            if (typeName == "int8")
                return sbyte.Parse(value);
            if (typeName == "uint8")
                return byte.Parse(value);
            if (typeName == "uint")
                return byte.Parse(value);
            if (typeName == "int16")
                return Int16.Parse(value);
            if (typeName == "uint16")
                return UInt16.Parse(value);
            if (typeName == "int32")
                return Int32.Parse(value);
            if (typeName == "int")
                return Int32.Parse(value);
            if (typeName == "uint32")
                return UInt32.Parse(value);
            if (typeName == "float32")
                return float.Parse(value);
            if (typeName == "float64")
                return double.Parse(value);
            if (typeName == "float")
                return double.Parse(value);
            else
                return value;
        }
      private void ReadBody(List<string> section, PlyElement element)
      {
          foreach (string line in section)
          {
              
              string[] tokens = line.Split(splitter,StringSplitOptions.RemoveEmptyEntries);
              if (element.Type == PlyElementType.vertex)
              {
                  float[] floatTokens = new float[tokens.Length];
                  PlyVertex Vertex = new PlyVertex();
                  for (int i = 0; i < tokens.Length; i++) 
                  {
                      string typeName = element.Properties[i].TypeName;
                      string name = element.Properties[i].Name;
                      PlyPropertyType propType = element.Properties[i].Type;
                     
                      if (propType == PlyPropertyType.x)
                          Vertex.X = float.Parse(tokens[i]);
                      if (propType == PlyPropertyType.y)
                          Vertex.Y = float.Parse(tokens[i]);
                      if (propType == PlyPropertyType.z)
                          Vertex.Z = float.Parse(tokens[i]);
                      if (propType == PlyPropertyType.nx)
                          Vertex.Normal.X = double.Parse(tokens[i]);
                      if (propType == PlyPropertyType.ny)
                          Vertex.Normal.Y = double.Parse(tokens[i]);
                      if (propType == PlyPropertyType.nz)
                          Vertex.Normal.Z = double.Parse(tokens[i]);
                      if (propType == PlyPropertyType.red)
                          Vertex.Col.Red = byte.Parse(tokens[i]);
                      if (propType == PlyPropertyType.green)
                          Vertex.Col.Green = byte.Parse(tokens[i]);
                      if (propType == PlyPropertyType.blue)
                          Vertex.Col.Blue = byte.Parse(tokens[i]);    

                  }
                  
                  Vertices.Add(Vertex);
              }
              if (element.Type == PlyElementType.face)
              {
                  
                  var indexList = new List<int>();
                  for (int i = 1; i < tokens.Length; i++) 
                  {
                      int temp = int.Parse(tokens[i]);
                      indexList.Add(temp);                      
                  }
                  Faces.Add(new PlyFace(indexList));

              }
              if (element.Type == PlyElementType.edge)
              {
                  PlyEdge edge = new PlyEdge(element.containsColor);
                  for (int i = 0; i < tokens.Length; i++) 
                  {
                      string typeName = element.Properties[i].TypeName;
                      string name = element.Properties[i].Name;
                      PlyPropertyType propType = element.Properties[i].Type;
                      
                      if (propType == PlyPropertyType.red)
                          edge.Color.Red = byte.Parse(tokens[i]);    
                      if (propType == PlyPropertyType.green)
                          edge.Color.Green = byte.Parse(tokens[i]);    
                      if (propType == PlyPropertyType.blue)
                          edge.Color.Blue = byte.Parse(tokens[i]);    
                      if (propType == PlyPropertyType.vertex1)
                          edge.Vertex1 = int.Parse(tokens[i]);    
                      if (propType == PlyPropertyType.vertex2)
                          edge.Vertex2 = int.Parse(tokens[i]);     
                  }
                  Edges.Add(edge);
              }
              if (element.Type == PlyElementType.material)
              {
                  for (int i = 0; i < tokens.Length; i++) { }
              }
              if (element.Type == PlyElementType.other)
              {
                  for (int i = 0; i < tokens.Length; i++) { }
              }
          }
      }
        private void WriteHeader(StreamWriter sw)
        {

            sw.WriteLine("ply");
            sw.WriteLine("format ascii 1.0 ");
            sw.WriteLine("comment");
            if (_containsVertices)
            {
                sw.WriteLine("element vertex "+Vertices.Count.ToString("D"));
                sw.WriteLine("property float x");
                sw.WriteLine("property float y");
                sw.WriteLine("property float z");
                //if (_containsNormals)
                //{
                //    sw.WriteLine("property float nx");
                //    sw.WriteLine("property float ny");
                //    sw.WriteLine("property float nz");
                //}
                if (_containsVertexColor)
                {
                    sw.WriteLine("property uint red");
                    sw.WriteLine("property uint green");
                    sw.WriteLine("property uint blue");
                }
 
            }
            if (_containsFaces)
            {
                sw.WriteLine("element face " + Faces.Count.ToString("D"));
                sw.WriteLine("property list uint int vertex_index");
            }
            if (_containsEdges)
            {
                sw.WriteLine("element edge " + Edges.Count.ToString("D"));
                sw.WriteLine("property int vertex1");
                sw.WriteLine("property int vertex2");

                if (_containsEdgeColor)
                {
                    sw.WriteLine("property uint red");
                    sw.WriteLine("property uint green");
                    sw.WriteLine("property uint blue");
                }                 

            }
            if (_containsMaterials)
            {
                sw.WriteLine("element material " + Materials.Count.ToString("D"));
            }
            sw.WriteLine("end_header");
        }
        
        private void WriteBody(StreamWriter sw)
        {
            
            foreach (PlyVertex vertex in _vertices)
            {
                string stb = "";
                stb = vertex.X.ToString("f7") + " " + vertex.Y.ToString("f7") + " " + vertex.Z.ToString("f7");
                //if (vertex.ContainsNormal)
                //{
                //    stb += " " + vertex.Normal.X.ToString("f7") + " " + vertex.Normal.Y.ToString("f7") + " " + vertex.Normal.Z.ToString("f7");
                //}
                if (vertex.ContainsColor)
                {
                    stb += " " + vertex.Col.Red.ToString("d") + " " + vertex.Col.Green.ToString("d") + " " + vertex.Col.Blue.ToString("d");
                }
                sw.WriteLine(stb);
               
            }
           
            foreach (PlyFace face in _faces)
            {
                string stb = "";
                stb = face.Indices.Count.ToString();
                foreach (int index in face.Indices)
                {
                    stb += " " + index.ToString() ;
                }
                sw.WriteLine(stb);
                

            }
            foreach (PlyEdge edge in _edges)
            {
                string stb = "";
                stb=edge.Vertex1.ToString() + " " + edge.Vertex2.ToString();
                if (edge.ContainsColor)
                {
                    stb +=  " " + edge.Color.Red.ToString() + " " + edge.Color.Green.ToString() + " " + edge.Color.Blue.ToString();
                }
                sw.WriteLine(stb);
            }
        }
        
      private void ReadHeader()
      {
          if (NextLine() != "ply")
          {
                 throw new Exception("Invalid PLY file. PLY files must start with the word \"ply\" on a single line.");
          }


          NextHeaderLine();
          this.Format = ReadFormat();
          NextHeaderLine();
          while(CurrentLine!= "end_header")
          {
                if(CurrentLine.StartsWith("element"))
                {
                   this.Header.Elements.Add(ReadElement());
                }
                //else
                //{
                //   throw new Exception(string.Format("Invalid line encountered in PLY header: \"{0}\".", currentLine));
                //}
          }
      }
      private PlyElement ReadElement()
      {
              string[] tokens = CurrentLine.Split(splitter,StringSplitOptions.RemoveEmptyEntries);  
                int count=0;
                if (!int.TryParse(tokens[2], out count))
                    throw new Exception(
                            string.Format("Invalid element count: \"{0}\"", tokens[2]));

                PlyElement element = new PlyElement();
                
                element.Name = tokens[1];
                PlyElementType tempType= PlyElementType.other;
                if (Enum.TryParse<PlyElementType>(tokens[1], out tempType))
                        element.Type = tempType;
                element.Count = count;
                

                while (NextHeaderLine().StartsWith("property"))
                {
                    element.AddProperty(ReadProperty(CurrentLine));
                }
                return element;
      }
      private PlyProperty ReadProperty(string line)
      {
          
          string[] tokens = line.Split(splitter, StringSplitOptions.RemoveEmptyEntries);

                if (tokens.Length != 3 && tokens.Length != 5)
                {
                    throw new Exception(string.Format("Invalid number of tokens in property line: \"{0}\".", CurrentLine));
                }
                if (tokens[0] != "property")
                {
                    throw new Exception(string.Format("Invalid property line: \"{0}\".", CurrentLine));
                }
                var property = new PlyProperty
                {
                    Name = tokens.Last(),

                    IsList = tokens[1] == "list",
                };

                if (property.IsList)
                {
                    property.ListCountTypeName = tokens[2];
                    property.TypeName = tokens[3];
                    property.Name = tokens[4];
                    if (tokens[4] == PlyPropertyType.vertex_indices.ToString())
                        property.Name = PlyPropertyType.vertex_index.ToString();
                    
                }
                else
                {
                    property.TypeName = tokens[1];
                    property.Name = tokens[2];
                }
                property.Type = PlyPropertyType.other;
                PlyPropertyType tempType = PlyPropertyType.other;
                if (Enum.TryParse<PlyPropertyType>(property.Name, out tempType))
                    property.Type = tempType;     

                return property;
      }          
      private PlyFormat ReadFormat()
      {
          string[] tokens = CurrentLine.Split(splitter, StringSplitOptions.RemoveEmptyEntries);

                if (tokens.Length != 3)
                {
                    throw new Exception("Incorrect number of tokens in format line. Format lines should follow the format: \"format <format_type> <format_version>\".");
                }

                if (tokens[0] != "format")
                    throw new Exception("Invalid format line in PLY file. PLY format lines must begin with the word \"format\".");

                PlyFormat format;

                if (tokens[1] == "ascii")
                {
                    if (tokens[2] == "1.0")
                    {
                        format = PlyFormat.ASCII;
                    }
                    else
                    {
                        throw new Exception(
                                string.Format("Invalid PLY format version: \"{0}\". Expected \"1.0\".", tokens[2]));
                    }
                }
                else
                {
                    throw new Exception(
                            string.Format("Invalid PLY format: \"{0}\". Right now the PlyMeshLoader can only handle PLY files in the ascii format.", tokens[1]));
                }

                
                return format;
            }
      public PlyFile()
      {
          Init();
      }
        
      }
    


   

}
