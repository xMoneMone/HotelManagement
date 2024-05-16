using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementAPI.Models;

public partial class HotelManagementContext : DbContext
{
    public HotelManagementContext()
    {
    }

    public HotelManagementContext(DbContextOptions<HotelManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountType> AccountTypes { get; set; }

    public virtual DbSet<Bed> Beds { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingsExtra> BookingsExtras { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Extra> Extras { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<HotelCode> HotelCodes { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomsBed> RoomsBeds { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersHotel> UsersHotels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=HotelManagement;Trusted_Connection=true;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccountT__3214EC07A0232143");

            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Bed>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Beds__3214EC077C2D3B28");

            entity.Property(e => e.BedType)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bookings__3214EC0760790A2B");

            entity.Property(e => e.DownPaymentPrice).HasColumnType("money");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.FirstName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FullPaymentPrice).HasColumnType("money");
            entity.Property(e => e.LastName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Notes)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Room).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__Bookings__RoomId__5441852A");
        });

        modelBuilder.Entity<BookingsExtra>(entity =>
        {
            entity.HasNoKey();

            entity.HasOne(d => d.Booking).WithMany()
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookingsE__Booki__59FA5E80");

            entity.HasOne(d => d.Extra).WithMany()
                .HasForeignKey(d => d.ExtraId)
                .HasConstraintName("FK__BookingsE__Extra__59063A47");
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Colors__3214EC07170D920E");

            entity.Property(e => e.Color1)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Color");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Currenci__3214EC074CDC9A84");

            entity.Property(e => e.FormattingString)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(300)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Extra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Extras__3214EC07E9B4E1FE");

            entity.Property(e => e.Name)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Extras)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("FK__Extras__HotelId__571DF1D5");
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Hotels__3214EC0777EA7534");

            entity.Property(e => e.Name)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.Currency).WithMany(p => p.Hotels)
                .HasForeignKey(d => d.CurrencyId)
                .HasConstraintName("FK__Hotels__Currency__412EB0B6");

            entity.HasOne(d => d.OwnerNavigation).WithMany(p => p.Hotels)
                .HasForeignKey(d => d.Owner)
                .HasConstraintName("FK__Hotels__Owner__4222D4EF");
        });

        modelBuilder.Entity<HotelCode>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Code)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Hotel).WithMany()
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("FK__HotelCode__Hotel__440B1D61");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rooms__3214EC07EDD86E4C");

            entity.Property(e => e.Notes)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.PricePerNight).HasColumnType("money");
            entity.Property(e => e.RoomNumber)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Hotel).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("FK__Rooms__HotelId__4CA06362");
        });

        modelBuilder.Entity<RoomsBed>(entity =>
        {
            entity.HasNoKey();

            entity.HasOne(d => d.Bed).WithMany()
                .HasForeignKey(d => d.BedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RoomsBeds__BedId__5165187F");

            entity.HasOne(d => d.Room).WithMany()
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__RoomsBeds__RoomI__5070F446");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07CB35F072");

            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.AccountType).WithMany(p => p.Users)
                .HasForeignKey(d => d.AccountTypeId)
                .HasConstraintName("FK__Users__AccountTy__3C69FB99");

            entity.HasOne(d => d.Color).WithMany(p => p.Users)
                .HasForeignKey(d => d.ColorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__ColorId__3B75D760");
        });

        modelBuilder.Entity<UsersHotel>(entity =>
        {
            entity.HasNoKey();

            entity.HasOne(d => d.Hotel).WithMany()
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("FK__UsersHote__Hotel__49C3F6B7");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsersHote__UserI__48CFD27E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
