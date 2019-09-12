using GetDataFromApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataFromApi.Repository
{
    public class APOContext : DbContext
    {
        public APOContext() : base("APOContext")
        {

        }
        public DbSet<APO> APOs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
