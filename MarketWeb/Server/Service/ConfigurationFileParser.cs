using MarketWeb.Service;
using System;
using System.Collections.Generic;
using System.IO;

namespace MarketWeb.Server.Service
{
    public class ConfigurationFileParser
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<String, String> configurations;
        private readonly String FILE_PATH = "";

        public ConfigurationFileParser()
        {
            InitConfigurations();
            FILE_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration-File.txt");
        }

        private void InitConfigurations()
        {
            configurations = new Dictionary<string, string>();
            configurations["admin_username"] = "";
            configurations["admin_password"] = "";
            configurations["db_ip"] = "";
            configurations["db_name"] = "";
            configurations["db_fullname"] = "";
            configurations["db_password"] = "";
            configurations["db_connection_string"] = "";
            configurations["external_stock"] = "";
            configurations["external_purchase"] = "";
        }

        public void ParseConfigurationFile()
        {
            if (!File.Exists(FILE_PATH))
                throw new FileNotFoundException("CONFIG: File not found.");
            int LineNumber = -1;
            foreach (string fileline in File.ReadLines(FILE_PATH))
            {
                LineNumber++;
                String line = fileline.Trim();
                if (line.Length == 0)
                    continue;
                else if (line.StartsWith("//"))
                    continue;
                try
                {
                    ParseLine(line, LineNumber);
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message);
                }
            }

            if (configurations["external_purchase"] == "" || configurations["external_stock"] == "")
                throw new Exception("CONFIG: External systems are not specified in the configuration file.");
            // CHECK IF PURCHASE AND STOCK SYSTEMS ARE CORRECT, ELSE THROW EXCEPTION.

            // CREATE DB BY SENDING THE VALUES OF DB:
            // configurations["db_ip"] = "";
            // configurations["db_name"] = "";
            // configurations["db_fullname"] = "";
            // configurations["db_password"] = "";
            // configurations["db_connection_string"] = "";

            // TODO
            // REMEMBER TO REMOVE ADMIN INITIALIZATION IN VisitorManagement
            // TODO
            if (configurations["admin_username"] == "" || configurations["admin_password"] == "")
            {
                // Call DB and add new registered user:
                // username = admin
                // password = admin
                // dob = 1.1.2000
                // Add role SystemAdmin
            }
            else
            {
                // Call DB and add new registered user:
                // username = configurations["admin_username"]
                // password = configurations["admin_password"]
                // dob = 1.1.2000
                // Add role SystemAdmin
            }
        }

        private void ParseLine(String line, int LineNumber)
        {
            int index = 0;
            String ConfigParam = "";

            while (index < line.Length && line[index] != ' ')
            {
                ConfigParam += line[index++];
            }
            if (index == line.Length || index + 1 == line.Length)
                throw new ArgumentException($"CONFIG: Configuration parameter with no value. Line number: {LineNumber}.");

            if (configurations.ContainsKey(ConfigParam))
            {
                ParseParam(line.Substring(index + 1), ConfigParam);
            }
            else
                throw new ArgumentException($"CONFIG: Unknown configuration parameter {ConfigParam}. Line number: {LineNumber}.");
        }

        private void ParseParam(String arg, String ConfigParam)
        {
            configurations[ConfigParam] = arg.Trim();
        }
    }
}
