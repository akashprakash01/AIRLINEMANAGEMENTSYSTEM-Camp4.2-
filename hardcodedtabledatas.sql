INSERT INTO Administrator (administratorId, username, password)
VALUES
(1, 'arun', 'arun@123');

INSERT INTO Country (countryId, countryName)
VALUES
(1, 'India'),
(2, 'United Arab Emirates'),
(3, 'United States');


INSERT INTO City (cityId, cityName, countryId)
VALUES
(1, 'Kochi', 1),
(2, 'Trivandrum', 1),
(3, 'Dubai', 2),
(4, 'New York', 3);


INSERT INTO AirportDetails (airportDetailsId, airportName, cityId)
VALUES
(1, 'Cochin International Airport', 1),
(2, 'Trivandrum International Airport', 2),
(3, 'Dubai International Airport', 3),
(4, 'John F Kennedy International Airport', 4);

INSERT INTO FlightDetails (flightDetailsId, flightName)
VALUES
(1, 'Air India'),
(2, 'IndiGo'),
(3, 'Emirates'),
(4, 'Qatar Airways');


SELECT * FROM Administrator;
SELECT * FROM Country;
SELECT * FROM City;
SELECT * FROM AirportDetails;
SELECT * FROM FlightDetails;

exec sp_help FlightSchedule
