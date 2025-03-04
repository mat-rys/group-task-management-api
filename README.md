# group_task_managment_api

TaskHub API is a task management system that allows users to register, log in, manage tasks, assign tasks to users, and manage task details.

## Project Overview

- **User authentication**: Register and log in users with JWT authentication.
- **Task management**: Add, update, assign, and delete tasks.
- **Task details management**: Add and update task details.
- **Role-based access control**: Users are assigned roles (`LEADER` or `USER`).

---

## Endpoints

### **1. User Authentication**

#### **Register a new user**
**POST** `/api/UserProfile/register`

##### Request:
```json
{
  "UserName": "john_doe",
  "Password": "securepassword"
}
```

##### Response:
```json
{
  "message": "User registered successfully"
}
```

---

#### **Login a user**
**POST** `/api/UserProfile/login`

##### Request:
```json
{
  "UserName": "john_doe",
  "Password": "securepassword"
}
```

##### Response:
```json
{
  "Token": "JWT_ACCESS_TOKEN"
}
```

---

### **2. Task Management**

#### **Get tasks assigned to a user**
**GET** `/api/tasks/{userName}`

##### Response:
```json
[
  {
    "TaskId": 1,
    "Title": "Complete documentation",
    "Description": "Write API documentation",
    "AssignedUser": "john_doe"
  }
]
```

---

#### **Create a new task** (Only for LEADER role)
**POST** `/api/tasks`

##### Request:
```json
{
  "Title": "New Task",
  "Description": "Task description",
  "Deadline": "2024-03-10"
}
```

##### Response:
```json
{
  "TaskId": 2,
  "Title": "New Task",
  "Description": "Task description",
  "Deadline": "2024-03-10"
}
```

---

#### **Assign a task to a user** (Only for LEADER role)
**PUT** `/api/tasks/{userName}/{taskId}/assign`

##### Response:
```json
{
  "message": "Task assigned successfully"
}
```

---

#### **Update a task (partial update)** (Only for LEADER role)
**PUT** `/api/tasks/{taskId}`

##### Request:
```json
{
  "Title": "Updated Task Title"
}
```

##### Response:
```json
{
  "TaskId": 1,
  "Title": "Updated Task Title"
}
```

---

#### **Remove a task from a user** (Only for LEADER role)
**DELETE** `/api/tasks/{taskId}/users/{userName}`

##### Response:
```json
{
  "message": "Task removed from user"
}
```

---

#### **Delete a task and all related details** (Only for LEADER role)
**DELETE** `/api/tasks/{taskId}`

##### Response:
```json
{
  "message": "Task deleted successfully"
}
```

---

### **3. Task Details Management**

#### **Get task details**
**GET** `/api/taskDetails/{taskId}`

##### Response:
```json
{
  "TaskId": 1,
  "Details": "Detailed information about the task"
}
```

---

#### **Create task details** (Only for LEADER role)
**POST** `/api/taskDetails/{taskId}`

##### Request:
```json
{
  "Details": "Task requires detailed documentation"
}
```

##### Response:
```json
{
  "TaskId": 1,
  "Details": "Task requires detailed documentation"
}
```

---

#### **Update task details** (Only for LEADER role)
**PUT** `/api/taskDetails/{taskDetailsId}`

##### Request:
```json
{
  "Details": "Updated task details"
}
```

##### Response:
```json
{
  "TaskId": 1,
  "Details": "Updated task details"
}
```

---

## **How to Use**

### **cURL Examples**
- **Register a user**
  ```bash
  curl -X POST "http://localhost:5000/api/UserProfile/register" -H "Content-Type: application/json" -d '{"UserName": "john_doe", "Password": "securepassword"}'
  ```

- **Login and get JWT**
  ```bash
  curl -X POST "http://localhost:5000/api/UserProfile/login" -H "Content-Type: application/json" -d '{"UserName": "john_doe", "Password": "securepassword"}'
  ```

- **Create a new task (LEADER role required)**
  ```bash
  curl -X POST "http://localhost:5000/api/tasks" -H "Content-Type: application/json" -H "Authorization: Bearer YOUR_JWT_TOKEN" -d '{"Title": "New Task", "Description": "Task details", "Deadline": "2024-03-10"}'
  ```

## **Installation and Running**

1. **Clone the repository:**
   ```bash
   git clone https://github.com/your-username/taskhub-api.git
   cd taskhub-api
   ```

2. **Build and run the project:**
   ```bash
   dotnet build
   dotnet run
   ```
---

## **Roles and Permissions**
| Role   | Permissions |
|--------|------------|
| **USER**  | Can view assigned tasks, view task details |
| **LEADER** | Can create, update, assign, and delete tasks |

---

