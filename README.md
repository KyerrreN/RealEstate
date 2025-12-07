# RealEstate

## Overview
This web application server a purpose of managing real estate and communicating regarding selling or renting estates

## Components

- DB: PostgreSQL, MongoDB, Redis (for caching)
- backend: 3 microservices - RealEstateService, ChatService, NotificationService
- frotnend: SPA application written using React

## Directories
- ChatService: directory for ChatService
- Frontend: folder with SPA
- RealEstate: directory for RealEstateService
- NotificationService: self-explanatory

## Usage
1) Inside a root folder, there is docker-compose.yml and .env.example. You must create .env file and fill in all the properties for an application to work properly.
2) If you wish to backup or restore a database, use restore.ps1 or backup.ps1 files, execute them inside PowerShell
3) To boot up an application, simply go to a root folder and run ```docker compose up --build```

Your application will be ready at http://localhost:5173
