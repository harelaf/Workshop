﻿// <auto-generated />
using System;
using MarketWeb.Server.DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
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
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MarketWeb.Server.DataLayer.BasketItemDAL", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ShoppingBasketDALsbId")
                        .HasColumnType("int");

                    b.Property<int?>("item_itemID")
                        .HasColumnType("int");

                    b.Property<int?>("purchaseDetails_itemID")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("ShoppingBasketDALsbId");

                    b.HasIndex("item_itemID");

                    b.HasIndex("purchaseDetails_itemID");

                    b.ToTable("BasketItemDAL");
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

            modelBuilder.Entity("MarketWeb.Server.DataLayer.ConditionDAL", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("PurchasePolicyDALid")
                        .HasColumnType("int");

                    b.HasKey("_id");

                    b.HasIndex("PurchasePolicyDALid");

                    b.ToTable("ConditionDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.DiscountDAL", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("DiscountPolicyDALid")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("_condition_id")
                        .HasColumnType("int");

                    b.HasKey("_id");

                    b.HasIndex("DiscountPolicyDALid");

                    b.HasIndex("_condition_id");

                    b.ToTable("DiscountDAL");

                    b.HasDiscriminator<string>("Discriminator").HasValue("DiscountDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.DiscountPolicyDAL", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("id");

                    b.ToTable("DiscountPolicyDAL");
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

                    b.Property<int?>("_ratingid")
                        .HasColumnType("int");

                    b.HasKey("_itemID");

                    b.HasIndex("_ratingid");

                    b.ToTable("ItemDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.MessageToStoreDAL", b =>
                {
                    b.Property<int>("mid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("StoreDAL_storeName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("_message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_replierFromStore")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_reply")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_senderUsername")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_storeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("mid");

                    b.HasIndex("StoreDAL_storeName");

                    b.ToTable("MessageToStoreDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.OperationWrapper", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("StoreFounderDALid")
                        .HasColumnType("int");

                    b.Property<int?>("StoreManagerDALid")
                        .HasColumnType("int");

                    b.Property<int?>("StoreOwnerDALid")
                        .HasColumnType("int");

                    b.Property<int>("op")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("StoreFounderDALid");

                    b.HasIndex("StoreManagerDALid");

                    b.HasIndex("StoreOwnerDALid");

                    b.ToTable("OperationWrapper");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.PurchaseDetailsDAL", b =>
                {
                    b.Property<int>("_itemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("amount")
                        .HasColumnType("int");

                    b.HasKey("_itemID");

                    b.ToTable("PurchaseDetailsDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.PurchasePolicyDAL", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("id");

                    b.ToTable("PurchasePolicyDAL");
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

                    b.Property<string>("RegisteredPurchasedCartDALuserName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("_PurchasedCartscId")
                        .HasColumnType("int");

                    b.HasKey("_purchaseDate");

                    b.HasIndex("RegisteredPurchasedCartDALuserName");

                    b.HasIndex("_PurchasedCartscId");

                    b.ToTable("PurchasedCartDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.RateDAL", b =>
                {
                    b.Property<int>("rid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("RatingDALid")
                        .HasColumnType("int");

                    b.Property<int>("rate")
                        .HasColumnType("int");

                    b.Property<string>("review")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("rid");

                    b.HasIndex("RatingDALid");

                    b.ToTable("RateDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.RatingDAL", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("id");

                    b.ToTable("RatingDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.RegisteredDAL", b =>
                {
                    b.Property<string>("_username")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("_username");

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

                    b.Property<string>("_storeName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("sbId");

                    b.HasIndex("ShoppingCartDALscId");

                    b.HasIndex("_storeName");

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

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StockDAL", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("id");

                    b.ToTable("StockDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StockItemDAL", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("StockDALid")
                        .HasColumnType("int");

                    b.Property<int>("amount")
                        .HasColumnType("int");

                    b.Property<int?>("item_itemID")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("StockDALid");

                    b.HasIndex("item_itemID");

                    b.ToTable("StockItemDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StoreDAL", b =>
                {
                    b.Property<string>("_storeName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("_discountPolicyid")
                        .HasColumnType("int");

                    b.Property<int?>("_founderid")
                        .HasColumnType("int");

                    b.Property<int?>("_purchasePolicyid")
                        .HasColumnType("int");

                    b.Property<int?>("_ratingid")
                        .HasColumnType("int");

                    b.Property<int>("_state")
                        .HasColumnType("int");

                    b.Property<int?>("_stockid")
                        .HasColumnType("int");

                    b.HasKey("_storeName");

                    b.HasIndex("_discountPolicyid");

                    b.HasIndex("_founderid");

                    b.HasIndex("_purchasePolicyid");

                    b.HasIndex("_ratingid");

                    b.HasIndex("_stockid");

                    b.ToTable("StoreDALs");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StoreFounderDAL", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("_storeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("StoreFounderDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StoreManagerDAL", b =>
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

                    b.Property<string>("_storeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("StoreDAL_storeName");

                    b.ToTable("StoreManagerDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StoreOwnerDAL", b =>
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

                    b.Property<string>("_storeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("StoreDAL_storeName");

                    b.ToTable("StoreOwnerDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StorePurchasedBasketDAL", b =>
                {
                    b.Property<string>("_storeName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("_storeName");

                    b.ToTable("StorePurchaseHistory");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.AtomicDiscountDAL", b =>
                {
                    b.HasBaseType("MarketWeb.Server.DataLayer.DiscountDAL");

                    b.Property<int?>("PurchaseDetailsDAL_itemID")
                        .HasColumnType("int");

                    b.Property<DateTime>("_expiration")
                        .HasColumnType("datetime2");

                    b.HasIndex("PurchaseDetailsDAL_itemID");

                    b.HasDiscriminator().HasValue("AtomicDiscountDAL");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.BasketItemDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.ShoppingBasketDAL", null)
                        .WithMany("_items")
                        .HasForeignKey("ShoppingBasketDALsbId");

                    b.HasOne("MarketWeb.Server.DataLayer.ItemDAL", "item")
                        .WithMany()
                        .HasForeignKey("item_itemID");

                    b.HasOne("MarketWeb.Server.DataLayer.PurchaseDetailsDAL", "purchaseDetails")
                        .WithMany()
                        .HasForeignKey("purchaseDetails_itemID");

                    b.Navigation("item");

                    b.Navigation("purchaseDetails");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.ConditionDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.PurchasePolicyDAL", null)
                        .WithMany("conditions")
                        .HasForeignKey("PurchasePolicyDALid");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.DiscountDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.DiscountPolicyDAL", null)
                        .WithMany("_discounts")
                        .HasForeignKey("DiscountPolicyDALid");

                    b.HasOne("MarketWeb.Server.DataLayer.ConditionDAL", "_condition")
                        .WithMany()
                        .HasForeignKey("_condition_id");

                    b.Navigation("_condition");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.ItemDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.RatingDAL", "_rating")
                        .WithMany()
                        .HasForeignKey("_ratingid");

                    b.Navigation("_rating");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.MessageToStoreDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.StoreDAL", null)
                        .WithMany("_messagesToStore")
                        .HasForeignKey("StoreDAL_storeName");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.OperationWrapper", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.StoreFounderDAL", null)
                        .WithMany("_operationsWrappers")
                        .HasForeignKey("StoreFounderDALid");

                    b.HasOne("MarketWeb.Server.DataLayer.StoreManagerDAL", null)
                        .WithMany("_operationsWrappers")
                        .HasForeignKey("StoreManagerDALid");

                    b.HasOne("MarketWeb.Server.DataLayer.StoreOwnerDAL", null)
                        .WithMany("_operationsWrappers")
                        .HasForeignKey("StoreOwnerDALid");
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
                    b.HasOne("MarketWeb.Server.DataLayer.RegisteredPurchasedCartDAL", null)
                        .WithMany("_PurchasedCarts")
                        .HasForeignKey("RegisteredPurchasedCartDALuserName");

                    b.HasOne("MarketWeb.Server.DataLayer.ShoppingCartDAL", "_PurchasedCart")
                        .WithMany()
                        .HasForeignKey("_PurchasedCartscId");

                    b.Navigation("_PurchasedCart");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.RateDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.RatingDAL", null)
                        .WithMany("_ratings")
                        .HasForeignKey("RatingDALid");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.ShoppingBasketDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.ShoppingCartDAL", null)
                        .WithMany("_shoppingBaskets")
                        .HasForeignKey("ShoppingCartDALscId");

                    b.HasOne("MarketWeb.Server.DataLayer.StoreDAL", "_store")
                        .WithMany()
                        .HasForeignKey("_storeName");

                    b.Navigation("_store");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StockItemDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.StockDAL", null)
                        .WithMany("_itemAndAmount")
                        .HasForeignKey("StockDALid");

                    b.HasOne("MarketWeb.Server.DataLayer.ItemDAL", "item")
                        .WithMany()
                        .HasForeignKey("item_itemID");

                    b.Navigation("item");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StoreDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.DiscountPolicyDAL", "_discountPolicy")
                        .WithMany()
                        .HasForeignKey("_discountPolicyid");

                    b.HasOne("MarketWeb.Server.DataLayer.StoreFounderDAL", "_founder")
                        .WithMany()
                        .HasForeignKey("_founderid");

                    b.HasOne("MarketWeb.Server.DataLayer.PurchasePolicyDAL", "_purchasePolicy")
                        .WithMany()
                        .HasForeignKey("_purchasePolicyid");

                    b.HasOne("MarketWeb.Server.DataLayer.RatingDAL", "_rating")
                        .WithMany()
                        .HasForeignKey("_ratingid");

                    b.HasOne("MarketWeb.Server.DataLayer.StockDAL", "_stock")
                        .WithMany()
                        .HasForeignKey("_stockid");

                    b.Navigation("_discountPolicy");

                    b.Navigation("_founder");

                    b.Navigation("_purchasePolicy");

                    b.Navigation("_rating");

                    b.Navigation("_stock");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StoreManagerDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.StoreDAL", null)
                        .WithMany("_managers")
                        .HasForeignKey("StoreDAL_storeName");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StoreOwnerDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.StoreDAL", null)
                        .WithMany("_owners")
                        .HasForeignKey("StoreDAL_storeName");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.AtomicDiscountDAL", b =>
                {
                    b.HasOne("MarketWeb.Server.DataLayer.PurchaseDetailsDAL", null)
                        .WithMany("discountList")
                        .HasForeignKey("PurchaseDetailsDAL_itemID");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.DiscountPolicyDAL", b =>
                {
                    b.Navigation("_discounts");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.PurchaseDetailsDAL", b =>
                {
                    b.Navigation("discountList");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.PurchasePolicyDAL", b =>
                {
                    b.Navigation("conditions");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.RatingDAL", b =>
                {
                    b.Navigation("_ratings");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.RegisteredPurchasedCartDAL", b =>
                {
                    b.Navigation("_PurchasedCarts");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.ShoppingBasketDAL", b =>
                {
                    b.Navigation("_items");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.ShoppingCartDAL", b =>
                {
                    b.Navigation("_shoppingBaskets");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StockDAL", b =>
                {
                    b.Navigation("_itemAndAmount");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StoreDAL", b =>
                {
                    b.Navigation("_managers");

                    b.Navigation("_messagesToStore");

                    b.Navigation("_owners");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StoreFounderDAL", b =>
                {
                    b.Navigation("_operationsWrappers");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StoreManagerDAL", b =>
                {
                    b.Navigation("_operationsWrappers");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StoreOwnerDAL", b =>
                {
                    b.Navigation("_operationsWrappers");
                });

            modelBuilder.Entity("MarketWeb.Server.DataLayer.StorePurchasedBasketDAL", b =>
                {
                    b.Navigation("_PurchasedBaskets");
                });
#pragma warning restore 612, 618
        }
    }
}