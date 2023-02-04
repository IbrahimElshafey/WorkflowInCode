﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ResumableFunction.Engine.EfDataImplementation;

#nullable disable

namespace ResumableFunction.Engine.Data.SqlServer.Migrations
{
    [DbContext(typeof(EngineDataContext))]
    [Migration("20230204113036_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("FunctionState")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InitiatedByClassType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FunctionId");

                    b.ToTable("FunctionRuntimeInfos");
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

                    b.Property<Guid>("FunctionId")
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

                    b.Property<int>("WaitType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FunctionId");

                    b.HasIndex("ParentFunctionWaitId");

                    b.ToTable("Waits");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Wait");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("ResumableFunction.Engine.InOuts.FunctionFolder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("LastScanDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FunctionFolders");

                    b.HasData(
                        new
                        {
                            Id = 61,
                            LastScanDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Path = "C:\\ResumableFunction\\Example.ProjectApproval\\bin\\Debug\\net7.0"
                        });
                });

            modelBuilder.Entity("ResumableFunction.Engine.InOuts.TypeInformation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("FunctionFolderId")
                        .HasColumnType("int");

                    b.Property<int?>("FunctionFolderId1")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FunctionFolderId");

                    b.HasIndex("FunctionFolderId1");

                    b.ToTable("TypeInfos");
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

                    b.Property<Guid?>("ManyEventsWaitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ManyEventsWaitId1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MatchExpression")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("NeedFunctionDataForMatch")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ParentGroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SetDataExpression")
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("ManyEventsWaitId");

                    b.HasIndex("ManyEventsWaitId1");

                    b.HasIndex("ParentGroupId");

                    b.HasDiscriminator().HasValue("EventWait");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.FunctionWait", b =>
                {
                    b.HasBaseType("ResumableFunction.Abstraction.InOuts.Wait");

                    b.Property<Guid?>("CurrentWaitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FunctionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ManyFunctionsWaitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ManyFunctionsWaitId1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ParentFunctionGroupId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("CurrentWaitId");

                    b.HasIndex("ManyFunctionsWaitId");

                    b.HasIndex("ManyFunctionsWaitId1");

                    b.HasIndex("ParentFunctionGroupId");

                    b.HasDiscriminator().HasValue("FunctionWait");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.ManyEventsWait", b =>
                {
                    b.HasBaseType("ResumableFunction.Abstraction.InOuts.Wait");

                    b.Property<string>("WhenCountExpression")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("ManyEventsWait");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.ManyFunctionsWait", b =>
                {
                    b.HasBaseType("ResumableFunction.Abstraction.InOuts.Wait");

                    b.HasDiscriminator().HasValue("ManyFunctionsWait");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.Wait", b =>
                {
                    b.HasOne("ResumableFunction.Abstraction.InOuts.FunctionRuntimeInfo", "FunctionRuntimeInfo")
                        .WithMany("FunctionWaits")
                        .HasForeignKey("FunctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ResumableFunction.Abstraction.InOuts.Wait", "ParentFunctionWait")
                        .WithMany()
                        .HasForeignKey("ParentFunctionWaitId");

                    b.Navigation("FunctionRuntimeInfo");

                    b.Navigation("ParentFunctionWait");
                });

            modelBuilder.Entity("ResumableFunction.Engine.InOuts.TypeInformation", b =>
                {
                    b.HasOne("ResumableFunction.Engine.InOuts.FunctionFolder", null)
                        .WithMany("EventProviderTypes")
                        .HasForeignKey("FunctionFolderId");

                    b.HasOne("ResumableFunction.Engine.InOuts.FunctionFolder", null)
                        .WithMany("FunctionInfos")
                        .HasForeignKey("FunctionFolderId1");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.EventWait", b =>
                {
                    b.HasOne("ResumableFunction.Abstraction.InOuts.ManyEventsWait", null)
                        .WithMany("MatchedEvents")
                        .HasForeignKey("ManyEventsWaitId");

                    b.HasOne("ResumableFunction.Abstraction.InOuts.ManyEventsWait", null)
                        .WithMany("WaitingEvents")
                        .HasForeignKey("ManyEventsWaitId1");

                    b.HasOne("ResumableFunction.Abstraction.InOuts.Wait", "ParentWaitsGroup")
                        .WithMany()
                        .HasForeignKey("ParentGroupId");

                    b.Navigation("ParentWaitsGroup");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.FunctionWait", b =>
                {
                    b.HasOne("ResumableFunction.Abstraction.InOuts.Wait", "CurrentWait")
                        .WithMany()
                        .HasForeignKey("CurrentWaitId");

                    b.HasOne("ResumableFunction.Abstraction.InOuts.ManyFunctionsWait", null)
                        .WithMany("CompletedFunctions")
                        .HasForeignKey("ManyFunctionsWaitId");

                    b.HasOne("ResumableFunction.Abstraction.InOuts.ManyFunctionsWait", null)
                        .WithMany("WaitingFunctions")
                        .HasForeignKey("ManyFunctionsWaitId1");

                    b.HasOne("ResumableFunction.Abstraction.InOuts.Wait", "ParentFunctionGroup")
                        .WithMany()
                        .HasForeignKey("ParentFunctionGroupId");

                    b.Navigation("CurrentWait");

                    b.Navigation("ParentFunctionGroup");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.FunctionRuntimeInfo", b =>
                {
                    b.Navigation("FunctionWaits");
                });

            modelBuilder.Entity("ResumableFunction.Engine.InOuts.FunctionFolder", b =>
                {
                    b.Navigation("EventProviderTypes");

                    b.Navigation("FunctionInfos");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.ManyEventsWait", b =>
                {
                    b.Navigation("MatchedEvents");

                    b.Navigation("WaitingEvents");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.ManyFunctionsWait", b =>
                {
                    b.Navigation("CompletedFunctions");

                    b.Navigation("WaitingFunctions");
                });
#pragma warning restore 612, 618
        }
    }
}
