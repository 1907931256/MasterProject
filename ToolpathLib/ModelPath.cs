using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
namespace ToolpathLib
{
    public class ModelPath:List<ModelPathEntity>
    {
       
        BoundingBox ext;
        BoundingBox jetOnBoundingBox;
        
        public BoundingBox BoundingBox 
        { 
            get 
            { 
                return ext; 
            } 
        }
        public BoundingBox JetOnBoundingBox
        {
            get
            {
                
                return jetOnBoundingBox;
            }
        }
        public bool IsFiveAxis { get; set; }
        public double MeshSize { get; set; }
        public ModelPath(BoundingBox ext,BoundingBox jetOnBoundingBox)
        {
            this.ext = ext;
            this.jetOnBoundingBox = jetOnBoundingBox;
        }
        public ModelPath()
        {
            ext = new BoundingBox();
            jetOnBoundingBox = new BoundingBox();
        }
       
    }
}
