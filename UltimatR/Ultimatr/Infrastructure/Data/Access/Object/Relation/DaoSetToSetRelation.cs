using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Instant.Linking;
using System.Instant;

namespace UltimatR
{
    public class DaoSetToSetRelation<TLeft, TRight> where TLeft : Entity where TRight : Entity
    {
        private readonly string RELATION_TABLE_NAME;
        private readonly string LEFT_TABLE_NAME      = typeof(TLeft).Name  + "s";
        private readonly string RIGHT_TABLE_NAME     = typeof(TRight).Name + "s";
        private readonly string LEFT_NAME            = typeof(TLeft).Name  + "s";
        private readonly string RIGHT_NAME           = typeof(TRight).Name.Replace(typeof(TLeft).Name, "") + "s";
        private readonly string LEFT_SCHEMA          = null;
        private readonly string RIGHT_SCHEMA         = null;

        private readonly ExpandSite _expandSite;
        private readonly ModelBuilder _modelBuilder;
        private readonly EntityTypeBuilder<TLeft> _firstBuilder;
        private readonly EntityTypeBuilder<TRight> _secondBuilder;
        private readonly EntityTypeBuilder<DaoRelation<TLeft, TRight>> _relationBuilder;

        public DaoSetToSetRelation(ModelBuilder modelBuilder, ExpandSite expandSite = ExpandSite.OnRight, string dbSchema = null)
           : this(modelBuilder, null, null, null, null, expandSite, dbSchema, dbSchema) { }

        public DaoSetToSetRelation(ModelBuilder modelBuilder, string leftName, string rightName, ExpandSite expandSite = ExpandSite.OnRight, string dbSchema = null)
            : this(modelBuilder, leftName, leftName, rightName, rightName, expandSite, dbSchema, dbSchema) { }

        public DaoSetToSetRelation(ModelBuilder modelBuilder, 
                                       string leftName,
                                       string leftTableName,
                                       string rightName,
                                       string rightTableName,
                                   ExpandSite expandSite = ExpandSite.OnRight,
                                       string parentSchema = null,
                                       string childSchema = null)
        {
            _modelBuilder    = modelBuilder;
            _firstBuilder    = _modelBuilder.Entity<TLeft>();
            _secondBuilder   = _modelBuilder.Entity<TRight>();
            _relationBuilder = _modelBuilder.Entity<DaoRelation<TLeft, TRight>>();
            _expandSite = expandSite;

            if(leftTableName != null)   LEFT_TABLE_NAME   = leftTableName;
            if(rightTableName != null)  RIGHT_TABLE_NAME  = rightTableName;
            if(leftName != null)        LEFT_NAME         = leftName;
            if(rightName != null)       RIGHT_NAME        = rightName;
            if(parentSchema != null)    LEFT_SCHEMA       = parentSchema;
            if(childSchema != null)     RIGHT_SCHEMA      = childSchema;

            RELATION_TABLE_NAME = LEFT_NAME + "To" + RIGHT_NAME;
        }
        public ModelBuilder Configure()
        {
            if (LEFT_SCHEMA != null && RIGHT_NAME != null)
            {
                _firstBuilder.ToTable(LEFT_TABLE_NAME, LEFT_SCHEMA);
                _secondBuilder.ToTable(RIGHT_TABLE_NAME, RIGHT_SCHEMA);
            }
            _relationBuilder.ToTable(RELATION_TABLE_NAME, DbSchema.RelationSchema);

            _firstBuilder.HasMany<TRight>(RIGHT_NAME)
                         .WithMany(LEFT_NAME)
                         .UsingEntity<DaoRelation<TLeft, TRight>>(

                         j => j.HasOne(a => a.RightEntity)
                               .WithMany(RELATION_TABLE_NAME)
                               .HasForeignKey(a => a.RightEntityId),

                         j => j.HasOne(a => a.LeftEntity)
                               .WithMany(RELATION_TABLE_NAME)
                               .HasForeignKey(a => a.LeftEntityId),

                         j => { j.HasKey(k => new { k.LeftEntityId, k.RightEntityId }); });

            //new Link(new Sleeve(typeof(TLeft), LEFT_NAME),
            //         new Sleeve(typeof(DboRelation<TLeft, TRight>), RELATION_TABLE_NAME),
            //         new Sleeve(typeof(TRight), RIGHT_NAME),
            //         new[] { "Id" }, new[] { "LeftEntityId" },
            //         new[] { "RightEntityId" }, new[] { "Id" });


            if (_expandSite != ExpandSite.None)
            {
                if ((_expandSite & (ExpandSite.OnRight | ExpandSite.WithMany)) > 0)
                    _firstBuilder.Navigation(RIGHT_NAME).AutoInclude();
                else
                    _secondBuilder.Navigation(LEFT_NAME).AutoInclude();
            }

            _firstBuilder.Navigation(RELATION_TABLE_NAME).AutoInclude();
            _secondBuilder.Navigation(RELATION_TABLE_NAME).AutoInclude();

            return _modelBuilder;
        }
    }
}