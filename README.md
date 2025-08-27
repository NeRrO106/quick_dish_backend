# 🍕 QuickDish – Food Ordering App - (backend)
> Backend pentru aplicația QuickDish.
> Gestionare produse, utilizatori, comenzi și autentificare cu cookie-uri.
> Construit cu ASP.NET Core, Entity Framework Core și autentificare bazată pe cookie-uri.
 
---
## 🚀 Tech Stack
- ⚡ ASP.NET Core – backend performant
- 🗄 Entity Framework Core – ORM pentru baze de date
- 🔐 Cookie Authentication – gestionare sesiuni și roluri
- 📦 Swagger/OpenAPI – documentare API
---

## 📂 Structura proiectului
```
QuickDish.API/            # Proxy pentru API .NET
├─ 📂 Controllers/       # Endpoint-uri API (Products, Users, Orders, Auth)
├─ 📂 Data/              # DbContext, Migrations
├─ 📂 Models/            # Entități (Product, User, Order)
├─ 📂 Repos/             # Conectare la baza de date
├─ 📂 Services/          # Logică business (CRUD, Cart, Orders)
├─ 📂 DTOs/              # Obiecte transfer date
├─ appsettings.json   # Configurații aplicație
└─ Startup.cs         # Configurare servicii și middleware

```
---

## ⚙️ Instalare & Rulare
1. Clonează repo
     ```bash
     git clone https://github.com/NeRrO106/quick_dish_backend.git
     cd quick_dish_backend
2. Restaurare pachete și build
     ```bash
     dotnet restore
     dotnet build
3. Rulează aplicația
   ```bash
   dotnet run
4. API va fi disponibil la http://localhost:5000 sau https://localhost:7100/

---

## ✨ Features
  - 🔑 Autentificare cu roluri (Admin, Manager, Courier, Client, Guest)
  - 📦 CRUD pentru produse, utilizatori și comenzi
  - 🛒 Coș de cumpărături cu finalizare comandă
  - 🔐 Autentificare și sesiuni prin cookie-uri
  - 📄 Documentare API prin Swagger

---
📜 License
  Distributed under the MIT License.
