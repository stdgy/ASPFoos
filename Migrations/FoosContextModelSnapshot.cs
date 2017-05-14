using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using foosball_asp.Models;

namespace foosballasp.Migrations
{
    [DbContext(typeof(FoosContext))]
    partial class FoosContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("foosball_asp.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("foosball_asp.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("TeamId");

                    b.Property<int>("Type");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("foosball_asp.Models.Score", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("OwnGoal");

                    b.Property<int>("PlayerId");

                    b.Property<DateTime>("TimeScored");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Scores");
                });

            modelBuilder.Entity("foosball_asp.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GameId");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("foosball_asp.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Birthdate");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("foosball_asp.Models.Player", b =>
                {
                    b.HasOne("foosball_asp.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("foosball_asp.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("foosball_asp.Models.Score", b =>
                {
                    b.HasOne("foosball_asp.Models.Player", "Player")
                        .WithMany("Scores")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("foosball_asp.Models.Team", b =>
                {
                    b.HasOne("foosball_asp.Models.Game", "Game")
                        .WithMany("Teams")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
