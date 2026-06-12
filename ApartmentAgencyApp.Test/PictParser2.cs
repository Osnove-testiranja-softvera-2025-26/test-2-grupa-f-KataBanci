using System;
using System.Collections.Generic;
using System.IO;
using ApartmentAgencyApp.Models;
using NUnit.Framework;

namespace ApartmentAgencyApp.Test
{
    public class PictParser2
    {
        private static readonly string PictResultPath =
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Pict2.txt");

        public static IEnumerable<TestCaseData> GetTestCases()
        {
            string[] lines = File.ReadAllLines(PictResultPath);

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split('\t');

                if (parts.Length < 6)
                    continue;

                ApartmentType apartmentType =
                    (ApartmentType)Enum.Parse(
                        typeof(ApartmentType),
                        parts[0].Trim());

                int distance =
                    int.Parse(parts[1].Trim());

                int beds =
                    int.Parse(parts[2].Trim());

                int days =
                    int.Parse(parts[3].Trim());

                int seasonDays =
                    int.Parse(parts[4].Trim());

                ApartmentComplex expected =
                    (ApartmentComplex)Enum.Parse(
                        typeof(ApartmentComplex),
                        parts[5].Trim());

                yield return new TestCaseData(
                    apartmentType,
                    distance,
                    beds,
                    days,
                    seasonDays,
                    expected)
                    .SetName(
                        $"Reservation_" +
                        $"Type={apartmentType}_" +
                        $"Distance={distance}_" +
                        $"Beds={beds}_" +
                        $"Days={days}_" +
                        $"SeasonDays={seasonDays}_" +
                        $"Expected={expected}");
            }
        }
    }
}