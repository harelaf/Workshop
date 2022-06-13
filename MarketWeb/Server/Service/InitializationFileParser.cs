using MarketWeb.Service;
using MarketWeb.Shared;
using MarketWeb.Shared.DTO;
using System;
using System.Collections.Generic;
using System.IO;

namespace MarketWeb.Server.Service
{
    public class InitializationFileParser
    {
        private MarketAPI api;
        private Dictionary<String, Action<String>> methods;
        private List<String> tokens;
        private readonly String FILE_PATH = "";

        public InitializationFileParser(MarketAPI api)
        {
            this.api = api;
            InitMethods();
            tokens = new List<String>();
            FILE_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Initialization-File.txt");
        }

        public void InitMethods()
        {
            methods = new Dictionary<String, Action<String>>();
            methods["EnterSystem"] = ParseEnterSystem;
            methods["Login"] = ParseLogin;
            methods["Logout"] = ParseLogout;
            methods["Register"] = ParseRegister;
            methods["RemoveRegisteredVisitor"] = ParseRemoveRegisteredVisitor;
            methods["AddItemToCart"] = ParseAddItemToCart;
            methods["RemoveItemFromCart"] = ParseRemoveItemFromCart;
            methods["UpdateQuantityOfItemInCart"] = ParseUpdateQuantityOfItemInCart;
            methods["ViewMyCart"] = ParseViewMyCart;
            methods["PurchaseMyCart"] = ParsePurchaseMyCart;
            methods["OpenNewStore"] = ParseOpenNewStore;
            methods["AddStoreManager"] = ParseAddStoreManager;
            methods["AddStoreOwner"] = ParseAddStoreOwner;
            methods["RemoveStoreOwner"] = ParseRemoveStoreOwner;
            methods["RemoveStoreManager"] = ParseRemoveStoreManager;
            methods["AddItemToStoreStock"] = ParseAddItemToStoreStock;
            methods["RemoveItemFromStore"] = ParseRemoveItemFromStore;
            methods["UpdateStockQuantityOfItem"] = ParseUpdateStockQuantityOfItem;
            methods["EditItemPrice"] = ParseEditItemPrice;
            methods["EditItemName"] = ParseEditItemName;
            methods["EditItemDescription"] = ParseEditItemDescription;
            methods["RateItem"] = ParseRateItem;
            methods["RateStore"] = ParseRateStore;
            methods["GetStoreInformation"] = ParseGetStoreInformation;
            methods["GetItemInformation"] = ParseGetItemInformation;
            methods["SendMessageToStore"] = ParseSendMessageToStore;
            methods["FileComplaint"] = ParseFileComplaint;
            methods["GetMyPurchasesHistory"] = ParseGetMyPurchasesHistory;
            methods["GetVisitorInformation"] = ParseGetVisitorInformation;
            methods["EditUsername"] = ParseEditUsername;
            methods["EditVisitorPassword"] = ParseEditVisitorPassword;
            methods["RemoveManagerPermission"] = ParseRemoveManagerPermission;
            methods["AddManagerPermission"] = ParseAddManagerPermission;
            methods["CloseStore"] = ParseCloseStore;
            methods["ReopenStore"] = ParseReopenStore;
            methods["GetStoreOwners"] = ParseGetStoreOwners;
            methods["GetStoreManagers"] = ParseGetStoreManagers;
            methods["GetStoreFounder"] = ParseGetStoreFounder;
            methods["GetStoreMessages"] = ParseGetStoreMessages;
            methods["GetRegisteredMessagesFromAdmin"] = ParseGetRegisteredMessagesFromAdmin;
            methods["GetRegisterAnsweredStoreMessages"] = ParseGetRegisterAnsweredStoreMessages;
            methods["GetRegisteredMessagesNotofication"] = ParseGetRegisteredMessagesNotofication;
            methods["AnswerStoreMesseage"] = ParseAnswerStoreMesseage;
            methods["GetStorePurchasesHistory"] = ParseGetStorePurchasesHistory;
            methods["CloseStorePermanently"] = ParseCloseStorePermanently;
            methods["GetRegisterdComplaints"] = ParseGetRegisterdComplaints;
            methods["ReplyToComplaint"] = ParseReplyToComplaint;
            methods["SendMessageToRegisterd"] = ParseSendMessageToRegisterd;
            methods["GetStoresOfUser"] = ParseGetStoresOfUser;
            methods["GetAllActiveStores"] = ParseGetAllActiveStores;
            methods["ExitSystem"] = ParseExitSystem;
            methods["AppointSystemAdmin"] = ParseAppointSystemAdmin;
            methods["GetItem"] = ParseGetItem;
            methods["AddStoreDiscount"] = ParseAddStoreDiscount;
        }

