﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RReplay
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class UnitsContext : DbContext
    {
        public UnitsContext()
            : base("name=UnitsContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Era> Era { get; set; }
        public virtual DbSet<MaxDeployableAmount> MaxDeployableAmount { get; set; }
        public virtual DbSet<Nations> Nations { get; set; }
        public virtual DbSet<OtanUnits> OtanUnits { get; set; }
        public virtual DbSet<PactUnits> PactUnits { get; set; }
        public virtual DbSet<ProductionPrice> ProductionPrice { get; set; }
        public virtual DbSet<ShowInMenu> ShowInMenu { get; set; }
        public virtual DbSet<Specialization> Specialization { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TUniteAuSol> TUniteAuSol { get; set; }
        public virtual DbSet<Units_Translation_US> Units_Translation_US { get; set; }
        public virtual DbSet<UnitTypeTokens> UnitTypeTokens { get; set; }
        public virtual DbSet<Interface_Translation_US> Interface_Translation_US { get; set; }
        public virtual DbSet<TModuleSelector> TModuleSelector { get; set; }
        public virtual DbSet<TTypeUnit> TTypeUnit { get; set; }
        public virtual DbSet<SimpleUnit> MainUnitsView { get; set; }
    }
}