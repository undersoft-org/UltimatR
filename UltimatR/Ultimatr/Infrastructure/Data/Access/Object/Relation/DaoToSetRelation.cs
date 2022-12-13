﻿//-----------------------------------------------------------------------
// <copyright file="ExpandSite.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UltimatR
{    
    public class DaoToSetRelation<TParent, TChild> where TParent : Entity where TChild : Entity
    {
        readonly ExpandSite _expandSite;
        readonly EntityTypeBuilder<TParent> _firstBuilder;
        readonly ModelBuilder _modelBuilder;
        readonly EntityTypeBuilder<TChild> _secondBuilder;
        readonly string CHILD_NAME = $"{typeof(TChild).Name.Replace(typeof(TParent).Name, string.Empty)}s";
        readonly string CHILD_SCHEMA = null;
        readonly string CHILD_TABLE_NAME = $"{typeof(TChild).Name}s";
        readonly string PARENT_NAME = typeof(TParent).Name;
        readonly string PARENT_SCHEMA = null;
        readonly string PARENT_TABLE_NAME = $"{typeof(TParent).Name}s";

        public DaoToSetRelation(
            ModelBuilder modelBuilder,
            ExpandSite expandSite = ExpandSite.OnRight,
            string parentSchema = null) : this(
            modelBuilder,
            null,
            null,
            null,
            null,
            expandSite,
            parentSchema,
            parentSchema)
        {
        }

        public DaoToSetRelation(
            ModelBuilder modelBuilder,
            string parentName,
            string childName,
            ExpandSite expandSite = ExpandSite.OnRight,
            string parentSchema = null) : this(
            modelBuilder,
            parentName,
            null,
            childName,
            null,
            expandSite,
            parentSchema,
            parentSchema)
        {
        }

        public DaoToSetRelation(
            ModelBuilder modelBuilder,
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
            if((PARENT_SCHEMA != null) && (CHILD_SCHEMA != null))
            {
                _firstBuilder.ToTable(PARENT_TABLE_NAME, PARENT_SCHEMA);
                _secondBuilder.ToTable(CHILD_TABLE_NAME, CHILD_SCHEMA);
            }

            _firstBuilder.HasMany(CHILD_NAME).WithOne(PARENT_NAME).HasForeignKey($"{PARENT_NAME}Id");

            _secondBuilder.HasOne(PARENT_NAME).WithMany(CHILD_NAME).HasForeignKey($"{PARENT_NAME}Id");

            if(_expandSite != ExpandSite.None)
            {
                if((_expandSite & (ExpandSite.OnRight | ExpandSite.WithMany)) > 0)
                    _firstBuilder.Navigation(CHILD_NAME).AutoInclude();
                else
                    _secondBuilder.Navigation(PARENT_NAME).AutoInclude();
            }
            return _modelBuilder;
        }
    }

    public enum ExpandSite
    {
        None = 0,
        OnRight = 1,
        OnLeft = 2,
        WithMany = 4,
        WithOne = 8
    }
}