        public void ParseInitializationFile()
        {
            if (!File.Exists(FILE_PATH))
                throw new FileNotFoundException("INITIALIZER: File not found.");
            int LineNumber = -1;
            foreach (string fileline in File.ReadLines(FILE_PATH))
            {
                LineNumber++;
                String line = fileline.Trim();
                if (line.Length == 0)
                    continue;
                else if (line.StartsWith("//"))
                    continue;
                ParseLine(line, LineNumber);
            }
        }

        private void ParseLine(String line, int LineNumber)
        {
            int index = 0;
            String MethodName = "";

            while (index < line.Length && line[index] != '(')
            {
                MethodName += line[index++];
            }
            if (index == line.Length)
                throw new ArgumentException($"INITIALIZER: Function call with no opening bracket '('. Line number: {LineNumber}.");

            if (methods.ContainsKey(MethodName))
            {
                methods[MethodName](line.Substring(index));
            }
            else
                throw new ArgumentException($"INITIALIZER: Call to unknown function {MethodName}. Line number: {LineNumber}.");
        }

        private String[] ParseArgs(String args, String MethodName, int ArgsCount)
        {
            args = args.Substring(1, args.Length - 2);
            String[] args_arr = args.Split(',');
            if (args_arr.Length != ArgsCount)
                throw new ArgumentException($"INITIALIZER: {MethodName} called without {ArgsCount} arguments.");
            for (int i = 0; i < args_arr.Length; i++)
            {
                args_arr[i] = args_arr[i].Trim();
            }
            return args_arr;
        }

        private int ParseAuthIndex(String arg, String MethodName)
        {
            int authIndex;
            try
            {
                authIndex = int.Parse(arg);
            }
            catch
            {
                throw new ArgumentException($"INITIALIZER: {MethodName} called with faulty authentication index. argument={arg}");
            }
            if (authIndex < 1 || authIndex > tokens.Count)
                throw new IndexOutOfRangeException($"INITIALIZER: {MethodName} called with faulty authentication index. argument={arg}");
            return authIndex;
        }

        private DateTime ParseDateTime(String arg, String MethodName)
        {
            DateTime date;
            try
            {
                date = DateTime.Parse(arg);
            }
            catch
            {
                throw new ArgumentException($"INITIALIZER: {MethodName} failed because of faulty DateTime format. argument={arg}");
            }
            return date;
        }

        private int ParseInt(String arg, String MethodName)
        {
            int value;
            try
            {
                value = int.Parse(arg);
            }
            catch
            {
                throw new ArgumentException($"INITIALIZER: {MethodName} failed because of faulty integer. argument={arg}");
            }
            return value;
        }

        private double ParseDouble(String arg, String MethodName)
        {
            double value;
            try
            {
                value = double.Parse(arg);
            }
            catch
            {
                throw new ArgumentException($"INITIALIZER: {MethodName} failed because of faulty double. argument={arg}");
            }
            return value;
        }

        private void ParseEnterSystem(String args)
        {
            if (!args.Equals("()"))
                throw new ArgumentException($"INITIALIZER: EnterSystem called with unknown args: {args}.");
            Response<String> response = api.EnterSystem();
            if (response.ErrorOccured)
                throw new ArgumentException($"INITIALIZER: EnterSystem failed. {response.ErrorMessage}");
            tokens.Add(response.Value);
        }

