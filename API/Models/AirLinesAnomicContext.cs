using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class AirLinesAnomicContext : DbContext
{
    public AirLinesAnomicContext()
    {
    }

    public AirLinesAnomicContext(DbContextOptions<AirLinesAnomicContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aircraft> Aircrafts { get; set; }

    public virtual DbSet<Airport> Airports { get; set; }

    public virtual DbSet<AmenitiesTicket> AmenitiesTickets { get; set; }

    public virtual DbSet<Amenity> Amenities { get; set; }

    public virtual DbSet<CabinType> CabinTypes { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Office> Offices { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MATEBOOK_JEKA\\SQLEXPRESS;Database=airLinesANOMIC;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aircraft>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AirPlan");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.MakeModel).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AirPort");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.Iatacode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("IATACode");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Country).WithMany(p => p.Airports)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AirPort_Country");
        });

        modelBuilder.Entity<AmenitiesTicket>(entity =>
        {
            entity.HasKey(e => new { e.AmenityId, e.TicketId }).HasName("PK_TicketAmenities");

            entity.Property(e => e.AmenityId).HasColumnName("AmenityID");
            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.Amenity).WithMany(p => p.AmenitiesTickets)
                .HasForeignKey(d => d.AmenityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AmenitiesTickets_Amenities");

            entity.HasOne(d => d.Ticket).WithMany(p => p.AmenitiesTickets)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AmenitiesTickets_Tickets");
        });

        modelBuilder.Entity<Amenity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AdditionalService");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Service).HasMaxLength(50);
        });

        modelBuilder.Entity<CabinType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ClassType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasMany(d => d.Amenities).WithMany(p => p.CabinTypes)
                .UsingEntity<Dictionary<string, object>>(
                    "AmenitiesCabinType",
                    r => r.HasOne<Amenity>().WithMany()
                        .HasForeignKey("AmenityId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TravelClassAdditionalService_AdditionalService"),
                    l => l.HasOne<CabinType>().WithMany()
                        .HasForeignKey("CabinTypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TravelClassAdditionalService_TravelClass"),
                    j =>
                    {
                        j.HasKey("CabinTypeId", "AmenityId").HasName("PK_TravelClassAdditionalService");
                        j.ToTable("AmenitiesCabinType");
                        j.IndexerProperty<int>("CabinTypeId").HasColumnName("CabinTypeID");
                        j.IndexerProperty<int>("AmenityId").HasColumnName("AmenityID");
                    });
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Country");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Office>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Office");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Contact).HasMaxLength(250);
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Country).WithMany(p => p.Offices)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Office_Country");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserRole");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ArrivalAirportId).HasColumnName("ArrivalAirportID");
            entity.Property(e => e.DepartureAirportId).HasColumnName("DepartureAirportID");

            entity.HasOne(d => d.ArrivalAirport).WithMany(p => p.RouteArrivalAirports)
                .HasForeignKey(d => d.ArrivalAirportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Routes_Airports3");

            entity.HasOne(d => d.DepartureAirport).WithMany(p => p.RouteDepartureAirports)
                .HasForeignKey(d => d.DepartureAirportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Routes_Airports2");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Serivec");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AircraftId).HasColumnName("AircraftID");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.EconomyPrice).HasColumnType("money");
            entity.Property(e => e.FlightNumber).HasMaxLength(10);
            entity.Property(e => e.RouteId).HasColumnName("RouteID");
            entity.Property(e => e.Time).HasPrecision(5);

            entity.HasOne(d => d.Aircraft).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.AircraftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_AirCraft");

            entity.HasOne(d => d.Route).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_Routes");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Ticket");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookingReference).HasMaxLength(6);
            entity.Property(e => e.CabinTypeId).HasColumnName("CabinTypeID");
            entity.Property(e => e.Firstname).HasMaxLength(50);
            entity.Property(e => e.Lastname).HasMaxLength(50);
            entity.Property(e => e.PassportCountryId).HasColumnName("PassportCountryID");
            entity.Property(e => e.PassportNumber).HasMaxLength(9);
            entity.Property(e => e.Phone).HasMaxLength(14);
            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.CabinType).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.CabinTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_TravelClass");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Schedule");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Birthdate).HasColumnType("date");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.OfficeId).HasColumnName("OfficeID");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Office).WithMany(p => p.Users)
                .HasForeignKey(d => d.OfficeId)
                .HasConstraintName("FK_Users_Offices");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
