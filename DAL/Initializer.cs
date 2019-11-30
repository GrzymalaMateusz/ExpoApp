using Inzynierka.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Inzynierka.DAL
{
    public class Initializer:DropCreateDatabaseIfModelChanges<ExpoesContext>
    {
        protected override void Seed(ExpoesContext context)
        {

            context.SaveChanges();
        }
    }
}