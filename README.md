# GSU Museum mobile application


![Platforms](https://img.shields.io/badge/Platforms-iOS%2FAndroid-green)
![GitHub top language](https://img.shields.io/github/languages/top/Neroz1x/GSU.Museum)
![Framework: ASP.NET Core](https://img.shields.io/badge/Framework-ASP.NET%20Core-blue)
![Framework: Xamarin.Forms](https://img.shields.io/badge/Framework-Xamarin.Forms-blue)
[![GitHub issues](https://img.shields.io/github/issues-raw/Neroz1x/GSU.Museum)](https://github.com/Neroz1x/GSU.Museum/issues)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![GitHub watchers](https://img.shields.io/github/watchers/Neroz1x/GSU.Museum?label=Watch&style=social)


## Table of contents

* [Project description](#project-description)
  * [Features](#features)
  * [Components and technologies](#components-and-technologies)
  * [Description](#short-description)
* [Getting started](#getting-started)
  * [Project structure](#Project-structure)
    * [GSU.Museum.Shared](#GSU.Museum.Shared)
    * [GSU.Museum.Android and GSU.Museum.iOS](#GSU.Museum.Android-and-GSU.Museum.iOS)
    * [GSU.Museum.CommonClassLibrary](#GSU.Museum.CommonClassLibrary)
    * [GSU.Museum.API](#GSU.Museum.API)
    * [GSU.Museum.API.Tests](#GSU.Museum.API.Tests)
    * [GSU.Museum.Web](#GSU.Museum.Web)
  * [Requirements](#requirements)
  * [Setting up](#setting-up)
  * [Usage](#usage)
* [Contributing](#contributing)
* [Available languages](#available-languages)
* [License](#license)


## Project description


### Features

* Cross-platform development on Xamarin
* Client-side data caching
* Localization
* Deep link
* Logging
* RESTful API
* Swagger
* NoSQL database
* Localization ([available languages](#available-languages))
* API-Key authentication
* JWT


### Components and technologies

- REST API (backend) - ASP.NET Core
- Mobile client (frontend) - Xamarin.Forms
- Web site for database management - ASP.NET Core MVC
- Database - NoSQL MongoDB


### Description

An application powered by Xamarin, Xamarin.Forms, and a ASP.NET Core backend with 100% shared C# code across iOS, Android. A web site developed by ASP.NET Core MVC.


Mobile appliaction provides users with information about the GSU Museum's exhibits. All data is stored in databse. Mobile app send request to the server, which authenticate it, retrieve requested data from and send it back to the client.


The web site is used for database managing. It allows admin to add new exhibits, view existing, update and delete them.


## Getting started


### Project structure

#### GSU.Museum.Shared


This project contains shared code like page markup and business logic for Android and iOS applications.


#### GSU.Museum.Android and GSU.Museum.iOS


All platform specific logic is located in this projects.


#### GSU.Museum.CommonClassLibrary


This project contains data models and classes, that used all over the solution.


#### GSU.Museum.API


This is a backend project. It contains all endpoint, that can be accessed by mobile app.


#### GSU.Museum.API.Tests


Unit-tests for backend are located in this project.


#### GSU.Museum.Web


This project contains all logic related to museum management site.


### Requirements

- [Install Visual Studio 2019](https://visualstudio.microsoft.com/en/downloads/)
- Android 9.0 API 28 SDK or higher [Instruction](https://docs.microsoft.com/en-us/xamarin/android/get-started/installation/android-sdk?tabs=windows)
- [.NET Core 3.1 SDK or later](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- [MongoDB database](https://www.mongodb.com/try/download/community)


### Setting up

1) Fork or clone repository https://github.com/Neroz1x/GSU.Museum.git.
2) Wait while VS download all essential NuGet packages.
3) Install MongoDB. [Link](https://www.mongodb.com/try/download/community) to MongoDB.


### Usage

1. Set up API backend
2. Set up mobile app
2. Start it


## Contributing

Contributions are always welcome! 

Please read the [contribution guidelines](https://github.com/Neroz1x/GSU.Museum/blob/master/contributing.md) first.
If you want to add a huge update in the project, make an issure before where we can discuss it. Issure template for [bug](https://github.com/Neroz1x/GSU.Museum/blob/master/.github/ISSUE_TEMPLATE/bug_report.md) and [new feature](https://github.com/Neroz1x/GSU.Museum/blob/master/.github/ISSUE_TEMPLATE/feature_request.md). Otherwise, follow next steps:
1) Fork repository.
2) Create your feature branch: ```git checkout -b my-new-feature.```
3) Commit your changes: ```git commit -am 'Add some feature'.```
4) Push to the branch: ```git push origin my-new-feature.```
5) Submit a pull request.

## Available languages

* English
* Беларуская
* Русский

## License

The ([MIT](https://choosealicense.com/licenses/mit/)) License Copyright (c) 2021