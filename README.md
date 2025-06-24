# AgriEnergy Connect – Prototype

AgriEnergy Connect is a web-based prototype that connects farmers and energy employees. Farmers can register and list products. Employees can manage farmer profiles and see submitted data. This document explains how to set it up and use it.

[YouTube Video](https://youtu.be/H0o7uScgHO8)
[GitHub Repository](https://github.com/VCSTDN2024/prog7311-part3-poe-Sandeep-Hari)

---

## Project Setup

### 1. System Requirements

- Windows 10 or later  
- Visual Studio 2022 or later  
- .NET 8.0 or later  
- SQLite  
- Chrome, Edge, or Firefox  

---

### 2. Installation Instructions

1. Clone the repository:
   ```bash
   git clone https://github.com/VCSTDN2024/prog7311-part-2-Sandeep-Hari
   ```

2. Open in Visual Studio:
   - Start Visual Studio.
   - Open `AgriEnergyConnect.sln`.

3. Restore NuGet Packages:
   - Visual Studio might ask you to restore packages.
   - If not, go to **Tools > NuGet Package Manager > Manage NuGet Packages for Solution** and click **Restore**.

4. Apply Migrations and Seed the Database:
   - Open **Package Manager Console**
   - Run:
     ```bash
     Update-Database
     ```

---

## Running the Application

1. Set `AgriEnergyConnect.Web` as the startup project.

2. Press **F5** or click **Start**.

3. Visit the app in your browser:
   - Default: `https://localhost:7150/`
   - You’ll land on the login or registration page.

---

## User Roles and What They Can Do

### Farmer
- Register or log in
- Add products (name, category, production date)
- See and manage their own products
- UI uses a farm-style theme with animations

#### Login Test Data for Farmer
- **Username:** Farmer.John@gmail.com
- **Password:** John_Farmer123_

### Employee
- Register or log in
- Add new farmer profiles
- Filter by Product Name, Price and etc
- View and Activate Farmer Accounts created by logged in Employee
- See all submitted products

#### Login Test Data for Employee
- **Username:** Employee.Bob@gmail.com
- **Password:** Employee_Bob123

---

## Role-Based Access

- After login:
  - Farmers go to the Farmer Dashboard
  - Employees go to the Employee Dashboard
- Users can't access pages that don’t match their role

---

## UI and UX

- Built with Bootstrap 5 and custom CSS
- Farm-style design with smooth animations
- Works well on desktop and tablets

---

## Features Summary

| Feature                  | Farmer | Employee |
|--------------------------|--------|----------|
| Register/Login           | Yes    | Yes      |
| Add Products             | Yes    | No       |
| View Own Products        | Yes    | No       |
| View Farmer Profiles     | No     | Yes      |
| View Submitted Products  | No     | Yes      |
| Role-Based Redirection   | Yes    | Yes      |

---