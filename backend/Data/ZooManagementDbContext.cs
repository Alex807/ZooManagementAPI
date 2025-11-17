using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Enums;

namespace backend.Data
{
    public class ZooManagementDbContext : DbContext
    {
        public ZooManagementDbContext(DbContextOptions<ZooManagementDbContext> options)
            : base(options) { }

        public DbSet<Animal> Animals { get; set; } // DbSets are used to can use proper types in navigation properties
        public DbSet<Category> Categories { get; set; }
        public DbSet<Enclosure> Enclosures { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<FeedingSchedule> FeedingSchedules { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<StaffAnimalAssignment> StaffAnimalAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.Name) 
                    .IsUnique();
            });

            modelBuilder.Entity<Enclosure>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Animal>(entity =>
            {
                entity.HasOne(a => a.Category)
                    .WithMany(c => c.Animals)
                    .HasForeignKey(a => a.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                entity.HasOne(a => a.Enclosure)
                    .WithMany(e => e.Animals)
                    .HasForeignKey(a => a.EnclosureId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);

                entity.HasIndex(e => new { e.Name, e.Specie, e.CategoryId })
                        .IsUnique();
                entity.HasIndex(e => e.EnclosureId);

                // Convert enum to string for MySQL storage
                entity.Property(e => e.Gender)
                    .HasConversion<string>();
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.Name)
                        .IsUnique();

                // Convert enum to string for MySQL storage
                entity.Property(e => e.Name)
                    .HasConversion<string>();
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasIndex(e => e.Username)
                        .IsUnique();
                entity.HasIndex(e => e.Email)
                        .IsUnique();

                entity.HasOne(u => u.CurrentRole)
                    .WithMany(r => r.UserAccounts)
                    .HasForeignKey(u => u.CurrentRoleId)
                    .OnDelete(DeleteBehavior.Restrict) //blocks deletion if users are assigned to this role
                    .IsRequired();

                entity.HasOne(u => u.UserDetails)
                    .WithOne(ud => ud.UserAccount)
                    .HasForeignKey<UserDetails>(ud => ud.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasIndex(e => e.UserAccountId)
                        .IsUnique();

                entity.HasOne(s => s.UserAccount)
                    .WithOne(u => u.Staff)
                    .HasForeignKey<Staff>(s => s.UserAccountId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity<UserDetails>(entity =>
            {
                entity.Property(e => e.Gender)
                    .HasConversion<string>();
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                // Composite primary key
                entity.HasKey(ur => new { ur.UserAccountId, ur.RoleId });

                // Relationship: UserRole -> UserAccount (Many-to-One)
                entity.HasOne(ur => ur.UserAccount)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserAccountId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relationship: UserRole -> Role (Many-to-One)
                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<FeedingSchedule>(entity =>
            {
                // Relationship: FeedingSchedule -> Animal (Many-to-One, Required)
                entity.HasOne(fs => fs.Animal)
                    .WithMany(a => a.FeedingSchedules)
                    .HasForeignKey(fs => fs.AnimalId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                // Relationship: FeedingSchedule -> Staff (Keeper) (Many-to-One, Optional)
                entity.HasOne(fs => fs.Staff)
                    .WithMany(u => u.FeedingSchedules)
                    .HasForeignKey(fs => fs.StaffId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);

                entity.HasIndex(e => e.AnimalId);
                entity.HasIndex(e => e.StaffId);
                entity.HasIndex(e => e.FeedingTime);

                entity.Property(e => e.Status)
                    .HasConversion<string>();

            });

            modelBuilder.Entity<MedicalRecord>(entity =>
            {
                // Relationship: MedicalRecord -> Animal (Many-to-One, Required)
                entity.HasOne(mr => mr.Animal)
                    .WithMany(a => a.MedicalRecords)
                    .HasForeignKey(mr => mr.AnimalId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                // Relationship: MedicalRecord -> Staff (Veterinarian) (Many-to-One, Optional)
                entity.HasOne(mr => mr.Staff)
                    .WithMany(u => u.MedicalRecords)
                    .HasForeignKey(mr => mr.StaffId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);

                entity.Property(e => e.Status)
                    .HasConversion<string>();

                entity.HasIndex(e => e.AnimalId);
                entity.HasIndex(e => e.StaffId);
                entity.HasIndex(e => e.Date);
            });

            modelBuilder.Entity<StaffAnimalAssignment>(entity =>
            {
                // Composite primary key
                entity.HasKey(saa => new { saa.StaffId, saa.AnimalId });

                // Relationship: StaffAnimalAssignment -> Staff (Many-to-One)
                entity.HasOne(saa => saa.Staff)
                    .WithMany(u => u.AnimalAssignments)
                    .HasForeignKey(saa => saa.StaffId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relationship: StaffAnimalAssignment -> Animal (Many-to-One)
                entity.HasOne(saa => saa.Animal)
                    .WithMany(a => a.StaffAssignments)
                    .HasForeignKey(saa => saa.AnimalId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}