using System;
using System.Collections.Generic;

namespace TaxiApp
{
    class District
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public int DistanceFromCenter { get; set; }
        public int NumberOfTaxis { get; set; }
    }

    class Taxi
    {
        public int Id { get; set; }
        public string Car { get; set; }
        public bool IsAvailable { get; set; }
        public string CurrentDistrict { get; set; }
        public int ArrivalTime { get; set; }
    }

    public class Program
    {
        static List<District> districts = new List<District>
        {
            new District { Number = 1, Name = "Retkinia", DistanceFromCenter = -2, NumberOfTaxis = 0 },
            new District { Number = 2, Name = "Łódź Kaliska", DistanceFromCenter = -1, NumberOfTaxis = 0 },
            new District { Number = 3, Name = "Śródmieście", DistanceFromCenter = 0, NumberOfTaxis = 0 },
            new District { Number = 4, Name = "Widzew", DistanceFromCenter = 1, NumberOfTaxis = 0 },
            new District { Number = 5, Name = "Janów", DistanceFromCenter = 2, NumberOfTaxis = 0 }
        };

        static List<Taxi> taxis = new List<Taxi>
        {
            new Taxi { Id = 1, Car = "Ford Mondeo", IsAvailable = true, CurrentDistrict = "Retkinia", ArrivalTime = 0 },
            new Taxi { Id = 2, Car = "Dacia Logan", IsAvailable = true, CurrentDistrict = "Łódź Kaliska", ArrivalTime = 0 },
            new Taxi { Id = 3, Car = "Toyota Avensis", IsAvailable = true, CurrentDistrict = "Śródmieście", ArrivalTime = 0 },
            new Taxi { Id = 4, Car = "Mercedes E220", IsAvailable = true, CurrentDistrict = "Widzew", ArrivalTime = 0 },
            new Taxi { Id = 5, Car = "Huindai Lantra", IsAvailable = true, CurrentDistrict = "Janów", ArrivalTime = 0 }
        };

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Wyświetl listę dzielnic");
                Console.WriteLine("2. Wyświetl listę taksówek");
                Console.WriteLine("3. Zamów taksówkę");
                Console.WriteLine("4. Wyjdź z aplikacji");

                Console.Write("Wybierz opcję: ");
                int option;
                if (int.TryParse(Console.ReadLine(), out option))
                {
                    switch (option)
                    {
                        case 1:
                            DisplayDistricts();
                            break;
                        case 2:
                            DisplayTaxis();
                            break;
                        case 3:
                            OrderTaxi();
                            break;
                        case 4:
                            return;
                        default:
                            Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                }

                Console.WriteLine();
            }
        }

        static void DisplayDistricts()
        {
            Console.WriteLine("Lista dzielnic:");
            Console.WriteLine("Numer\tNazwa\tOdległość od centrum\tIlość taksówek");
            foreach (var district in districts)
            {
                Console.WriteLine(district.Number + "\t" + district.Name + "\t" + district.DistanceFromCenter + " km\t" + district.NumberOfTaxis);
            }
        }

        static void DisplayTaxis()
        {
            Console.WriteLine("Lista taksówek:");
            Console.WriteLine("Id\tSamochód\tStatus\tAktualna dzielnica\tCzas dojazdu do ostatniego zlecenia");
            foreach (var taxi in taxis)
            {
                Console.WriteLine(taxi.Id + "\t" + taxi.Car + "\t" + (taxi.IsAvailable ? "wolny" : "zajęty") + "\t" + taxi.CurrentDistrict + "\t" + taxi.ArrivalTime + " min");
            }
        }

        static void OrderTaxi()
        {
            Console.Write("Podaj numer dzielnicy do której chcesz zamówić taksówkę: ");
            int districtNumber;
            if (int.TryParse(Console.ReadLine(), out districtNumber))
            {
                District requestedDistrict = districts.Find(d => d.Number == districtNumber);
                if (requestedDistrict != null)
                {
                    List<Taxi> availableTaxis = taxis.FindAll(t => t.IsAvailable);
                    if (availableTaxis.Count > 0)
                    {
                        Taxi optimalTaxi = availableTaxis[0];
                        int minArrivalTime = CalculateArrivalTime(optimalTaxi.CurrentDistrict, requestedDistrict.Name);
                        foreach (var taxi in availableTaxis)
                        {
                            int arrivalTime = CalculateArrivalTime(taxi.CurrentDistrict, requestedDistrict.Name);
                            if (arrivalTime < minArrivalTime)
                            {
                                minArrivalTime = arrivalTime;
                                optimalTaxi = taxi;
                            }
                        }

                        Console.WriteLine("Taksówka zrealizuje zlecenie: " + optimalTaxi.Car + ". Przewidywany czas dojazdu: " + minArrivalTime + " min.");
                        optimalTaxi.IsAvailable = false;
                        optimalTaxi.CurrentDistrict = requestedDistrict.Name;
                        optimalTaxi.ArrivalTime = minArrivalTime;

                        requestedDistrict.NumberOfTaxis++;

                        Console.WriteLine("Zaktualizowana lista dzielnic:");
                        DisplayDistricts();
                        Console.WriteLine("Zaktualizowana lista taksówek:");
                        DisplayTaxis();
                    }
                    else
                    {
                        Console.WriteLine("Brak dostępnych taksówek.");
                    }
                }
                else
                {
                    Console.WriteLine("Podano nieprawidłowy numer dzielnicy.");
                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowy numer dzielnicy. Spróbuj ponownie.");
            }
        }

        static int CalculateArrivalTime(string currentDistrict, string requestedDistrict)
        {
            foreach (var district in districts)
            {
                if (district.Name == currentDistrict)
                {
                    int currentDistance = Math.Abs(district.DistanceFromCenter);
                    foreach (var d in districts)
                    {
                        if (d.Name == requestedDistrict)
                        {
                            int requestedDistance = Math.Abs(d.DistanceFromCenter);
                            int distanceDifference = Math.Abs(requestedDistance - currentDistance);
                            int arrivalTime = (district.Name == requestedDistrict) ? 4 : distanceDifference * 5;
                            return arrivalTime;
                        }
                    }
                }
            }
            return -1;
        }
    }
}
