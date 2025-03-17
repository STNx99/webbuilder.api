﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using webbuilder.api.data;

#nullable disable

namespace webbuilder.api.Migrations
{
    [DbContext(typeof(ElementStoreContext))]
    partial class ElementStoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("webbuilder.api.models.Element", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<string>("Content")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "content");

                    b.Property<string>("Href")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "href");

                    b.Property<bool>("IsSelected")
                        .HasColumnType("boolean")
                        .HasAnnotation("Relational:JsonPropertyName", "isSelected");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<string>("ParentId")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "parentId");

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "projectId");

                    b.Property<string>("Src")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "src");

                    b.Property<string>("Styles")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "styles");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)")
                        .HasAnnotation("Relational:JsonPropertyName", "type");

                    b.Property<double>("X")
                        .HasColumnType("double precision")
                        .HasAnnotation("Relational:JsonPropertyName", "x");

                    b.Property<double>("Y")
                        .HasColumnType("double precision")
                        .HasAnnotation("Relational:JsonPropertyName", "y");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Elements");

                    b.HasDiscriminator<string>("Type").HasValue("Element");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("webbuilder.api.models.Project", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("webbuilder.api.models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("webbuilder.api.models.AElement", b =>
                {
                    b.HasBaseType("webbuilder.api.models.Element");

                    b.HasDiscriminator().HasValue("A");
                });

            modelBuilder.Entity("webbuilder.api.models.FrameElement", b =>
                {
                    b.HasBaseType("webbuilder.api.models.Element");

                    b.HasDiscriminator().HasValue("Frame");
                });

            modelBuilder.Entity("webbuilder.api.models.TextElement", b =>
                {
                    b.HasBaseType("webbuilder.api.models.Element");

                    b.HasDiscriminator().HasValue("Text");
                });

            modelBuilder.Entity("webbuilder.api.models.Element", b =>
                {
                    b.HasOne("webbuilder.api.models.FrameElement", null)
                        .WithMany("Elements")
                        .HasForeignKey("ParentId");

                    b.HasOne("webbuilder.api.models.Project", null)
                        .WithMany("Elements")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("webbuilder.api.models.Project", b =>
                {
                    b.HasOne("webbuilder.api.models.User", null)
                        .WithMany("Projects")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("webbuilder.api.models.Project", b =>
                {
                    b.Navigation("Elements");
                });

            modelBuilder.Entity("webbuilder.api.models.User", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("webbuilder.api.models.FrameElement", b =>
                {
                    b.Navigation("Elements");
                });
#pragma warning restore 612, 618
        }
    }
}