        private void ParseLogin(String args_)
        {
            String MethodName = "Login";
            String[] args = ParseArgs(args_, MethodName, 3);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response<String> response = api.Login(tokens[authIndex - 1], args[1], args[2]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
            tokens[authIndex - 1] = response.Value;
        }

        private void ParseLogout(String args_)
        {
            String MethodName = "Logout";
            String[] args = ParseArgs(args_, MethodName, 1);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response<String> response = api.Logout(tokens[authIndex - 1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseRegister(String args_)
        {
            String MethodName = "Register";
            String[] args = ParseArgs(args_, MethodName, 4);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            DateTime dob = ParseDateTime(args[3], MethodName);
            Response response = api.Register(tokens[authIndex - 1], args[1], args[2], dob);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseRemoveRegisteredVisitor(String args_)
        {
            String MethodName = "RemoveRegisteredVisitor";
            String[] args = ParseArgs(args_, MethodName, 2);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.RemoveRegisteredVisitor(tokens[authIndex - 1], args[1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseAddItemToCart(String args_)
        {
            String MethodName = "AddItemToCart";
            String[] args = ParseArgs(args_, MethodName, 4);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            int itemid = ParseInt(args[1], MethodName);
            int amount = ParseInt(args[3], MethodName);
            Response response = api.AddItemToCart(tokens[authIndex - 1], itemid, args[2], amount);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseRemoveItemFromCart(String args_)
        {
            String MethodName = "RemoveItemFromCart";
            String[] args = ParseArgs(args_, MethodName, 3);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            int itemid = ParseInt(args[1], MethodName);
            Response response = api.RemoveItemFromCart(tokens[authIndex - 1], itemid, args[2]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseUpdateQuantityOfItemInCart(String args_)
        {
            String MethodName = "UpdateQuantityOfItemInCart";
            String[] args = ParseArgs(args_, MethodName, 4);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            int itemid = ParseInt(args[1], MethodName);
            int quantity = ParseInt(args[3], MethodName);
            Response response = api.UpdateQuantityOfItemInCart(tokens[authIndex - 1], itemid, args[2], quantity);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseViewMyCart(String args_)
        {
            String MethodName = "ViewMyCart";
            String[] args = ParseArgs(args_, MethodName, 1);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.ViewMyCart(tokens[authIndex - 1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private async void ParsePurchaseMyCart(String args_)
        {
            String MethodName = "PurchaseMyCart";
            String[] args = ParseArgs(args_, MethodName, 8);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = await api.PurchaseMyCart(tokens[authIndex - 1], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseOpenNewStore(String args_)
        {
            String MethodName = "OpenNewStore";
            String[] args = ParseArgs(args_, MethodName, 2);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.OpenNewStore(tokens[authIndex - 1], args[1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseAddStoreManager(String args_)
        {
            String MethodName = "AddStoreManager";
            String[] args = ParseArgs(args_, MethodName, 3);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.AddStoreManager(tokens[authIndex - 1], args[1], args[2]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseAddStoreOwner(String args_)
        {
            String MethodName = "AddStoreOwner";
            String[] args = ParseArgs(args_, MethodName, 3);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.AddStoreOwner(tokens[authIndex - 1], args[1], args[2]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseRemoveStoreOwner(String args_)
        {
            String MethodName = "RemoveStoreOwner";
            String[] args = ParseArgs(args_, MethodName, 3);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.RemoveStoreOwner(tokens[authIndex - 1], args[1], args[2]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseRemoveStoreManager(String args_)
        {
            String MethodName = "RemoveStoreManager";
            String[] args = ParseArgs(args_, MethodName, 3);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.RemoveStoreManager(tokens[authIndex - 1], args[1], args[2]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseAddItemToStoreStock(String args_)
        {
            String MethodName = "AddItemToStoreStock";
            String[] args = ParseArgs(args_, MethodName, 8);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            int itemid = ParseInt(args[2], MethodName);
            double price = ParseDouble(args[4], MethodName);
            int quantity = ParseInt(args[7], MethodName);
            Response response = api.AddItemToStoreStock(tokens[authIndex - 1], args[1], itemid, args[3], price, args[5], args[6], quantity);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseRemoveItemFromStore(String args_)
        {
            String MethodName = "RemoveItemFromStore";
            String[] args = ParseArgs(args_, MethodName, 3);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            int itemid = ParseInt(args[2], MethodName);
            Response response = api.RemoveItemFromStore(tokens[authIndex - 1], args[1], itemid);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseUpdateStockQuantityOfItem(String args_)
        {
            String MethodName = "UpdateStockQuantityOfItem";
            String[] args = ParseArgs(args_, MethodName, 4);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            int itemid = ParseInt(args[2], MethodName);
            int quantity = ParseInt(args[3], MethodName);
            Response response = api.UpdateStockQuantityOfItem(tokens[authIndex - 1], args[1], itemid, quantity);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseEditItemPrice(String args_)
        {
            String MethodName = "EditItemPrice";
            String[] args = ParseArgs(args_, MethodName, 4);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            int itemid = ParseInt(args[2], MethodName);
            double price = ParseDouble(args[3], MethodName);
            Response response = api.EditItemPrice(tokens[authIndex - 1], args[1], itemid, price);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseEditItemName(String args_)
        {
            String MethodName = "EditItemName";
            String[] args = ParseArgs(args_, MethodName, 4);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            int itemid = ParseInt(args[2], MethodName);
            Response response = api.EditItemName(tokens[authIndex - 1], args[1], itemid, args[3]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseEditItemDescription(String args_)
        {
            String MethodName = "EditItemDescription";
            String[] args = ParseArgs(args_, MethodName, 4);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            int itemid = ParseInt(args[2], MethodName);
            Response response = api.EditItemDescription(tokens[authIndex - 1], args[1], itemid, args[3]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseRateItem(String args_)
        {
            String MethodName = "RateItem";
            String[] args = ParseArgs(args_, MethodName, 5);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            int itemid = ParseInt(args[1], MethodName);
            int rating = ParseInt(args[3], MethodName);
            Response response = api.RateItem(tokens[authIndex - 1], itemid, args[2], rating, args[4]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseRateStore(String args_)
        {
            String MethodName = "RateStore";
            String[] args = ParseArgs(args_, MethodName, 4);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            int rating = ParseInt(args[2], MethodName);
            Response response = api.RateStore(tokens[authIndex - 1], args[1], rating, args[3]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseGetStoreInformation(String args_)
        {
            String MethodName = "GetStoreInformation";
            String[] args = ParseArgs(args_, MethodName, 2);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.GetStoreInformation(tokens[authIndex - 1], args[1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseGetItemInformation(String args_)
        {
            String MethodName = "GetItemInformation";
            String[] args = ParseArgs(args_, MethodName, 4);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.GetItemInformation(tokens[authIndex - 1], args[1], args[2], args[3]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseSendMessageToStore(String args_)
        {
            String MethodName = "SendMessageToStore";
            String[] args = ParseArgs(args_, MethodName, 5);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            int id = ParseInt(args[4], MethodName);
            Response response = api.SendMessageToStore(tokens[authIndex - 1], args[1], args[2], args[3], id);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseFileComplaint(String args_)
        {
            String MethodName = "FileComplaint";
            String[] args = ParseArgs(args_, MethodName, 3);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            int cartid = ParseInt(args[1], MethodName);
            Response response = api.FileComplaint(tokens[authIndex - 1], cartid, args[2]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseGetMyPurchasesHistory(String args_)
        {
            String MethodName = "GetMyPurchasesHistory";
            String[] args = ParseArgs(args_, MethodName, 1);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.GetMyPurchasesHistory(tokens[authIndex - 1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseGetVisitorInformation(String args_)
        {
            String MethodName = "GetVisitorInformation";
            String[] args = ParseArgs(args_, MethodName, 1);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.GetVisitorInformation(tokens[authIndex - 1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseEditUsername(String args_)
        {
            throw new Exception($"INITIALIZER: EDITUSERNAME failed. THIS IS NOT IMPLEMENTED.");
        }

        private void ParseEditVisitorPassword(String args_)
        {
            String MethodName = "EditVisitorPassword";
            String[] args = ParseArgs(args_, MethodName, 3);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.EditVisitorPassword(tokens[authIndex - 1], args[1], args[2]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseRemoveManagerPermission(String args_)
        {
            String MethodName = "RemoveManagerPermission";
            String[] args = ParseArgs(args_, MethodName, 4);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.RemoveManagerPermission(tokens[authIndex - 1], args[1], args[2], args[3]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseAddManagerPermission(String args_)
        {
            String MethodName = "AddManagerPermission";
            String[] args = ParseArgs(args_, MethodName, 4);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.AddManagerPermission(tokens[authIndex - 1], args[1], args[2], args[3]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseCloseStore(String args_)
        {
            String MethodName = "CloseStore";
            String[] args = ParseArgs(args_, MethodName, 2);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.CloseStore(tokens[authIndex - 1], args[1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseReopenStore(String args_)
        {
            String MethodName = "ReopenStore";
            String[] args = ParseArgs(args_, MethodName, 2);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.ReopenStore(tokens[authIndex - 1], args[1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseGetStoreOwners(String args_)
        {
            String MethodName = "GetStoreOwners";
            String[] args = ParseArgs(args_, MethodName, 2);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.GetStoreOwners(tokens[authIndex - 1], args[1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseGetStoreManagers(String args_)
        {
            String MethodName = "GetStoreManagers";
            String[] args = ParseArgs(args_, MethodName, 2);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.GetStoreManagers(tokens[authIndex - 1], args[1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseGetStoreFounder(String args_)
        {
            String MethodName = "GetStoreFounder";
            String[] args = ParseArgs(args_, MethodName, 2);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.GetStoreFounder(tokens[authIndex - 1], args[1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseGetStoreMessages(String args_)
        {
            String MethodName = "GetStoreMessages";
            String[] args = ParseArgs(args_, MethodName, 2);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.GetStoreMessages(tokens[authIndex - 1], args[1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseGetRegisteredMessagesFromAdmin(String args_)
        {
            String MethodName = "GetRegisteredMessagesFromAdmin";
            String[] args = ParseArgs(args_, MethodName, 1);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.GetRegisteredMessagesFromAdmin(tokens[authIndex - 1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseGetRegisterAnsweredStoreMessages(String args_)
        {
            String MethodName = "GetRegisterAnsweredStoreMessages";
            String[] args = ParseArgs(args_, MethodName, 1);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.GetRegisterAnsweredStoreMessages(tokens[authIndex - 1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseGetRegisteredMessagesNotofication(String args_)
        {
            String MethodName = "GetRegisteredMessagesNotofication";
            String[] args = ParseArgs(args_, MethodName, 1);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.GetRegisteredMessagesNotofication(tokens[authIndex - 1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseAnswerStoreMesseage(String args_)
        {
            String MethodName = "AnswerStoreMesseage";
            String[] args = ParseArgs(args_, MethodName, 5);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            int msgid = ParseInt(args[2], MethodName);
            Response response = api.AnswerStoreMesseage(tokens[authIndex - 1], args[1], msgid, args[3], args[4]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseGetStorePurchasesHistory(String args_)
        {
            String MethodName = "GetStorePurchasesHistory";
            String[] args = ParseArgs(args_, MethodName, 2);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.GetStorePurchasesHistory(tokens[authIndex - 1], args[1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseCloseStorePermanently(String args_)
        {
            String MethodName = "CloseStorePermanently";
            String[] args = ParseArgs(args_, MethodName, 2);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.CloseStorePermanently(tokens[authIndex - 1], args[1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseGetRegisterdComplaints(String args_)
        {
            String MethodName = "GetRegisterdComplaints";
            String[] args = ParseArgs(args_, MethodName, 1);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.GetRegisterdComplaints(tokens[authIndex - 1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseReplyToComplaint(String args_)
        {
            String MethodName = "ReplyToComplaint";
            String[] args = ParseArgs(args_, MethodName, 3);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            int complaintid = ParseInt(args[1], MethodName);
            Response response = api.ReplyToComplaint(tokens[authIndex - 1], complaintid, args[2]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseSendMessageToRegisterd(String args_)
        {
            String MethodName = "SendMessageToRegisterd";
            String[] args = ParseArgs(args_, MethodName, 4);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.SendMessageToRegisterd(tokens[authIndex - 1], args[1], args[2], args[3]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseGetStoresOfUser(String args_)
        {
            String MethodName = "GetStoresOfUser";
            String[] args = ParseArgs(args_, MethodName, 1);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.GetStoresOfUser(tokens[authIndex - 1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseGetAllActiveStores(String args_)
        {
            String MethodName = "GetAllActiveStores";
            String[] args = ParseArgs(args_, MethodName, 1);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.GetAllActiveStores(tokens[authIndex - 1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseExitSystem(String args_)
        {
            String MethodName = "ExitSystem";
            String[] args = ParseArgs(args_, MethodName, 1);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.ExitSystem(tokens[authIndex - 1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseAppointSystemAdmin(String args_)
        {
            String MethodName = "AppointSystemAdmin";
            String[] args = ParseArgs(args_, MethodName, 2);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.AppointSystemAdmin(tokens[authIndex - 1], args[1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseGetItem(String args_)
        {
            String MethodName = "GetItem";
            String[] args = ParseArgs(args_, MethodName, 3);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            int itemid = ParseInt(args[2], MethodName);
            Response response = api.GetItem(tokens[authIndex - 1], args[1], itemid);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }

        private void ParseAddStoreDiscount(String args_)
        {
            String MethodName = "AddStoreDiscount";
            String[] args = ParseArgs(args_, MethodName, 4);
            int authIndex = ParseAuthIndex(args[0], MethodName);
            Response response = api.AddStoreDiscount(tokens[authIndex - 1], args[1], args[2], args[3]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: {MethodName} failed. {response.ErrorMessage}");
        }
    }
}
