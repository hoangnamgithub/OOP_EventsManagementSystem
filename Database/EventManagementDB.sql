
CREATE DATABASE EventManagementDB;

USE master;
ALTER DATABASE EventManagementDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE EventManagementDB;

ALTER DATABASE EventManagementDB SET MULTI_USER;

USE EventManagementDB;

-- Create the database
IF DB_ID('EventManagementDB') IS NULL
    CREATE DATABASE EventManagementDB;
GO
USE EventManagementDB;
GO

-- Create schemas
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Events') EXEC('CREATE SCHEMA Events');
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Shows') EXEC('CREATE SCHEMA Shows');
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Employees') EXEC('CREATE SCHEMA Employees');
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Equipment') EXEC('CREATE SCHEMA Equipment');
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Account') EXEC('CREATE SCHEMA Account');
GO

-- Create tables

-- Account schema
CREATE TABLE Account.permission (
    permission_id INT PRIMARY KEY IDENTITY(1,1),
    permission NVARCHAR(100) NOT NULL
);

CREATE TABLE Account.account (
    account_id INT PRIMARY KEY IDENTITY(1,1),
    email NVARCHAR(200) NOT NULL UNIQUE,
    password NVARCHAR(MAX) NOT NULL,
    permission_id INT NOT NULL,
    FOREIGN KEY (permission_id) REFERENCES Account.permission(permission_id) ON DELETE CASCADE
);

-- Events schema
CREATE TABLE Events.partner_role (
    partner_role_id INT PRIMARY KEY IDENTITY(1,1),
    role_name NVARCHAR(100) NOT NULL
);

CREATE TABLE Events.event_type (
    event_type_id INT PRIMARY KEY IDENTITY(1,1),
    type_name NVARCHAR(100) NOT NULL
);

CREATE TABLE Events.venue (
    venue_id INT PRIMARY KEY IDENTITY(1,1),
    venue_name NVARCHAR(200) NOT NULL,
    cost DECIMAL(10, 2) NOT NULL CHECK (cost >= 0),
    address NVARCHAR(MAX) NOT NULL,
    capacity INT NOT NULL
);

CREATE TABLE Events.partner (
    partner_id INT PRIMARY KEY IDENTITY(1,1),
    partner_name NVARCHAR(200) NOT NULL,
    partner_details NVARCHAR(MAX) NULL
);

CREATE TABLE Events.event (
    event_id INT PRIMARY KEY IDENTITY(1,1),
    event_name NVARCHAR(200) NOT NULL,
    event_description NVARCHAR(MAX) NULL,
    start_time DATETIME NOT NULL,
    end_time DATETIME NOT NULL,
    account_id INT NOT NULL,
    venue_id INT NOT NULL,
    event_type_id INT NOT NULL,
    expected_attendee INT NOT NULL CHECK (expected_attendee >= 0),
    FOREIGN KEY (account_id) REFERENCES Account.account(account_id) ON DELETE CASCADE,
    FOREIGN KEY (venue_id) REFERENCES Events.venue(venue_id) ON DELETE CASCADE,
    FOREIGN KEY (event_type_id) REFERENCES Events.event_type(event_type_id) ON DELETE CASCADE
);

CREATE TABLE Events.is_partner (
    is_partner_id INT PRIMARY KEY IDENTITY(1,1),
    event_id INT NOT NULL,
    partner_id INT NOT NULL,
    partner_role_id INT NOT NULL,
    FOREIGN KEY (event_id) REFERENCES Events.event(event_id) ON DELETE CASCADE,
    FOREIGN KEY (partner_id) REFERENCES Events.partner(partner_id) ON DELETE CASCADE,
    FOREIGN KEY (partner_role_id) REFERENCES Events.partner_role(partner_role_id) ON DELETE CASCADE
);

-- Shows schema
CREATE TABLE Shows.genre (
    genre_id INT PRIMARY KEY IDENTITY(1,1),
    genre NVARCHAR(100) NOT NULL
);

