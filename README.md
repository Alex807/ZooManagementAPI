# 🦁 Zoo Management System API
<br>

## 🛠️ Technologies

- **Backend**: ASP.NET Core 9 (Web API)
- **Database**: MySQL 8 with Entity Framework Core
- **Authentication**: JWT tokens with role-based authorization
- **Mapping**: Mapster (DTO mapping)
- **Image Storage**: ImgBB cloud service
- **API Documentation**: Scalar
- **Frontend**: React 18 + Vite + TailwindCSS

---

## 📊 Database Schema

<table>
<tr>
<td valign="top" width="50%">

### Core Entities

- **UserAccount** – login credentials, hashed password, main role
- **UserDetails** – personal info (birthdate, gender, profile image)
- **Staff** – employee records linked to UserAccount (1:1)
- **Role** – Admin, Zookeeper, Veterinarian, Visitor
- **UserRole** – junction table for multiple roles per user
- **Category** – animal types (Mammal, Bird, etc.)
- **Enclosure** – animal habitats
- **Animal** – core entity linked to Category & Enclosure
- **FeedingSchedule** – when, what, and who feeds each animal
- **MedicalRecord** – veterinarian reports for animals
- **StaffAnimalAssignment** – junction table for staff-animal assignments

</td>
<td valign="top" width="50%">

### Relationships

| Parent        | Child             | Relationship | Delete Rule |
| ------------- | ----------------- | ------------ | ----------- |
| `UserAccount` | `UserDetails`     | One-to-One   | Cascade     |
| `UserAccount` | `Staff`           | One-to-One   | Cascade     |
| `UserAccount` | `Role`            | Many-to-Many | Cascade     |
| `Role`        | `UserAccount`     | One-to-Many  | Restrict    |
| `Category`    | `Animal`          | One-to-Many  | Cascade     |
| `Enclosure`   | `Animal`          | One-to-Many  | Set Null    |
| `Animal`      | `FeedingSchedule` | One-to-Many  | Cascade     |
| `Animal`      | `MedicalRecord`   | One-to-Many  | Cascade     |
| `Animal`      | `Staff`           | Many-to-Many | Cascade     |
| `Staff`       | `FeedingSchedule` | One-to-Many  | Set Null    |
| `Staff`       | `MedicalRecord`   | One-to-Many  | Set Null    |
</td>
</tr>
</table>

---

## 🛡 Permissions
| Role             | Permissions                 |
| ---------------- | --------------------------- |
| **Admin**        | Full access on all entities |
| **Zookeeper**    | Manage animals & feeding    |
| **Veterinarian** | Manage medical records      |
| **Visitor**      | Read-only access to animals |

---

## ⚙️ API Endpoints

### 🔐 Auth

#### JWT Implementation
- User logs in → receives JWT token
- Token contains claims: `UserId`, `Username`, `Email`, `Role`
- Protected endpoints require `[Authorize]` attribute
- Admin-only endpoints use `[Authorize(Policy = "AdminOnly")]`

| Method | Endpoint                    | Description                                      |
| ------ | --------------------------- | ------------------------------------------------ |
| POST   | `/api/auth/register`        | Register a new user                              |
| POST   | `/api/auth/login`           | Authenticate & return JWT                        |
| POST   | `/api/auth/change-password` | Change current user's password _(Auth required)_ |
| POST   | `/api/auth/refresh-token`   | Refresh JWT token _(Auth required)_              |

---

### 🧑 Users

| Method | Endpoint                                | Description                                    |
| ------ | --------------------------------------- | ---------------------------------------------- |
| GET    | `/api/users`                            | Get all users _(Admin only)_                   |
| GET    | `/api/users/{id}`                       | Get user by ID                                 |
| GET    | `/api/users/me`                         | Get current user info                          |
| GET    | `/api/users/search/username/{username}` | Get user by username _(Admin only)_            |
| GET    | `/api/users/search/email/{email}`       | Get user by email _(Admin only)_               |
| GET    | `/api/users/search/role/{roleId}`       | Get users by role _(Admin only)_               |
| PUT    | `/api/users/{id}`                       | Update user details                            |
| PUT    | `/api/users/{userId}/change-role`       | Change user's current role _(Admin only)_      |
| DELETE | `/api/users/{id}`                       | Delete user _(cascade details)_ _(Admin only)_ |
| POST   | `/api/users/{userId}/roles/{roleId}`    | Assign role to user _(Admin only)_             |
| DELETE | `/api/users/{userId}/roles/{roleId}`    | Remove role from user _(Admin only)_           |

**Query Params:** `?role=Zookeeper`, `?gender=Female`, `?search=John`

---

### 👤 User Details

