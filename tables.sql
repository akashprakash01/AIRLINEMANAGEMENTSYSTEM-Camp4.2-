CREATE TABLE Administrator 
(
	administratorId INT PRIMARY KEY,
	username varchar(20),
	password varchar(20)
)

CREATE TABLE Country 
(
	countryId INT PRIMARY KEY,
	countrName varchar(25),
)

CREATE TABLE City 
(
	 cityId INT PRIMARY KEY,
	cityName varchar(50),
	countryId INT
				FOREIGN KEY REFERENCES Country(countryId)
)

CREATE TABLE AirportDetails 
(
	 airportDetailsId INT PRIMARY KEY,
	airportName varchar(70),
	cityId INT
				FOREIGN KEY REFERENCES City(cityId)
)

CREATE TABLE FlightDetails 
(
	 flightDetailsId INT PRIMARY KEY,
	flightName varchar(70)
)

CREATE TABLE FlightSchedule
(
	
	flightScheduleId INT PRIMARY KEY IDENTITY(1,1),
	flightDetailsId INT
						FOREIGN KEY REFERENCES FlightDetails(flightDetailsId),
	departureAirport INT
						FOREIGN KEY REFERENCES AirportDetails(airportDetailsId),
	arrivalAirport INT
						FOREIGN KEY REFERENCES AirportDetails(airportDetailsId),
	departureDate DATETIME2(0),
	departureTime DATETIME2(0),
	arrivalDate DATETIME2(0),
	arrivalTime DATETIME2(0),
	administratorId INT
						FOREIGN KEY REFERENCES Administrator(administratorId),
	createdAt DATETIME2(0) NOT NULL DEFAULT SYSDATETIME(),
	updatedAt DATETIME2(0) NOT NULL DEFAULT SYSDATETIME()
)

EXEC sp_rename 
    'Country.countrName',
    'countryName',
    'COLUMN';

	SELECT * FROM Country;

	ALTER TABLE Country
ALTER COLUMN countryName VARCHAR(25) NOT NULL;


CREATE PROCEDURE sp_ListAllFlightSchedules
AS
BEGIN
    SELECT 
        fs.flightScheduleId,
        fd.flightName,
        dep.airportName AS DepartureAirport,
        arr.airportName AS ArrivalAirport,
        fs.departureDate,
        fs.departureTime,
        fs.arrivalDate,
        fs.arrivalTime,
        a.username AS CreatedBy,
        fs.createdAt,
        fs.updatedAt
    FROM FlightSchedule fs
    INNER JOIN FlightDetails fd 
        ON fs.flightDetailsId = fd.flightDetailsId
    INNER JOIN AirportDetails dep 
        ON fs.departureAirport = dep.airportDetailsId
    INNER JOIN AirportDetails arr 
        ON fs.arrivalAirport = arr.airportDetailsId
    INNER JOIN Administrator a
        ON fs.administratorId = a.administratorId
    ORDER BY fs.departureDate, fs.departureTime;
END;

CREATE PROCEDURE sp_SearchFlightSchedule
(
    @flightName VARCHAR(70) = NULL,
    @departureAirport VARCHAR(70) = NULL,
    @arrivalAirport VARCHAR(70) = NULL,
    @departureDate DATE = NULL
)
AS
BEGIN
    SELECT 
        fs.flightScheduleId,
        fd.flightName,
        dep.airportName AS DepartureAirport,
        arr.airportName AS ArrivalAirport,
        fs.departureDate,
        fs.departureTime,
        fs.arrivalDate,
        fs.arrivalTime
    FROM FlightSchedule fs
    INNER JOIN FlightDetails fd 
        ON fs.flightDetailsId = fd.flightDetailsId
    INNER JOIN AirportDetails dep 
        ON fs.departureAirport = dep.airportDetailsId
    INNER JOIN AirportDetails arr 
        ON fs.arrivalAirport = arr.airportDetailsId
    WHERE
        (@flightName IS NULL OR fd.flightName LIKE '%' + @flightName + '%')
        AND (@departureAirport IS NULL OR dep.airportName LIKE '%' + @departureAirport + '%')
        AND (@arrivalAirport IS NULL OR arr.airportName LIKE '%' + @arrivalAirport + '%')
        AND (@departureDate IS NULL OR fs.departureDate = @departureDate);
END;


