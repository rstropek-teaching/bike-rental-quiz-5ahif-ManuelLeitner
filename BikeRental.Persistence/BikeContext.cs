﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace BikeRental.Persistence {
    public class BikeContext : DbContext {

        public readonly string ConnectionString;

        public BikeContext(DbContextOptions<BikeContext> options) : base(options) { }
        public BikeContext(string connectionString) {
            ConnectionString = connectionString;
        }

        public BikeContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bike>().HasOne(typeof(Category)).WithMany();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured) {
                if (string.IsNullOrWhiteSpace(ConnectionString)) {
                    Console.WriteLine("Use in-memory DB");
                    optionsBuilder.UseInMemoryDatabase("InMemoryDB");
                } else {
                    Console.WriteLine("Use SQL-Server");
                    optionsBuilder.UseSqlServer(ConnectionString);
                }
            }
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Category> Category { get; set; }

    }

    public class ContextFactory : IDesignTimeDbContextFactory<BikeContext> {

        public BikeContext CreateDbContext(string[] args) {
            var builder = new ConfigurationBuilder().SetBasePath(Path.GetFullPath("../BikeRental"));
            builder.AddJsonFile("appsettings.json").AddEnvironmentVariables();

            var config = builder.Build();
            return new BikeContext(config.GetConnectionString("Azure"));
        }
    }
}
