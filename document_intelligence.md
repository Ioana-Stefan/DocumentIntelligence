# Document Intelligence Platform -- Full Architecture & Detailed Development Plan

## 1. High-Level Overview

This document provides a complete architecture and detailed step-by-step
development roadmap for building a Document Intelligence Platform
using: - **Angular 18 (Frontend)** - **ASP.NET Core 8/9 (Backend)** -
**PostgreSQL (Database)** - **JWT Authentication (Access + Refresh
Tokens)** - **Background Processing (OCR, Translation, Export)** -
**File Storage (Local/S3/Blob)** - **Clean Architecture**

------------------------------------------------------------------------

# 2. System Architecture

## 2.1 Layered Solution Structure (Clean Architecture)

    Solution/
     ├── Domain/                  // Entities, Value Objects, Interfaces
     ├── Application/             // Use cases, Services, DTOs, Validators
     ├── Infrastructure/          // EF Core, PostgreSQL, Repositories, Storage
     ├── WebApi/                  // Controllers, Auth, Background jobs
     └── ClientApp/               // Angular Frontend

------------------------------------------------------------------------

# 3. Database Schema (ERD)

## 3.1 Entities

### Users

-   Id
-   Email
-   PasswordHash
-   Role (User/Admin)
-   CreatedAt

### Documents

-   Id
-   UserId
-   FileName
-   OriginalFilePath
-   Status (Uploaded / Processing / Completed / Failed)
-   LanguageDetected
-   CreatedAt

### DocumentPages

-   Id
-   DocumentId
-   PageNumber
-   ImagePath
-   TextExtracted
-   CreatedAt

### Annotations

-   Id
-   DocumentPageId
-   X, Y, Width, Height
-   Label
-   Comment
-   CreatedAt

### ProcessingJobs

-   Id
-   DocumentId
-   JobType (OCR / Translation / Export)
-   Status (Pending / Running / Completed / Failed)
-   CreatedAt

------------------------------------------------------------------------

# 4. Backend Technologies

-   ASP.NET Core Web API
-   EF Core + Npgsql provider
-   ASP.NET Identity
-   JWT Authentication
-   Hangfire or Quartz.NET for jobs
-   Tesseract OCR (local)
-   DeepL/Google/Azure Translate API

------------------------------------------------------------------------

# 5. Angular Frontend Architecture

## Core frontend modules:

-   Auth module
-   Dashboard module
-   Document viewer module
-   Annotation tools
-   Upload module
-   Settings module
-   Admin module

## Key Angular features:

-   Standalone components
-   Angular Signals
-   Http Interceptors
-   Lazy-loaded routes
-   Reactive Forms
-   File upload with progress
-   Document viewer canvas
-   Real-time status updates via SignalR or polling

------------------------------------------------------------------------

# 6. Detailed Implementation Phases

------------------------------------------------------------------------

# Phase 1 --- Solution Setup

### Step 1.1 -- Create .NET solution

    dotnet new sln -n DocumentIntelligence

### Step 1.2 -- Create backend projects

    dotnet new classlib -n Domain
    dotnet new classlib -n Application
    dotnet new classlib -n Infrastructure
    dotnet new webapi -n WebApi

### Step 1.3 -- Add references

    dotnet add Application reference Domain
    dotnet add Infrastructure reference Application
    dotnet add WebApi reference Application
    dotnet add WebApi reference Infrastructure

### Step 1.4 -- Install EF Core with PostgreSQL

    dotnet add Infrastructure package Npgsql.EntityFrameworkCore.PostgreSQL
    dotnet add Infrastructure package Microsoft.EntityFrameworkCore.Design

### Step 1.5 -- Create Angular project

    ng new ClientApp --standalone --routing --style=scss

### Step 1.6 -- Setup Angular Proxy

Create `proxy.conf.json`:

``` json
{
  "/api": {
    "target": "https://localhost:5001",
    "secure": false,
    "changeOrigin": true
  }
}
```

------------------------------------------------------------------------

# Phase 2 --- Authentication

### Step 2.1 -- Add Identity to WebApi

-   Configure PostgreSQL Identity store
-   Add password hashing
-   Add user roles

### Step 2.2 -- Implement JWT Access + Refresh Tokens

-   Access token: 5--15 minutes
-   Refresh token: 7--30 days (HttpOnly cookie)

### Step 2.3 -- Expose Auth Endpoints

    POST /api/auth/register
    POST /api/auth/login
    POST /api/auth/refresh
    POST /api/auth/logout

### Step 2.4 -- Angular Auth Pages

-   Login form
-   Register form
-   Forgot password
-   Token refresh via HttpInterceptor

------------------------------------------------------------------------

# Phase 3 --- Document Upload

### Step 3.1 -- File Upload API Endpoint

    POST /api/documents/upload

### Step 3.2 -- Save files under:

    /Storage/Users/<userId>/Documents/<docId>/

### Step 3.3 -- Insert Document entity into DB

-   Set status = "Uploaded"

### Step 3.4 -- Angular Upload Component

-   Drag-and-drop upload
-   Progress bar
-   Store document ID returned from server

------------------------------------------------------------------------

# Phase 4 --- OCR Pipeline

### Step 4.1 -- Create OCR Job

Insert new row in `ProcessingJobs`.

### Step 4.2 -- Queue job using Hangfire

    BackgroundJob.Enqueue(() => RunOcr(documentId));

### Step 4.3 -- Execute OCR

-   Split PDF → images
-   For each page:
    -   Run Tesseract
    -   Save text to `DocumentPages`

### Step 4.4 -- Update Document Status

-   "Processing" → "Completed" or "Failed"

### Step 4.5 -- Angular UI updates

-   Polling or SignalR
-   Show "Document ready"

------------------------------------------------------------------------

# Phase 5 --- Document Viewer & Annotation Tools

### Step 5.1 -- Angular Page Viewer

-   Page thumbnail list
-   Canvas overlay
-   Text overlay

### Step 5.2 -- Implement Annotation Drawing

-   Click & drag to draw a rectangle
-   Add label/comment
-   Save via:

```{=html}
<!-- -->
```
    POST /api/annotations

### Step 5.3 -- Load annotations with Document Page

    GET /api/documents/{id}/pages

------------------------------------------------------------------------

# Phase 6 --- Translation Pipeline

### Step 6.1 -- Translation endpoint

    POST /api/documents/{id}/translate

### Step 6.2 -- Background job: Translate each page

-   Save translated text in new column (e.g., TextTranslated)

### Step 6.3 -- Angular UI

-   Dropdown: select target language
-   Show translated text next to original

------------------------------------------------------------------------

# Phase 7 --- Exporting

### Step 7.1 -- Implement Export Service

Exports supported: - PDF (QuestPDF/iText) - Word (OpenXML) - JSON - CSV
(for structured extraction)

### Step 7.2 -- Angular export button

    GET /api/documents/{id}/export?format=pdf

------------------------------------------------------------------------

# Phase 8 --- Admin & User Management

### Step 8.1 -- Admin roles & permissions

-   Manage users
-   View system metrics
-   Delete documents
-   View processing statistics

### Step 8.2 -- Angular Admin Dashboard

-   Charts (documents per user, job processing times)
-   User table with pagination

------------------------------------------------------------------------

# 7. Deployment Strategy

### Option A --- Monolith deployment

-   Angular built to `/WebApi/wwwroot`
-   Single Docker image
-   PostgreSQL separate container

### Option B --- Split deployment

-   Angular static hosting + CDN
-   .NET API separate
-   PostgreSQL separate
-   Hangfire server separate

------------------------------------------------------------------------

# End of Document
