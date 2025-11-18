# 🦁 Zoo Management System API

> Full-stack project built with **C# (.NET 9)**, **MySQL**, and **React framework**.  
> Designed to manage a zoo’s data: animals, enclosures, feeding, staff, and user access.

---

## 🧱 Tech Stack

**Backend:** ASP.NET Core 9 (Web API)  
**Frontend:** React 18 + Vite  
**Database:** MySQL 8  
**ORM:** Entity Framework Core  
**Auth:** JWT + Role-based Access Control (RBAC)  
**Tooling:** Scalar, Postman, ImgBB, Mapster <br>
**DbCaching:** Delta (https://github.com/SimonCropp/Delta?tab=readme-ov-file)

---

## 📘 Overview

The **Zoo Management System** is a RESTful API allowing admins and staff to manage:
- Animals and their enclosures
- Feeding schedules and medical records
- Users, authentication, and role management

The React frontend consumes these APIs for a clean dashboard experience.

---

## 🧠 Frontend (React + Vite)

**Key Features:**
- Login & register forms (JWT-based)
- Dashboard with protected routes
- Data tables for Animals, Users, Enclosures
- CRUD operations integrated via Axios
- Dynamic filters and search with query params
- Role-based access control (e.g., Admin-only pages)
- Responsive design using TailwindCSS

---

## 🛡 Roles & Permissions
| Role             | Permissions                 |
| ---------------- | --------------------------- |
| **Admin**        | Full CRUD on all entities   |
| **Zookeeper**    | Manage animals & feeding    |
| **Veterinarian** | Manage medical records      |
| **Visitor**      | Read-only access to animals |

## 🧩 Database Design (Summary)

- **UserAccount** – login credentials, hashed password, main role  
- **UserDetails** – personal info (birthdate, gender, profile image)  
- **Role** – Admin, Zookeeper, Veterinarian, Visitor  
- **UserRole** – junction table for multiple roles per user  
- **Category** – animal types (Mammal, Bird, etc.)  
- **Enclosure** – animal habitats  
- **Animal** – core entity linked to Category & Enclosure  
- **FeedingSchedule** – when, what, and who feeds each animal  
- **MedicalRecord** – veterinarian reports for animals  

---

## ⚙️ API Endpoints

### 🔐 Auth
| Method | Endpoint | Description |
|--------|-----------|-------------|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Authenticate & return JWT |
| GET | `/api/auth/profile` | Get current user info |

---

### 🧑 Users
| Method | Endpoint | Description |
|--------|-----------|-------------|
| GET | `/api/users` | Get all users *(Admin only)* |
| GET | `/api/users/{id}` | Get user by ID |
| PUT | `/api/users/{id}` | Update user details |
| DELETE | `/api/users/{id}` | Delete user *(cascade details)* |

**Query Params:** `?role=Zookeeper`, `?gender=Female`, `?search=John`

---

### 🐾 Animals
| Method | Endpoint | Description |
|--------|-----------|-------------|
| GET | `/api/animals` | Get all animals |
| GET | `/api/animals/{id}` | Get animal by ID |
| POST | `/api/animals` | Create new animal |
| PUT | `/api/animals/{id}` | Update animal info |
| DELETE | `/api/animals/{id}` | Delete animal *(cascade related)* |

**Query Params:**  
`?category=Mammal`, `?health=Healthy`, `?name=Leo`

---

### 🏡 Enclosures
| Method | Endpoint | Description |
|--------|-----------|-------------|
| GET | `/api/enclosures` | List all enclosures |
| POST | `/api/enclosures` | Add new enclosure |
| PUT | `/api/enclosures/{id}` | Update enclosure |
| DELETE | `/api/enclosures/{id}` | Delete enclosure *(set animals’ enclosure_id = null)* |

---

### 🍽 Feeding Schedules
| Method | Endpoint | Description |
|--------|-----------|-------------|
| GET | `/api/feedings` | Get all feeding schedules |
| GET | `/api/feedings/{id}` | Get one feeding record |
| POST | `/api/feedings` | Assign feeding to keeper |
| PUT | `/api/feedings/{id}` | Update feeding record |
| DELETE | `/api/feedings/{id}` | Delete feeding entry |

**Query Params:** `?animalId=3`, `?keeperId=2`, `?date=2025-03-01`

---

### 🏥 Medical Records
| Method | Endpoint | Description |
|--------|-----------|-------------|
| GET | `/api/medical` | List medical records |
| POST | `/api/medical` | Add new medical entry |
| PUT | `/api/medical/{id}` | Update treatment info |
| DELETE | `/api/medical/{id}` | Delete record *(cascade)* |

**Query Params:** `?animalId=5`, `?vetId=7`, `?status=Under_Treatment`

---

## 🔗 Relationships Summary

| Parent | Child | Rule |
|---------|--------|------|
| `UserAccount` | `UserDetails` | cascade |
| `UserAccount` | `UserRole` | cascade |
| `Role` | `UserRole` | cascade |
| `Category` | `Animal` | cascade |
| `Enclosure` | `Animal` | set null |
| `Animal` | `FeedingSchedule` | cascade |
| `Animal` | `MedicalRecord` | cascade |


