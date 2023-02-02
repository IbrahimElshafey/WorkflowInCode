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
    [Migration("20230202095304_Add all waits types")]
    partial class Addallwaitstypes
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

                    b.ToTable("Waits", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AllEventsWait", b =>
                {
                    b.HasBaseType("ResumableFunction.Abstraction.InOuts.Wait");

                    b.Property<string>("WhenCountExpression")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("AllEventsWaits", (string)null);
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AllFunctionsWait", b =>
                {
                    b.HasBaseType("ResumableFunction.Abstraction.InOuts.Wait");

                    b.ToTable("AllFunctionsWaits", (string)null);
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AnyEventWait", b =>
                {
                    b.HasBaseType("ResumableFunction.Abstraction.InOuts.Wait");

                    b.Property<Guid?>("MatchedEventId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("MatchedEventId");

                    b.ToTable("AnyEventWaits", (string)null);
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AnyFunctionWait", b =>
                {
                    b.HasBaseType("ResumableFunction.Abstraction.InOuts.Wait");

                    b.Property<Guid?>("MatchedFunctionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("MatchedFunctionId");

                    b.ToTable("AnyFunctionWaits", (string)null);
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.EventWait", b =>
                {
                    b.HasBaseType("ResumableFunction.Abstraction.InOuts.Wait");

                    b.Property<Guid?>("AllEventsWaitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AllEventsWaitId1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AnyEventWaitId")
                        .HasColumnType("uniqueidentifier");

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

                    b.HasIndex("AllEventsWaitId");

                    b.HasIndex("AllEventsWaitId1");

                    b.HasIndex("AnyEventWaitId");

                    b.ToTable("EventWaits", (string)null);
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.FunctionWait", b =>
                {
                    b.HasBaseType("ResumableFunction.Abstraction.InOuts.Wait");

                    b.Property<Guid?>("AllFunctionsWaitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AllFunctionsWaitId1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AnyFunctionWaitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CurrentWaitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FunctionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ParentFunctionGroupId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("AllFunctionsWaitId");

                    b.HasIndex("AllFunctionsWaitId1");

                    b.HasIndex("AnyFunctionWaitId");

                    b.HasIndex("CurrentWaitId");

                    b.ToTable("FunctionWaits", (string)null);
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.Wait", b =>
                {
                    b.HasOne("ResumableFunction.Abstraction.InOuts.FunctionRuntimeInfo", "FunctionRuntimeInfo")
                        .WithMany("FunctionWaits")
                        .HasForeignKey("FunctionRuntimeInfoFunctionId");

                    b.Navigation("FunctionRuntimeInfo");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AllEventsWait", b =>
                {
                    b.HasOne("ResumableFunction.Abstraction.InOuts.Wait", null)
                        .WithOne()
                        .HasForeignKey("ResumableFunction.Abstraction.InOuts.AllEventsWait", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AllFunctionsWait", b =>
                {
                    b.HasOne("ResumableFunction.Abstraction.InOuts.Wait", null)
                        .WithOne()
                        .HasForeignKey("ResumableFunction.Abstraction.InOuts.AllFunctionsWait", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AnyEventWait", b =>
                {
                    b.HasOne("ResumableFunction.Abstraction.InOuts.Wait", null)
                        .WithOne()
                        .HasForeignKey("ResumableFunction.Abstraction.InOuts.AnyEventWait", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ResumableFunction.Abstraction.InOuts.EventWait", "MatchedEvent")
                        .WithMany()
                        .HasForeignKey("MatchedEventId");

                    b.Navigation("MatchedEvent");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AnyFunctionWait", b =>
                {
                    b.HasOne("ResumableFunction.Abstraction.InOuts.Wait", null)
                        .WithOne()
                        .HasForeignKey("ResumableFunction.Abstraction.InOuts.AnyFunctionWait", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ResumableFunction.Abstraction.InOuts.FunctionWait", "MatchedFunction")
                        .WithMany()
                        .HasForeignKey("MatchedFunctionId");

                    b.Navigation("MatchedFunction");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.EventWait", b =>
                {
                    b.HasOne("ResumableFunction.Abstraction.InOuts.AllEventsWait", null)
                        .WithMany("MatchedEvents")
                        .HasForeignKey("AllEventsWaitId");

                    b.HasOne("ResumableFunction.Abstraction.InOuts.AllEventsWait", null)
                        .WithMany("WaitingEvents")
                        .HasForeignKey("AllEventsWaitId1");

                    b.HasOne("ResumableFunction.Abstraction.InOuts.AnyEventWait", null)
                        .WithMany("WaitingEvents")
                        .HasForeignKey("AnyEventWaitId");

                    b.HasOne("ResumableFunction.Abstraction.InOuts.Wait", null)
                        .WithOne()
                        .HasForeignKey("ResumableFunction.Abstraction.InOuts.EventWait", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.FunctionWait", b =>
                {
                    b.HasOne("ResumableFunction.Abstraction.InOuts.AllFunctionsWait", null)
                        .WithMany("CompletedFunctions")
                        .HasForeignKey("AllFunctionsWaitId");

                    b.HasOne("ResumableFunction.Abstraction.InOuts.AllFunctionsWait", null)
                        .WithMany("WaitingFunctions")
                        .HasForeignKey("AllFunctionsWaitId1");

                    b.HasOne("ResumableFunction.Abstraction.InOuts.AnyFunctionWait", null)
                        .WithMany("WaitingFunctions")
                        .HasForeignKey("AnyFunctionWaitId");

                    b.HasOne("ResumableFunction.Abstraction.InOuts.Wait", "CurrentWait")
                        .WithMany()
                        .HasForeignKey("CurrentWaitId");

                    b.HasOne("ResumableFunction.Abstraction.InOuts.Wait", null)
                        .WithOne()
                        .HasForeignKey("ResumableFunction.Abstraction.InOuts.FunctionWait", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrentWait");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.FunctionRuntimeInfo", b =>
                {
                    b.Navigation("FunctionWaits");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AllEventsWait", b =>
                {
                    b.Navigation("MatchedEvents");

                    b.Navigation("WaitingEvents");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AllFunctionsWait", b =>
                {
                    b.Navigation("CompletedFunctions");

                    b.Navigation("WaitingFunctions");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AnyEventWait", b =>
                {
                    b.Navigation("WaitingEvents");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AnyFunctionWait", b =>
                {
                    b.Navigation("WaitingFunctions");
                });
#pragma warning restore 612, 618
        }
    }
}
