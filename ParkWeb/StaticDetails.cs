using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkWeb
{
    public static class StaticDetails
    {
        public static string APIBaseUrl = "https://localhost:44338/";
        public static string NationalParkAPIPath = APIBaseUrl +"api/v1/nationalparks/";
        public static string TrailApiPath = APIBaseUrl +"api/v1/trails";
    }
}
