﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Session3.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Session3.DAL.Data.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(D => D.Id).UseIdentityColumn(10,10);
        
            //builder.HasMany(D=>D.Employees)
            //    .WithOne(E=>E.Department)
            //    .HasForeignKey(E=>E.DepartmentId)
            //    .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
