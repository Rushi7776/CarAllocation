namespace CarAllocation.Model
{
    public class ParkingAllocation
    {
        public string CarPlateNumber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ParkingNumber { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
