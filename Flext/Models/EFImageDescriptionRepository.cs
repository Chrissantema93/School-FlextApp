using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flext.Models
{
    public interface IDescriptionRepository
    {
        IQueryable<ImageDescription> Detecties { get; }
        void SaveToDB(ImageDescription instance);
        void RemoveFromDB(ImageDescription instance);
    }

    public class EFImageDescriptionRepository : IDescriptionRepository
    {
        private ApplicatieDbContext context;
        public EFImageDescriptionRepository(ApplicatieDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<ImageDescription> Detecties => context.Detecties;

        public void SaveToDB(ImageDescription instance)
        {
            context.Add(instance);
            context.SaveChanges();
        }

        public void RemoveFromDB(ImageDescription instance)
        {
            if (instance != null)
            {
                context.Remove(instance);
                context.SaveChanges();
            }
        }
    }
}
