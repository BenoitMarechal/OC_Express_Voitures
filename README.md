# OC_Express_Voiture

## Description

This is Project n°5 from OpenClassRooms.
For educationnal purpose only.
OC_Express_Voiture is a .NET application that allows to store vehicles, add repairs to them, and mark them as sold, available or available soon. 
The price of the vehicle is calculated automatically.

## Table of Contents

- [Installation](#installation)
- [Versions](#version)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Installation

To install and run this application, follow these steps:

1. Clone the repository:
    ```bash
    git clone https://github.com/BenoitMarechal/OC_Express_Voitures
    ```

2. Navigate to the project directory:
    ```bash
    cd MyDotNetApp
    ```

3. Install the necessary dependencies:
    ```bash
    dotnet restore
    ```

4. Build the project:
    ```bash
    dotnet build
    ```

5. Run the application:
    ```bash
    dotnet run --urls http://localhost:5000
    ```

## Versions
Developped on :
- Visual Studio 4.8
- .NET SDK 8.0.200

## Usage
- Access the application by navigating to `http://localhost:5000` in your web browser, or launch from your IDE in debug mode.

##### Populating select menus
- Possible choices for vehicle creation select menus are defined in `Utils/Constants` 
##### Statuses
- There are three possile statuses for a vehicle: "In Stock", "Available soon", and "Sold". Status is determined by a combination of boolean "IsAvailable" and "SaleDate" as coded in `Utils/StatusHelper.cs` 
##### Prices
- Fix margin (currently 500€) is set in `Utils/Constants`
- Retail Price is equivalent to Purchase price + cost of all repairs + Fixed margin as coded in `Utils/RetailPriceCalculator`


## License

This project does not have any license.

## Contact

If you have any questions or suggestions, please feel free to reach out:
- [Benoit Maréchal](mailto:benoit.marechal56@gmail.com)
- [GitHub Issues](https://github.com/BenoitMarechal/OC_Express_Voitures/issues)

