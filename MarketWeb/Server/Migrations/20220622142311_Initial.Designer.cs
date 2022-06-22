﻿// <auto-generated />
using System;
using MarketWeb.Server.DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MarketWeb.Server.Migrations
{
    [DbContext(typeof(MarketContext))]
    [Migration("20220622142311_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MarketWeb.Server.DataLayer.AdminMessageToRegisteredDAL", b =>
                {
                    b.Property<int>("mid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RegisteredDAL_username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("_message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_senderUsername")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("mid");

                    b.HasIndex("RegisteredDAL_username");

                    b.ToTable("AdminMessageToRegisteredDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.BasketItemDAL", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ShoppingBasketDALsbId")
                        .HasColumnType("int");

                    b.Property<int>("itemID")
                        .HasColumnType("int");

                    b.Property<int?>("purchaseDetailsID")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("ShoppingBasketDALsbId");

                    b.HasIndex("purchaseDetailsID");

                    b.ToTable("BasketItemDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.BidDAL", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BidsOfVisitorid")
                        .HasColumnType("int");

                    b.Property<int?>("ShoppingBasketDALsbId")
                        .HasColumnType("int");

                    b.Property<bool>("_acceptedByAll")
                        .HasColumnType("bit");

                    b.Property<int>("_amount")
                        .HasColumnType("int");

                    b.Property<double>("_biddedPrice")
                        .HasColumnType("float");

                    b.Property<string>("_bidder")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("_counterOffer")
                        .HasColumnType("float");

                    b.Property<int>("_itemId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("BidsOfVisitorid");

                    b.HasIndex("ShoppingBasketDALsbId");

                    b.ToTable("BidDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.BidsOfVisitor", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("StoreDAL_storeName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("_bidder")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("StoreDAL_storeName");

                    b.ToTable("BidOfVisitor");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.ComplaintDAL", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("_cartID")
                        .HasColumnType("int");

                    b.Property<string>("_complainer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_response")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("_id");

                    b.ToTable("ComplaintDALs");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.ItemDAL", b =>
                {
                    b.Property<int>("_itemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("_category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("_price")
                        .HasColumnType("float");

                    b.HasKey("_itemID");

                    b.ToTable("itemDALs");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.MessageToStoreDAL", b =>
                {
                    b.Property<int>("mid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("_message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_replierFromStore")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_reply")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_senderUsername")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_storeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("mid");

                    b.ToTable("MessageToStoreDALs");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.NotifyMessageDAL", b =>
                {
                    b.Property<int>("mid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RegisteredDAL_username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("_message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_storeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("mid");

                    b.HasIndex("RegisteredDAL_username");

                    b.ToTable("NotifyMessageDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.OperationWrapper", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("SystemRoleDALid")
                        .HasColumnType("int");

                    b.Property<int>("op")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("SystemRoleDALid");

                    b.ToTable("OperationWrapper");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.OwnerAcceptors", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("StoreDAL_storeName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("_appointer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_newOwner")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("StoreDAL_storeName");

                    b.ToTable("OwnerAcceptors");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.PurchaseDetailsDAL", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("_itemID")
                        .HasColumnType("int");

                    b.Property<int>("amount")
                        .HasColumnType("int");

                    b.Property<string>("discountListJSON")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("PurchaseDetailsDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.PurchasedBasketDAL", b =>
                {
                    b.Property<DateTime>("_purchaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("StorePurchasedBasketDAL_storeName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("_PurchasedBasketsbId")
                        .HasColumnType("int");

                    b.HasKey("_purchaseDate");

                    b.HasIndex("StorePurchasedBasketDAL_storeName");

                    b.HasIndex("_PurchasedBasketsbId");

                    b.ToTable("PurchasedBasketDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.PurchasedCartDAL", b =>
                {
                    b.Property<DateTime>("_purchaseDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PurchasedCartDAL")
                        .HasColumnType("int");

                    b.Property<string>("RegisteredPurchasedCartDALuserName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("_purchaseDate");

                    b.HasIndex("PurchasedCartDAL");

                    b.HasIndex("RegisteredPurchasedCartDALuserName");

                    b.ToTable("PurchasedCartDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.RateDAL", b =>
                {
                    b.Property<int>("rid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("RatingDAL")
                        .HasColumnType("int");

                    b.Property<string>("StoreDAL_storeName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("rate")
                        .HasColumnType("int");

                    b.Property<string>("review")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("rid");

                    b.HasIndex("RatingDAL");

                    b.HasIndex("StoreDAL_storeName");

                    b.ToTable("RateDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.RegisteredDAL", b =>
                {
                    b.Property<string>("_username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CartDAL")
                        .HasColumnType("int");

                    b.Property<DateTime>("_birthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("_password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_salt")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("_username");

                    b.HasIndex("CartDAL")
                        .IsUnique();

                    b.ToTable("RegisteredDALs");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.RegisteredPurchasedCartDAL", b =>
                {
                    b.Property<string>("userName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("userName");

                    b.ToTable("RegisteredPurchaseHistory");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.ShoppingBasketDAL", b =>
                {
                    b.Property<int>("sbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ShoppingCartDALscId")
                        .HasColumnType("int");

                    b.Property<string>("StoreDAL")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("_additionalDiscountsID")
                        .HasColumnType("int");

                    b.HasKey("sbId");

                    b.HasIndex("ShoppingCartDALscId");

                    b.HasIndex("StoreDAL");

                    b.HasIndex("_additionalDiscountsID");

                    b.ToTable("ShoppingBasketDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.ShoppingCartDAL", b =>
                {
                    b.Property<int>("scId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("scId");

                    b.ToTable("ShoppingCartDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StockItemDAL", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("StoreDAL_storeName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("amount")
                        .HasColumnType("int");

                    b.Property<int>("itemID")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("StoreDAL_storeName");

                    b.ToTable("StockItemDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StoreDAL", b =>
                {
                    b.Property<string>("_storeName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("_discountPolicyJSON")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_purchasePolicyJSON")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("_state")
                        .HasColumnType("int");

                    b.HasKey("_storeName");

                    b.ToTable("StoreDALs");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StorePurchasedBasketDAL", b =>
                {
                    b.Property<string>("_storeName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("_storeName");

                    b.ToTable("StorePurchaseHistory");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StringData", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BidDALid")
                        .HasColumnType("int");

                    b.Property<int?>("OwnerAcceptorsid")
                        .HasColumnType("int");

                    b.Property<string>("data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("BidDALid");

                    b.HasIndex("OwnerAcceptorsid");

                    b.ToTable("StringData");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.SystemRoleDAL", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_appointer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_storeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("SystemRoleDALs");

                    b.HasDiscriminator<string>("Discriminator").HasValue("SystemRoleDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StoreFounderDAL", b =>
                {
                    b.HasBaseType("MarketWeb.Server.DataLayer.SystemRoleDAL");

                    b.HasDiscriminator().HasValue("StoreFounderDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StoreManagerDAL", b =>
                {
                    b.HasBaseType("MarketWeb.Server.DataLayer.SystemRoleDAL");

                    b.HasDiscriminator().HasValue("StoreManagerDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StoreOwnerDAL", b =>
                {
                    b.HasBaseType("MarketWeb.Server.DataLayer.SystemRoleDAL");

                    b.HasDiscriminator().HasValue("StoreOwnerDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.SystemAdminDAL", b =>
                {
                    b.HasBaseType("MarketWeb.Server.DataLayer.SystemRoleDAL");

                    b.HasDiscriminator().HasValue("SystemAdminDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.AdminMessageToRegisteredDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.RegisteredDAL", null)
                        .WithMany("_adminMessages")
                        .HasForeignKey("RegisteredDAL_username");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.BasketItemDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.ShoppingBasketDAL", null)
                        .WithMany("_items")
                        .HasForeignKey("ShoppingBasketDALsbId");

                    b.HasOne("MarketWeb.Server.DataLayer.PurchaseDetailsDAL", "purchaseDetails")
                        .WithMany()
                        .HasForeignKey("purchaseDetailsID");

                    b.Navigation("purchaseDetails");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.BidDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.BidsOfVisitor", null)
                        .WithMany("_bids")
                        .HasForeignKey("BidsOfVisitorid");

                    b.HasOne("MarketWeb.Server.DataLayer.ShoppingBasketDAL", null)
                        .WithMany("_bids")
                        .HasForeignKey("ShoppingBasketDALsbId");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.BidsOfVisitor", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.StoreDAL", null)
                        .WithMany("_bidsOfVisitors")
                        .HasForeignKey("StoreDAL_storeName");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.NotifyMessageDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.RegisteredDAL", null)
                        .WithMany("_notifications")
                        .HasForeignKey("RegisteredDAL_username");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.OperationWrapper", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.SystemRoleDAL", null)
                        .WithMany("_operationsWrappers")
                        .HasForeignKey("SystemRoleDALid")
                        .OnDelete(DeleteBehavior.ClientCascade);
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.OwnerAcceptors", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.StoreDAL", null)
                        .WithMany("_standbyOwners")
                        .HasForeignKey("StoreDAL_storeName");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.PurchasedBasketDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.StorePurchasedBasketDAL", null)
                        .WithMany("_PurchasedBaskets")
                        .HasForeignKey("StorePurchasedBasketDAL_storeName");

                    b.HasOne("MarketWeb.Server.DataLayer.ShoppingBasketDAL", "_PurchasedBasket")
                        .WithMany()
                        .HasForeignKey("_PurchasedBasketsbId");

                    b.Navigation("_PurchasedBasket");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.PurchasedCartDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.ShoppingCartDAL", "_PurchasedCart")
                        .WithMany()
                        .HasForeignKey("PurchasedCartDAL");

                    b.HasOne("MarketWeb.Server.DataLayer.RegisteredPurchasedCartDAL", null)
                        .WithMany("_PurchasedCarts")
                        .HasForeignKey("RegisteredPurchasedCartDALuserName");

                    b.Navigation("_PurchasedCart");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.RateDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.ItemDAL", null)
                        .WithMany("_rating")
                        .HasForeignKey("RatingDAL")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MarketWeb.Server.DataLayer.StoreDAL", null)
                        .WithMany("_rating")
                        .HasForeignKey("StoreDAL_storeName");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.RegisteredDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.ShoppingCartDAL", "_cart")
                        .WithOne()
                        .HasForeignKey("MarketWeb.Server.DataLayer.RegisteredDAL", "CartDAL")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("_cart");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.ShoppingBasketDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.ShoppingCartDAL", null)
                        .WithMany("_shoppingBaskets")
                        .HasForeignKey("ShoppingCartDALscId");

                    b.HasOne("MarketWeb.Server.DataLayer.StoreDAL", "_store")
                        .WithMany()
                        .HasForeignKey("StoreDAL")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MarketWeb.Server.DataLayer.PurchaseDetailsDAL", "_additionalDiscounts")
                        .WithMany()
                        .HasForeignKey("_additionalDiscountsID");

                    b.Navigation("_additionalDiscounts");

                    b.Navigation("_store");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StockItemDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.StoreDAL", null)
                        .WithMany("_stock")
                        .HasForeignKey("StoreDAL_storeName")
                        .OnDelete(DeleteBehavior.ClientCascade);
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StringData", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.BidDAL", null)
                        .WithMany("_acceptors")
                        .HasForeignKey("BidDALid");

                    b.HasOne("MarketWeb.Server.DataLayer.OwnerAcceptors", null)
                        .WithMany("_acceptors")
                        .HasForeignKey("OwnerAcceptorsid");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.BidDAL", b =>
                {
                    b.Navigation("_acceptors");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.BidsOfVisitor", b =>
                {
                    b.Navigation("_bids");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.ItemDAL", b =>
                {
                    b.Navigation("_rating");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.OwnerAcceptors", b =>
                {
                    b.Navigation("_acceptors");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.RegisteredDAL", b =>
                {
                    b.Navigation("_adminMessages");

                    b.Navigation("_notifications");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.RegisteredPurchasedCartDAL", b =>
                {
                    b.Navigation("_PurchasedCarts");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.ShoppingBasketDAL", b =>
                {
                    b.Navigation("_bids");

                    b.Navigation("_items");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.ShoppingCartDAL", b =>
                {
                    b.Navigation("_shoppingBaskets");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StoreDAL", b =>
                {
                    b.Navigation("_bidsOfVisitors");

                    b.Navigation("_rating");

                    b.Navigation("_standbyOwners");

                    b.Navigation("_stock");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StorePurchasedBasketDAL", b =>
                {
                    b.Navigation("_PurchasedBaskets");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.SystemRoleDAL", b =>
                {
                    b.Navigation("_operationsWrappers");
                });
#pragma warning restore 612, 618
        }
    }
}
