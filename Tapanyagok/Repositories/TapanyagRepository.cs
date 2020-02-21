using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapanyagok.Models;

namespace Tapanyagok.Repositories
{
    class TapanyagRepository : IDisposable
    {
        private EtelContext db = new EtelContext();
        private int _totalItems;

        public BindingList<tapanyag> GetAllTapanyag(
            int page = 0,
            int itemsPerPage = 0,
            string search = null,
            string sortBy = null,
            bool ascending = true)
        {
            var query = db.tapanyag.OrderBy(x => x.id).AsQueryable();

            // Keresés
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();

                float ertek;
                float.TryParse(search.Replace('.',','), out ertek);

                if (ertek > 0)
                {
                    query = query.Where(x => 
                        x.energia == ertek ||
                        x.feherje.Value == ertek ||
                        x.zsir.Value == ertek ||
                        x.szenhidrat.Value == ertek);
                }
                else
                {
                    query = query.Where(x => x.nev.Contains(search));
                }
            }

            // Sorbarendezés
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy)
                {
                    default:
                        query = ascending ? query.OrderBy(x => x.id) : query.OrderByDescending(x => x.id);
                        break;
                    case "nev":
                        query = ascending ? query.OrderBy(x => x.nev) : query.OrderByDescending(x => x.nev);
                        break;
                    case "energia":
                        query = ascending ? query.OrderBy(x => x.energia) : query.OrderByDescending(x => x.energia);
                        break;
                    case "feherje":
                        query = ascending ? query.OrderBy(x => x.feherje) : query.OrderByDescending(x => x.feherje);
                        break;
                    case "zsir":
                        query = ascending ? query.OrderBy(x => x.zsir) : query.OrderByDescending(x => x.zsir);
                        break;
                    case "szenhidrat":
                        query = ascending ? query.OrderBy(x => x.szenhidrat) : query.OrderByDescending(x => x.szenhidrat);
                        break;
                }
            }


            // Összes találat kiszámítása
            _totalItems = query.Count();

            // Oldaltördelés
            if (page + itemsPerPage > 0)
            {
                query = query.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);
            }

            return new BindingList<tapanyag>(query.ToList());
        }

        public int Count()
        {
            return _totalItems;
        }

        public void Insert(tapanyag jk)
        {
            db.tapanyag.Add(jk);
        }

        public void Delete(int id)
        {
            var jk = db.tapanyag.Find(id);
            db.tapanyag.Remove(jk);
        }

        public void Update(tapanyag param)
        {
            var jk = db.tapanyag.Find(param.id);
            if (jk != null)
            {
                db.Entry(jk).CurrentValues.SetValues(param);
            }
        }

        public bool Exists(tapanyag tapanyag)
        {
            return db.tapanyag.Any(x => x.id == tapanyag.id);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }
    }
}
