using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Infrastructure.DataAccess
{
    public class DatabaseContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configurationDbContext;

        public DatabaseContextFactory(Action<DbContextOptionsBuilder> configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
        }

        public DataBaseContext  CreateDbContext()
        {
            DbContextOptionsBuilder<DataBaseContext> optionsBuilder = new();
            _configurationDbContext(optionsBuilder);

            return new DataBaseContext(optionsBuilder.Options);
        }
    }
}
