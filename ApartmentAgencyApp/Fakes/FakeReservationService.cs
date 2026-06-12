using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApartmentAgencyApp.Models;
using ApartmentAgencyApp.Services;

namespace ApartmentAgencyApp.Fakes
{
    public class FakeReservationService : IReservationService
    {
        public Reservation Reservation { get; set; }

        public void MakeReservationInComplex(Reservation reservation)
        {
            Reservation = reservation;
        }
    }
}