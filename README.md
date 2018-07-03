# MyLittleUrlApp
MyLittleUrlApp - Web App that consumes the URL Shortener service: MyLittleUrlApi

This Web App runs on:
* .NET Core 2.0

Pre-requisite
* MyLittleUrlApi Service

## Old Style Networking (linking)

Run the MyLittleUrlApi service in its own named container (name=mylittleurlapi)
* Expose port 80, optionally to localhost for testing

```
docker run -d -p 127.0.0.1:32780:80 --name=mylittleurlapi  mylittleurlapi:latest
```

Run the MyLittleUrlApp web application in its own container and use 'old-style' linking to connect the two
* If using a different name for the API service container, change that in appsettings.json 

```
docker run -d -P --name mylittleurlweb --link mylittleurlapi  mylittleurlapp:latest
```

## Custom 'bridge' Networking

Create a custom network with 'bridge' driver

```
docker network create --driver bridge mylittleurl-net
```

Attach your containers to this network

```
docker run -d -p=127.0.0.1:32780:80 --name=mylittleurlapi  --network=mylittleurl-net mylittleurlapi:latest
```
* Optionally map the API service endpoint port out to localhost for testing

```
docker run -d -p=127.0.0.1:32880:80 --name=mylittleurlweb --network=mylittleurl-net  mylittleurlapp:latest
```


