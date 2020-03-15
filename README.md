# Execute scripts to SQLite database

## How to use

Install the tool at first:
```
dotnet tool install --global sqlite-exec
```
Then you can use `sqlite-exec` command to execute the scripts
```
sqlite-exec -d "D:\Database.sqlte" -f "D:\script.sql"
```
Execute script to working directory `*.sqlte` database
```
sqlite-exec -f "D:\script.sql"
```
