namespace xFordPassLite.net.Models
{
    public struct FordInfo
    {
        public FordInfo(VehicleStatus vehicle, string jSONstring) : this()
        {
            Vehicle = vehicle;
            JSONstring = jSONstring;
        }

        public VehicleStatus Vehicle { get; set; }
        public string JSONstring { get; set; }

    }
}
