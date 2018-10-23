using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AWJModel;
namespace AWJModelLibTests
{
    [TestClass]
    public class MaterialDictionaryTests
    {
        [TestMethod]
        public void MaterialDictionary_constructor_returnsZeroCount()
        {
            var matDict = new MaterialDictionary();
            int count = matDict.Count;
            Assert.AreEqual(0, count);
        }
        [TestMethod]
        public void MaterialDictionary_addMaterial_CountOK()
        {
            var matDict = new MaterialDictionary();
            var mat = new Material(MaterialType.Metal, "steel", .25, 100, 79, 70);
            matDict.AddMaterial(mat);
            var alum = new Material(MaterialType.Metal, "aluminum", .25, 100, 79, 70);
            matDict.AddMaterial(alum);
            Assert.AreEqual(2, matDict.Count, "mat count");
            var matOut  = matDict.GetMaterial("steel");
            Assert.AreEqual("STEEL", matOut.Name,"name");
            Assert.AreEqual(mat.MillMachinability, matOut.MillMachinability);
        }
        [TestMethod]
        public void MaterialDictionary_retrieveMaterial_propertyOK()
        {
            var matDict = new MaterialDictionary();
            var mat = new Material(MaterialType.Metal, "steel", .25, 100, 79, 70);
            matDict.AddMaterial(mat);
        }
        [TestMethod]
        public void MaterialDictionary_getMaterial_matOK()
        {
            var matDict = new MaterialDictionary();
            var mat = new Material(MaterialType.Metal, "steel", .25, 100, 79, 70);
            matDict.AddMaterial(mat);          
            var matOut = matDict.GetMaterial("steel");
            bool exists = matDict.MaterialExists("steel");
            Assert.IsTrue(exists);
            Assert.AreEqual("STEEL", matOut.Name, "name");
            Assert.AreEqual(mat.MillMachinability, matOut.MillMachinability);
        }
        [TestMethod]
        public void MaterialDictionary_getNonExistantMaterial_DoesntExist()
        {
            var matDict = new MaterialDictionary();
            var steel = new Material(MaterialType.Metal, "steel", .25, 100, 79, 70);
            var alum = new Material(MaterialType.Metal, "aluminum", .25, 100, 79, 70);
            matDict.AddMaterial(steel);
            matDict.AddMaterial(alum);
            bool exists = matDict.MaterialExists("plastic");
            Assert.IsFalse(exists);
            
        }
    }
}