| Method | Endpoint                                 | Description                                |
| ------ | ---------------------------------------- | ------------------------------------------ |
| GET    | `/api/user-details`                      | Get all user details _(Admin only)_        |
| GET    | `/api/user-details/{userId}`             | Get user details by user ID                |
| GET    | `/api/user-details/me`                   | Get current user's details                 |
| GET    | `/api/user-details/search/name`          | Search user details by name _(Admin only)_ |
| GET    | `/api/user-details/search/phone/{phone}` | Get user details by phone _(Admin only)_   |
| PUT    | `/api/user-details/{userId}`             | Update user details                        |
| DELETE | `/api/user-details/{userId}`             | Delete user details _(Admin only)_         |

**Query Params:** `?firstName=John`, `?lastName=Doe`, `?gender=Male`, `?search=John`

---

### 🐾 Animals

| Method | Endpoint                                         | Description                       |
| ------ | ------------------------------------------------ | --------------------------------- |
| GET    | `/api/animals`                                   | Get all animals                   |
| GET    | `/api/animals/{id}`                              | Get animal by ID                  |
| GET    | `/api/animals/search/by-category/{categoryId}`   | Get animals by category           |
| GET    | `/api/animals/search/by-enclosure/{enclosureId}` | Get animals by enclosure          |
| GET    | `/api/animals/search/by-specie`                  | Get animals by species            |
| GET    | `/api/animals/search/by-gender/{gender}`         | Get animals by gender             |
| GET    | `/api/animals/search/by-age-range`               | Get animals by age range          |
| GET    | `/api/animals/search/by-arrival-date`            | Get animals by arrival date range |
| POST   | `/api/animals`                                   | Create new animal                 |
| PUT    | `/api/animals/{id}`                              | Update animal info                |
| DELETE | `/api/animals/{id}`                              | Delete animal _(cascade related)_ |

**Query Params:** `?category=Mammal`, `?health=Healthy`, `?name=Leo`, `?specie=Tiger`, `?minAge=1`, `?maxAge=10`, `?from=2025-01-01`, `?to=2025-12-31`

---

### 🏷️ Categories

| Method | Endpoint                                 | Description                          |
| ------ | ---------------------------------------- | ------------------------------------ |
| GET    | `/api/categories`                        | Get all categories                   |
| GET    | `/api/categories/{id}`                   | Get category by ID                   |
| GET    | `/api/categories/search/by-name`         | Get categories by name               |
| GET    | `/api/categories/search/by-animal-count` | Get categories by animal count range |
| GET    | `/api/categories/search/empty`           | Get empty categories (no animals)    |
| POST   | `/api/categories`                        | Create new category                  |
| PUT    | `/api/categories/{id}`                   | Update category                      |
| DELETE | `/api/categories/{id}`                   | Delete category                      |

**Query Params:** `?name=Mammal`, `?search=Bird`, `?min=5`, `?max=20`

---

### 🏡 Enclosures

