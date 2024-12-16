using BryophytaClassifier.Models;
using Microsoft.EntityFrameworkCore;

namespace BryophytaClassifier.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
    public DbSet<User> Users { get; set; }

    public DbSet<Prediction> Predictions { get; set; }
}
