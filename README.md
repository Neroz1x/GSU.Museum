# GSU Museum mobile application
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
## Table of contents
* [Project description](#project-description)
* [Components and technologies](#components-and-technologies)
* [Getting started](#getting-started)
* [Contributing](#contributing)
* [Available languages](#available-languages)
* [License](#license)

## Project description

An application powered by Xamarin, Xamarin.Forms, and a ASP.NET Core backend with 100% shared C# code across iOS, Android. A web site developed by ASP.NET Core MVC.


Mobile appliaction provides users with information about the GSU Museum's exhibits. All data are stored in databse. Mobile app send request to the server, which authenticate it, retrieve requested data from the database and send it back to the client.


The web site for managing database. It allows to view all the records, add new, edit and delete them.


### Features
* Cross-platform development
* RESTful API
* Client-side caching
* Logging
* NoSQL database
* Localization ([available languages](#languages))
* API-Key authentication

## Components and technologies

- REST API (backend) - ASP.NET Core
- Mobile client (frontend) - Xamarin.Forms
- Web database manager - ASP.NET Core MVC
- Database - NoSQL MongoDB

## Getting started
### Requirements
- [Install Visual Studio 2019](https://visualstudio.microsoft.com/en/downloads/)
- Android 10.0 API 29 SDK and Android 9.0 API 28 SDK [Instruction](https://docs.microsoft.com/en-us/xamarin/android/get-started/installation/android-sdk?tabs=windows)
- [.NET Core 3.1 SDK or later](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- [MongoDB database](https://www.mongodb.com/try/download/community)

### Setting up
1) Fork or clone repository https://github.com/Neroz1x/GSU.Museum.git.
2) Wait while VS download all essential NuGet packages.
3) Install MongoDB. [Link](https://www.mongodb.com/try/download/community) to MongoDB.

### Usage
1. Set up API backend
2. Start mobile app


## Contributing
Contributions are always welcome! 

Please read the [contribution guidelines](https://github.com/Neroz1x/GSU.Museum/blob/master/contributing.md) first.
If you want to add a huge update in the project, make an issure before where we can discuss it. Otherwise, follow next steps:
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
The ([MIT](https://choosealicense.com/licenses/mit/)) License Copyright (c) 2020