namespace Graph.Component.Models.Options
{
    public class PhysicsOptions
    {
        public ForceAtlas2Based ForceAtlas2Based { get; set; }
        public double MaxVelocity { get; set; }
        public double MinVelocity { get; set; }
        public string Solver { get; set; }
        public int Timestep { get; set; }
    }

    public class ForceAtlas2Based
    {
        public int GravitationalConstant { get; set; }
        public double CentralGravity { get; set; }
        public int SpringLength { get; set; }
        public int SpringConstant { get; set; }
        public int Damping { get; set; }
    }
}