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
                    _id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    _complainer = table.Column<string>(type: "TEXT", nullable: false),
                    _cartID = table.Column<int>(type: "INTEGER", nullable: false),
                    _message = table.Column<string>(type: "TEXT", nullable: false),
                    _response = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintDALs", x => x._id);
                });

            migrationBuilder.CreateTable(
                name: "itemDALs",
                columns: table => new
                {
                    _itemID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    _name = table.Column<string>(type: "TEXT", nullable: false),
                    _price = table.Column<double>(type: "REAL", nullable: false),
                    _description = table.Column<string>(type: "TEXT", nullable: false),
                    _category = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itemDALs", x => x._itemID);
                });

            migrationBuilder.CreateTable(
                name: "MessageToStoreDALs",
                columns: table => new
                {
                    mid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    _senderUsername = table.Column<string>(type: "TEXT", nullable: true),
                    _storeName = table.Column<string>(type: "TEXT", nullable: true),
                    _message = table.Column<string>(type: "TEXT", nullable: false),
                    _title = table.Column<string>(type: "TEXT", nullable: false),
                    _reply = table.Column<string>(type: "TEXT", nullable: true),
                    _replierFromStore = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageToStoreDALs", x => x.mid);
                });

            migrationBuilder.CreateTable(
                name: "PopulationStatisticsDALs",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    _userNane = table.Column<string>(type: "TEXT", nullable: true),
                    _visitDay = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PopulationStatisticsDALs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseDetailsDAL",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    _itemID = table.Column<int>(type: "INTEGER", nullable: false),
                    amount = table.Column<int>(type: "INTEGER", nullable: false),
                    discountListJSON = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseDetailsDAL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RegisteredPurchaseHistory",
                columns: table => new
                {
                    userName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredPurchaseHistory", x => x.userName);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartDAL",
                columns: table => new
                {
                    scId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartDAL", x => x.scId);
                });

            migrationBuilder.CreateTable(
                name: "StoreDALs",
                columns: table => new
                {
                    _storeName = table.Column<string>(type: "TEXT", nullable: false),
                    _discountPolicyJSON = table.Column<string>(type: "TEXT", nullable: false),
                    _purchasePolicyJSON = table.Column<string>(type: "TEXT", nullable: false),
                    _state = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreDALs", x => x._storeName);
                });

            migrationBuilder.CreateTable(
                name: "StorePurchaseHistory",
                columns: table => new
                {
                    _storeName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorePurchaseHistory", x => x._storeName);
                });

            migrationBuilder.CreateTable(
                name: "SystemRoleDALs",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    _username = table.Column<string>(type: "TEXT", nullable: true),
                    _storeName = table.Column<string>(type: "TEXT", nullable: true),
                    _appointer = table.Column<string>(type: "TEXT", nullable: true),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemRoleDALs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PurchasedCartDAL",
                columns: table => new
                {
                    _purchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PurchasedCartDAL = table.Column<int>(type: "INTEGER", nullable: true),
                    RegisteredPurchasedCartDALuserName = table.Column<string>(type: "TEXT", nullable: true)
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
                        name: "FK_PurchasedCartDAL_ShoppingCartDAL_PurchasedCartDAL",
                        column: x => x.PurchasedCartDAL,
                        principalTable: "ShoppingCartDAL",
                        principalColumn: "scId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegisteredDALs",
                columns: table => new
                {
                    _username = table.Column<string>(type: "TEXT", nullable: false),
                    _password = table.Column<string>(type: "TEXT", nullable: true),
                    _salt = table.Column<string>(type: "TEXT", nullable: true),
                    CartDAL = table.Column<int>(type: "INTEGER", nullable: false),
                    _birthDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    _populationSection = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredDALs", x => x._username);
                    table.ForeignKey(
                        name: "FK_RegisteredDALs_ShoppingCartDAL_CartDAL",
                        column: x => x.CartDAL,
                        principalTable: "ShoppingCartDAL",
                        principalColumn: "scId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingBasketDAL",
                columns: table => new
                {
                    sbId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    _storeName = table.Column<string>(type: "TEXT", nullable: false),
                    _additionalDiscountsID = table.Column<int>(type: "INTEGER", nullable: true),
                    ShoppingCartDALscId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingBasketDAL", x => x.sbId);
                    table.ForeignKey(
                        name: "FK_ShoppingBasketDAL_PurchaseDetailsDAL__additionalDiscountsID",
                        column: x => x._additionalDiscountsID,
                        principalTable: "PurchaseDetailsDAL",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShoppingBasketDAL_ShoppingCartDAL_ShoppingCartDALscId",
                        column: x => x.ShoppingCartDALscId,
                        principalTable: "ShoppingCartDAL",
                        principalColumn: "scId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BidOfVisitor",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    _bidder = table.Column<string>(type: "TEXT", nullable: false),
                    StoreDAL_storeName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidOfVisitor", x => x.id);
                    table.ForeignKey(
                        name: "FK_BidOfVisitor_StoreDALs_StoreDAL_storeName",
                        column: x => x.StoreDAL_storeName,
                        principalTable: "StoreDALs",
                        principalColumn: "_storeName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OwnerAcceptors",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    _newOwner = table.Column<string>(type: "TEXT", nullable: false),
                    _appointer = table.Column<string>(type: "TEXT", nullable: false),
                    StoreDAL_storeName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerAcceptors", x => x.id);
                    table.ForeignKey(
                        name: "FK_OwnerAcceptors_StoreDALs_StoreDAL_storeName",
                        column: x => x.StoreDAL_storeName,
                        principalTable: "StoreDALs",
                        principalColumn: "_storeName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RateDAL",
                columns: table => new
                {
                    rid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    username = table.Column<string>(type: "TEXT", nullable: false),
                    rate = table.Column<int>(type: "INTEGER", nullable: false),
                    review = table.Column<string>(type: "TEXT", nullable: true),
                    RatingDAL = table.Column<int>(type: "INTEGER", nullable: true),
                    StoreDAL_storeName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateDAL", x => x.rid);
                    table.ForeignKey(
                        name: "FK_RateDAL_itemDALs_RatingDAL",
                        column: x => x.RatingDAL,
                        principalTable: "itemDALs",
                        principalColumn: "_itemID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RateDAL_StoreDALs_StoreDAL_storeName",
                        column: x => x.StoreDAL_storeName,
                        principalTable: "StoreDALs",
                        principalColumn: "_storeName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockItemDAL",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    itemID = table.Column<int>(type: "INTEGER", nullable: false),
                    amount = table.Column<int>(type: "INTEGER", nullable: false),
                    StoreDAL_storeName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockItemDAL", x => x.id);
                    table.ForeignKey(
                        name: "FK_StockItemDAL_StoreDALs_StoreDAL_storeName",
                        column: x => x.StoreDAL_storeName,
                        principalTable: "StoreDALs",
                        principalColumn: "_storeName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OperationWrapper",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    op = table.Column<int>(type: "INTEGER", nullable: false),
                    SystemRoleDALid = table.Column<int>(type: "INTEGER", nullable: true)
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
                name: "AdminMessageToRegisteredDAL",
                columns: table => new
                {
                    mid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    _senderUsername = table.Column<string>(type: "TEXT", nullable: false),
                    _title = table.Column<string>(type: "TEXT", nullable: false),
                    _message = table.Column<string>(type: "TEXT", nullable: false),
                    RegisteredDAL_username = table.Column<string>(type: "TEXT", nullable: true)
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
                    mid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    _storeName = table.Column<string>(type: "TEXT", nullable: false),
                    _title = table.Column<string>(type: "TEXT", nullable: false),
                    _message = table.Column<string>(type: "TEXT", nullable: false),
                    RegisteredDAL_username = table.Column<string>(type: "TEXT", nullable: true)
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
                name: "BasketItemDAL",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    itemID = table.Column<int>(type: "INTEGER", nullable: false),
                    purchaseDetailsID = table.Column<int>(type: "INTEGER", nullable: true),
                    ShoppingBasketDALsbId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketItemDAL", x => x.id);
                    table.ForeignKey(
                        name: "FK_BasketItemDAL_PurchaseDetailsDAL_purchaseDetailsID",
                        column: x => x.purchaseDetailsID,
                        principalTable: "PurchaseDetailsDAL",
                        principalColumn: "ID",
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
                    _purchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    _PurchasedBasketsbId = table.Column<int>(type: "INTEGER", nullable: true),
                    StorePurchasedBasketDAL_storeName = table.Column<string>(type: "TEXT", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "BidDAL",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    _bidder = table.Column<string>(type: "TEXT", nullable: false),
                    _itemId = table.Column<int>(type: "INTEGER", nullable: false),
                    _amount = table.Column<int>(type: "INTEGER", nullable: false),
                    _biddedPrice = table.Column<double>(type: "REAL", nullable: false),
                    _counterOffer = table.Column<double>(type: "REAL", nullable: false),
                    _acceptedByAll = table.Column<bool>(type: "INTEGER", nullable: false),
                    BidsOfVisitorid = table.Column<int>(type: "INTEGER", nullable: true),
                    ShoppingBasketDALsbId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidDAL", x => x.id);
                    table.ForeignKey(
                        name: "FK_BidDAL_BidOfVisitor_BidsOfVisitorid",
                        column: x => x.BidsOfVisitorid,
                        principalTable: "BidOfVisitor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BidDAL_ShoppingBasketDAL_ShoppingBasketDALsbId",
                        column: x => x.ShoppingBasketDALsbId,
                        principalTable: "ShoppingBasketDAL",
                        principalColumn: "sbId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StringData",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    data = table.Column<string>(type: "TEXT", nullable: false),
                    BidDALid = table.Column<int>(type: "INTEGER", nullable: true),
                    OwnerAcceptorsid = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StringData", x => x.id);
                    table.ForeignKey(
                        name: "FK_StringData_BidDAL_BidDALid",
                        column: x => x.BidDALid,
                        principalTable: "BidDAL",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StringData_OwnerAcceptors_OwnerAcceptorsid",
                        column: x => x.OwnerAcceptorsid,
                        principalTable: "OwnerAcceptors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminMessageToRegisteredDAL_RegisteredDAL_username",
                table: "AdminMessageToRegisteredDAL",
                column: "RegisteredDAL_username");

            migrationBuilder.CreateIndex(
                name: "IX_BasketItemDAL_purchaseDetailsID",
                table: "BasketItemDAL",
                column: "purchaseDetailsID");

            migrationBuilder.CreateIndex(
                name: "IX_BasketItemDAL_ShoppingBasketDALsbId",
                table: "BasketItemDAL",
                column: "ShoppingBasketDALsbId");

            migrationBuilder.CreateIndex(
                name: "IX_BidDAL_BidsOfVisitorid",
                table: "BidDAL",
                column: "BidsOfVisitorid");

            migrationBuilder.CreateIndex(
                name: "IX_BidDAL_ShoppingBasketDALsbId",
                table: "BidDAL",
                column: "ShoppingBasketDALsbId");

            migrationBuilder.CreateIndex(
                name: "IX_BidOfVisitor_StoreDAL_storeName",
                table: "BidOfVisitor",
                column: "StoreDAL_storeName");

            migrationBuilder.CreateIndex(
                name: "IX_NotifyMessageDAL_RegisteredDAL_username",
                table: "NotifyMessageDAL",
                column: "RegisteredDAL_username");

            migrationBuilder.CreateIndex(
                name: "IX_OperationWrapper_SystemRoleDALid",
                table: "OperationWrapper",
                column: "SystemRoleDALid");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerAcceptors_StoreDAL_storeName",
                table: "OwnerAcceptors",
                column: "StoreDAL_storeName");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedBasketDAL__PurchasedBasketsbId",
                table: "PurchasedBasketDAL",
                column: "_PurchasedBasketsbId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedBasketDAL_StorePurchasedBasketDAL_storeName",
                table: "PurchasedBasketDAL",
                column: "StorePurchasedBasketDAL_storeName");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedCartDAL_PurchasedCartDAL",
                table: "PurchasedCartDAL",
                column: "PurchasedCartDAL");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedCartDAL_RegisteredPurchasedCartDALuserName",
                table: "PurchasedCartDAL",
                column: "RegisteredPurchasedCartDALuserName");

            migrationBuilder.CreateIndex(
                name: "IX_RateDAL_RatingDAL",
                table: "RateDAL",
                column: "RatingDAL");

            migrationBuilder.CreateIndex(
                name: "IX_RateDAL_StoreDAL_storeName",
                table: "RateDAL",
                column: "StoreDAL_storeName");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredDALs_CartDAL",
                table: "RegisteredDALs",
                column: "CartDAL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingBasketDAL__additionalDiscountsID",
                table: "ShoppingBasketDAL",
                column: "_additionalDiscountsID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingBasketDAL_ShoppingCartDALscId",
                table: "ShoppingBasketDAL",
                column: "ShoppingCartDALscId");

            migrationBuilder.CreateIndex(
                name: "IX_StockItemDAL_StoreDAL_storeName",
                table: "StockItemDAL",
                column: "StoreDAL_storeName");

            migrationBuilder.CreateIndex(
                name: "IX_StringData_BidDALid",
                table: "StringData",
                column: "BidDALid");

            migrationBuilder.CreateIndex(
                name: "IX_StringData_OwnerAcceptorsid",
                table: "StringData",
                column: "OwnerAcceptorsid");
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
                name: "MessageToStoreDALs");

            migrationBuilder.DropTable(
                name: "NotifyMessageDAL");

            migrationBuilder.DropTable(
                name: "OperationWrapper");

            migrationBuilder.DropTable(
                name: "PopulationStatisticsDALs");

            migrationBuilder.DropTable(
                name: "PurchasedBasketDAL");

            migrationBuilder.DropTable(
                name: "PurchasedCartDAL");

            migrationBuilder.DropTable(
                name: "RateDAL");

            migrationBuilder.DropTable(
                name: "StockItemDAL");

            migrationBuilder.DropTable(
                name: "StringData");

            migrationBuilder.DropTable(
                name: "RegisteredDALs");

            migrationBuilder.DropTable(
                name: "SystemRoleDALs");

            migrationBuilder.DropTable(
                name: "StorePurchaseHistory");

            migrationBuilder.DropTable(
                name: "RegisteredPurchaseHistory");

            migrationBuilder.DropTable(
                name: "itemDALs");

            migrationBuilder.DropTable(
                name: "BidDAL");

            migrationBuilder.DropTable(
                name: "OwnerAcceptors");

            migrationBuilder.DropTable(
                name: "BidOfVisitor");

            migrationBuilder.DropTable(
                name: "ShoppingBasketDAL");

            migrationBuilder.DropTable(
                name: "StoreDALs");

            migrationBuilder.DropTable(
                name: "PurchaseDetailsDAL");

            migrationBuilder.DropTable(
                name: "ShoppingCartDAL");
        }
    }
}
