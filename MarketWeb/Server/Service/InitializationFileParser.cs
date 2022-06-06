using MarketWeb.Service;
using MarketWeb.Shared;
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
            //DISCOUNT STUFF IS MISSING CURRENTLY
        }

        public void ParseInitializationFile()
        {
            if (!File.Exists(FILE_PATH))
                return;
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
                throw new ArgumentException($"INITIALIZER: {MethodName} called with faulty authentication index.");
            }
            if (authIndex < 1 || authIndex > tokens.Count)
                throw new IndexOutOfRangeException($"INITIALIZER: {MethodName} called with faulty authentication index.");
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
                throw new ArgumentException($"INITIALIZER: {MethodName} failed because of faulty DateTime format.");
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
                throw new ArgumentException($"INITIALIZER: {MethodName} failed because of faulty integer.");
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
            String[] args = ParseArgs(args_, "Login", 3);
            int authIndex = ParseAuthIndex(args[0], "Login");
            Response<String> response = api.Login(tokens[authIndex - 1], args[1], args[2]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: Login failed. {response.ErrorMessage}");
            tokens[authIndex - 1] = response.Value;
        }

        private void ParseLogout(String args_)
        {
            String[] args = ParseArgs(args_, "Logout", 1);
            int authIndex = ParseAuthIndex(args[0], "Logout");
            Response<String> response = api.Logout(tokens[authIndex - 1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: Logout failed. {response.ErrorMessage}");
        }

        private void ParseRegister(String args_)
        {
            String[] args = ParseArgs(args_, "Register", 4);
            int authIndex = ParseAuthIndex(args[0], "Register");
            DateTime dob = ParseDateTime(args[3], "Register");
            Response response = api.Register(tokens[authIndex - 1], args[1], args[2], dob);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: Register failed. {response.ErrorMessage}");
        }

        private void ParseRemoveRegisteredVisitor(String args_)
        {
            String[] args = ParseArgs(args_, "RemoveRegisteredVisitor", 2);
            int authIndex = ParseAuthIndex(args[0], "RemoveRegisteredVisitor");
            Response response = api.RemoveRegisteredVisitor(tokens[authIndex - 1], args[1]);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: RemoveRegisteredVisitor failed. {response.ErrorMessage}");
        }


        private void ParseAddItemToCart(string args_)
        {
            String[] args = ParseArgs(args_, "AddItemToCart", 2);
            int authIndex = ParseAuthIndex(args[0], "AddItemToCart");
            int itemid = ParseInt(args[1], "AddItemToCart");
            int amount = ParseInt(args[3], "AddItemToCart");
            Response response = api.AddItemToCart(tokens[authIndex - 1], itemid, args[2], amount);
            if (response.ErrorOccured)
                throw new Exception($"INITIALIZER: AddItemToCart failed. {response.ErrorMessage}");
        }
    }
}
