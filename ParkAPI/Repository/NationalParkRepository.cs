using ParkAPI.Data;
using ParkAPI.Models;
using ParkAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkAPI.Repository
{
    public class NationalParkRepository : INationalParkRepository

    {
        private readonly ApplicationDbContext _db;

        //injecting dependencies in order to access them
        public NationalParkRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateNationalPark(Models.NationalPark nationalPark)
        {
            _db.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(Models.NationalPark nationalPark)
        {
            _db.NationalParks.Remove(nationalPark);
            return Save();
        }

        public Models.NationalPark GetNationalPark(int nationalParkId)
        {
            return _db.NationalParks.FirstOrDefault(a => a.Id == nationalParkId);
        }

        public ICollection<Models.NationalPark> GetNationalParks()
        {
            return _db.NationalParks.OrderBy(a => a.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
            bool value = _db.NationalParks.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool NationalParkExists(int id)
        {
            return _db.NationalParks.Any(a => a.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateNationalPark(Models.NationalPark nationalPark)
        {
            _db.NationalParks.Update(nationalPark);
            return Save();
        }
    }
}
