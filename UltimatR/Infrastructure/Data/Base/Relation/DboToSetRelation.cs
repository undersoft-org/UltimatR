using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Instant;
using System.Instant.Linking;

namespace UltimatR
{
    public enum ExpandSite
    {
        None = 0,
        OnRight = 1,
        OnLeft = 2,
        WithMany = 4,
        WithOne = 8        
    }

    public class DboToSetRelation<TParent, TChild> where TParent : Entity where TChild : Entity
    {
        private readonly string PARENT_TABLE_NAME = typeof(TParent).Name + "s";
        private readonly string CHILD_TABLE_NAME = typeof(TChild).Name + "s";
        private readonly string PARENT_NAME = typeof(TParent).Name;
        private readonly string CHILD_NAME = typeof(TChild).Name.Replace(typeof(TParent).Name, "") + "s";
        private readonly string PARENT_SCHEMA = null;
        private readonly string CHILD_SCHEMA = null;

        private readonly ExpandSite _expandSite;
        private readonly ModelBuilder _modelBuilder;
        private readonly EntityTypeBuilder<TParent> _firstBuilder;
        private readonly EntityTypeBuilder<TChild> _secondBuilder;

        public DboToSetRelation(ModelBuilder modelBuilder, ExpandSite expandSite = ExpandSite.OnRight, string parentSchema = null)
         : this(modelBuilder, null, null, null, null, expandSite, parentSchema, parentSchema) { }

        public DboToSetRelation(ModelBuilder modelBuilder, string parentName, string childName, ExpandSite expandSite = ExpandSite.OnRight, string parentSchema = null)
            : this(modelBuilder, parentName, null, childName, null, expandSite, parentSchema, parentSchema) { }

        public DboToSetRelation(ModelBuilder modelBuilder,
                                       string parentName,
                                       string parentTableName,
                                       string childName,
                                       string childTableName,
                                   ExpandSite expandSite = ExpandSite.OnRight,
                                       string parentSchema = null,
                                       string childSchema = null)
        {
            _modelBuilder = modelBuilder;
            _firstBuilder = modelBuilder.Entity<TParent>();
            _secondBuilder = modelBuilder.Entity<TChild>();
            _expandSite = expandSite;

            if(parentTableName != null)
                PARENT_TABLE_NAME = parentTableName;
            if(childTableName != null)
                CHILD_TABLE_NAME = childTableName;
            if(parentName != null)
                PARENT_NAME = parentName;
            if(childName != null)
                CHILD_NAME = childName;
            if(parentSchema != null)
                PARENT_SCHEMA = parentSchema;
            if(childSchema != null)
                CHILD_SCHEMA = childSchema;
        }

        public ModelBuilder Configure()
        {
            if (PARENT_SCHEMA != null && CHILD_SCHEMA != null)
            {
                _firstBuilder.ToTable(PARENT_TABLE_NAME, PARENT_SCHEMA);
                _secondBuilder.ToTable(CHILD_TABLE_NAME, CHILD_SCHEMA);
            }

            _firstBuilder.HasMany(CHILD_NAME)
                .WithOne(PARENT_NAME)
                .HasForeignKey(PARENT_NAME + "Id");

            _secondBuilder.HasOne(PARENT_NAME)
                .WithMany(CHILD_NAME)
                .HasForeignKey(PARENT_NAME + "Id");

            //new Link(new Sleeve(typeof(TParent), PARENT_NAME),                     
            //         new Sleeve(typeof(TChild), CHILD_NAME),
            //         new[] { "Id" }, new[] { PARENT_NAME + "Id" });

            if (_expandSite != ExpandSite.None)
            {
                if ((_expandSite & (ExpandSite.OnRight | ExpandSite.WithMany)) > 0)
                    _firstBuilder.Navigation(CHILD_NAME).AutoInclude();
                else
                    _secondBuilder.Navigation(PARENT_NAME).AutoInclude();
            }
            return _modelBuilder;
        }
    }
}