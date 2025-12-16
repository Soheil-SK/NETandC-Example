# .NET SaaS - Architecture de Référence

Cette solution est une **architecture SaaS complète en .NET** qui démontre tous les concepts mentionnés dans le document `context.md`.

## 📋 Structure de la Solution

La solution est composée de **4 applications** formant un écosystème SaaS cohérent :

### 1. CoreAPI - API REST Principale
**Technologies :**
- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core 9.0
- MediatR (CQRS)
- AutoMapper
- FluentValidation
- JWT Authentication

**Architecture :** Clean Architecture avec séparation en couches :
- **Domain** : Entités, Value Objects, Interfaces
- **Application** : Commands, Queries, Handlers, DTOs, Validators, Mappings
- **Infrastructure** : EF Core, Repositories, Services
- **API** : Controllers REST

**Concepts démontrés :**
- ✅ RESTful API design
- ✅ Dependency Injection
- ✅ Async/await et programmation basée sur les tâches
- ✅ EF Core + LINQ
- ✅ Clean Architecture
- ✅ CQRS avec MediatR
- ✅ Validation avec FluentValidation
- ✅ Mapping avec AutoMapper
- ✅ Authentification JWT
- ✅ Logging structuré

### 2. WebApp - Application MVC Serveur
**Technologies :**
- ASP.NET Core MVC
- Razor Views
- Cookie-based Authentication
- HttpClient pour consommation d'API

**Architecture :**
- Consomme la Core API pour toutes les opérations métier
- Pas d'accès direct à la base de données
- Authentification par cookies

**Concepts démontrés :**
- ✅ Pattern MVC
- ✅ Server-side rendering
- ✅ Authentification & autorisation
- ✅ Consommation d'API depuis .NET
- ✅ HttpClient et gestion des sessions

### 3. AuthService - Microservice d'Authentification
**Technologies :**
- ASP.NET Core Web API
- ASP.NET Core Identity
- JWT Token Generation
- Base de données SQL dédiée

**Responsabilités :**
- Inscription et authentification des utilisateurs
- Gestion des mots de passe
- Gestion des rôles et permissions
- Génération et validation de tokens JWT

**Concepts démontrés :**
- ✅ Architecture microservices
- ✅ Frontières de sécurité
- ✅ OAuth2 / OpenID Connect flows
- ✅ Authentification basée sur tokens
- ✅ ASP.NET Core Identity

### 4. NotificationService - Microservice de Traitement en Arrière-plan
**Technologies :**
- .NET Worker Service
- BackgroundService
- Dapper pour accès aux données optimisé
- Serilog pour logging structuré

**Responsabilités :**
- Traitement de jobs en arrière-plan
- Envoi d'emails/notifications
- Consommation d'événements du domaine
- Gestion des retries et tolérance aux pannes

**Concepts démontrés :**
- ✅ Traitement en arrière-plan
- ✅ Architecture orientée événements
- ✅ Résilience et retries
- ✅ Séparation des préoccupations
- ✅ Dapper pour requêtes optimisées
- ✅ Logging structuré avec Serilog

## 🚀 Démarrage Rapide

### Prérequis
- .NET 9.0 SDK
- SQL Server (LocalDB ou SQL Server Express)
- Visual Studio 2022 ou VS Code

### Configuration

1. **Cloner le repository**
```bash
git clone <repository-url>
cd NETandC-Example
```

2. **Restaurer les packages NuGet**
```bash
dotnet restore
```

3. **Configurer les chaînes de connexion**
Modifiez les fichiers `appsettings.json` de chaque projet pour pointer vers votre instance SQL Server.

4. **Démarrer les applications**

Dans des terminaux séparés :

```bash
# Terminal 1 - AuthService
cd src/AuthService
dotnet run

# Terminal 2 - CoreAPI
cd src/CoreAPI
dotnet run

# Terminal 3 - WebApp
cd src/WebApp
dotnet run

# Terminal 4 - NotificationService
cd src/NotificationService
dotnet run
```

### URLs par défaut
- **AuthService** : `https://localhost:7002`
- **CoreAPI** : `https://localhost:7001`
- **WebApp** : `https://localhost:5001`
- **NotificationService** : Service en arrière-plan (pas d'URL HTTP)

## 📚 Concepts Étudiables

### CoreAPI
- **Clean Architecture** : Séparation Domain/Application/Infrastructure/API
- **CQRS** : Séparation Commands (écriture) et Queries (lecture) via MediatR
- **Repository Pattern** : Abstraction de l'accès aux données
- **FluentValidation** : Validation des commandes avant traitement
- **AutoMapper** : Mapping automatique entre entités et DTOs
- **JWT Authentication** : Sécurisation des endpoints

### WebApp
- **MVC Pattern** : Controllers, Views, Models
- **API Consumption** : Utilisation de HttpClient pour consommer la Core API
- **Cookie Authentication** : Authentification basée sur cookies
- **Server-Side Rendering** : Génération HTML côté serveur

### AuthService
- **Microservices** : Service dédié à l'authentification
- **ASP.NET Core Identity** : Gestion complète des utilisateurs
- **JWT Generation** : Création de tokens JWT sécurisés
- **Role Management** : Gestion des rôles et permissions

### NotificationService
- **Worker Service** : Traitement en arrière-plan
- **Dapper** : Accès aux données optimisé (alternative à EF Core)
- **Background Processing** : Traitement asynchrone
- **Retry Logic** : Gestion des erreurs et retries
- **Structured Logging** : Logging avec Serilog

## 🧪 Tests

Les tests unitaires peuvent être ajoutés dans chaque projet. La structure est prête pour l'ajout de tests avec xUnit.

## 📝 Notes Importantes

- Les bases de données sont créées automatiquement au démarrage (EnsureCreated)
- Pour la production, utilisez des migrations EF Core
- Les secrets JWT doivent être changés en production
- Les chaînes de connexion doivent être sécurisées (variables d'environnement)

## 🔒 Sécurité

- Tous les endpoints de la Core API sont protégés par JWT
- Les mots de passe sont hashés via ASP.NET Core Identity
- HTTPS est activé partout
- CORS est configuré pour le développement

## 📖 Documentation

Chaque fichier contient des commentaires XML expliquant les concepts utilisés. Consultez le code source pour une compréhension approfondie.

## 🎯 Objectifs Pédagogiques

Cette solution permet d'étudier :
1. Architecture Clean Architecture
2. Pattern CQRS avec MediatR
3. Microservices architecture
4. Authentification et autorisation
5. Traitement en arrière-plan
6. Consommation d'API
7. Patterns de design (Repository, Factory, etc.)

---

**Note** : Cette solution est conçue à des fins éducatives et de démonstration. Pour la production, des améliorations supplémentaires seraient nécessaires (migrations EF Core, tests, monitoring, etc.).

