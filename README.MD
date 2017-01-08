eLab
=======
eLab is an application based on ASP.NET Web Api 2 as a backend side and AngularJS as a frontend, which allows to conduct workshops. Authors decided to develop this project, because there is no any smiliar project, with availability for real-time participant's desktop previewing and evaluation of students' work during the course.

Official website of the project:
[Link od Bartka]

Application is available in: [Link](http://elabclient.azurewebsites.net/app/index.html#)

Instalation
---------
FRONTEND
* Go to dictionary `WebSites/eLab.Client` and run:
```
npm install
```
BACKEND
* Restore all required packages using Nuget.
* Go to file `eLab.Web/Web.config` and set appropriate connection string to the SQL Server.
* In Package Manager Console under VisualStudio, type update-database to create new database with some default values.

Browser compatibility
---------------------
* IE 8+
* Chrome 8+
* Firefox 10+
* Safari 3+
* Opera 10.6+
