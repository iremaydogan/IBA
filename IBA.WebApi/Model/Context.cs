using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;

namespace IBA.WebApi.Model
{
    public class Context : DbContext
    {
        public Context()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=213.238.168.103;Database=IremBeyzaDB; User Id=iremBeyzaUser;Password=irem-beyza-06;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<TeacherStudent> TeacherStudents { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Country> Countrys { get; set; }
        public DbSet<Janitor> Janitors { get; set; }
    }
    }
