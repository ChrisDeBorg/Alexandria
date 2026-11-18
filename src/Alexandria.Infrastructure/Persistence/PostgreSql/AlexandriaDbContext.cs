
using Alexandria.Domain.Entities;
using Alexandria.Domain.Relations;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Infrastructure.Persistence.PostgreSql;

public sealed class AlexandriaDbContext(
    DbContextOptions<AlexandriaDbContext> options)
    : DbContext(options)
{
    public DbSet<BaseEntity> Entities => Set<BaseEntity>();
    public DbSet<Entity> GenericEntities => Set<Entity>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<Company> Companies => Set<Company>();

    public DbSet<Relation> Relations => Set<Relation>();
    public DbSet<RelationType> RelationTypes => Set<RelationType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureEntities(modelBuilder);
        ConfigureRelations(modelBuilder);
        ConfigureRelationTypes(modelBuilder);
    }

    private static void ConfigureEntities(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<BaseEntity>();

        entity.ToTable("Entities");
        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .ValueGeneratedNever();

        entity.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(500);

        entity.Property(x => x.Description)
            .HasMaxLength(4000);

        entity.Property(x => x.EntityTypeId)
            .IsRequired();

        entity.Property(x => x.MetadataJson)
            .HasColumnType("jsonb");

        entity.Property(x => x.CreatedAt)
            .IsRequired();

        entity.Property(x => x.UpdatedAt);

        entity.HasIndex(x => x.Name);

        // EF Core verwaltet den Discriminator als Shadow Property.
        // Er ist kein Bestandteil des Domain-Modells.
        entity.HasDiscriminator<string>("Discriminator")
            .HasValue<BaseEntity>("BaseEntity")
            .HasValue<Entity>("Entity")
            .HasValue<Person>("Person")
            .HasValue<Organization>("Organization")
            .HasValue<Company>("Company");

        modelBuilder.Entity<Entity>(e =>
        {
            e.Property(x => x.ShortName)
                .HasMaxLength(200);

            // Vorläufig keine bidirektionalen Relation-Navigationen.
            //e.Ignore(x => x.Outgoing);
            //e.Ignore(x => x.Incoming);
        });

        modelBuilder.Entity<Person>(e =>
        {
            e.Property(x => x.GivenName).HasMaxLength(200);
            e.Property(x => x.FamilyName).HasMaxLength(200);
            e.Property(x => x.CallingName).HasMaxLength(200);
            e.Property(x => x.AdditionalName).HasMaxLength(200);
            e.Property(x => x.Ethnicity).HasMaxLength(200);

            e.Property(x => x.Gender)
                .HasConversion<string>()
                .HasMaxLength(50);

            // PostgreSQL-Array für die alternativen Namen.
            e.PrimitiveCollection(x => x.AlternateNames)
                .HasColumnType("text[]");
        });

        modelBuilder.Entity<Organization>(e =>
        {
            e.Property(x => x.Website)
                .HasMaxLength(1000);
        });

        modelBuilder.Entity<Company>(e =>
        {
            e.Property(x => x.StockSymbol).HasMaxLength(20);
            e.Property(x => x.Exchange).HasMaxLength(100);

            e.Property(x => x.AnnualRevenue)
                .HasPrecision(20, 2);
        });
    }

    private static void ConfigureRelations(ModelBuilder modelBuilder)
    {
        var relation = modelBuilder.Entity<Relation>();

        relation.ToTable("Relations");
        relation.HasKey(x => x.Id);

        relation.Property(x => x.Id)
            .ValueGeneratedNever();

        relation.Property(x => x.Context)
            .HasMaxLength(4000);

        // SourceEntity und TargetEntity referenzieren
        // die gemeinsame TPH-Tabelle Entities.
        relation.HasOne<BaseEntity>()
            .WithMany()
            .HasForeignKey(x => x.SourceEntityId)
            .OnDelete(DeleteBehavior.Restrict);

        relation.HasOne<BaseEntity>()
            .WithMany()
            .HasForeignKey(x => x.TargetEntityId)
            .OnDelete(DeleteBehavior.Restrict);

        relation.HasOne<RelationType>()
            .WithMany()
            .HasForeignKey(x => x.RelationTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        relation.HasIndex(x => x.SourceEntityId);
        relation.HasIndex(x => x.TargetEntityId);
        relation.HasIndex(x => x.RelationTypeId);
    }

    private static void ConfigureRelationTypes(ModelBuilder modelBuilder)
    {
        var relationType = modelBuilder.Entity<RelationType>();

        relationType.ToTable("RelationTypes");
        relationType.HasKey(x => x.Id);

        relationType.Property(x => x.Id)
            .ValueGeneratedNever();

        relationType.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        relationType.Property(x => x.DisplayName)
            .IsRequired()
            .HasMaxLength(200);

        relationType.Property(x => x.Description)
            .HasMaxLength(4000);

        relationType.HasIndex(x => x.Name)
            .IsUnique();

        // Optionaler Verweis auf den inversen RelationType.
        relationType.HasOne<RelationType>()
            .WithMany()
            .HasForeignKey(x => x.InverseRelationTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
