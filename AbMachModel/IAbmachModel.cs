using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SurfaceModel;
using ToolpathLib;
namespace AbMachModel
{
    public enum ModelType
    {
        Array2D,
        Octree3D
    }
    public interface IAbmachModel<T> where T : SurfacePoint, new()
    {
        ISurface<T> GetSurface();
        ModelPath GetPath();
        RemovalRate GetRemovalRate();

        void Run(CancellationToken ct, IProgress<int> progress);
    }
}
