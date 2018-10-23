namespace GeometryLib
{
    public class Ray
    {
        public Vector3 Origin { get; set; }
        public Vector3 Direction { get; set; }
        public Vector3 InverseDir { get { return inverseDir; } }
        public int[] Sign;
        public double TMin;
        public double TMax;

        Vector3 inverseDir;

        public BoundingBox BoundingBox()
        {
            return new BoundingBox(Origin.X, Origin.Y, Origin.Z, Direction.X, Direction.Y, Direction.Z);
        }
        public Vector3 PointOnRayAt(double distance)
        {
            Vector3 pt = new Vector3(Origin.X + distance * Direction.X, Origin.Y + distance * Direction.Y, Origin.Z + distance * Direction.Z);
            return pt;
        }
        public Ray()
        {
            Sign = new int[3];
            TMin = 0;
            TMax = double.MaxValue;
            Origin = new Vector3();
            Direction = new Vector3();
            inverseDir = new Vector3();
        }
        public Ray(Vector3 origin, Vector3 dir,double minLength, double maxLength)
        {
            inverseDir = new Vector3(1 / dir.X, 1 / dir.Y, 1 / dir.Z);
            Sign = new int[3];
            TMin = minLength;
            TMax = maxLength;
            Sign[0] = inverseDir.X < 0 ? 1 : 0;
            Sign[1] = inverseDir.Y < 0 ? 1 : 0;
            Sign[2] = inverseDir.Z < 0 ? 1 : 0;
            Origin = origin;
            Direction = dir;
        }
        public Ray(Vector3 origin, Vector3 dir)
        {
            inverseDir = new Vector3(1 / dir.X, 1 / dir.Y, 1 / dir.Z);
            Sign = new int[3];
            TMin = 0;
            TMax = double.MaxValue;
            Sign[0] = inverseDir.X < 0 ? 1 : 0;
            Sign[1] = inverseDir.Y < 0 ? 1 : 0;
            Sign[2] = inverseDir.Z < 0 ? 1 : 0;
            Origin = origin;
            Direction = dir;
        }
        
    }
}
