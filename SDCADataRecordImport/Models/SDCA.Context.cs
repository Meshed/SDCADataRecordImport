﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SDCADataRecordImport.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SDCAV2Entities : DbContext
    {
        public SDCAV2Entities()
            : base("name=SDCAV2Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AnimalCount> AnimalCounts { get; set; }
        public virtual DbSet<AnimalIntake> AnimalIntakes { get; set; }
        public virtual DbSet<AnimalOutcome> AnimalOutcomes { get; set; }
        public virtual DbSet<Org_Facility> Org_Facility { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
    }
}
