# Gestion de Contacts

Ce projet vise à créer une application de gestion de contacts avec des fonctionnalités de base ainsi que des fonctionnalités avancées de persistance et de sécurité des données.

## Fonctionnalités

### Socle Applicatif

Le projet comprend les fonctionnalités suivantes pour la gestion de contacts :

- **Structure de données hiérarchique** :
  - Dossiers contenant des contacts avec des attributs tels que Nom, Prénom, Courriel, Société, Type de lien, Dates de création et de modification.
- **Application Console** :
  - Affichage de la structure de contacts
  - Gestion des dossiers : création dans un dossier parent
  - Gestion des contacts : création dans un dossier parent
- **Améliorations possibles** :
  - Validation du format des adresses e-mail
  - Sélection du dossier courant pour enregistrement

### Sérialisation

- **Deux Méthodes de Sérialisation** :
  - Sérialisation binaire avec BinaryFormatter
  - Sérialisation XML avec XmlSerializer
- **Gestion d'Erreurs** :
  - Prise en charge explicite des erreurs de chargement de fichier
- **Perspectives** :
  - Utilisation de l'identité Windows pour le nom de fichier
  - Emplacement du fichier dans le dossier "Mes Documents"
  - Protection par mot de passe avec suppression de la base après erreurs répétées

### Cryptage

- **Sécurité des Données** :
  - Cryptage réversible des fichiers de contacts avec CryptoStream
  - Saisie d'une clé de cryptage lors du chargement ou de l'enregistrement
- **Gestion des Erreurs** :
  - Traitement des erreurs lors du décryptage pour éviter la destruction de l'application
- **Perspectives** :
  - Utilisation de l'identifiant interne (SID) en l'absence de clé de cryptage

## Structure du Projet

Ce projet est constitué de trois composants principaux :

1. **Application Console**
2. **Projet de Gestion des Données**
3. **Projet de Sérialisation**
