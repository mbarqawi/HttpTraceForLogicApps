
# Https trace for Logic Apps v2

Application enables the trace and tries to call the HTTP endpoint and display the error in the console,


## How to download  the application

Open the Kudo https://docs.microsoft.com/en-us/azure/app-service/resources-kudu from Logic app site.

Then chose CMD

Execute the below command to clone the GitHub repository 

`git clone https://github.com/mbarqawi/HttpTraceForLogicApps.git`


The above commend will create a folder 

`HttpTraceForLogicApps`

Then you will have to publish the solution to be able to use the executable by running the Build.cmd 


```
Cd HttpTraceForLogicApps
build.cmd
HttpsTrace.exe wrong.host.badssl.com
HttpsTrace.exe client.badssl.com -c badssl.com-client.pfx -p badssl.com
```



You need to set the environment variable DOTNET_ADD_GLOBAL_TOOLS_TO_PATH to false to be able to run the dotnet publish  on the Kudu if the buld fails

## License

This project is licensed under the MIT License.
