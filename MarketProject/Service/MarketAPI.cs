﻿using MarketProject.Domain;
using MarketProject.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service
{
    internal class MarketAPI
    {
        private Market _market;

        public MarketAPI()
        {
            _market = new Market();

        }

        public Boolean RestartSystem(String sysManegerUsername, String ipShippingService, String ipPaymentService)
        {//I.1
            throw new NotImplementedException();
        }
        public Boolean Login(String authToken, String username, String password)
        {//II.1.4
            throw new NotImplementedException();
        }
        public Boolean Logout(String authToken)
        {//II.3.1
            throw new NotImplementedException();
        }
        public Boolean Register(String authToken, String username, String password)
        {//II.1.3
            throw new NotImplementedException();
        }
        public Boolean RemoveRegisteredUser(String authToken, String usr_toremove )
        {//II.6.2
            //remmeber to fire him for all its roles
            throw new NotImplementedException();
        }
        public Response AddItemToCart(String authToken, int itemID, String storeName, int amount)
        {//II.2.3
            Response response;
            try
            {
                _market.AddItemToCart(authToken, itemID, storeName, amount);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response RemoveItemFromCart(String authToken, int itemID, String storeName)
        {//II.2.4
            Response response;
            try
            {
                _market.RemoveItemFromCart(authToken, itemID, storeName);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response UpdateQuantityOfItemInCart(String authToken, int itemID, String storeName, int newQuantity)
        {//II.2.4
            Response response;
            try
            {
                _market.UpdateQuantityOfItemInCart(authToken, itemID, storeName, newQuantity);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response<ShoppingCartDTO> ViewMyCart(String authToken) /*Add data object of cart*/
        {//II.2.4
            Response<ShoppingCartDTO> response;
            try
            {
                ShoppingCart shoppingCart= _market.ViewMyCart(authToken);
                response = new Response<ShoppingCartDTO>(new ShoppingCartDTO(shoppingCart));
            }
            catch (Exception e)
            {
                response = new Response<ShoppingCartDTO>(null, e);
            }
            return response;
        }
        public Response PurchaseMyCart(String authToken, String address, String city, String country, String zip, String purchaserName)
        {//II.2.5
            Response response;
            try
            {
                _market.PurchaseMyCart(authToken, address, city, country, zip, purchaserName); 
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response OpenNewStore(String authToken, String storeName)
        {//II.3.2
            Response response;
            try
            {
                _market.OpenNewStore(authToken, storeName, new PurchasePolicy(), new DiscountPolicy());
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Boolean AddStoreManager(String authToken, String ownerUsername, String storeName)
        {//II.4.6
            throw new NotImplementedException();
        }
        public Boolean AddStoreOwner(String authToken, String ownerUsername, String storeName)
        {//II.4.4
            throw new NotImplementedException();
        }
        public Boolean RemoveStoreOwner(String authToken, String ownerUsername, String storeName)
        {//II.4.5
            throw new NotImplementedException();
        }
        public Boolean RemoveStoreManager(String authToken, String ownerUsername, String storeName)
        {//II.4.8
            throw new NotImplementedException();
        }
        public Response AddItemToStoreStock(String authToken, String storeName, int itemID, String name, double price, String description, String category, int quantity)
        {//II.4.1
            Response response;
            try
            {
                _market.AddItemToStoreStock(authToken, storeName, itemID, name, price, description, category, quantity);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response RemoveItemFromStore(String authToken, String storeName, int itemID)
        {//II.4.1
            Response response;
            try
            {
                _market.RemoveItemFromStore(authToken, storeName, itemID);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response UpdateStockQuantityOfItem(String authToken, String storeName, int itemID, int newQuantity)
        {//II.4.1
            Response response;
            try
            {
                _market.UpdateStockQuantityOfItem(authToken, storeName, itemID, newQuantity);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Boolean EditItemPrice(String username, int storeID, int itemID, float new_price)
        {//II.4.1
            throw new NotImplementedException();
        }
        public Boolean EditItemName(String username, String storeName, int itemID, int new_price, String newName)
        {//II.4.1
            throw new NotImplementedException();
        }
        public Boolean EditItemDescription(String authToken, String storeName, int itemID, String newDescription)
        {//II.4.1
            throw new NotImplementedException();
        }
        public Boolean RateItem(String authToken, int itemID, String storeName, int rating, String review)
        {//II.3.3,  II.3.4
            //should check that this user bought this item by his purches History
            throw new NotImplementedException();
        }
        public Response RateStore(String authToken, String storeName, int rating, String review) // 0 < rating < 10
        {//II.3.4
            Response response;
            try
            {
                _market.RateStore(authToken, storeName, rating, review);
                response = new Response();
            } 
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response<String> GetStoreInformation(String authToken, String storeName)
        {//II.2.1
            Response<String> response;
            try
            {
                String result = _market.GetStoreInformation(authToken, storeName);
                response = new Response<String>(result);
            }
            catch (Exception e)
            {
                response = new Response<String>(e);
            }
            return response;
        }
        public Boolean GetItemInformation(String authToken, String itemName, String itemCategory, String keyWord)
        {//II.2.2
            //filters!!!!!!!!!!!
            throw new NotImplementedException();
        }
        public Boolean SendMessageToStore(String authToken, String storeName, String title, String description)
        {//II.3.5
            throw new NotImplementedException();
        }
        public Boolean FileComplaint(String authToken, int cartID,  String message)
        {//II.3.6
            //to system admin!! should define some queue of messages for admin
            throw new NotImplementedException();
        }
        public Response<ICollection<PurchasedCartDTO>> GetMyPurchasesHistory(String authToken)
        {//II.3.7
            Response<ICollection<PurchasedCartDTO>> response;
            try
            {
                ICollection<Tuple<DateTime ,ShoppingCart>> purchasedCarts = _market.GetMyPurchases(authToken);
                ICollection<PurchasedCartDTO> purchasedCartsDTO = new List<PurchasedCartDTO>();
                foreach (Tuple<DateTime ,ShoppingCart> purchase in purchasedCarts)
                {
                    purchasedCartsDTO.Add(new PurchasedCartDTO(purchase.Item1, purchase.Item2));
                }

                response = new Response<ICollection<PurchasedCartDTO>>(purchasedCartsDTO);
            }
            catch (Exception e)
            {
                response = new Response<ICollection<PurchasedCartDTO>>(null, e);
            }
            return response;
        }
        public Boolean GetUserInformation(String authToken)
        {//II.3.8
            throw new NotImplementedException();
        }
        public Boolean EditUsername(String authToken, String newUsername )
        {//II.3.8
            throw new NotImplementedException();
        }
        public Boolean EditUserPassword(String authToken, String newPassword)
        {//II.3.8
            throw new NotImplementedException();
        }
        public Boolean RemoveManagerPermission(String authToken, String managerUsername)//permission param is Enum
        {//II.4.7

            throw new NotImplementedException();
        }
        public Boolean AddManagerPermission(String authToken, String managerUsername)//permission param is Enum
        {//II.4.7
            throw new NotImplementedException();
        }
        public Response CloseStore(String authToken, String storeName)
        {//II.4.9
            Response response;
            try
            {
                _market.CloseStore(authToken, storeName);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response ReopenStore(String authToken, String storeName)
        {//II.4.10
            Response response;
            try
            {
                _market.ReopenStore(authToken, storeName);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Boolean GetStoreRoleInformation(String authToken, String storeName)
        {//II.4.11
            throw new NotImplementedException();
        }
        public Boolean GetStoreMesseage(String authToken, String storeName)
        {//II.4.12
            //should return with id
            throw new NotImplementedException();
        }
        public Boolean AnswerStoreMesseage(String authToken, String storeName, int messageID, String reply)
        {//II.4.12
            throw new NotImplementedException();
        }
        public Response<List<Tuple<DateTime, ShoppingBasketDTO>>> GetStorePurchasesHistory(String authToken, String storeName)
        {//II.4.13
            Response<List<Tuple<DateTime, ShoppingBasketDTO>>> response;
            try
            {
                List<Tuple<DateTime, ShoppingBasket>> result = _market.GetStorePurchasesHistory(authToken, storeName);
                List<Tuple<DateTime, ShoppingBasketDTO>> dtos = new List<Tuple<DateTime, ShoppingBasketDTO>>();

                foreach(Tuple<DateTime, ShoppingBasket> tuple in result)
                {
                    ShoppingBasketDTO dto = new ShoppingBasketDTO(tuple.Item2);
                    Tuple<DateTime, ShoppingBasketDTO> toAdd = new Tuple<DateTime, ShoppingBasketDTO>(tuple.Item1, dto);
                    dtos.Add(toAdd);
                }

                response = new Response<List<Tuple<DateTime, ShoppingBasketDTO>>>(dtos);
            }
            catch (Exception e)
            {
                response = new Response<List<Tuple<DateTime, ShoppingBasketDTO>>>(e);
            }
            return response;
        }
        public Response CloseStorePermanently(String authToken, String storeName)
        {//II.6.1
            Response response;
            try
            {
                _market.CloseStorePermanently(authToken, storeName);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Boolean GetRegisterdComplaints(String authToken)
        {//II.6.3
            //return each complaint id in addition to its information
            throw new NotImplementedException();
        }
        public Boolean ReplyToComplaint(String authToken, int complaintID)
        {//II.6.3
            throw new NotImplementedException();
        }
        public Boolean SendMessageToRegisterd(String authToken, String usernameReciever, String message)
        {//II.6.3
            throw new NotImplementedException();
        }

        public String EnterSystem() // Generating token and returning it
        { //II.1.1
            throw new NotImplementedException();
        }

        public void ExitSystem(String authToken) // Removing cart and token assigned to guest
        { //II.1.2
            throw new NotImplementedException();
        }
    }
}