CREATE TABLE Shows.performer (
    performer_id INT PRIMARY KEY IDENTITY(1,1),
    full_name NVARCHAR(200) NOT NULL,
    contact_detail NVARCHAR(MAX) NULL,
    cost DECIMAL(10, 2) NOT NULL CHECK (cost >= 0)
);

CREATE TABLE Shows.show (
    show_id INT PRIMARY KEY IDENTITY(1,1),
    show_name NVARCHAR(200) NOT NULL,
    performer_id INT NOT NULL,
    genre_id INT NOT NULL,
    event_id INT NOT NULL,
    FOREIGN KEY (performer_id) REFERENCES Shows.performer(performer_id) ON DELETE CASCADE,
    FOREIGN KEY (genre_id) REFERENCES Shows.genre(genre_id) ON DELETE CASCADE,
    FOREIGN KEY (event_id) REFERENCES Events.event(event_id) ON DELETE CASCADE
);

CREATE TABLE Shows.show_schedule (
    show_time_id INT PRIMARY KEY IDENTITY(1,1),
    start_time TIME NOT NULL,
    est_duration TIME NOT NULL,
    datetime DATE NOT NULL,
    show_id INT NOT NULL,
    FOREIGN KEY (show_id) REFERENCES Shows.show(show_id) ON DELETE CASCADE
);

-- Equipment schema
CREATE TABLE Equipment.equipment_type (
    equip_type_id INT PRIMARY KEY IDENTITY(1,1),
    type_name NVARCHAR(100) NOT NULL
);

CREATE TABLE Equipment.equipment_name (
    equip_name_id INT PRIMARY KEY IDENTITY(1,1),
    equip_name NVARCHAR(200) NOT NULL,
    equip_cost DECIMAL(10, 2) NOT NULL CHECK (equip_cost >= 0),
    equip_type_id INT NOT NULL,
    FOREIGN KEY (equip_type_id) REFERENCES Equipment.equipment_type(equip_type_id) ON DELETE CASCADE
);

CREATE TABLE Equipment.equipment (
    equipment_id INT PRIMARY KEY IDENTITY(1,1),
    equip_name_id INT NOT NULL,
    available BIT NOT NULL,
    FOREIGN KEY (equip_name_id) REFERENCES Equipment.equipment_name(equip_name_id) ON DELETE CASCADE
);

CREATE TABLE Equipment.required (
    required_id INT PRIMARY KEY IDENTITY(1,1),
    quantity INT NOT NULL CHECK (quantity > 0),
    event_id INT NOT NULL,
    equip_name_id INT NOT NULL,
    FOREIGN KEY (event_id) REFERENCES Events.event(event_id) ON DELETE CASCADE,
    FOREIGN KEY (equip_name_id) REFERENCES Equipment.equipment_name(equip_name_id) ON DELETE CASCADE
);

-- Employees schema
CREATE TABLE Employees.employee_role (
    role_id INT PRIMARY KEY IDENTITY(1,1),
    role_name NVARCHAR(100) NOT NULL,
    salary DECIMAL(10, 2) NOT NULL CHECK (salary >= 0)
);

CREATE TABLE Employees.employee (
    employee_id INT PRIMARY KEY IDENTITY(1,1),
    full_name NVARCHAR(200) NOT NULL,
    contact NVARCHAR(MAX) NOT NULL,
    role_id INT NOT NULL,
    FOREIGN KEY (role_id) REFERENCES Employees.employee_role(role_id) ON DELETE CASCADE
);

CREATE TABLE Employees.engaged (
    engaged_id INT PRIMARY KEY IDENTITY(1,1),
    event_id INT NOT NULL,
    employee_id INT NOT NULL,
    FOREIGN KEY (event_id) REFERENCES Events.event(event_id) ON DELETE CASCADE,
    FOREIGN KEY (employee_id) REFERENCES Employees.employee(employee_id) ON DELETE CASCADE
);


