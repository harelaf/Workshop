﻿// <auto-generated />
using System;
using MarketWeb.Server.DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MarketWeb.Server.Migrations
{
    [DbContext(typeof(MarketContext))]
    partial class MarketContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("MarketWeb.Server.DataLayer.AdminMessageToRegisteredDAL", b =>
                {
                    b.Property<int>("mid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("RegisteredDAL_username")
                        .HasColumnType("TEXT");

                    b.Property<string>("_message")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("_senderUsername")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("_title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("mid");

                    b.HasIndex("RegisteredDAL_username");

                    b.ToTable("AdminMessageToRegisteredDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.BasketItemDAL", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ShoppingBasketDALsbId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("itemID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("purchaseDetailsID")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.HasIndex("ShoppingBasketDALsbId");

                    b.HasIndex("purchaseDetailsID");

                    b.ToTable("BasketItemDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.BidDAL", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BidsOfVisitorid")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ShoppingBasketDALsbId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("_acceptedByAll")
                        .HasColumnType("INTEGER");

                    b.Property<int>("_amount")
                        .HasColumnType("INTEGER");

                    b.Property<double>("_biddedPrice")
                        .HasColumnType("REAL");

                    b.Property<string>("_bidder")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("_counterOffer")
                        .HasColumnType("REAL");

                    b.Property<int>("_itemId")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.HasIndex("BidsOfVisitorid");

                    b.HasIndex("ShoppingBasketDALsbId");

                    b.ToTable("BidDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.BidsOfVisitor", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("StoreDAL_storeName")
                        .HasColumnType("TEXT");

                    b.Property<string>("_bidder")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.HasIndex("StoreDAL_storeName");

                    b.ToTable("BidOfVisitor");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.ComplaintDAL", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("_cartID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("_complainer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("_message")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("_response")
                        .HasColumnType("TEXT");

                    b.HasKey("_id");

                    b.ToTable("ComplaintDALs");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.ItemDAL", b =>
                {
                    b.Property<int>("_itemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("_category")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("_description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("_name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("_price")
                        .HasColumnType("REAL");

                    b.HasKey("_itemID");

                    b.ToTable("itemDALs");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.MessageToStoreDAL", b =>
                {
                    b.Property<int>("mid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("_message")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("_replierFromStore")
                        .HasColumnType("TEXT");

                    b.Property<string>("_reply")
                        .HasColumnType("TEXT");

                    b.Property<string>("_senderUsername")
                        .HasColumnType("TEXT");

                    b.Property<string>("_storeName")
                        .HasColumnType("TEXT");

                    b.Property<string>("_title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("mid");

                    b.ToTable("MessageToStoreDALs");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.NotifyMessageDAL", b =>
                {
                    b.Property<int>("mid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("RegisteredDAL_username")
                        .HasColumnType("TEXT");

                    b.Property<string>("_message")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("_storeName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("_title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("mid");

                    b.HasIndex("RegisteredDAL_username");

                    b.ToTable("NotifyMessageDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.OperationWrapper", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SystemRoleDALid")
                        .HasColumnType("INTEGER");

                    b.Property<int>("op")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.HasIndex("SystemRoleDALid");

                    b.ToTable("OperationWrapper");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.OwnerAcceptors", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("StoreDAL_storeName")
                        .HasColumnType("TEXT");

                    b.Property<string>("_appointer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("_newOwner")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.HasIndex("StoreDAL_storeName");

                    b.ToTable("OwnerAcceptors");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.PopulationStatisticsDAL", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("_userNane")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("_visitDay")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.ToTable("PopulationStatisticsDALs");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.PurchaseDetailsDAL", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("_itemID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("amount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("discountListJSON")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("PurchaseDetailsDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.PurchasedBasketDAL", b =>
                {
                    b.Property<DateTime>("_purchaseDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("StorePurchasedBasketDAL_storeName")
                        .HasColumnType("TEXT");

                    b.Property<int?>("_PurchasedBasketsbId")
                        .HasColumnType("INTEGER");

                    b.HasKey("_purchaseDate");

                    b.HasIndex("StorePurchasedBasketDAL_storeName");

                    b.HasIndex("_PurchasedBasketsbId");

                    b.ToTable("PurchasedBasketDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.PurchasedCartDAL", b =>
                {
                    b.Property<DateTime>("_purchaseDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PurchasedCartDAL")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RegisteredPurchasedCartDALuserName")
                        .HasColumnType("TEXT");

                    b.HasKey("_purchaseDate");

                    b.HasIndex("PurchasedCartDAL");

                    b.HasIndex("RegisteredPurchasedCartDALuserName");

                    b.ToTable("PurchasedCartDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.RateDAL", b =>
                {
                    b.Property<int>("rid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RatingDAL")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StoreDAL_storeName")
                        .HasColumnType("TEXT");

                    b.Property<int>("rate")
                        .HasColumnType("INTEGER");

                    b.Property<string>("review")
                        .HasColumnType("TEXT");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("rid");

                    b.HasIndex("RatingDAL");

                    b.HasIndex("StoreDAL_storeName");

                    b.ToTable("RateDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.RegisteredDAL", b =>
                {
                    b.Property<string>("_username")
                        .HasColumnType("TEXT");

                    b.Property<int>("CartDAL")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("_birthDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("_password")
                        .HasColumnType("TEXT");

                    b.Property<int>("_populationSection")
                        .HasColumnType("int");

                    b.Property<string>("_salt")
                        .HasColumnType("TEXT");

                    b.HasKey("_username");

                    b.HasIndex("CartDAL")
                        .IsUnique();

                    b.ToTable("RegisteredDALs");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.RegisteredPurchasedCartDAL", b =>
                {
                    b.Property<string>("userName")
                        .HasColumnType("TEXT");

                    b.HasKey("userName");

                    b.ToTable("RegisteredPurchaseHistory");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.ShoppingBasketDAL", b =>
                {
                    b.Property<int>("sbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ShoppingCartDALscId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("_additionalDiscountsID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("_storeName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("sbId");

                    b.HasIndex("ShoppingCartDALscId");

                    b.HasIndex("_additionalDiscountsID");

                    b.ToTable("ShoppingBasketDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.ShoppingCartDAL", b =>
                {
                    b.Property<int>("scId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("scId");

                    b.ToTable("ShoppingCartDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StockItemDAL", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("StoreDAL_storeName")
                        .HasColumnType("TEXT");

                    b.Property<int>("amount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("itemID")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.HasIndex("StoreDAL_storeName");

                    b.ToTable("StockItemDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StoreDAL", b =>
                {
                    b.Property<string>("_storeName")
                        .HasColumnType("TEXT");

                    b.Property<string>("_discountPolicyJSON")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("_purchasePolicyJSON")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("_state")
                        .HasColumnType("INTEGER");

                    b.HasKey("_storeName");

                    b.ToTable("StoreDALs");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StorePurchasedBasketDAL", b =>
                {
                    b.Property<string>("_storeName")
                        .HasColumnType("TEXT");

                    b.HasKey("_storeName");

                    b.ToTable("StorePurchaseHistory");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StringData", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BidDALid")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("OwnerAcceptorsid")
                        .HasColumnType("INTEGER");

                    b.Property<string>("data")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.HasIndex("BidDALid");

                    b.HasIndex("OwnerAcceptorsid");

                    b.ToTable("StringData");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.SystemRoleDAL", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("_appointer")
                        .HasColumnType("TEXT");

                    b.Property<string>("_storeName")
                        .HasColumnType("TEXT");

                    b.Property<string>("_username")
                        .HasColumnType("TEXT");

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

                    b.HasOne("MarketWeb.Server.DataLayer.PurchaseDetailsDAL", "_additionalDiscounts")
                        .WithMany()
                        .HasForeignKey("_additionalDiscountsID");

                    b.Navigation("_additionalDiscounts");
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
