using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApartmentAgencyApp.Models;
using ApartmentAgencyApp.Services;


namespace ApartmentAgencyApp.Fakes
{
    public class FakeApartmentService : IApartmentService
    {
        public List<Apartment> Apartments { get; set; }

        public FakeApartmentService()
        {
            Apartments = new List<Apartment>();
        }

        public List<Apartment> GetAvailableApartments(ReservationRequest request)
        {
            return Apartments;
        }
    }
}