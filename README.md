# Modern Enterprise Software Engineering (AWS)

Ce dépôt présente un projet personnel orienté Cloud AWS, construit autour de cas d'usage concrets en architecture logicielle moderne.

## 📁 Structure du Projet

- **Étape 1** : Mise en place de l'environnement (Installation AWS CLI, SDK .NET 8).
- **Étape 2** : Sécurité & IAM (Création d'utilisateurs, groupes et politiques de confiance).
- **`S3Manager/`** : Couche de services AWS pour la gestion des données.
  - CRUD programmatique S3 et DynamoDB.
  - Première Lambda déclenchée par S3.
- **`ImageProcessor/`** : Fonctions serverless de traitement et d'exposition des résultats.
  - `ImageAnalysisFunction.cs` : Analyse d'images via **Amazon Rekognition**.
  - `GetImageLabels.cs` : API REST sécurisée via **API Gateway** & **Cognito**.
- **`Documentation/`** : Documentation projet (captures d'écran, preuves de tests, ressources complémentaires).

## 🚀 Déploiement des Lambdas

Chaque partie peut être déployée indépendamment depuis le dossier `ImageProcessor` :

### Analyse d'images (Rekognition)

```powershell
dotnet lambda deploy-function --config-file aws-lambda-image-analysis.json
```

### API de consultation (Gateway/Cognito)

```powershell
dotnet lambda deploy-function --config-file aws-lambda-image-api.json
```

## 🛠️ Technologies

- **Langage** : C# / .NET 8
- **Services AWS** : Lambda, S3, Rekognition, DynamoDB, API Gateway, Cognito, IAM.
- **Outils** : AWS CLI, Amazon.Lambda.Tools.
