# LetsPlan - Calendar Application

A modern, feature-rich calendar application built with SvelteKit and .NET, designed to provide a Google Calendar-like experience with drag-and-drop functionality, event management, and a beautiful user interface. Features a fully event-driven notification architecture powered by Apache Kafka.

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![Docker](https://img.shields.io/badge/docker-ready-brightgreen.svg)
![SvelteKit](https://img.shields.io/badge/SvelteKit-5.x-orange.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)
![Kafka](https://img.shields.io/badge/Kafka-7.6-black.svg)

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Prerequisites](#prerequisites)
- [Quick Start](#quick-start)
- [Configuration](#configuration)
- [Development](#development)
- [Testing](#testing)
- [Project Structure](#project-structure)
- [API Documentation](#api-documentation)

## 🎯 Overview

LetsPlan is a full-stack calendar application that allows users to create, manage, and organize events with an intuitive drag-and-drop interface. The application runs entirely in Docker containers, making it easy to deploy and run on any system with minimal configuration.

It uses an **event-driven architecture**: every CRUD operation on the calendar publishes a message to an **Apache Kafka** topic. A dedicated **.NET Worker microservice** consumes these messages and sends beautiful **HTML email notifications** to the user.

### Key Highlights

- **Zero Configuration**: Run the entire application with a single command
- **Modern UI/UX**: Built with SvelteKit 5, TailwindCSS 4, and DaisyUI
- **Drag & Drop**: Intuitive event manipulation across days and weeks
- **Responsive Design**: Works seamlessly on desktop and mobile devices
- **Dark Mode**: Built-in theme toggle for comfortable viewing
- **Type-Safe**: Full TypeScript support across the frontend
- **Automated Testing**: Comprehensive E2E tests with Playwright
- **Event-Driven**: Kafka-based asynchronous notification system

## ✨ Features

### Event Management
- ✅ **Create Events**: Add new calendar events with title, description, date, and time
- ✅ **Edit Events**: Modify existing event details
- ✅ **Delete Events**: Remove events with confirmation dialog
- ✅ **Color Coding**: Customize event colors for better organization
- ✅ **Event Overlapping**: Smart layout for concurrent events

### 🔔 Notifications
- 📧 **Email Notifications**: Receive a beautiful HTML email whenever you create, update, or delete an event
- 🎨 **Color-Coded Emails**: Green for created, amber for updated, red for deleted
- ⚡ **Asynchronous Delivery**: Notifications are sent without blocking the API

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

LetsPlan uses a microservices architecture connected by an Apache Kafka message broker.

```
┌─────────────┐     HTTP      ┌─────────────┐     MongoDB     ┌──────────────┐
│   Frontend  │ ────────────► │  API (.NET) │ ◄──────────────► │   MongoDB    │
│  SvelteKit  │               │    :5000    │                  │     :27017   │
└─────────────┘               └──────┬──────┘                  └──────────────┘
      :5174                          │
                               Kafka Event
                            (calendar-events)
                                     │
                               ┌─────▼──────┐     SMTP      ┌──────────────┐
                               │   Worker   │ ────────────► │  Email Inbox │
                               │   (.NET)   │               │  (Mailtrap / │
                               └────────────┘               │    Brevo)    │
                                                             └──────────────┘
```

### Container Details

| Container | Technology | Port | Purpose |
|-----------|-----------|------|---------|
| **frontend** | SvelteKit 5 + TypeScript | 5174 | User interface and client-side logic |
| **backend** | .NET 8.0 Web API | 5000 | Business logic, data persistence, Kafka producer |
| **worker** | .NET 8.0 Worker Service | — | Kafka consumer & email notification sender |
| **mongo** | MongoDB 7 | 27017 | Event & user data storage |
| **kafka** | Confluent Kafka 7.6 | 9092 | Message broker for event streaming |

## 🛠️ Tech Stack

### Frontend
- **Framework**: [SvelteKit 5](https://kit.svelte.dev/) with Svelte 5
- **Language**: [TypeScript 5](https://www.typescriptlang.org/)
- **Styling**: [TailwindCSS 4](https://tailwindcss.com/) + [DaisyUI 5](https://daisyui.com/)
- **Date Handling**: [date-fns 4](https://date-fns.org/)
- **Testing**: [Playwright](https://playwright.dev/)

### Backend API
- **Framework**: [.NET 8.0](https://dotnet.microsoft.com/) Web API
- **Language**: C# 12
- **Authentication**: JWT with HttpOnly Cookies
- **Database Driver**: [MongoDB.Driver](https://www.mongodb.com/docs/drivers/csharp/)
- **Messaging**: [Confluent.Kafka](https://github.com/confluentinc/confluent-kafka-dotnet) (Producer)

### Notification Worker
- **Type**: .NET 8.0 Worker Service (Background Service)
- **Messaging**: [Confluent.Kafka](https://github.com/confluentinc/confluent-kafka-dotnet) (Consumer)
- **Email**: [MailKit](https://github.com/jstedfast/MailKit) via SMTP

### Infrastructure
- **Database**: MongoDB 7
- **Message Broker**: Apache Kafka 7.6 (KRaft mode — no Zookeeper)
- **Containerization**: Docker & Docker Compose

## 📦 Prerequisites

- **Docker**: Version 20.10 or higher
- **Docker Compose**: Version 2.0 or higher

```bash
docker --version
docker compose version
```

### For Development (Optional)
- **Node.js**: Version 20 or higher
- **.NET SDK**: Version 8.0 or higher

## 🚀 Quick Start

### 1. Clone the project
```bash
git clone https://github.com/CantarinoG/letsplan
cd letsplan
```

### 2. Configure Email Notifications (Optional)
Copy your SMTP credentials into the `.env` file in the root directory:

```bash
# .env
SMTP_HOST=sandbox.smtp.mailtrap.io
SMTP_PORT=587
SMTP_USER=your_user_here
SMTP_PASS=your_pass_here
FROM_EMAIL=no-reply@letsplan.com
```

> **Free Options**: Use [Mailtrap](https://mailtrap.io) (sandbox testing) or [Brevo](https://www.brevo.com) (300 real emails/day free tier). If left blank, the worker will log a warning but the API will work normally.

### 3. Start the application
```bash
docker compose up
```

### 4. Access the application
- **Frontend**: http://localhost:5174
- **Backend API**: http://localhost:5000

### 5. Stop the application
```bash
docker compose down
```

## ⚙️ Configuration

### Secret Management

Sensitive credentials are managed via a **`.env` file** in the project root. This file is listed in `.gitignore` and will **never** be committed to the repository.

| Variable | Description | Example |
|---|---|---|
| `SMTP_HOST` | SMTP server hostname | `sandbox.smtp.mailtrap.io` |
| `SMTP_PORT` | SMTP server port | `587` |
| `SMTP_USER` | SMTP username | `abc123` |
| `SMTP_PASS` | SMTP password | `secret` |
| `FROM_EMAIL` | Sender address | `no-reply@letsplan.com` |

Docker Compose automatically reads this file and injects the variables into the worker container using the double-underscore (`__`) convention for .NET configuration hierarchy (e.g., `EmailSettings__SmtpHost`).

### Backend (`api/appsettings.json`)
```json
{
  "KafkaSettings": {
    "BootstrapServers": "kafka:29092",
    "EventsTopic": "calendar-events"
  }
}
```

### Worker (`worker/appsettings.json`)
```json
{
  "KafkaSettings": {
    "BootstrapServers": "kafka:29092",
    "EventsTopic": "calendar-events",
    "GroupId": "notification-group"
  },
  "EmailSettings": {
    "SmtpHost": "",
    "SmtpPort": "587",
    "SmtpUser": "",
    "SmtpPass": "",
    "FromEmail": "no-reply@letsplan.com"
  }
}
```
> ⚠️ Leave `EmailSettings` blank in `appsettings.json`. Use the `.env` file for real credentials.

## 🧪 Testing

The project includes comprehensive end-to-end tests using Playwright.

```bash
cd frontend

# Run all tests
npx playwright test browser.spec.ts

# Run specific test
npx playwright test browser.spec.ts -g "should create a new event"

# View report
npx playwright show-report
```

### Test Coverage
- ✅ Event creation, editing, deletion
- ✅ Drag and drop within the same week
- ✅ Drag and drop across weeks
- ✅ Calendar navigation & date picker

## 📁 Project Structure

```
letsplan/
├── frontend/                    # SvelteKit frontend application
│   ├── src/
│   │   ├── lib/
│   │   │   ├── components/     # Svelte components (Calendar, Modals, etc.)
│   │   │   ├── api.ts          # API client functions
│   │   │   └── stores.ts       # Svelte stores (state management)
│   │   └── routes/
│   │       ├── auth/+page.svelte    # Login & Registration page
│   │       └── +page.svelte         # Main calendar page
│   ├── tests/browser.spec.ts   # Playwright E2E tests
│   └── Dockerfile
│
├── api/                         # .NET 8.0 Web API (Kafka Producer)
│   ├── Controllers/
│   │   ├── EventsController.cs # Event CRUD endpoints
│   │   └── UsersController.cs  # Auth & User endpoints
│   ├── Models/
│   │   ├── CalendarEvent.cs    # Event data model
│   │   └── User.cs             # User data model
│   ├── Services/
│   │   ├── EventsService.cs    # Business logic + Kafka publishing
│   │   ├── AuthService.cs      # JWT & password hashing
│   │   └── UsersService.cs     # User data persistence
│   ├── Program.cs              # App entry + DI (Kafka producer singleton)
│   ├── appsettings.json
│   └── Dockerfile
│
├── worker/                      # .NET 8.0 Worker Service (Kafka Consumer)
│   ├── Services/
│   │   ├── IEmailService.cs    # Email service interface
│   │   └── EmailService.cs     # MailKit SMTP implementation
│   ├── Worker.cs               # Background Kafka consumer loop
│   ├── Program.cs              # App entry + DI
│   ├── appsettings.json
│   └── Dockerfile
│
├── docker-compose.yml           # 5-service orchestration (+ Kafka + Worker)
├── .env                         # Secret credentials (NOT committed to git)
├── .gitignore
└── README.md
```

## 📡 API Documentation

### Base URL
```
http://localhost:5000/api
```

All event endpoints require authentication (`Authorization: Bearer <token>` or JWT cookie).

### Events Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/events` | Get all events (supports `?start_date` & `?end_date`) |
| `GET` | `/api/events/{id}` | Get a single event by ID |
| `POST` | `/api/events` | Create a new event → publishes `CREATED` to Kafka |
| `PUT` | `/api/events/{id}` | Update an event → publishes `UPDATED` to Kafka |
| `DELETE` | `/api/events/{id}` | Delete an event → publishes `DELETED` to Kafka |

### Kafka Message Payload

Every mutation on an event publishes the following JSON to the `calendar-events` topic:

```json
{
  "EventId": "507f1f77bcf86cd799439011",
  "Title": "Team Meeting",
  "Action": "CREATED",
  "Email": "user@example.com"
}
```

### Users Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/users/register` | Create a new account |
| `POST` | `/api/users/login` | Login and receive JWT cookie |
| `POST` | `/api/users/logout` | Clear session cookie |
| `GET` | `/api/users/profile` | Get authenticated user profile |
| `POST` | `/api/users/change-password` | Change account password |

---

**Built with ❤️ using SvelteKit, .NET, MongoDB, and Apache Kafka**

