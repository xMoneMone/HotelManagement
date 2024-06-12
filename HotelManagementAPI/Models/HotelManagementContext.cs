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

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<HotelCode> HotelCodes { get; set; }

    public virtual DbSet<HotelCodeStatus> HotelCodeStatuses { get; set; }

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
            entity.HasKey(e => e.Id).HasName("PK__AccountT__3214EC07E6BABABA");

            entity.HasIndex(e => e.Type, "UQ__AccountT__F9B8A48BC448E5EE").IsUnique();

            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Bed>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Beds__3214EC076B87C8B5");

            entity.Property(e => e.BedType)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bookings__3214EC07D60A3359");

            entity.Property(e => e.DownPaymentPrice).HasColumnType("money");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullPaymentPrice).HasColumnType("money");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Notes)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Room).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__Bookings__RoomId__5CD6CB2B");
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Colors__3214EC07BD342C43");

            entity.HasIndex(e => e.Color1, "UQ__Colors__E11D3845ED86E867").IsUnique();

            entity.Property(e => e.Color1)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Color");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Currenci__3214EC07C353BD27");

            entity.Property(e => e.FormattingString)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(300)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Hotels__3214EC077DB86E24");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Currency).WithMany(p => p.Hotels)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Hotels__Currency__440B1D61");

            entity.HasOne(d => d.Owner).WithMany(p => p.Hotels)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK__Hotels__OwnerId__44FF419A");
        });

        modelBuilder.Entity<HotelCode>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK__HotelCod__A25C5AA667AB189A");

            entity.Property(e => e.Code)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Hotel).WithMany(p => p.HotelCodes)
                .HasForeignKey(d => d.HotelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HotelCode__Hotel__4AB81AF0");

            entity.HasOne(d => d.Sender).WithMany(p => p.HotelCodeSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HotelCode__Sende__4D94879B");

            entity.HasOne(d => d.Status).WithMany(p => p.HotelCodes)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HotelCode__Statu__4CA06362");

            entity.HasOne(d => d.User).WithMany(p => p.HotelCodeUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HotelCode__UserI__4BAC3F29");
        });

        modelBuilder.Entity<HotelCodeStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HotelCod__3214EC0704E78A4E");

            entity.HasIndex(e => e.Status, "UQ__HotelCod__3A15923FB73591C8").IsUnique();

            entity.Property(e => e.Status)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rooms__3214EC079A144C90");

            entity.Property(e => e.Notes)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.PricePerNight).HasColumnType("money");
            entity.Property(e => e.RoomNumber)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Hotel).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("FK__Rooms__HotelId__5441852A");
        });

        modelBuilder.Entity<RoomsBed>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoomsBed__3214EC076042AE86");

            entity.HasOne(d => d.Bed).WithMany(p => p.RoomsBeds)
                .HasForeignKey(d => d.BedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RoomsBeds__BedId__59FA5E80");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomsBeds)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__RoomsBeds__RoomI__59063A47");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0719D76903");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105343AA788EE").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.AccountType).WithMany(p => p.Users)
                .HasForeignKey(d => d.AccountTypeId)
                .HasConstraintName("FK__Users__AccountTy__3F466844");

            entity.HasOne(d => d.Color).WithMany(p => p.Users)
                .HasForeignKey(d => d.ColorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__ColorId__3E52440B");
        });

        modelBuilder.Entity<UsersHotel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsersHot__3214EC078DDF5AF3");

            entity.HasOne(d => d.Hotel).WithMany(p => p.UsersHotels)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("FK__UsersHote__Hotel__5165187F");

            entity.HasOne(d => d.User).WithMany(p => p.UsersHotels)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsersHote__UserI__5070F446");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
