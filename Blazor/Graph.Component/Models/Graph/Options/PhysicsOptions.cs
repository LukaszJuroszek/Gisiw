namespace Graph.Component.Models.Graph.Options
{
    public class PhysicsOptions
    {
        public ForceAtlas2Based ForceAtlas2Based { get; set; }
        public double MaxVelocity { get; set; }
        public double MinVelocity { get; set; }
        public string Solver { get; set; }
        public double Timestep { get; set; }
        public Stabilization Stabilization { get; set; }
    }

    public class ForceAtlas2Based
    {
        public int GravitationalConstant { get; set; }
        public double CentralGravity { get; set; }
        public int SpringLength { get; set; }
        public double SpringConstant { get; set; }
        public double Damping { get; set; }
    }

    public class Stabilization
    {
        public bool Enabled { get; set; }
        public int Iterations { get; set; }
        public int UpdateInterval { get; set; }
    }
}