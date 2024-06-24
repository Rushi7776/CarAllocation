using CarAllocation.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CarAllocation.Service
{
    public class ParkingService
    {
        private readonly string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "parkingData.json");
        private List<ParkingAllocation> allocatedParkings;
        private int nextParkingNumber;

        public ParkingService()
        {
            if (File.Exists(filePath))
            {
                allocatedParkings = JsonConvert.DeserializeObject<List<ParkingAllocation>>(File.ReadAllText(filePath));
                nextParkingNumber = allocatedParkings.Count > 0 ? allocatedParkings.Max(p => p.ParkingNumber) + 1 : 1;
            }
            else
            {
                allocatedParkings = new List<ParkingAllocation>();
                nextParkingNumber = 1;
            }
        }

        public ParkingAllocation AllocateParking(string carPlateNumber)
        {
            if (allocatedParkings.Count >= 100)
            {
                throw new Exception("Parking is full");
            }

            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddHours(1); // Example: allocate for 1 hour

            decimal totalAmount = CalculateTotalAmount(startTime, endTime);

            var allocatedParking = new ParkingAllocation
            {
                CarPlateNumber = carPlateNumber,
                StartTime = startTime,
                EndTime = endTime,
                ParkingNumber = nextParkingNumber++,
                TotalAmount = totalAmount
            };

            allocatedParkings.Add(allocatedParking);
            SaveToFile();

            return allocatedParking;
        }

        public List<ParkingAllocation> GetParkingList(int pageNumber, int pageSize)
        {
            return allocatedParkings.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        private decimal CalculateTotalAmount(DateTime startTime, DateTime endTime)
        {
            decimal pricePerHour = IsNightTime(startTime) ? 50 : 30;
            TimeSpan duration = endTime - startTime;
            int totalHours = (int)Math.Ceiling(duration.TotalHours);
            decimal totalAmount = totalHours * pricePerHour;
            return totalAmount;
        }

        private bool IsNightTime(DateTime dateTime)
        {
            TimeSpan timeOfDay = dateTime.TimeOfDay;
            return timeOfDay < TimeSpan.FromHours(6) || timeOfDay >= TimeSpan.FromHours(22);
        }

        private void SaveToFile()
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(allocatedParkings));
        }
    }
}
