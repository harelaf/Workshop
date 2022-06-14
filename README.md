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
5. Run the MarketWeb/Server project.
6. If the configuration file was unchanged the default admin is: username: admin, password: admin.

## Initialization & Configuration File Formats
### Initialization
The basic initialization file in the wiki has the instructions on how to add dummy data.
If you would like the system to ignore the file, the first line should be: "ignore_file true".
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
* db_initial_catalog - the database name.
* db_username - the username of a user of the database.
* db_password - the password of the database.
* external_shipping - the ip/url of the external shipping system.
* external_payment - the ip/url of the external payment system.

## Colaborators
- Harel Afriat 211980776
- Afik Malka 323920207
- Ron Moshe 322361783
- Avishay Mamrud 315746560
- Oren Partesana 209058445