CREATE PROCEDURE sp_AddFlightSchedule
(
    @flightDetailsId INT,
    @departureAirport INT,
    @arrivalAirport INT,
    @departureDate DATETIME2(0),
    @departureTime DATETIME2(0),
    @arrivalDate DATETIME2(0),
    @arrivalTime DATETIME2(0),
    @administratorId INT
)
AS
BEGIN
    -- Basic validation
    IF @departureAirport = @arrivalAirport
    BEGIN
        RAISERROR('Departure and Arrival airports cannot be the same', 16, 1);
        RETURN;
    END

    INSERT INTO FlightSchedule
    (
        flightDetailsId,
        departureAirport,
        arrivalAirport,
        departureDate,
        departureTime,
        arrivalDate,
        arrivalTime,
        administratorId
    )
    VALUES
    (
        @flightDetailsId,
        @departureAirport,
        @arrivalAirport,
        @departureDate,
        @departureTime,
        @arrivalDate,
        @arrivalTime,
        @administratorId
    );
END;

CREATE PROCEDURE sp_UpdateFlightSchedule
(
    @flightScheduleId INT,
    @flightDetailsId INT,
    @departureAirport INT,
    @arrivalAirport INT,
    @departureDate DATETIME2(0),
    @departureTime DATETIME2(0),
    @arrivalDate DATETIME2(0),
    @arrivalTime DATETIME2(0)
)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM FlightSchedule WHERE flightScheduleId = @flightScheduleId)
    BEGIN
        RAISERROR('Flight Schedule not found', 16, 1);
        RETURN;
    END

    UPDATE FlightSchedule
    SET
        flightDetailsId = @flightDetailsId,
        departureAirport = @departureAirport,
        arrivalAirport = @arrivalAirport,
        departureDate = @departureDate,
        departureTime = @departureTime,
        arrivalDate = @arrivalDate,
        arrivalTime = @arrivalTime,
        updatedAt = SYSDATETIME()
    WHERE flightScheduleId = @flightScheduleId;
END;


CREATE PROCEDURE sp_AdminLogin
(
    @username VARCHAR(20),
    @password VARCHAR(20)
)
AS
BEGIN
    SELECT 
        administratorId,
        username
    FROM Administrator
    WHERE username = @username
      AND password = @password;
END;

CREATE PROCEDURE sp_ListAllAirportDetails
AS
BEGIN
    SELECT
        ad.airportDetailsId,
        ad.airportName,
        c.cityName,
        co.countryName
    FROM AirportDetails ad
    INNER JOIN City c
        ON ad.cityId = c.cityId
    INNER JOIN Country co
        ON c.countryId = co.countryId
    ORDER BY co.countryName, c.cityName, ad.airportName;
END;

CREATE PROCEDURE sp_ListAllFlightDetails
AS
BEGIN
    SELECT
        flightDetailsId,
        flightName
    FROM FlightDetails
    ORDER BY flightName;
END;


INSERT INTO FlightSchedule
(
    flightDetailsId,
    departureAirport,
    arrivalAirport,
    departureDate,
    departureTime,
    arrivalDate,
    arrivalTime,
    administratorId
)
VALUES
(
    1,              
    1,             
    3,              
    '2026-02-10',  
    '2026-02-10 10:30:00',
    '2026-02-10',   
    '2026-02-10 14:15:00', 
    1              
);

select * from FlightSchedule

CREATE PROCEDURE sp_GetFlightScheduleById
(
    @flightScheduleId INT
)
AS
BEGIN
    SELECT 
        fs.flightScheduleId,
        fd.flightName,
        dep.airportName AS DepartureAirport,
        arr.airportName AS ArrivalAirport,
        fs.departureDate,
        fs.departureTime,
        fs.arrivalDate,
        fs.arrivalTime,
        a.username AS CreatedBy,
        fs.createdAt,
        fs.updatedAt
    FROM FlightSchedule fs
    INNER JOIN FlightDetails fd
        ON fs.flightDetailsId = fd.flightDetailsId
    INNER JOIN AirportDetails dep
        ON fs.departureAirport = dep.airportDetailsId
    INNER JOIN AirportDetails arr
        ON fs.arrivalAirport = arr.airportDetailsId
    INNER JOIN Administrator a
        ON fs.administratorId = a.administratorId
    WHERE fs.flightScheduleId = @flightScheduleId;
END;
select * from FlightSchedule