# Architecture Technique - Guide d'Étude

Ce document explique l'architecture de la solution et comment étudier chaque concept.

## 🏗️ Vue d'Ensemble de l'Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    Écosystème SaaS .NET                    │
└─────────────────────────────────────────────────────────────┘

┌──────────────┐      ┌──────────────┐      ┌──────────────┐
│   WebApp     │──────▶│   CoreAPI    │◀─────│ AuthService  │
│   (MVC)      │ HTTP │   (REST)     │ JWT  │ (Identity)   │
└──────────────┘      └──────────────┘      └──────────────┘
                              │
                              │ Events
                              ▼
                    ┌──────────────┐
                    │Notification  │
                    │   Service    │
                    │  (Worker)    │
                    └──────────────┘
```

## 📚 Guide d'Étude par Concept

### 1. Clean Architecture (CoreAPI)

**Fichiers à étudier :**
- `src/CoreAPI/Domain/Entities/Product.cs` - Entité du domaine avec règles métier
- `src/CoreAPI/Domain/Interfaces/IProductRepository.cs` - Interface du repository
- `src/CoreAPI/Application/Commands/` - Commandes CQRS
- `src/CoreAPI/Application/Queries/` - Queries CQRS
- `src/CoreAPI/Application/Handlers/` - Handlers MediatR
- `src/CoreAPI/Infrastructure/Repositories/ProductRepository.cs` - Implémentation EF Core

**Concepts clés :**
- **Dependency Inversion** : Les couches supérieures dépendent d'abstractions
- **Séparation des responsabilités** : Chaque couche a un rôle précis
- **Testabilité** : Les dépendances sont injectées

### 2. CQRS avec MediatR (CoreAPI)

**Fichiers à étudier :**
- `src/CoreAPI/Application/Commands/CreateProductCommand.cs` - Commande
- `src/CoreAPI/Application/Handlers/CreateProductCommandHandler.cs` - Handler
- `src/CoreAPI/Application/Queries/GetProductByIdQuery.cs` - Query
- `src/CoreAPI/Application/Handlers/GetProductByIdQueryHandler.cs` - Handler Query

**Concepts clés :**
- **Séparation Commands/Queries** : Écriture vs Lecture
- **MediatR** : Pattern Mediator pour découpler les contrôleurs des handlers
- **Single Responsibility** : Un handler = une responsabilité

### 3. FluentValidation (CoreAPI)

**Fichiers à étudier :**
- `src/CoreAPI/Application/Validators/CreateProductCommandValidator.cs`

**Concepts clés :**
- Validation déclarative avec règles fluides
- Validation automatique via pipeline MediatR
- Messages d'erreur personnalisés

### 4. AutoMapper (CoreAPI)

**Fichiers à étudier :**
- `src/CoreAPI/Application/Mappings/ProductMappingProfile.cs`
- `src/CoreAPI/Application/Handlers/GetProductByIdQueryHandler.cs` (ligne avec `_mapper.Map`)

**Concepts clés :**
- Mapping automatique entre entités et DTOs
- Configuration centralisée
- Réduction du code boilerplate

### 5. Entity Framework Core (CoreAPI)

**Fichiers à étudier :**
- `src/CoreAPI/Infrastructure/Data/ApplicationDbContext.cs` - Configuration EF Core
- `src/CoreAPI/Infrastructure/Repositories/ProductRepository.cs` - Utilisation de EF Core

**Concepts clés :**
- Code First avec migrations
- LINQ pour requêtes
- Tracking et changement tracking
- Async/await pour opérations DB

### 6. JWT Authentication (CoreAPI + AuthService)

**Fichiers à étudier :**
- `src/AuthService/Services/TokenService.cs` - Génération de tokens
- `src/AuthService/Controllers/AuthController.cs` - Endpoints d'authentification
- `src/CoreAPI/Program.cs` - Configuration JWT Bearer

**Concepts clés :**
- Génération de tokens JWT
- Validation de tokens
- Claims et rôles
- Sécurisation des endpoints

### 7. ASP.NET Core Identity (AuthService)

**Fichiers à étudier :**
- `src/AuthService/Data/ApplicationDbContext.cs` - DbContext avec Identity
- `src/AuthService/Program.cs` - Configuration Identity
- `src/AuthService/Controllers/AuthController.cs` - Utilisation de UserManager

**Concepts clés :**
- Gestion des utilisateurs
- Hashage de mots de passe
- Gestion des rôles
- Claims et permissions

### 8. Microservices Architecture

**Fichiers à étudier :**
- Structure complète de `AuthService` - Service indépendant
- Structure complète de `NotificationService` - Service indépendant
- Communication entre services via HTTP/JWT

**Concepts clés :**
- Services indépendants
- Bases de données dédiées
- Communication via APIs REST
- Déploiement indépendant

### 9. MVC Pattern (WebApp)

**Fichiers à étudier :**
- `src/WebApp/Controllers/ProductsController.cs` - Contrôleur MVC
- `src/WebApp/Models/` - ViewModels
- `src/WebApp/Services/CoreApiService.cs` - Consommation d'API

**Concepts clés :**
- Séparation Controller/View/Model
- Server-side rendering
- Consommation d'API externe
- Gestion des sessions

### 10. Worker Service (NotificationService)

**Fichiers à étudier :**
- `src/NotificationService/Workers/NotificationWorker.cs` - BackgroundService
- `src/NotificationService/Program.cs` - Configuration du Worker

**Concepts clés :**
- Traitement en arrière-plan
- Exécution continue
- Gestion du cycle de vie
- Cancellation tokens

### 11. Dapper (NotificationService)

**Fichiers à étudier :**
- `src/NotificationService/Services/NotificationService.cs` - Utilisation de Dapper

**Concepts clés :**
- Requêtes SQL directes
- Mapping objet-relationnel léger
- Performance optimisée
- Alternative à EF Core pour requêtes complexes

### 12. Structured Logging (NotificationService)

**Fichiers à étudier :**
- `src/NotificationService/Program.cs` - Configuration Serilog
- Utilisation de `ILogger<T>` dans tous les services

**Concepts clés :**
- Logging structuré avec Serilog
- Niveaux de log
- Contextualisation des logs
- Intégration avec différents sinks

## 🔍 Parcours d'Apprentissage Recommandé

### Niveau 1 : Fondamentaux
1. Étudier l'entité `Product` (Domain)
2. Comprendre le Repository Pattern
3. Explorer les DTOs et le mapping

### Niveau 2 : Patterns Avancés
1. Comprendre CQRS avec MediatR
2. Étudier FluentValidation
3. Explorer AutoMapper

### Niveau 3 : Architecture
1. Comprendre Clean Architecture
2. Étudier la séparation des couches
3. Explorer Dependency Injection

### Niveau 4 : Services
1. Comprendre l'authentification JWT
2. Explorer ASP.NET Core Identity
3. Étudier les microservices

### Niveau 5 : Traitement Asynchrone
1. Comprendre Worker Services
2. Explorer Dapper
3. Étudier le logging structuré

## 🧪 Exercices Pratiques

1. **Ajouter une nouvelle entité** : Créer une entité `Order` avec toutes les couches
2. **Ajouter un nouveau endpoint** : Créer un endpoint pour une nouvelle fonctionnalité
3. **Ajouter une validation** : Créer un validateur pour une nouvelle commande
4. **Ajouter un test unitaire** : Tester un handler avec xUnit et Moq
5. **Créer une nouvelle notification** : Ajouter un nouveau type de notification

## 📖 Ressources Complémentaires

- [Clean Architecture par Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)

---

**Note** : Cette architecture est conçue pour l'apprentissage. Explorez chaque fichier, modifiez le code, et expérimentez pour mieux comprendre chaque concept !

