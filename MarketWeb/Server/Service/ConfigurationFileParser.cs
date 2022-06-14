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
            configurations["db_initial_catalog"] = "";
            configurations["db_username"] = "";
            configurations["db_password"] = "";
            configurations["external_shipping"] = "";
            configurations["external_payment"] = "";
        }

        public Dictionary<String, String> ParseConfigurationFile()
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

            if (configurations["external_shipping"] == "" || configurations["external_payment"] == "")
                throw new Exception("CONFIG: External systems are not specified in the configuration file.");
            if (configurations["db_ip"] == "" || configurations["db_initial_catalog"] == "" || configurations["db_username"] == "" || configurations["db_password"] == "")
                throw new Exception("CONFIG: DB values are missing (ip, initial_catalog, username, password).");

            if (configurations["admin_username"] == "")
            {
                configurations["admin_username"] = "admin";
            }
            if (configurations["admin_password"] == "")
            {
                configurations["admin_password"] = "admin";
            }

            return configurations;
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
