using Infrastructure.EFModel;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class SieveCustomFilterMethods : ISieveCustomFilterMethods
    {
        /// <summary>
        /// Apply filter to get only those projects withouot choosen people
        /// </summary>
        /// <param name="source"></param>
        /// <param name="op"></param>
        /// <param name="values">list of personIds</param>
        /// <returns></returns>
        //public IQueryable<Osoba> BezUloga(IQueryable<Osoba> source, string op, string[] values)
        //{
        //    if (values != null)
        //    {
        //        foreach (string value in values)
        //        {
        //            int ulogaId = int.Parse(value);
        //            source = source.Where(p => !p.IdUloga.Any(pr => pr.IdUloga == ulogaId));
        //        }
        //    }
        //    return source;
        //}

        /// <summary>
        /// Used to filter people such that only projects' non-members are included
        /// </summary>
        /// <param name="source"></param>
        /// <param name="op"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public IQueryable<Racun> SpecificPerson(IQueryable<Racun> source, string op, string[] values)
        {
            if (values != null)
            {
                foreach (string value in values)
                {
                    source = source.Where(p => p.IdOsoba == int.Parse(value));
                }
            }
            return source;
        }

        public IQueryable<Racun> RacunPostoji(IQueryable<Racun> source, string op, string[] values)
        {
            if (values != null)
            {
                foreach (string value in values)
                {
                    source = source.Where(p => p.IdOsoba.ToString() == value)
                        .Where(x => x.DatumRacuna.Month == DateTime.Now.Month)
                        .Where(x => x.DatumRacuna.Year == DateTime.Now.Year);
                }
            }
            return source;
        }
    }
}
