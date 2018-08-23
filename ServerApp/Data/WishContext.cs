﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Data {
    public class WishContext : IdentityDbContext<User> {
        public WishContext(DbContextOptions<WishContext> options) : base(options) {
        }

        public virtual DbSet<List> List { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<List>().ToTable("List");
            modelBuilder.Entity<Item>().ToTable("Item");
        }
    }
}