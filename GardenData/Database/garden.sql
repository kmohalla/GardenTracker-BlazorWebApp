-- Garden database: DDL + DML (SQL Server, including SQL Server on Linux)
-- 5 tables, 33 records

IF DB_ID('GardenDb') IS NULL CREATE DATABASE GardenDb;
GO
USE GardenDb;
GO

DROP TABLE IF EXISTS GardenBeds, Guides, Plants, Tools, Suppliers;
GO

------------------------------------------------------------
-- DDL
------------------------------------------------------------
CREATE TABLE Suppliers (             -- admin-only table
    Id    INT IDENTITY PRIMARY KEY,
    Name  NVARCHAR(100) NOT NULL,
    State NVARCHAR(50)  NOT NULL
);

CREATE TABLE Plants (                -- searchable table
    Id         INT IDENTITY PRIMARY KEY,
    Name       NVARCHAR(100) NOT NULL,
    Family     NVARCHAR(50)  NOT NULL,
    Type       NVARCHAR(20)  NOT NULL, -- Vegetable, Herb, Flower, Fruit
    Color      NVARCHAR(50)  NOT NULL,
    SupplierId INT NULL REFERENCES Suppliers(Id) ON DELETE SET NULL
);

CREATE TABLE Tools (
    Id       INT IDENTITY PRIMARY KEY,
    WeightKg DECIMAL(4,2) NOT NULL,
    Type     NVARCHAR(30) NOT NULL,  -- Trowel, Hoe, Rake, Pruner, ...
    Material NVARCHAR(30) NOT NULL,
    LengthCm INT          NOT NULL
);

CREATE TABLE Guides (
    Id         INT IDENTITY PRIMARY KEY,
    Name       NVARCHAR(100) NOT NULL,
    Author     NVARCHAR(100) NOT NULL,
    Season     NVARCHAR(30)  NOT NULL,
    Difficulty NVARCHAR(20)  NOT NULL
);

CREATE TABLE GardenBeds (
    Id          INT IDENTITY PRIMARY KEY,
    Name        NVARCHAR(100) NOT NULL,
    Status      NVARCHAR(20)  NOT NULL, -- Planned, Growing, Harvested
    PlantedDate DATE          NOT NULL,
    GuideId     INT NULL REFERENCES Guides(Id) ON DELETE SET NULL,
    PlantId     INT NULL REFERENCES Plants(Id) ON DELETE SET NULL,
    ToolId      INT NULL REFERENCES Tools(Id)  ON DELETE SET NULL
);
GO

------------------------------------------------------------
-- DML
------------------------------------------------------------
INSERT INTO Suppliers (Name, State) VALUES
('Green Thumb Seeds',    'Vermont'),
('Sunny Acres Nursery',  'Ohio'),
('Heirloom Harvest Co.', 'Oregon'),
('Bloom & Burrow',       'North Carolina'),
('Prairie Root Farms',   'Iowa');

INSERT INTO Plants (Name, Family, Type, Color, SupplierId) VALUES
('Cherokee Purple Tomato', 'Nightshade', 'Vegetable', 'Dusky Purple', 3),
('Sungold Tomato',         'Nightshade', 'Vegetable', 'Orange',       1),
('Genovese Basil',         'Mint',       'Herb',      'Green',        1),
('Lavender Hidcote',       'Mint',       'Herb',      'Purple',       4),
('Mammoth Sunflower',      'Aster',      'Flower',    'Yellow',       5),
('Zinnia State Fair',      'Aster',      'Flower',    'Red',          2),
('Sugar Snap Pea',         'Legume',     'Vegetable', 'Green',        3),
('Everbearing Strawberry', 'Rose',       'Fruit',     'Red',          2);

INSERT INTO Tools (WeightKg, Type, Material, LengthCm) VALUES
(0.25, 'Trowel',         'Stainless Steel', 30),
(0.20, 'Transplanter',   'Aluminum',        30),
(1.40, 'Hoe',            'Steel/Ash',      150),
(1.10, 'Rake',           'Steel/Ash',      160),
(0.30, 'Pruner',         'Carbon Steel',    20),
(2.00, 'Spade',          'Steel/Fiberglass',105),
(0.90, 'Watering Can',   'Galvanized Steel',45),
(0.35, 'Hand Cultivator','Cast Aluminum',   28);

INSERT INTO Guides (Name, Author, Season, Difficulty) VALUES
('Tomatoes from Seed',       'Rosa Delgado',  'Spring', 'Intermediate'),
('Herbs on the Patio',       'Liam Chen',     'Spring', 'Beginner'),
('Cut-Flower Border',        'Grace Mbeki',   'Summer', 'Intermediate'),
('Early Peas and Greens',    'Owen Fraser',   'Spring', 'Beginner'),
('Strawberry Bed Renewal',   'Rosa Delgado',  'Fall',   'Advanced'),
('Pollinator Patch',         'Hannah Weiss',  'Summer', 'Beginner');

INSERT INTO GardenBeds (Name, Status, PlantedDate, GuideId, PlantId, ToolId) VALUES
('Back Fence Tomatoes',  'Harvested', '2026-05-10', 1, 1, 6),
('Kitchen Window Herbs', 'Growing',   '2026-04-22', 2, 3, 1),
('Front Walk Flowers',   'Harvested', '2026-05-28', 3, 6, 8),
('Raised Bed Peas',      'Planned',   '2027-03-20', 4, 7, 3),
('Strawberry Patch',     'Growing',   '2026-09-15', 5, 8, 2),
('Sunflower Wall',       'Planned',   '2027-05-15', 6, 5, 4);
GO
