# LetsPlan - Calendar Application

A modern, feature-rich calendar application built with SvelteKit and .NET, designed to provide a Google Calendar-like experience with drag-and-drop functionality, event management, and a beautiful user interface.

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![Docker](https://img.shields.io/badge/docker-ready-brightgreen.svg)
![SvelteKit](https://img.shields.io/badge/SvelteKit-5.x-orange.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Prerequisites](#prerequisites)
- [Quick Start](#quick-start)
- [Development](#development)
- [Testing](#testing)
- [Project Structure](#project-structure)
- [API Documentation](#api-documentation)

## 🎯 Overview

LetsPlan is a full-stack calendar application that allows users to create, manage, and organize events with an intuitive drag-and-drop interface. The application runs entirely in Docker containers, making it easy to deploy and run on any system with minimal configuration.

### Key Highlights

- **Zero Configuration**: Run the entire application with a single command
- **Modern UI/UX**: Built with SvelteKit 5, TailwindCSS 4, and DaisyUI
- **Drag & Drop**: Intuitive event manipulation across days and weeks
- **Responsive Design**: Works seamlessly on desktop and mobile devices
- **Dark Mode**: Built-in theme toggle for comfortable viewing
- **Type-Safe**: Full TypeScript support across the frontend
- **Automated Testing**: Comprehensive E2E tests with Playwright

## ✨ Features

### Event Management
- ✅ **Create Events**: Add new calendar events with title, description, date, and time
- ✅ **Edit Events**: Modify existing event details
- ✅ **Delete Events**: Remove events with confirmation dialog
- ✅ **Color Coding**: Customize event colors for better organization
- ✅ **Event Overlapping**: Smart layout for concurrent events

### Authentication & Account
- 🔑 **Secure Authentication**: Register and login with email or username
- 👤 **User Profiles**: View account information and custom initials avatar
- 🔒 **Account Security**: Secure password change functionality
- 🚪 **Session Management**: JWT-based authentication with secure cookies

### Calendar Navigation
- 📅 **Weekly View**: Display events in a weekly grid format
- 🗓️ **Mini Calendar**: Side panel date picker for quick navigation
- ⬅️➡️ **Week Navigation**: Move between weeks with arrow controls
- 📍 **Today Button**: Quick return to current week

### Drag & Drop
- 🖱️ **Drag Events**: Move events between time slots
- 📆 **Cross-Day Dragging**: Drag events across different days
- 📊 **Cross-Week Dragging**: Automatically navigate weeks when dragging to edges
- ⏱️ **Time Adjustment**: Visual feedback during drag operations

### User Experience
- 🌓 **Dark/Light Mode**: Toggle between themes
- 👤 **Profile Dropdown**: Access account settings and logout from the navbar
- 📱 **Responsive Design**: Optimized for all screen sizes
- ⚡ **Real-time Updates**: Instant UI updates on event changes
- 🎨 **Modern UI**: Clean, professional interface with smooth animations
- 🔔 **Toast Notifications**: User feedback for all actions

## 🏗️ Architecture

### Container Details

| Container | Technology | Port | Purpose |
|-----------|-----------|------|---------|
| **Frontend** | SvelteKit 5 + TypeScript | 5174 | User interface and client-side logic |
| **Backend** | .NET 8.0 Web API | 5000 | Business logic and data persistence |
| **Database** | MongoDB 7 | 27017 | Event data storage (non-persistent) |

## 🛠️ Tech Stack

### Frontend
- **Framework**: [SvelteKit 5](https://kit.svelte.dev/) - Modern web framework with Svelte 5
- **Language**: [TypeScript 5](https://www.typescriptlang.org/) - Type-safe JavaScript
- **Styling**: 
  - [TailwindCSS 4](https://tailwindcss.com/) - Utility-first CSS framework
  - [DaisyUI 5](https://daisyui.com/) - Component library for Tailwind
- **Date Handling**: [date-fns 4](https://date-fns.org/) - Modern date utility library
- **Calendar Component**: [Cally](https://wicky.nillia.ms/cally/) - Accessible calendar component
- **Build Tool**: [Vite 7](https://vitejs.dev/) - Fast build tool and dev server
- **Testing**: [Playwright](https://playwright.dev/) - E2E testing framework

### Backend
- **Framework**: [.NET 8.0](https://dotnet.microsoft.com/) - Modern web API framework
- **Language**: C# 12 - Type-safe, object-oriented language
- **Authentication**: [JWT](https://jwt.io/) (JSON Web Tokens) with HttpOnly Cookies
- **Database Driver**: [MongoDB.Driver 2.25](https://www.mongodb.com/docs/drivers/csharp/) - Official MongoDB C# driver
- **API Style**: RESTful API with JSON responses

### Database
- **Database**: [MongoDB 7](https://www.mongodb.com/) - NoSQL document database
- **Persistence**: Non-persistent (data resets on container restart)

### DevOps
- **Containerization**: [Docker](https://www.docker.com/) & Docker Compose
- **Deployment**: Static site deployment via `adapter-static`

## 📦 Prerequisites

Before running the application, ensure you have the following installed:

- **Docker**: Version 20.10 or higher
- **Docker Compose**: Version 2.0 or higher

To verify your installation:

```bash
docker --version
docker compose version
```

### For Development (Optional)
- **Node.js**: Version 20 or higher
- **.NET SDK**: Version 8.0 or higher
- **npm**: Version 9 or higher

## 🚀 Quick Start

### Running with Docker (Recommended)

1. **Clone or extract the project**:
   ```bash
   git clone https://github.com/CantarinoG/letsplan
   cd letsplan
   ```

2. **Start the application**:
   ```bash
   docker compose up
   ```

3. **Access the application**:
   - Open your browser and navigate to: **http://localhost:5174**
   - The backend API is available at: **http://localhost:5000**

4. **Stop the application**:
   ```bash
   # Press Ctrl+C in the terminal, then:
   docker compose down
   ```

### First-Time Setup

On the first run, Docker will:
1. Build the frontend container (install dependencies, build SvelteKit app)
2. Build the backend container (restore NuGet packages, compile .NET app)
3. Pull the MongoDB 7 image
4. Start all three containers

This process may take 2-5 minutes depending on your internet connection.

## 💻 Development

### Environment Variables

#### Frontend
The frontend connects to the backend via the API URL defined in `src/lib/api.ts`:

```typescript
const API_URL = 'http://localhost:5000/api';
```

#### Backend
MongoDB connection string is configured in `appsettings.json`:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://mongo:27017",
    "DatabaseName": "letsplan"
  }
}
```

## 🧪 Testing

The project includes comprehensive end-to-end tests using Playwright.

### Running Tests

```bash
cd frontend

# Run all tests (non-headless mode)
npx playwright test browser.spec.ts

# Run specific test
npx playwright test browser.spec.ts -g "should create a new event"
```

### Test Configuration

The Playwright configuration is defined in `frontend/playwright.config.ts`:

```typescript
const config: PlaywrightTestConfig = {
  webServer: {
    command: 'npm run build && npm run preview',
    port: 4173
  },
  testDir: 'tests',
  testMatch: /(.+.)?(test|spec).[jt]s/,
  use: {
    headless: false  // Tests run with visible browser
  }
};
```

### Test Coverage

Current test suite covers:
- ✅ Event creation
- ✅ Event editing
- ✅ Event deletion
- ✅ Drag and drop within the same week
- ✅ Drag and drop across weeks
- ✅ Calendar navigation
- ✅ Date picker functionality

### Test Reports

After running tests, view the report:

```bash
npx playwright show-report
```

## 📁 Project Structure

```
letsplan/
├── frontend/                    # SvelteKit frontend application
│   ├── src/
│   │   ├── lib/
│   │   │   ├── components/     # Svelte components
│   │   │   │   ├── CalendarGrid.svelte      # Main calendar grid
│   │   │   │   ├── CalendarHeader.svelte    # Week navigation header
│   │   │   │   ├── EventCard.svelte         # Individual event display
│   │   │   │   ├── EventModal.svelte        # Event create/edit modal
│   │   │   │   ├── MiniCalendar.svelte      # Date picker sidebar
│   │   │   │   ├── Navbar.svelte            # Top navigation bar
│   │   │   │   ├── Sidebar.svelte           # Left sidebar
│   │   │   │   ├── ThemeToggle.svelte       # Dark/light mode toggle
│   │   │   │   ├── Toast.svelte             # Notification system
│   │   │   │   └── ConfirmModal.svelte      # Confirmation dialogs
│   │   │   ├── api.ts          # API client functions
│   │   │   ├── stores.ts       # Svelte stores (state management)
│   │   │   ├── eventForm.svelte.ts  # Event form state
│   │   │   └── constants/      # Application constants
│   │   ├── routes/
│   │   │   ├── auth/
│   │   │   │   └── +page.svelte    # Login and Registration page
│   │   │   └── +page.svelte        # Main calendar page
│   │   └── app.html                # HTML template
│   ├── static/                 # Static assets (favicon, etc.)
│   ├── tests/
│   │   └── browser.spec.ts     # Playwright E2E tests
│   ├── Dockerfile              # Frontend container definition
│   ├── package.json            # Node dependencies
│   ├── playwright.config.ts    # Playwright configuration
│   ├── svelte.config.js        # SvelteKit configuration
│   ├── tailwind.config.js      # TailwindCSS configuration
│   ├── tsconfig.json           # TypeScript configuration
│   └── vite.config.ts          # Vite build configuration
│
├── backend/                     # .NET backend API
│   ├── Controllers/
│   │   ├── EventsController.cs # Event API endpoints
│   │   └── UsersController.cs  # Auth & User API endpoints
│   ├── Models/
│   │   ├── DTOs/
│   │   │   └── ChangePasswordRequest.cs # Password update payload
│   │   ├── CalendarEvent.cs    # Event data model
│   │   └── User.cs             # User data model
│   ├── Services/
│   │   ├── MongoDbService.cs   # MongoDB connection service
│   │   └── EventsService.cs    # Business logic for events
│   ├── Configurations/
│   │   └── MongoDbSettings.cs  # MongoDB configuration
│   ├── Dockerfile              # Backend container definition
│   ├── Program.cs              # Application entry point
│   ├── appsettings.json        # Application configuration
│   └── backend.csproj          # .NET project file
│
├── docker-compose.yml          # Multi-container orchestration
├── .gitignore                  # Git ignore rules
└── README.md                   # This file
```

### Key Files Explained

#### Frontend
- **`CalendarGrid.svelte`**: Main calendar component with drag-and-drop logic
- **`EventModal.svelte`**: Form for creating and editing events
- **`api.ts`**: Centralized API calls to backend
- **`stores.ts`**: Reactive state management using Svelte stores

#### Backend
- **`EventsController.cs`**: RESTful API endpoints for events
- **`UsersController.cs`**: Authentication and profile management
- **`CalendarEvent.cs`**: MongoDB document model for events
- **`User.cs`**: User account model with hashed passwords
- **`EventsService.cs`**: Business logic for event management
- **`UsersService.cs`**: User data handling and persistence
- **`AuthService.cs`**: JWT generation and password hashing
- **`MongoDbService.cs`**: Database connection and initialization

## 📡 API Documentation

### Base URL
```
http://localhost:5000/api
```

### Endpoints

#### Get All Events
```http
GET /api/events?start_date={start_date}&end_date={end_date}
```

**Query Parameters** (optional):
- `start_date` (DateTime): Filter events starting from this date
- `end_date` (DateTime): Filter events ending before this date

**Example**:
```http
GET /api/events?start_date=2026-02-10T00:00:00Z&end_date=2026-02-16T23:59:59Z
```

**Response**: `200 OK`
```json
[
  {
    "id": "507f1f77bcf86cd799439011",
    "title": "Team Meeting",
    "description": "Weekly sync with the team",
    "startAt": "2026-02-14T10:00:00Z",
    "endAt": "2026-02-14T11:00:00Z",
    "color": "#3b82f6",
    "createdAt": "2026-02-13T18:00:00Z",
    "updatedAt": "2026-02-13T18:00:00Z"
  }
]
```

#### Get Event by ID
```http
GET /api/events/{id}
```

**Path Parameters**:
- `id` (string): MongoDB ObjectId of the event

**Response**: `200 OK`
```json
{
  "id": "507f1f77bcf86cd799439011",
  "title": "Team Meeting",
  "description": "Weekly sync with the team",
  "startAt": "2026-02-14T10:00:00Z",
  "endAt": "2026-02-14T11:00:00Z",
  "color": "#3b82f6",
  "createdAt": "2026-02-13T18:00:00Z",
  "updatedAt": "2026-02-13T18:00:00Z"
}
```

**Error Response**: `404 Not Found` (if event doesn't exist)

#### Create Event
```http
POST /api/events
Content-Type: application/json

{
  "title": "New Event",
  "description": "Event description",
  "startAt": "2026-02-14T14:00:00Z",
  "endAt": "2026-02-14T15:00:00Z",
  "color": "#ef4444"
}
```

**Response**: `201 Created`
```json
{
  "id": "507f1f77bcf86cd799439012",
  "title": "New Event",
  "description": "Event description",
  "startAt": "2026-02-14T14:00:00Z",
  "endAt": "2026-02-14T15:00:00Z",
  "color": "#ef4444",
  "createdAt": "2026-02-13T19:00:00Z",
  "updatedAt": "2026-02-13T19:00:00Z"
}
```

**Headers**:
```
Location: /api/events/507f1f77bcf86cd799439012
```

**Error Response**: `400 Bad Request` (if validation fails)

#### Update Event
```http
PUT /api/events/{id}
Content-Type: application/json

{
  "title": "Updated Event",
  "description": "Updated description",
  "startAt": "2026-02-14T15:00:00Z",
  "endAt": "2026-02-14T16:00:00Z",
  "color": "#10b981"
}
```

**Path Parameters**:
- `id` (string): MongoDB ObjectId of the event to update

**Response**: `204 No Content`

**Error Responses**:
- `400 Bad Request` (if validation fails)
- `404 Not Found` (if event doesn't exist)

#### Delete Event
```http
DELETE /api/events/{id}
```

**Path Parameters**:
- `id` (string): MongoDB ObjectId of the event to delete

**Response**: `204 No Content`

**Error Response**: `404 Not Found` (if event doesn't exist)

### Data Model

#### CalendarEvent
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `id` | string | Auto-generated | MongoDB ObjectId |
| `title` | string | Yes | Event title |
| `description` | string | No | Event description |
| `startAt` | DateTime (UTC) | Yes | Event start time |
| `endAt` | DateTime (UTC) | Yes | Event end time |
| `color` | string | No | Hex color code (default: `#3b82f6`) |
| `createdAt` | DateTime (UTC) | Auto-generated | Creation timestamp |
| `updatedAt` | DateTime (UTC) | Auto-generated | Last update timestamp |

### Users API

#### Register User
```http
POST /api/users/register
Content-Type: application/json

{
  "username": "johndoe",
  "email": "john@example.com",
  "password": "securepassword123"
}
```

#### Login User
```http
POST /api/users/login
Content-Type: application/json

{
  "emailOrUsername": "johndoe",
  "password": "securepassword123"
}
```

#### Change Password
```http
POST /api/users/change-password
Authorization: Bearer <token> (or JWT Cookie)

{
  "currentPassword": "oldpassword",
  "newPassword": "newpassword123"
}
```

#### Logout
```http
POST /api/users/logout
```

---

**Built with ❤️ using SvelteKit, .NET, and MongoDB**
