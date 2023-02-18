using Microsoft.EntityFrameworkCore;
using ParkyApi.Data;
using ParkyApi.Models;
using ParkyApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _Db;
        public TrailRepository(ApplicationDbContext Db)
        {
            _Db = Db;
        }
        public bool CreateTrail(Trail trail)
        {
            _Db.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _Db.Trails.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int trailId)
        {
           return _Db.Trails.Include(c => c.NationalPark).FirstOrDefault(x => x.Id == trailId);
        }

        public ICollection<Trail> GetTrails()
        {
            return _Db.Trails.Include(c => c.NationalPark).OrderBy(x => x.Name).ToList();
        }

        public bool TrailExists(string name)
        {
            bool value = _Db.Trails.Any(x => x.Name.ToLower() == name.ToLower().Trim());
            return value;
        }

        public bool TrailExists(int id)
        {
            return _Db.Trails.Any(x => x.Id == id);
        }

        public bool Save()
        {
            return _Db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTrail(Trail trail)
        {
            _Db.Trails.Update(trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int npId)
        {
            return _Db.Trails.Include(c => c.NationalPark).Where(c => c.NationalParkId == npId).ToList();
        }
    }
}
