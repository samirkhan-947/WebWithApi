using ParkyApi.Data;
using ParkyApi.Models;
using ParkyApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _Db;
        public NationalParkRepository(ApplicationDbContext Db)
        {
            _Db = Db;
        }
        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _Db.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _Db.NationalParks.Remove(nationalPark);
            return Save();
        }

        public NationalPark GetNationalPark(int nationalparkid)
        {
           return _Db.NationalParks.FirstOrDefault(x => x.Id == nationalparkid);
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _Db.NationalParks.OrderBy(x => x.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
            bool value = _Db.NationalParks.Any(x => x.Name.ToLower() == name.ToLower().Trim());
            return value;
        }

        public bool NationalParkExists(int id)
        {
            return _Db.NationalParks.Any(x => x.Id == id);
        }

        public bool Save()
        {
            return _Db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _Db.NationalParks.Update(nationalPark);
            return Save();
        }
    }
}
