using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketWeb.Server.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComplaintDALs",
                columns: table => new
                {
                    _id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _complainer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _cartID = table.Column<int>(type: "int", nullable: false),
                    _message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _response = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintDALs", x => x._id);
                });

            migrationBuilder.CreateTable(
                name: "DiscountPolicyDAL",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountPolicyDAL", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MessageToStoreDALs",
                columns: table => new
                {
                    mid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _senderUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _storeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _reply = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _replierFromStore = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageToStoreDALs", x => x.mid);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseDetailsDAL",
                columns: table => new
                {
                    _itemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseDetailsDAL", x => x._itemID);
                });

            migrationBuilder.CreateTable(
                name: "PurchasePolicyDAL",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasePolicyDAL", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RatingDAL",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingDAL", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RegisteredPurchaseHistory",
                columns: table => new
                {
                    userName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredPurchaseHistory", x => x.userName);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartDAL",
                columns: table => new
                {
                    scId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartDAL", x => x.scId);
                });

            migrationBuilder.CreateTable(
                name: "StockDAL",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockDAL", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "StorePurchaseHistory",
                columns: table => new
                {
                    _storeName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorePurchaseHistory", x => x._storeName);
                });

            migrationBuilder.CreateTable(
                name: "SystemRoleDALs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _storeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _appointer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemRoleDALs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ConditionDAL",
                columns: table => new
                {
                    _id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _negative = table.Column<bool>(type: "bit", nullable: false),
                    PurchasePolicyDALid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConditionDAL", x => x._id);
                    table.ForeignKey(
                        name: "FK_ConditionDAL_PurchasePolicyDAL_PurchasePolicyDALid",
                        column: x => x.PurchasePolicyDALid,
                        principalTable: "PurchasePolicyDAL",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemDAL",
                columns: table => new
                {
                    _itemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _ratingid = table.Column<int>(type: "int", nullable: true),
                    _name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _price = table.Column<double>(type: "float", nullable: false),
                    _description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _category = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemDAL", x => x._itemID);
                    table.ForeignKey(
                        name: "FK_ItemDAL_RatingDAL__ratingid",
                        column: x => x._ratingid,
                        principalTable: "RatingDAL",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RateDAL",
                columns: table => new
                {
                    rid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    rate = table.Column<int>(type: "int", nullable: false),
                    review = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RatingDALid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateDAL", x => x.rid);
                    table.ForeignKey(
                        name: "FK_RateDAL_RatingDAL_RatingDALid",
                        column: x => x.RatingDALid,
                        principalTable: "RatingDAL",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchasedCartDAL",
                columns: table => new
                {
                    _purchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _PurchasedCartscId = table.Column<int>(type: "int", nullable: true),
                    RegisteredPurchasedCartDALuserName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedCartDAL", x => x._purchaseDate);
                    table.ForeignKey(
                        name: "FK_PurchasedCartDAL_RegisteredPurchaseHistory_RegisteredPurchasedCartDALuserName",
                        column: x => x.RegisteredPurchasedCartDALuserName,
                        principalTable: "RegisteredPurchaseHistory",
                        principalColumn: "userName",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasedCartDAL_ShoppingCartDAL__PurchasedCartscId",
                        column: x => x._PurchasedCartscId,
                        principalTable: "ShoppingCartDAL",
                        principalColumn: "scId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegisteredDALs",
                columns: table => new
                {
                    _username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    _password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _cartscId = table.Column<int>(type: "int", nullable: true),
                    _birthDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredDALs", x => x._username);
                    table.ForeignKey(
                        name: "FK_RegisteredDALs_ShoppingCartDAL__cartscId",
                        column: x => x._cartscId,
                        principalTable: "ShoppingCartDAL",
                        principalColumn: "scId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StoreDALs",
                columns: table => new
                {
                    _storeName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    _stockid = table.Column<int>(type: "int", nullable: true),
                    _ratingid = table.Column<int>(type: "int", nullable: true),
                    _state = table.Column<int>(type: "int", nullable: false),
                    _purchasePolicyid = table.Column<int>(type: "int", nullable: true),
                    _discountPolicyid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreDALs", x => x._storeName);
                    table.ForeignKey(
                        name: "FK_StoreDALs_DiscountPolicyDAL__discountPolicyid",
                        column: x => x._discountPolicyid,
                        principalTable: "DiscountPolicyDAL",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreDALs_PurchasePolicyDAL__purchasePolicyid",
                        column: x => x._purchasePolicyid,
                        principalTable: "PurchasePolicyDAL",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreDALs_RatingDAL__ratingid",
                        column: x => x._ratingid,
                        principalTable: "RatingDAL",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreDALs_StockDAL__stockid",
                        column: x => x._stockid,
                        principalTable: "StockDAL",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OperationWrapper",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    op = table.Column<int>(type: "int", nullable: false),
                    SystemRoleDALid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationWrapper", x => x.id);
                    table.ForeignKey(
                        name: "FK_OperationWrapper_SystemRoleDALs_SystemRoleDALid",
                        column: x => x.SystemRoleDALid,
                        principalTable: "SystemRoleDALs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiscountDAL",
                columns: table => new
                {
                    _id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _condition_id = table.Column<int>(type: "int", nullable: true),
                    DiscountPolicyDALid = table.Column<int>(type: "int", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PurchaseDetailsDAL_itemID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountDAL", x => x._id);
                    table.ForeignKey(
                        name: "FK_DiscountDAL_ConditionDAL__condition_id",
                        column: x => x._condition_id,
                        principalTable: "ConditionDAL",
                        principalColumn: "_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscountDAL_DiscountPolicyDAL_DiscountPolicyDALid",
                        column: x => x.DiscountPolicyDALid,
                        principalTable: "DiscountPolicyDAL",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscountDAL_PurchaseDetailsDAL_PurchaseDetailsDAL_itemID",
                        column: x => x.PurchaseDetailsDAL_itemID,
                        principalTable: "PurchaseDetailsDAL",
                        principalColumn: "_itemID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockItemDAL",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    item_itemID = table.Column<int>(type: "int", nullable: true),
                    amount = table.Column<int>(type: "int", nullable: false),
                    StockDALid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockItemDAL", x => x.id);
                    table.ForeignKey(
                        name: "FK_StockItemDAL_ItemDAL_item_itemID",
                        column: x => x.item_itemID,
                        principalTable: "ItemDAL",
                        principalColumn: "_itemID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockItemDAL_StockDAL_StockDALid",
                        column: x => x.StockDALid,
                        principalTable: "StockDAL",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdminMessageToRegisteredDAL",
                columns: table => new
                {
                    mid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _senderUsername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegisteredDAL_username = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminMessageToRegisteredDAL", x => x.mid);
                    table.ForeignKey(
                        name: "FK_AdminMessageToRegisteredDAL_RegisteredDALs_RegisteredDAL_username",
                        column: x => x.RegisteredDAL_username,
                        principalTable: "RegisteredDALs",
                        principalColumn: "_username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotifyMessageDAL",
                columns: table => new
                {
                    mid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _storeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegisteredDAL_username = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotifyMessageDAL", x => x.mid);
                    table.ForeignKey(
                        name: "FK_NotifyMessageDAL_RegisteredDALs_RegisteredDAL_username",
                        column: x => x.RegisteredDAL_username,
                        principalTable: "RegisteredDALs",
                        principalColumn: "_username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingBasketDAL",
                columns: table => new
                {
                    sbId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _storeName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ShoppingCartDALscId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingBasketDAL", x => x.sbId);
                    table.ForeignKey(
                        name: "FK_ShoppingBasketDAL_ShoppingCartDAL_ShoppingCartDALscId",
                        column: x => x.ShoppingCartDALscId,
                        principalTable: "ShoppingCartDAL",
                        principalColumn: "scId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShoppingBasketDAL_StoreDALs__storeName",
                        column: x => x._storeName,
                        principalTable: "StoreDALs",
                        principalColumn: "_storeName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BasketItemDAL",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    item_itemID = table.Column<int>(type: "int", nullable: true),
                    purchaseDetails_itemID = table.Column<int>(type: "int", nullable: true),
                    ShoppingBasketDALsbId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketItemDAL", x => x.id);
                    table.ForeignKey(
                        name: "FK_BasketItemDAL_ItemDAL_item_itemID",
                        column: x => x.item_itemID,
                        principalTable: "ItemDAL",
                        principalColumn: "_itemID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BasketItemDAL_PurchaseDetailsDAL_purchaseDetails_itemID",
                        column: x => x.purchaseDetails_itemID,
                        principalTable: "PurchaseDetailsDAL",
                        principalColumn: "_itemID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BasketItemDAL_ShoppingBasketDAL_ShoppingBasketDALsbId",
                        column: x => x.ShoppingBasketDALsbId,
                        principalTable: "ShoppingBasketDAL",
                        principalColumn: "sbId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchasedBasketDAL",
                columns: table => new
                {
                    _purchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _PurchasedBasketsbId = table.Column<int>(type: "int", nullable: true),
                    StorePurchasedBasketDAL_storeName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedBasketDAL", x => x._purchaseDate);
                    table.ForeignKey(
                        name: "FK_PurchasedBasketDAL_ShoppingBasketDAL__PurchasedBasketsbId",
                        column: x => x._PurchasedBasketsbId,
                        principalTable: "ShoppingBasketDAL",
                        principalColumn: "sbId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasedBasketDAL_StorePurchaseHistory_StorePurchasedBasketDAL_storeName",
                        column: x => x.StorePurchasedBasketDAL_storeName,
                        principalTable: "StorePurchaseHistory",
                        principalColumn: "_storeName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminMessageToRegisteredDAL_RegisteredDAL_username",
                table: "AdminMessageToRegisteredDAL",
                column: "RegisteredDAL_username");

            migrationBuilder.CreateIndex(
                name: "IX_BasketItemDAL_item_itemID",
                table: "BasketItemDAL",
                column: "item_itemID");

            migrationBuilder.CreateIndex(
                name: "IX_BasketItemDAL_purchaseDetails_itemID",
                table: "BasketItemDAL",
                column: "purchaseDetails_itemID");

            migrationBuilder.CreateIndex(
                name: "IX_BasketItemDAL_ShoppingBasketDALsbId",
                table: "BasketItemDAL",
                column: "ShoppingBasketDALsbId");

            migrationBuilder.CreateIndex(
                name: "IX_ConditionDAL_PurchasePolicyDALid",
                table: "ConditionDAL",
                column: "PurchasePolicyDALid");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountDAL__condition_id",
                table: "DiscountDAL",
                column: "_condition_id");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountDAL_DiscountPolicyDALid",
                table: "DiscountDAL",
                column: "DiscountPolicyDALid");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountDAL_PurchaseDetailsDAL_itemID",
                table: "DiscountDAL",
                column: "PurchaseDetailsDAL_itemID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDAL__ratingid",
                table: "ItemDAL",
                column: "_ratingid");

            migrationBuilder.CreateIndex(
                name: "IX_NotifyMessageDAL_RegisteredDAL_username",
                table: "NotifyMessageDAL",
                column: "RegisteredDAL_username");

            migrationBuilder.CreateIndex(
                name: "IX_OperationWrapper_SystemRoleDALid",
                table: "OperationWrapper",
                column: "SystemRoleDALid");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedBasketDAL__PurchasedBasketsbId",
                table: "PurchasedBasketDAL",
                column: "_PurchasedBasketsbId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedBasketDAL_StorePurchasedBasketDAL_storeName",
                table: "PurchasedBasketDAL",
                column: "StorePurchasedBasketDAL_storeName");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedCartDAL__PurchasedCartscId",
                table: "PurchasedCartDAL",
                column: "_PurchasedCartscId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedCartDAL_RegisteredPurchasedCartDALuserName",
                table: "PurchasedCartDAL",
                column: "RegisteredPurchasedCartDALuserName");

            migrationBuilder.CreateIndex(
                name: "IX_RateDAL_RatingDALid",
                table: "RateDAL",
                column: "RatingDALid");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredDALs__cartscId",
                table: "RegisteredDALs",
                column: "_cartscId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingBasketDAL__storeName",
                table: "ShoppingBasketDAL",
                column: "_storeName");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingBasketDAL_ShoppingCartDALscId",
                table: "ShoppingBasketDAL",
                column: "ShoppingCartDALscId");

            migrationBuilder.CreateIndex(
                name: "IX_StockItemDAL_item_itemID",
                table: "StockItemDAL",
                column: "item_itemID");

            migrationBuilder.CreateIndex(
                name: "IX_StockItemDAL_StockDALid",
                table: "StockItemDAL",
                column: "StockDALid");

            migrationBuilder.CreateIndex(
                name: "IX_StoreDALs__discountPolicyid",
                table: "StoreDALs",
                column: "_discountPolicyid");

            migrationBuilder.CreateIndex(
                name: "IX_StoreDALs__purchasePolicyid",
                table: "StoreDALs",
                column: "_purchasePolicyid");

            migrationBuilder.CreateIndex(
                name: "IX_StoreDALs__ratingid",
                table: "StoreDALs",
                column: "_ratingid");

            migrationBuilder.CreateIndex(
                name: "IX_StoreDALs__stockid",
                table: "StoreDALs",
                column: "_stockid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminMessageToRegisteredDAL");

            migrationBuilder.DropTable(
                name: "BasketItemDAL");

            migrationBuilder.DropTable(
                name: "ComplaintDALs");

            migrationBuilder.DropTable(
                name: "DiscountDAL");

            migrationBuilder.DropTable(
                name: "MessageToStoreDALs");

            migrationBuilder.DropTable(
                name: "NotifyMessageDAL");

            migrationBuilder.DropTable(
                name: "OperationWrapper");

            migrationBuilder.DropTable(
                name: "PurchasedBasketDAL");

            migrationBuilder.DropTable(
                name: "PurchasedCartDAL");

            migrationBuilder.DropTable(
                name: "RateDAL");

            migrationBuilder.DropTable(
                name: "StockItemDAL");

            migrationBuilder.DropTable(
                name: "ConditionDAL");

            migrationBuilder.DropTable(
                name: "PurchaseDetailsDAL");

            migrationBuilder.DropTable(
                name: "RegisteredDALs");

            migrationBuilder.DropTable(
                name: "SystemRoleDALs");

            migrationBuilder.DropTable(
                name: "ShoppingBasketDAL");

            migrationBuilder.DropTable(
                name: "StorePurchaseHistory");

            migrationBuilder.DropTable(
                name: "RegisteredPurchaseHistory");

            migrationBuilder.DropTable(
                name: "ItemDAL");

            migrationBuilder.DropTable(
                name: "ShoppingCartDAL");

            migrationBuilder.DropTable(
                name: "StoreDALs");

            migrationBuilder.DropTable(
                name: "DiscountPolicyDAL");

            migrationBuilder.DropTable(
                name: "PurchasePolicyDAL");

            migrationBuilder.DropTable(
                name: "RatingDAL");

            migrationBuilder.DropTable(
                name: "StockDAL");
        }
    }
}
