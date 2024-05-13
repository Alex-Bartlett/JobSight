# JobSight

![JobSight Poster](https://alexbartlett.com/images/jobsight/jobsight.png)

## Contents

- [Overview](#overview)
- [Setup](/Docs/Setup.md)
- [Getting Started](/Docs/GettingStarted.md)
- [Solution Overview](#solution-overview)
- [Libraries Used](#libaries-used)

## Overview

**Issue: If you get a Postgres error regarding SCRAM, simply refresh the page.**

**Note that this project is still under development, and as such the project is configured for debugging.**

JobSight is a job management application, with a focus on tracking job progress through the use of tasks and images. The product is targetted towards smaller construction businesses, tradesmen, and other similar SMEs.

The app consists of two separate systems: 
- Management App
- Site App

The management app is designed for the use of the mangement team. Users with access to this system can create, edit, and delete jobs and customers. This app encompasses the majority of the features of the program.

The site app is designed for the use of the site workers, and is stripped back to the minimal features those users require. Users can find their job, create a task, and upload images.


## Solution Overview
The software is constructed such that a seperate mobile app project can be created in the future, so the solution is segmented into separate projects for modularity.

### Infrastructure

The Infrastructure project contains data access utilities and services, such as the migrations and dbcontext for entity framework, and a service for communicating with the bucket storage.

### ManagementApp

The Management App is built with .NET Blazor Server framework. It provides the full functionality of the app, but also includes a mobile-first site app for users that are site workers.

### Shared

The Shared project is a library that contains models and repositories. Services are currently in the ManagementApp, but these will be relocated to the Shared project soon so that they can be used by the mobile app too.

### UnitTests

The Unit Tests project contains the unit tests for the services.

## Libaries Used
The solution makes use of the following libraries:
- [HAVIT Blazor](https://havit.blazor.eu/) - Bootstrap component library for Blazor
- [ImageSharp](https://github.com/SixLabors/ImageSharp) - Image file conversion
- [Supabase C#](https://github.com/supabase-community/supabase-csharp) - Client library for Supabase, used to communicate with the bucket storage
- XUnit - Unit testing
- EntityFramework - ORM
- IdentityCore - Identity service