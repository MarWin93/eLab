# Instrukcja instalacji

## FRONTEND
* zainstalować npm
* w folderze `WebSites/eLab.Client` uruchomić polecenie:

```
npm install
```

## BACKEND
* W pliku `eWarsztaty.Web/Web.config`, ustawić poprawnie connectionString, podając poprawną nazwę serwera SQL.
* W Package Manager Console wybrać jako Default project: eWarsztaty.Web i wpisać komendę "update-database", aby stworzyć bazę danych.
* Potrzebna migracja na inny DBMS
