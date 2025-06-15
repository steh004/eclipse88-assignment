# CarParkFinder.API

## Overview

CarParkFinder.API is a .NET 8 Web API that returns the closest available car parks based on the user’s latitude and longitude. It uses static car park location data (SVY21 format) and live car park availability data from the [Data.gov.sg API](https://data.gov.sg/dataset/carpark-availability).

---

## Features

- Returns nearby car parks with real-time availability.
- Converts SVY21 coordinates to WGS84 (latitude/longitude).
- Persists static car park data into PostgreSQL.
- Dockerized environment for easy setup and testing.

---

## Getting Started

### Prerequisites

- [Docker](https://www.docker.com/)
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)

---

### Run Using Docker Compose
1. Ensure docket desktop has been opened in the background for Windows & macOS
```bash
docker-compose up --build
```
2. The API is hosted under http://localhost:5000

---

## API Ussage

### `GET /carparks/nearest`
- **Description:** Retrieves the closest car parks to a user, together with each parking lot’s availability.
- **Query Parameters:**
  - `lattitude`: (required) — Latitude of user's current location
  - `longitude`: (required) — Longitude of user's current location
  - `page`: (optional) — The current page of results. Defaults to 1 if not provided.
  - `per_page`: (optional) — The number of results returned per page. Defaults to 10.
  
- **Sample URL and CURL request with localhost:5000** 
 ```bash
 curl "http://localhost:5000/carparks/nearest?latitude=1.3079&longitude=103.8541&page=2&per_page=5"
 ```
 
- **Response:**  
```json
 [
    {
        "address": "BLK 29/30 KELANTAN LANE",
        "latitude": 1.3126856620262441,
        "longitude": 103.85679299063978,
        "totalLots": 56,
        "availableLots": 2
    },
    {
        "address": "BLK 31 KELANTAN LANE MSCP",
        "latitude": 1.3131688803661135,
        "longitude": 103.85680055014925,
        "totalLots": 345,
        "availableLots": 253
    },
    {
        "address": "BLK 665 BUFFALO ROAD BASEMENT CAR PARK",
        "latitude": 1.3129685780542584,
        "longitude": 103.85058569248744,
        "totalLots": 170,
        "availableLots": 145
    },
    {
        "address": "BLK 638 VEERASAMY ROAD",
        "latitude": 1.3141121463123702,
        "longitude": 103.85535216740251,
        "totalLots": 151,
        "availableLots": 62
    },
    {
        "address": "BLK 665 BUFFALO ROAD LOADING/UNLOADING BAYS",
        "latitude": 1.3134987238773097,
        "longitude": 103.85070720521064,
        "totalLots": 10,
        "availableLots": 4
    }
]
```

## Swagger UI

1. When the program is running, may access Swagger in your browser by passing this URL:
```bash
http://localhost:5000/swagger
```
2. The GET /carparks/nearest API can be testing by filling in the inputs in the swagger URL

3. Some sample inputs 
```text
Orchard Road
latitude=1.3048
longitude=103.8318

Tampines MRT
latitude=1.3530
longitude=103.9455

Jurong East
latitude=1.3331
longitude=103.7422

Woodlands
latitude=1.4360
longitude=103.7865

Changi Airport
latitude=1.3644
longitude=103.9915
```

### 
---

## Unit Testing

### 1. Redirect to CarParkFinder.Tests directory
```bash
cd CarParkFinder.Tests
```

### 2. Run the commands below to trigger the test and report
```bash
dotnet clean
rm -r TestResults/  # run this if within the CarParkFinder.Tests directory has this folder
dotnet test --collect:"XPlat Code Coverage" --settings ..\coverlet.runsettings
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html # to generate the test report including coverage

```

### 3. Open the report
- In the same directory, go to coverage-report folder.
- Open the index.html file in your browser to view the report.


---

## Notes
- Coordinate Conversion: SVY21 data was converted to WGS84 to enable distance calculations with user-provided latitude/longitude.
- Docker Usage: Chose Docker Compose to ensure reviewers can run the API and PostgreSQL easily without manual setup.
- Live API Dependency: Using real-time availability from Data.gov.sg introduces a dependency but simulates realistic scenarios.
- Performance: For simplicity, car park distances are calculated in-memory. In production, geospatial indexes or spatial queries should be considered.
- Solutions and guidance in the asssignment completion was assisted by querying questions and concepts regarding the assignment from ChatGPT.