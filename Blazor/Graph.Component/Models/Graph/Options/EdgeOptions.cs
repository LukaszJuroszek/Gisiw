namespace Graph.Component.Models.Graph.Options
{
    public class EdgeOptions
    {
        public bool Shadow { get; set; }
        public SmoothOption Smooth { get; set; }
        public ColorOption Color { get; set; }
    }

    public class ColorOption
    {
        public string Color { get; set; }
    }

    public class SmoothOption
    {
        public string Type { get; set; }
        public string ForceDirection { get; set; }
        public double Roundness { get; set; }
        public bool Enabled { get; set; }
    }

}