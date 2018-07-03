# MyLittleUrlApp
MyLittleUrlApp - Web App that consumes the URL Shortener service: MyLittleUrlApi

This Web App runs on:
* .NET Core 2.0



'''

docker run -d -p 127.0.0.1:32780:80 --name=mylittleurlapi  mylittleurlapi:latest

docker run -d -P --name mylittleurlweb --link mylittleurlapi  mylittleurlapp:latest

'''