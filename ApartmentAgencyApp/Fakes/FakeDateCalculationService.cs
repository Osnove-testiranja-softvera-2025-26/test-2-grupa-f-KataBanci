using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApartmentAgencyApp.Models;
using ApartmentAgencyApp.Services;

namespace ApartmentAgencyApp.Fakes
{
    public class FakeDateCalculationService : IDateCalculationService
    {
        public RequestDaysInfo DaysInfo { get; set; }

        public FakeDateCalculationService()
        {
            DaysInfo = new RequestDaysInfo();
        }

        public RequestDaysInfo GetDaysInfo(DateTime from, DateTime to)
        {
            return DaysInfo;
        }
    }
}