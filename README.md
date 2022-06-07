# Online Shopping System
This is an online shopping system project. Users may join the website and traverse through it.
They are able to create new stores, add new items, change values and manage their employees.

## About The Project
The team worked on the project during our "Sostware Project Implementation Workshop" course.
The project had 5 iterations, each 2-3 weeks long. Written in C# and uses Blazor as the client-server architecture.
This is a fullstack project, consisting of a DAL, DL, SL, and a webpage GUI.

## Usage
Follow these steps to run the system:

1. Download the repository.
2. Download the "Initialization-File.txt" and "Configuration-File.txt" from the wiki pages to the following directory:
   MarketWeb/Server/bin/Debug/netcoreapp3.1
4. You may change the initialization file and configuration file, please read the formats for these files specified later in this README. 
5. If you wish to use the Initialization file, open the MarketWeb/Server/Service/MarketAPI.cs file in a text editor, and change "useInitializationFile" to "true".
6. Run the MarketWeb/Server project.
7. A default admin user is already initialized into the system. If the configuration file was unchanged: username: admin, password: admin.

## Initialization & Configuration File Formats
### Initialization
The basic initialization file in the wiki has the instructions on how to add dummy data.
Start by using EnterSystem(), and then call the wanted functions from MarketAPI.
### Configuration
The format for this file is:
parameter_name1 parameter value1
parameter_name2 parameter value2
...
The allowed parameters are:
* admin_username - the username of the initial admin.
* admin_password - the password of the initial admin.
* db_ip - the ip of the database.
* db_name - the name of the database.
* db_fullname - the full name of the database.
* db_password - the password of the database.
* db_connection_string - the connection string for the database.
* external_stock - the ip/url of the external stock system.
* external_purchase - the ip/url of the external purchase processing system.

## Colaborators
- Harel Afriat 211980776
- Afik Malka 323920207
- Ron Moshe 322361783
- Avishay Mamrud 315746560
- Oren Partesana 209058445
