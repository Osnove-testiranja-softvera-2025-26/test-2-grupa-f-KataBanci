using ApartmentAgencyApp.Exceptions;
using ApartmentAgencyApp.Models;
using ApartmentAgencyApp.Services;
using ApartmentAgencyApp.Fakes;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ApartmentAgencyApp.Test
{
    public class ApartmentAgencyServiceTest
    {
        private Guid requestId;
        private Guid apartmentId;

        [SetUp]
        public void SetUp()
        {
            requestId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            apartmentId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        }

        [Test]
        public void MakeApartmentReservation_BedOnlyDistanceLessThan500AndBedsGreaterOrEqual3_ComplexA()
        {
            var dateService = new FakeDateCalculationService();
            var apartmentService = new FakeApartmentService();
            var reservationService = new FakeReservationService();

            apartmentService.Apartments.Add(new Apartment { Id = apartmentId });

            var service = new ApartmentAgencyService(dateService, apartmentService, reservationService);

            var request = new ReservationRequest
            {
                Id = requestId,
                ApartmentType = ApartmentType.BedOnly,
                DistanceFromTheBeach = 499,
                NumberOfBeds = 3
            };

            service.MakeApartmentReservation(request);

            Assert.That(reservationService.Reservation.ApartmentComplex, Is.EqualTo(ApartmentComplex.ComplexA));
            Assert.That(reservationService.Reservation.ApartmentId, Is.EqualTo(apartmentId));
            Assert.That(reservationService.Reservation.ReservationRequestId, Is.EqualTo(requestId));
        }

        [Test]
        public void MakeApartmentReservation_BedOnlyDistanceLessThan500AndBedsLessThan3_ComplexB()
        {
            var dateService = new FakeDateCalculationService();
            var apartmentService = new FakeApartmentService();
            var reservationService = new FakeReservationService();

            apartmentService.Apartments.Add(new Apartment { Id = apartmentId });

            var service = new ApartmentAgencyService(dateService, apartmentService, reservationService);

            var request = new ReservationRequest
            {
                Id = requestId,
                ApartmentType = ApartmentType.BedOnly,
                DistanceFromTheBeach = 499,
                NumberOfBeds = 2
            };

            service.MakeApartmentReservation(request);

            Assert.That(reservationService.Reservation.ApartmentComplex, Is.EqualTo(ApartmentComplex.ComplexB));
        }

        [Test]
        public void MakeApartmentReservation_StudioNumberOfDaysGreaterOrEqual5_ComplexB()
        {
            var dateService = new FakeDateCalculationService
            {
                DaysInfo = new RequestDaysInfo
                {
                    NumberOfDays = 5,
                    NumberOfSeasonDays = 0
                }
            };

            var apartmentService = new FakeApartmentService();
            var reservationService = new FakeReservationService();

            apartmentService.Apartments.Add(new Apartment { Id = apartmentId });

            var service = new ApartmentAgencyService(dateService, apartmentService, reservationService);

            var request = new ReservationRequest
            {
                Id = requestId,
                ApartmentType = ApartmentType.Studio,
                DistanceFromTheBeach = 1000,
                NumberOfBeds = 2
            };

            service.MakeApartmentReservation(request);

            Assert.That(reservationService.Reservation.ApartmentComplex, Is.EqualTo(ApartmentComplex.ComplexB));
        }

        [Test]
        public void MakeApartmentReservation_StudioSeasonDaysGreaterThan2_ComplexB()
        {
            var dateService = new FakeDateCalculationService
            {
                DaysInfo = new RequestDaysInfo
                {
                    NumberOfDays = 4,
                    NumberOfSeasonDays = 3
                }
            };

            var apartmentService = new FakeApartmentService();
            var reservationService = new FakeReservationService();

            apartmentService.Apartments.Add(new Apartment { Id = apartmentId });

            var service = new ApartmentAgencyService(dateService, apartmentService, reservationService);

            var request = new ReservationRequest
            {
                Id = requestId,
                ApartmentType = ApartmentType.Studio
            };

            service.MakeApartmentReservation(request);

            Assert.That(reservationService.Reservation.ApartmentComplex, Is.EqualTo(ApartmentComplex.ComplexB));
        }

        [Test]
        public void MakeApartmentReservation_StudioNumberOfDaysLessThan5AndSeasonDaysLessOrEqual2_ComplexC()
        {
            var dateService = new FakeDateCalculationService
            {
                DaysInfo = new RequestDaysInfo
                {
                    NumberOfDays = 4,
                    NumberOfSeasonDays = 2
                }
            };

            var apartmentService = new FakeApartmentService();
            var reservationService = new FakeReservationService();

            apartmentService.Apartments.Add(new Apartment { Id = apartmentId });

            var service = new ApartmentAgencyService(dateService, apartmentService, reservationService);

            var request = new ReservationRequest
            {
                Id = requestId,
                ApartmentType = ApartmentType.Studio
            };

            service.MakeApartmentReservation(request);

            Assert.That(reservationService.Reservation.ApartmentComplex, Is.EqualTo(ApartmentComplex.ComplexC));
        }

        [Test]
        public void MakeApartmentReservation_StudioWithTerrace_ComplexD()
        {
            var dateService = new FakeDateCalculationService();
            var apartmentService = new FakeApartmentService();
            var reservationService = new FakeReservationService();

            apartmentService.Apartments.Add(new Apartment { Id = apartmentId });

            var service = new ApartmentAgencyService(dateService, apartmentService, reservationService);

            var request = new ReservationRequest
            {
                Id = requestId,
                ApartmentType = ApartmentType.StudioWithTerrace
            };

            service.MakeApartmentReservation(request);

            Assert.That(reservationService.Reservation.ApartmentComplex, Is.EqualTo(ApartmentComplex.ComplexD));
        }

        [Test]
        public void MakeApartmentReservation_NoAvailableApartments_ThrowsException()
        {
            var dateService = new FakeDateCalculationService();
            var apartmentService = new FakeApartmentService();
            var reservationService = new FakeReservationService();

            var service = new ApartmentAgencyService(dateService, apartmentService, reservationService);

            var request = new ReservationRequest
            {
                Id = requestId,
                ApartmentType = ApartmentType.BedOnly,
                DistanceFromTheBeach = 400,
                NumberOfBeds = 3
            };

            Assert.Throws<NoAvailableApartmentsException>(() => service.MakeApartmentReservation(request));
        }

        [Test]
        public void MakeApartmentReservation_UsingNSubstitute_CallsReservationService()
        {
            var dateService = Substitute.For<IDateCalculationService>();
            var apartmentService = Substitute.For<IApartmentService>();
            var reservationService = Substitute.For<IReservationService>();

            var request = new ReservationRequest
            {
                Id = requestId,
                ApartmentType = ApartmentType.BedOnly,
                DistanceFromTheBeach = 400,
                NumberOfBeds = 3,
                DateOfArrival = new DateTime(2026, 6, 10),
                DateOfDeparture = new DateTime(2026, 6, 15)
            };

            dateService.GetDaysInfo(request.DateOfArrival, request.DateOfDeparture)
                .Returns(new RequestDaysInfo { NumberOfDays = 5, NumberOfSeasonDays = 0 });

            apartmentService.GetAvailableApartments(request)
                .Returns(new List<Apartment> { new Apartment { Id = apartmentId } });

            var service = new ApartmentAgencyService(dateService, apartmentService, reservationService);

            service.MakeApartmentReservation(request);

            reservationService.Received(1).MakeReservationInComplex(
                Arg.Is<Reservation>(r =>
                    r.ApartmentId == apartmentId &&
                    r.ReservationRequestId == requestId &&
                    r.ApartmentComplex == ApartmentComplex.ComplexA));
        }

        // Boundary Value Analysis za granične vrednosti:
        // 500/501, 80/81, 70/71, 1500/1501
        [TestCase(500, 81, ApartmentType.Studio, true, ApartmentRank.First)]
        [TestCase(500, 81, ApartmentType.Studio, false, ApartmentRank.Second)]
        [TestCase(501, 81, ApartmentType.Studio, true, ApartmentRank.Third)]
        [TestCase(500, 80, ApartmentType.Studio, true, ApartmentRank.Third)]
        [TestCase(1500, 71, ApartmentType.StudioWithTerrace, false, ApartmentRank.First)]
        [TestCase(1501, 71, ApartmentType.StudioWithTerrace, false, ApartmentRank.Second)]
        [TestCase(1500, 70, ApartmentType.StudioWithTerrace, false, ApartmentRank.Second)]
        [TestCase(500, 90, ApartmentType.BedOnly, false, ApartmentRank.Forth)]
        public void CalculateApartmentRank_BoundaryTests(
            double distanceFromTheBeach,
            int percentOfPositiveReviews,
            ApartmentType apartmentType,
            bool renovatedInTheLastYear,
            ApartmentRank expectedResult)
        {
            var service = new ApartmentAgencyService(null, null, null);

            var result = service.CalculateApartmentRank(
                distanceFromTheBeach,
                percentOfPositiveReviews,
                apartmentType,
                renovatedInTheLastYear);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        // PICT model korišćen za generisanje optimalnog broja test slučajeva za CalculateApartmentRank.
        [TestCaseSource(typeof(PictParser), nameof(PictParser.GetTestCases))]
        public void CalculateApartmentRank_PictTests(
            double distanceFromTheBeach,
            int percentOfPositiveReviews,
            ApartmentType apartmentType,
            bool renovatedInTheLastYear,
            ApartmentRank expectedResult)
        {
            var service = new ApartmentAgencyService(null, null, null);

            var result = service.CalculateApartmentRank(
                distanceFromTheBeach,
                percentOfPositiveReviews,
                apartmentType,
                renovatedInTheLastYear);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        // PICT model korišćen za određivanje optimalnog broja test slučajeva za funkciju MakeApartmentReservation.
        [TestCaseSource(typeof(PictParser2), nameof(PictParser2.GetTestCases))]
        public void MakeApartmentReservation_PictTests(
            ApartmentType apartmentType,
            int distance,
            int beds,
            int days,
            int seasonDays,
            ApartmentComplex expected)
        {
            var dateService = new FakeDateCalculationService
            {
                DaysInfo = new RequestDaysInfo
                {
                    NumberOfDays = days,
                    NumberOfSeasonDays = seasonDays
                }
            };

            var apartmentService = new FakeApartmentService();
            var reservationService = new FakeReservationService();

            apartmentService.Apartments.Add(new Apartment { Id = apartmentId });

            var service = new ApartmentAgencyService(dateService, apartmentService, reservationService);

            var request = new ReservationRequest
            {
                Id = requestId,
                ApartmentType = apartmentType,
                DistanceFromTheBeach = distance,
                NumberOfBeds = beds
            };

            service.MakeApartmentReservation(request);

            Assert.That(reservationService.Reservation.ApartmentComplex, Is.EqualTo(expected));
        }
    }
}
