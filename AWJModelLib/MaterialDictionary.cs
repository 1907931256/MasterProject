using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWJModel
{
    public class MaterialDictionary
    {
        Dictionary<string, Material> dict;
        public int Count { get { return dict.Count; } }
      
        public bool MaterialExists(string name)
        {
            string nameUpper = name.ToUpper();     

            bool exists =  dict.ContainsKey(nameUpper);
            return exists;
        }
        public bool MaterialExists(Material mat)
        {
            bool exists = dict.ContainsKey(mat.Name);
            return exists;
        }

        public Material GetMaterial(string name)
        {
            Material mat= new Material();
            string nameUpper = name.ToUpper();
            dict.TryGetValue(nameUpper, out mat);
            
            return mat;
        }
        public void RemoveMaterial(string name)
        {
            string nameUpper = name.ToUpper();
            if (dict.ContainsKey(nameUpper))
            {
                dict.Remove(nameUpper);
            }

        }
        public void AddMaterial(Material mat)
        {
            Material matOut = new Material();
            string nameUpper = mat.Name.ToUpper();
            if (!dict.ContainsKey(nameUpper))
            {
                dict.Add(nameUpper, mat);
            }
            
        }
        public List<string> GetMaterialNames()
        {
            var keyList = new List<string>();
            foreach(string key in dict.Keys)
            {
                keyList.Add(key);
            }
            return keyList;
        }
        public MaterialDictionary()
        {
            dict = new Dictionary<string, Material>();

        }

    }
}
