using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Wpf_Kurvovaya_BD;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cities> Cities { get; set; }

    public virtual DbSet<Countries> Countries { get; set; }

    public virtual DbSet<EnemyClubs> EnemyClubs { get; set; }

    public virtual DbSet<Games> Games { get; set; }

    public virtual DbSet<GameIns> GameIns { get; set; }

    public virtual DbSet<GameLevels> GameLevels { get; set; }

    public virtual DbSet<Gamers> Gamers { get; set; }

    public virtual DbSet<Leages> Leages { get; set; }

    public virtual DbSet<Managers> Managers { get; set; }

    public virtual DbSet<OurClubs> OurClubs { get; set; }

    public virtual DbSet<Positions> Positions { get; set; }

    public virtual DbSet<TrainingBasis> TrainingBases { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=781227xy");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_catalog", "adminpack");

        modelBuilder.Entity<Cities>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Cities_pkey");

            entity.Property(e => e.Id)
                .HasComment("Код города")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasComment("Название города")
                .HasColumnName("City");
        });

        modelBuilder.Entity<Countries>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Countries_pkey");

            entity.Property(e => e.Id)
                .HasComment("Код страны")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.Country)
                .HasMaxLength(255)
                .HasComment("Название страны")
                .HasColumnName("Country");
        });

        modelBuilder.Entity<EnemyClubs>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EnemyClubs_pkey");

            entity.Property(e => e.Id)
                .HasComment("Код клуба противника")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.IdCountry).HasComment("Код страны размещения клуба");
            entity.Property(e => e.NameCoach)
                .HasMaxLength(255)
                .HasComment("Имя тренера клуба");
            entity.Property(e => e.Opposing)
                .HasMaxLength(255)
                .HasComment("Название команды");
            entity.Property(e => e.PatronymicCoach)
                .HasMaxLength(255)
                .HasComment("Отчество тренера клуба");
            entity.Property(e => e.SurnameCoach)
                .HasMaxLength(255)
                .HasComment("Фамилия тренера клуба ");

            entity.HasOne(d => d.IdCountryNavigation).WithMany(p => p.EnemyClubs)
                .HasForeignKey(d => d.IdCountry)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EnemyClubs_IdCountry_fkey");
        });

        modelBuilder.Entity<Games>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Games_pkey");

            entity.Property(e => e.Id)
                .HasComment("Код игры")
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, null, 1000000L, null, null);
            entity.Property(e => e.CountFinish).HasComment("Количество пропущенных мячей");
            entity.Property(e => e.DateGame).HasComment("Дата проведения игры");
            entity.Property(e => e.IdCountry).HasComment("Код страны проведения игры");
            entity.Property(e => e.IdEnemyClub).HasComment("Код команды противника");
            entity.Property(e => e.IdLevel).HasComment("Код уровня игры");
            entity.Property(e => e.IdOurClub).HasComment("Код нашего клуба");

            entity.HasOne(d => d.IdCountryNavigation).WithMany(p => p.Games)
                .HasForeignKey(d => d.IdCountry)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Games_IdCountry_fkey");

            entity.HasOne(d => d.IdEnemyClubNavigation).WithMany(p => p.Games)
                .HasForeignKey(d => d.IdEnemyClub)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Games_IdEnemyClub_fkey");

            entity.HasOne(d => d.IdLevelNavigation).WithMany(p => p.Games)
                .HasForeignKey(d => d.IdLevel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Games_IdLevel_fkey");

            entity.HasOne(d => d.IdOurClubNavigation).WithMany(p => p.Games)
                .HasForeignKey(d => d.IdOurClub)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Games_IdOurClub_fkey");
        });

        modelBuilder.Entity<GameIns>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GameIns_pkey");

            entity.Property(e => e.Id)
                .HasComment("Код участия в игре")
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, null, 1000000L, null, null);
            entity.Property(e => e.CountStart).HasComment("Забитые игроком мячи");
            entity.Property(e => e.IdGame).HasComment("Код игры");
            entity.Property(e => e.IdGamer).HasComment("Код игрока");
            entity.Property(e => e.Order).HasComment("Участие в игре");
            entity.Property(e => e.Salary).HasComment("Премия за игру");

            entity.HasOne(d => d.IdGameNavigation).WithMany(p => p.GameIns)
                .HasForeignKey(d => d.IdGame)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GameIns_IdGame_fkey");

            entity.HasOne(d => d.IdGamerNavigation).WithMany(p => p.GameIns)
                .HasForeignKey(d => d.IdGamer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GameIns_IdGamer_fkey");
        });

        modelBuilder.Entity<GameLevels>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GameLevels_pkey");

            entity.Property(e => e.Id)
                .HasComment("Код уровня игры")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.GameLevel)
                .HasMaxLength(255)
                .HasComment("Уровень игры")
                .HasColumnName("GameLevel");
        });

        modelBuilder.Entity<Gamers>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Gamers_pkey");

            entity.Property(e => e.Id)
                .HasComment("Код игрока")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.Birthday).HasComment("День рождения игрока");
            entity.Property(e => e.Comments).HasComment("Контракт с игроком");
            entity.Property(e => e.Cost).HasComment("Стоимость контракта");
            entity.Property(e => e.IdClub).HasComment("Код клуба игрока");
            entity.Property(e => e.IdPosition).HasComment("Код позиции игрока на поле");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasComment("Имя игрока");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(255)
                .HasComment("Отчество игрока");
            entity.Property(e => e.Photo).HasComment("Фото игрока");
            entity.Property(e => e.Surname)
                .HasMaxLength(255)
                .HasComment("Фамилия игрока");
            entity.Property(e => e.YearFact).HasComment("Год принятия игрока в клуб");

            entity.HasOne(d => d.IdClubNavigation).WithMany(p => p.Gamers)
                .HasForeignKey(d => d.IdClub)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Gamers_IdClub_fkey");

            entity.HasOne(d => d.IdPositionNavigation).WithMany(p => p.Gamers)
                .HasForeignKey(d => d.IdPosition)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Gamers_IdPosition_fkey");
        });

        modelBuilder.Entity<Leages>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Leages_pkey");

            entity.Property(e => e.Id)
                .HasComment("Код названия лиги")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.Leage)
                .HasMaxLength(255)
                .HasComment("Название лиги")
                .HasColumnName("Leage");
        });

        modelBuilder.Entity<Managers>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Managers_pkey");

            entity.HasIndex(e => e.Phone, "Managers_Phone_key").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Код руководителя клуба/ов")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasComment("Имя руководителя");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(255)
                .HasComment("Отчество руководителя");
            entity.Property(e => e.Phone)
                .HasMaxLength(13)
                .HasComment("Номер телефона руководителя");
            entity.Property(e => e.Surname)
                .HasMaxLength(255)
                .HasComment("Фамилия руководителя");
        });

        modelBuilder.Entity<OurClubs>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("OurClubs_pkey");

            entity.Property(e => e.Id)
                .HasComment("Код нашего клуба")
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, null, 1000000L, null, null);
            entity.Property(e => e.Club)
                .HasMaxLength(255)
                .HasComment("Название нашего клуба");
            entity.Property(e => e.IdBase).HasComment("Код тренировочной базы клуба");
            entity.Property(e => e.IdCity).HasComment("Код города размещения клуба");
            entity.Property(e => e.IdLeage).HasComment("Код лиги клуба");
            entity.Property(e => e.IdManager).HasComment("Код руководителя клуба");
            entity.Property(e => e.Year)
                .HasDefaultValueSql("1900")
                .HasComment("Год создания клуба");

            entity.HasOne(d => d.IdBaseNavigation).WithMany(p => p.OurClubs)
                .HasForeignKey(d => d.IdBase)
                .HasConstraintName("OurClubs_IdBase_fkey");

            entity.HasOne(d => d.IdCityNavigation).WithMany(p => p.OurClubs)
                .HasForeignKey(d => d.IdCity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OurClubs_IdCity_fkey");

            entity.HasOne(d => d.IdLeageNavigation).WithMany(p => p.OurClubs)
                .HasForeignKey(d => d.IdLeage)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OurClubs_IdLeage_fkey");

            entity.HasOne(d => d.IdManagerNavigation).WithMany(p => p.OurClubs)
                .HasForeignKey(d => d.IdManager)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OurClubs_IdManager_fkey");
        });

        modelBuilder.Entity<Positions>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Positions_pkey");

            entity.Property(e => e.Id)
                .HasComment("Код позиции игрока на поле")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.Position)
                .HasMaxLength(255)
                .HasComment("Позиция игрока на поле")
                .HasColumnName("Position");
        });

        modelBuilder.Entity<TrainingBasis>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TrainingBases_pkey");

            entity.Property(e => e.Id)
                .HasComment("Код тренировочной базы")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.Base)
                .HasMaxLength(255)
                .HasComment("Название тренировчной базы");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}


