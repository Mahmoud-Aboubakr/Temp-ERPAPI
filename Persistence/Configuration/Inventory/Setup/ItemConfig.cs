using Domain.Entities.Inventory.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration.Inventory.Setup
{
    public class ItemConfig : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder
                .Property(x => x.Retail)
                .HasColumnType("decimal(18,3)");
            builder
               .Property(x => x.Wholesale)
               .HasColumnType("decimal(18,3)");
            builder
               .Property(x => x.CostPrice)
               .HasColumnType("decimal(18,3)");
        }
    }
}
