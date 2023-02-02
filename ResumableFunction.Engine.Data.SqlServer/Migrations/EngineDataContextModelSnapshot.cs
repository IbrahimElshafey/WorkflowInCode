﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ResumableFunction.Engine.EfDataImplementation;

#nullable disable

namespace ResumableFunction.Engine.Data.SqlServer.Migrations
{
    [DbContext(typeof(EngineDataContext))]
    partial class EngineDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.FunctionRuntimeInfo", b =>
                {
                    b.Property<Guid>("FunctionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("InitiatedByClassType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FunctionId");

                    b.ToTable("FunctionInfos");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.Wait", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventIdentifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("FunctionRuntimeInfoFunctionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("InitiatedByFunctionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsFirst")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNode")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ParentFunctionWaitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ReplayType")
                        .HasColumnType("int");

                    b.Property<int>("StateAfterWait")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FunctionRuntimeInfoFunctionId");

                    b.ToTable("Wait");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Wait");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.EventWait", b =>
                {
                    b.HasBaseType("ResumableFunction.Abstraction.InOuts.Wait");

                    b.Property<string>("EventData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventDataType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventProviderName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsOptional")
                        .HasColumnType("bit");

                    b.Property<string>("MatchExpression")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("NeedFunctionData")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ParentGroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SetDataExpression")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("EventWait");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.Wait", b =>
                {
                    b.HasOne("ResumableFunction.Abstraction.InOuts.FunctionRuntimeInfo", "FunctionRuntimeInfo")
                        .WithMany("FunctionWaits")
                        .HasForeignKey("FunctionRuntimeInfoFunctionId");

                    b.Navigation("FunctionRuntimeInfo");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.FunctionRuntimeInfo", b =>
                {
                    b.Navigation("FunctionWaits");
                });
#pragma warning restore 612, 618
        }
    }
}
