

CREATE DATABASE EventManagementDB;

USE master
ALTER DATABASE EventManagementDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE EventManagementDB;



ALTER DATABASE EventManagementDB SET MULTI_USER;

USE master
USE EventManagementDB;

-- Kiểm tra xem PermissionId = 1 đã tồn tại
-- Thêm tài khoản mới
INSERT INTO Accounts.account (Email, Password, permission_id, employee_id)
VALUES ('admin', '123456', 1, NULL);


SELECT * FROM Accounts.account
UPDATE Accounts.account
SET Password = 'admin123'
WHERE Email = 'newuser@example.com';

-- Create Schemas
go
CREATE SCHEMA Events;
go
CREATE SCHEMA Shows;
go
CREATE SCHEMA Employees;
GO
--IF NOT EXISTS (SELECT schema_name FROM information_schema.schemata WHERE schema_name = 'Events');
go
CREATE SCHEMA Events;
go;

-- Events Schema
-- Table: partner_role
CREATE TABLE Events.partner_role (
    partner_role_id INT PRIMARY KEY IDENTITY(1,1),
    role_name NVARCHAR(100) NOT NULL
);

-- Table: event_type
CREATE TABLE Events.event_type (
    event_type_id INT PRIMARY KEY IDENTITY(1,1),
    type_name NVARCHAR(100) NOT NULL
);

-- Table: partner
CREATE TABLE Events.partner (
    partner_id INT PRIMARY KEY IDENTITY(1,1),
    partner_name NVARCHAR(200) NOT NULL,
    partner_description NVARCHAR(MAX) NULL -- Consistent naming
);

-- Table: event
CREATE TABLE Events.event (
    event_id INT PRIMARY KEY IDENTITY(1,1),
    event_name NVARCHAR(200) NOT NULL,
    event_type_id INT NOT NULL,
    event_description NVARCHAR(MAX) NULL,
    start_time DATETIME NOT NULL,
    end_time DATETIME NOT NULL,
    FOREIGN KEY (event_type_id) REFERENCES Events.event_type(event_type_id) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Table: is_partner
CREATE TABLE Events.is_partner (
    is_partner_id INT PRIMARY KEY IDENTITY(1,1),
    event_id INT NOT NULL,
    partner_id INT NOT NULL,
    partner_role_id INT NOT NULL,
    FOREIGN KEY (event_id) REFERENCES Events.event(event_id) ON DELETE CASCADE,
    FOREIGN KEY (partner_id) REFERENCES Events.partner(partner_id) ON DELETE CASCADE,
    FOREIGN KEY (partner_role_id) REFERENCES Events.partner_role(partner_role_id)
);

-- Shows Schema
-- Table: equipment_type
CREATE TABLE Shows.equipment_type (
    equipment_type_id INT PRIMARY KEY IDENTITY(1,1),
    equipment_type_name NVARCHAR(100) NOT NULL
);

-- Table: equipment
CREATE TABLE Shows.equipment (
    equipment_id INT PRIMARY KEY IDENTITY(1,1),
    equipment_name NVARCHAR(200) NOT NULL,
    equipment_type_id INT NOT NULL,
    available BIT NOT NULL,
    quantity INT NOT NULL DEFAULT 0, -- Add stock tracking
    FOREIGN KEY (equipment_type_id) REFERENCES Shows.equipment_type(equipment_type_id) ON DELETE CASCADE
);

-- Table: show
CREATE TABLE Shows.show (
    show_id INT PRIMARY KEY IDENTITY(1,1),
    show_name NVARCHAR(200) NOT NULL,
    show_description NVARCHAR(MAX) NULL,
    event_id INT NOT NULL,
    start_time DATETIME NOT NULL,
    end_time DATETIME NOT NULL,
    FOREIGN KEY (event_id) REFERENCES Events.event(event_id) ON DELETE CASCADE
);

-- Table: required
CREATE TABLE Shows.required (
    required_id INT PRIMARY KEY IDENTITY(1,1),
    show_id INT NOT NULL,
    equipment_id INT NOT NULL,
    quantity INT NOT NULL CHECK (quantity >= 0), -- Integrity constraint
    cost DECIMAL(10, 2) NOT NULL CHECK (cost >= 0), -- Integrity constraint
    FOREIGN KEY (show_id) REFERENCES Shows.show(show_id) ON DELETE CASCADE,
    FOREIGN KEY (equipment_id) REFERENCES Shows.equipment(equipment_id) ON DELETE CASCADE
);

-- Table: performer
CREATE TABLE Shows.performer (
    performer_id INT PRIMARY KEY IDENTITY(1,1),
    performer_name NVARCHAR(200) NOT NULL,
    genre NVARCHAR(100) NOT NULL,
    performer_contact NVARCHAR(MAX) NULL -- Consistent naming
);

-- Table: participate
CREATE TABLE Shows.participate (
    participate_id INT PRIMARY KEY IDENTITY(1,1),
    show_id INT NOT NULL,
    performer_id INT NOT NULL,
    cost DECIMAL(10, 2) NOT NULL CHECK (cost >= 0), -- Integrity constraint
    FOREIGN KEY (show_id) REFERENCES Shows.show(show_id) ON DELETE CASCADE,
    FOREIGN KEY (performer_id) REFERENCES Shows.performer(performer_id) ON DELETE CASCADE
);

-- Employees Schema
-- Table: role
CREATE TABLE Employees.role (
    role_id INT PRIMARY KEY IDENTITY(1,1),
    role_name NVARCHAR(100) NOT NULL
);

-- Table: employee
CREATE TABLE Employees.employee (
    employee_id INT PRIMARY KEY IDENTITY(1,1),
    employee_name NVARCHAR(200) NOT NULL,
    employee_contact NVARCHAR(MAX) NULL -- Consistent naming
);

-- Table: has_role
CREATE TABLE Employees.has_role (
    has_role_id INT PRIMARY KEY IDENTITY(1,1),
    employee_id INT NOT NULL,
    role_id INT NOT NULL,
    FOREIGN KEY (employee_id) REFERENCES Employees.employee(employee_id) ON DELETE CASCADE,
    FOREIGN KEY (role_id) REFERENCES Employees.role(role_id) ON DELETE CASCADE
);

-- Table: engaged
CREATE TABLE Employees.engaged (
    engaged_id INT PRIMARY KEY IDENTITY(1,1),
    show_id INT NOT NULL,
    employee_id INT NOT NULL, -- Direct reference to employee
    start_time DATETIME NOT NULL,
    end_time DATETIME NOT NULL,
    cost DECIMAL(10, 2) NOT NULL CHECK (cost >= 0), -- Integrity constraint
    FOREIGN KEY (show_id) REFERENCES Shows.show(show_id) ON DELETE CASCADE,
    FOREIGN KEY (employee_id) REFERENCES Employees.employee(employee_id) ON DELETE CASCADE
);