| Method | Endpoint                             | Description                                           |
| ------ | ------------------------------------ | ----------------------------------------------------- |
| GET    | `/api/enclosures`                    | List all enclosures                                   |
| GET    | `/api/enclosures/{id}`               | Get enclosure by ID                                   |
| GET    | `/api/enclosures/search/by-name`     | Get enclosures by name                                |
| GET    | `/api/enclosures/search/by-type`     | Get enclosures by type                                |
| GET    | `/api/enclosures/search/by-location` | Get enclosures by location                            |
| GET    | `/api/enclosures/search/by-capacity` | Get enclosures by capacity range                      |
| GET    | `/api/enclosures/search/at-capacity` | Get enclosures at full capacity                       |
| GET    | `/api/enclosures/search/available`   | Get available enclosures                              |
| GET    | `/api/enclosures/search/empty`       | Get empty enclosures                                  |
| POST   | `/api/enclosures`                    | Add new enclosure                                     |
| PUT    | `/api/enclosures/{id}`               | Update enclosure                                      |
| DELETE | `/api/enclosures/{id}`               | Delete enclosure _(set animals' enclosure_id = null)_ |

**Query Params:** `?name=Savanna`, `?type=Outdoor`, `?location=North`, `?min=10`, `?max=50`

---

### 🍽️ Feeding Schedules

| Method | Endpoint                                            | Description                         |
| ------ | --------------------------------------------------- | ----------------------------------- |
| GET    | `/api/feedingschedules`                             | Get all feeding schedules           |
| GET    | `/api/feedingschedules/{id}`                        | Get one feeding record              |
| GET    | `/api/feedingschedules/search/by-animal/{animalId}` | Get feeding schedules by animal     |
| GET    | `/api/feedingschedules/search/by-staff/{staffId}`   | Get feeding schedules by staff      |
| GET    | `/api/feedingschedules/search/by-status/{status}`   | Get feeding schedules by status     |
| GET    | `/api/feedingschedules/search/by-food-type`         | Get feeding schedules by food type  |
| GET    | `/api/feedingschedules/search/by-date-range`        | Get feeding schedules by date range |
| GET    | `/api/feedingschedules/search/today`                | Get today's feeding schedules       |
| GET    | `/api/feedingschedules/search/upcoming`             | Get upcoming feeding schedules      |
| GET    | `/api/feedingschedules/search/overdue`              | Get overdue feeding schedules       |
| POST   | `/api/feedingschedules`                             | Assign feeding to keeper            |
| PUT    | `/api/feedingschedules/{id}`                        | Update feeding record               |
| DELETE | `/api/feedingschedules/{id}`                        | Delete feeding entry                |

**Query Params:** `?animalId=3`, `?staffId=2`, `?status=Completed`, `?foodType=Meat`, `?from=2025-03-01`, `?to=2025-03-31`, `?hours=24`

---

### 🏥 Medical Records

| Method | Endpoint                                          | Description                                 |
| ------ | ------------------------------------------------- | ------------------------------------------- |
| GET    | `/api/medicalrecords`                             | List all medical records                    |
| GET    | `/api/medicalrecords/{id}`                        | Get medical record by ID                    |
| GET    | `/api/medicalrecords/search/by-animal/{animalId}` | Get medical records by animal               |
| GET    | `/api/medicalrecords/search/by-staff/{staffId}`   | Get medical records by staff (veterinarian) |
| GET    | `/api/medicalrecords/search/by-status/{status}`   | Get medical records by health status        |
| GET    | `/api/medicalrecords/search/by-date-range`        | Get medical records by date range           |
| GET    | `/api/medicalrecords/search/recent`               | Get recent medical records                  |
| POST   | `/api/medicalrecords`                             | Add new medical entry                       |
| PUT    | `/api/medicalrecords/{id}`                        | Update treatment info                       |
| DELETE | `/api/medicalrecords/{id}`                        | Delete record _(cascade)_                   |

**Query Params:** `?animalId=5`, `?staffId=7`, `?status=Under_Treatment`, `?from=2025-01-01`, `?to=2025-12-31`, `?days=7`

---

### 👨‍⚕️ Staff

| Method | Endpoint                                            | Description                  |
| ------ | --------------------------------------------------- | ---------------------------- |
| GET    | `/api/staff`                                        | Get all staff members        |
| GET    | `/api/staff/{id}`                                   | Get staff by ID              |
| GET    | `/api/staff/search/by-department`                   | Get staff by department      |
| GET    | `/api/staff/search/by-position`                     | Get staff by position        |
| GET    | `/api/staff/search/by-salary-range`                 | Get staff by salary range    |
| GET    | `/api/staff/search/by-hire-date`                    | Get staff by hire date range |
| GET    | `/api/staff/search/by-user-account/{userAccountId}` | Get staff by user account ID |
| POST   | `/api/staff`                                        | Create new staff member      |
| PUT    | `/api/staff/{id}`                                   | Update staff details         |
| DELETE | `/api/staff/{id}`                                   | Delete staff member          |

**Query Params:** `?department=Veterinary`, `?position=Zookeeper`, `?min=30000`, `?max=80000`, `?from=2020-01-01`, `?to=2025-12-31`

---

### 🔗 Staff-Animal Assignments

| Method | Endpoint                                                  | Description                       |
| ------ | --------------------------------------------------------- | --------------------------------- |
| GET    | `/api/staffanimalassignments`                             | Get all assignments               |
| GET    | `/api/staffanimalassignments/{staffId}/{animalId}`        | Get specific assignment           |
| GET    | `/api/staffanimalassignments/search/by-staff/{staffId}`   | Get assignments by staff          |
| GET    | `/api/staffanimalassignments/search/by-animal/{animalId}` | Get assignments by animal         |
| GET    | `/api/staffanimalassignments/search/with-observations`    | Get assignments with observations |
| GET    | `/api/staffanimalassignments/search/by-date-range`        | Get assignments by date range     |
| POST   | `/api/staffanimalassignments`                             | Create new assignment             |
| PUT    | `/api/staffanimalassignments/{staffId}/{animalId}`        | Update assignment                 |
| DELETE | `/api/staffanimalassignments/{staffId}/{animalId}`        | Delete assignment                 |

**Query Params:** `?staffId=2`, `?animalId=5`, `?from=2025-01-01`, `?to=2025-12-31`

---

### 📸 Image Upload

| Method | Endpoint                           | Description                                      |
| ------ | ---------------------------------- | ------------------------------------------------ |
| POST   | `/api/imageupload/upload`          | Upload single image (multipart/form-data)        |
| POST   | `/api/imageupload/upload-base64`   | Upload image as Base64 string (application/json) |
| POST   | `/api/imageupload/upload-multiple` | Upload multiple images (max 10)                  |
| POST   | `/api/imageupload/validate`        | Validate image file without uploading            |

**Notes:**

- Supported formats: jpg, jpeg, png, gif, bmp, webp
- Maximum size: 32MB per image
- Multiple upload limited to 10 images per request


