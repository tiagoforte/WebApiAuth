using System;
using System.Linq;
using WebApiToken.Data;

namespace WebApiToken.Config
{
  public static class DBInitializer
  {
    public static void Initialize(DataContext context)
    {
      context.Database.EnsureCreated();

      if (context.Product.Any())
      {
        return;
      }

      context.Product.AddRange(
        new Model.Product { Id = Guid.NewGuid(), Name = "Arroz", Price = 10.0m },
        new Model.Product { Id = Guid.NewGuid(), Name = "Feijão", Price = 8.0m },
        new Model.Product { Id = Guid.NewGuid(), Name = "Carne", Price = 25.0m },
        new Model.Product { Id = Guid.NewGuid(), Name = "Açucar", Price = 5.0m }
        );
      context.SaveChanges();
    }
  }
}
