# Modern Enterprise Software Engineering (AWS)

Ce dépôt regroupe l'ensemble des travaux pratiques (TP) réalisés dans le cadre du module "Modern Enterprise Software Engineering" chez ESTIAM, axé sur les services Cloud AWS.

## 📁 Structure du Projet

- **TP 1** : Mise en place de l'environnement (Installation AWS CLI, SDK .NET 8).
- **TP 2** : Sécurité & IAM (Création d'utilisateurs, groupes et politiques de confiance).
- **`S3Manager/`** : TP 3 & TP 4.
  - CRUD programmatique S3 et DynamoDB.
  - Première Lambda déclenchée par S3.
- **`ImageProcessor/`** : TP 5 & TP 6.
  - `FunctionTP5.cs` : Analyse d'images via **Amazon Rekognition** (TP 5).
  - `GetImageLabels.cs` : API REST sécurisée via **API Gateway** & **Cognito** (TP 6).
- **`Documentation/`** : Énoncés des TP (PDF) et captures d'écran des tests réussis.

## 🚀 Déploiement des Lambdas

Chaque partie peut être déployée indépendamment depuis le dossier `ImageProcessor` :

### TP 5 - Analyse d'images (Rekognition)

```powershell
dotnet lambda deploy-function --config-file aws-lambda-tp5.json
```

### TP 6 - API de consultation (Gateway/Cognito)

```powershell
dotnet lambda deploy-function --config-file aws-lambda-tp6.json
```

## 🛠️ Technologies

- **Langage** : C# / .NET 8
- **Services AWS** : Lambda, S3, Rekognition, DynamoDB, API Gateway, Cognito, IAM.
- **Outils** : AWS CLI, Amazon.Lambda.Tools.
