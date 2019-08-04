﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductRepository.DbContext;

namespace ProductRepository.Migrations
{
    [DbContext(typeof(ProductRepositoryDbContext))]
    [Migration("20190803123138_InitialDatabase")]
    partial class InitialDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ProductRepository.Models.Category", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Category");

                    b.HasData(
                        new { Id = new Guid("efb1bd93-adaf-4f73-9676-062ee092ee78"), Name = "Cocktails" },
                        new { Id = new Guid("11ec62b1-a1a3-4454-b703-8ffeef5f0b86"), Name = "Beers" },
                        new { Id = new Guid("a612d7ba-26e5-4dd8-adeb-3273aa1ee35a"), Name = "Wines" }
                    );
                });

            modelBuilder.Entity("ProductRepository.Models.Product", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<Guid>("CategoryId");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Product");

                    b.HasData(
                        new { Id = new Guid("bfa2cc92-7c17-4600-8057-19d5dc78ba13"), CategoryId = new Guid("efb1bd93-adaf-4f73-9676-062ee092ee78"), Description = "chilled liqueur with tiny, floating, gold flakes", Name = "Shot of Goldschl�ger" },
                        new { Id = new Guid("732cf6f1-9ff3-4938-b4cd-cda38f622b24"), CategoryId = new Guid("efb1bd93-adaf-4f73-9676-062ee092ee78"), Description = "sloe gin, Southern Comfort, amaretto, orange juice", Name = "Alabama Slammer" },
                        new { Id = new Guid("f8f6675d-07a0-4ec1-9d2a-c6e097d964f3"), CategoryId = new Guid("efb1bd93-adaf-4f73-9676-062ee092ee78"), Description = "light rum, orange cura�ao, sour mix, pineapple juice, dark rum float", Name = "Mai Tai" },
                        new { Id = new Guid("8a8e7a71-be29-4398-a34e-a8827ae1cffb"), CategoryId = new Guid("efb1bd93-adaf-4f73-9676-062ee092ee78"), Description = "vodka, gin, rum, triple sec, sour mix, Coke", Name = "Long Island Iced Tea" },
                        new { Id = new Guid("77f4714c-79b0-4636-b885-87694253b4ce"), CategoryId = new Guid("11ec62b1-a1a3-4454-b703-8ffeef5f0b86"), Description = "Despite its name, a Barleywine (or Barley Wine) is very much a beer, albeit a very strong and often intense beer! In fact, it's one of the strongest of the beer styles. Lively and fruity, sometimes sweet, sometimes bittersweet, but always alcoholic. A brew of this strength and complexity can be a challenge to the palate. Expect anything from an amber to dark brown colored beer, with aromas ranging from intense fruits to intense hops. Body is typically thick, alcohol will definitely be perceived, and flavors can range from dominant fruits to palate smacking, resiny hops.", Name = "English Barleywine" },
                        new { Id = new Guid("36637463-1626-4171-b06f-7ba395155a82"), CategoryId = new Guid("11ec62b1-a1a3-4454-b703-8ffeef5f0b86"), Description = "First brewed in England and exported for the British troops in India during the late 1700s. To withstand the voyage, IPA's were basically tweaked Pale Ales that were, in comparison, much more malty, boasted a higher alcohol content and were well-hopped, as hops are a natural preservative. Historians believe that an IPA was then watered down for the troops, while officers and the elite would savor the beer at full strength. The English IPA has a lower alcohol due to taxation over the decades. The leaner the brew the less amount of malt there is and less need for a strong hop presence which would easily put the brew out of balance. Some brewers have tried to recreate the origianl IPA with strengths close to 8-9% abv.", Name = "English India Pale Ale" },
                        new { Id = new Guid("7007a23f-ea66-4ebd-8847-d44f70eb66fa"), CategoryId = new Guid("11ec62b1-a1a3-4454-b703-8ffeef5f0b86"), Description = "Flanders Reds are commonly referred to as the 'red' beers of West Flanders. Belgian Red Beers are typically light-bodied brews with reddish-brown colors. They are infamous for their distinct sharp, fruity, sour and tart flavors which are created by special yeast strains. Very complex beers, they are produced under the age old tradition of long-term cask aging in oak, and the blending of young and old beers.", Name = "Flanders Red Ale" },
                        new { Id = new Guid("218e650a-b4e9-4f2f-997c-8b171c3ae616"), CategoryId = new Guid("a612d7ba-26e5-4dd8-adeb-3273aa1ee35a"), Description = "Pinot Grigio is one of the world's most popular wines. It's also known by a few different names around the world, including 'Pinot Gris' in France 'Rul�nder' in Germany.", Name = "Pinot Grigio" },
                        new { Id = new Guid("e69e9cf9-56a0-4c2a-a8f5-ebf108e23d46"), CategoryId = new Guid("a612d7ba-26e5-4dd8-adeb-3273aa1ee35a"), Description = "The third most planted grape variety in California, Sauvignon Blanc may not have been featured in a movie but it is certainly one of the popular kids. Typically considered a warm weather wine, this dry white can be found all over the world.", Name = "Sauvignon Blanc" },
                        new { Id = new Guid("5ef3b01e-c131-48f5-b624-74b51f028861"), CategoryId = new Guid("a612d7ba-26e5-4dd8-adeb-3273aa1ee35a"), Description = "One wine took a hit from the movie 'Sideways' and it's infamous line about not drinking any Merlot. But Pinot Noir may have gotten a bit of a boost. The wine didn't get it's own notorious line from the film but it was mentioned favorably once or twice.", Name = "Pinot Noir" }
                    );
                });

            modelBuilder.Entity("ProductRepository.Models.Product", b =>
                {
                    b.HasOne("ProductRepository.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
