using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteOptimization
{
    internal class Location
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double Distance { get; set; }
        public Location Previous { get; set; }
        public bool Visited { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<Location> locations = new List<Location>()
            {
                new Location { Name = "A", Latitude = 37.788022, Longitude = -122.399797 },
                new Location { Name = "B", Latitude = 37.3318456, Longitude = -121.88107 },
                new Location { Name = "C", Latitude = 37.7749, Longitude = -122.4194 },
                new Location { Name = "D", Latitude = 37.7647, Longitude = -122.4631 }
            };

            Console.WriteLine("Starting location: ");
            Location start = locations[int.Parse(Console.ReadLine())];

            Console.WriteLine("Ending location: ");
            Location end = locations[int.Parse(Console.ReadLine())];

            List<Location> optimizedRoute = OptimizeRoute(start, end, locations);

            Console.WriteLine("Optimized Route: ");
            foreach (Location location in optimizedRoute)
            {
                Console.WriteLine(location.Name);
            }

            Console.ReadLine();
        }

        static List<Location> OptimizeRoute(Location start, Location end, List<Location> locations)
        {
            List<Location> route = new List<Location>();

            foreach (Location location in locations)
            {
                location.Distance = double.MaxValue;
                location.Previous = null;
                location.Visited = false;
            }

            start.Distance = 0;
            start.Previous = null;

            List<Location> unvisited = new List<Location>(locations);

            while (unvisited.Count > 0)
            {
                Location current = GetClosestLocation(unvisited);
                current.Visited = true;

                if (current == end)
                {
                    break;
                }

                foreach (Location neighbor in GetNeighbors(current, locations))
                {
                    double distance = GetDistance(current, neighbor);
                    if (neighbor.Distance > current.Distance + distance)
                    {
                        neighbor.Distance = current.Distance + distance;
                        neighbor.Previous = current;
                    }
                }
            }

            Location currentLocation = end;
            while (currentLocation != null)
            {
                route.Add(currentLocation);
                currentLocation = currentLocation.Previous;
            }

            route.Reverse();

            return route;
        }
        static Location GetClosestLocation(List<Location> locations)
        {
            Location closest = null;
            double closestDistance = double.MaxValue;

            foreach (Location location in locations)
            {
                if (!location.Visited && location.Distance < closestDistance)
                {
                    closest = location;
                    closestDistance = location.Distance;
                }
            }

            return closest;
        }

        static List<Location> GetNeighbors(Location location, List<Location> allLocations)
        {
            List<Location> neighbors = new List<Location>();

            foreach (Location otherLocation in allLocations)
            {
                if (otherLocation != location)
                {
                    neighbors.Add(otherLocation);
                }
            }

            return neighbors;
        }

        static double GetDistance(Location a, Location b)
        {
            double earthRadius = 6371; // Earth's radius in kilometers

            double lat1 = a.Latitude * Math.PI / 180;
            double lat2 = b.Latitude * Math.PI / 180;
            double deltaLat = (b.Latitude - a.Latitude) * Math.PI / 180;
            double deltaLon = (b.Longitude - a.Longitude) * Math.PI / 180;

            double a1 = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                        Math.Cos(lat1) * Math.Cos(lat2) *
                        Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a1), Math.Sqrt(1 - a1));
            double distance = earthRadius * c;

            return distance;
        }
    }

}
