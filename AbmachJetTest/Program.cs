using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbMachModel;
namespace AbmachJetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            double meshSize =.002;
            int index =3;
            double jetD =.050;

            AbMachJet abmachJet = new AbMachJet(jetD,meshSize,index);
            Console.WriteLine(abmachJet.Diameter.ToString() );
            Console.WriteLine(abmachJet.JetMeshRadius.ToString());
            Console.WriteLine(abmachJet.EquationIndex.ToString());
            Console.ReadLine();
            double[,] footprint = abmachJet.FootPrint();
            List<string> file = new List<string>();
            List<DrawingIO.DwgEntity> pointList = new List<DrawingIO.DwgEntity>();
            DrawingIO.DXFFile dxffile = new DrawingIO.DXFFile();
           

            for(int i = 0;i<footprint.GetLength(0);i++)
            {
                for (int j = 0 ;j<footprint.GetLength(1);j++)
                {
                    double x = i * meshSize;
                    double y = j * meshSize;
                    double z = footprint[i, j];
                    string l = x + "," + y + "," + z;
                    DrawingIO.Vector3 pt = new DrawingIO.Vector3(x, y, z);
                    pointList.Add(pt);
                    file.Add(l);
                    Console.WriteLine(l);
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter("jetfootprint.txt"))
                    {
                        foreach (string line in file)
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
            }
            dxffile.Save(pointList, "jetfootprint.dxf");
            Console.ReadLine();

        }
    }
}
