using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiToken.Data.DataContexts
{

    public class DataContext : DbContext
    {        
        public DataContext(DbContextOptions<DataContext> option) : base (option)
        {
            Database.EnsureCreated();
        }

    }


}
