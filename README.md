
# Https trace for Logic Apps

Application enables the trace and tries to call the HTTP endpoint and display the error in the console,



## How to download  the application

Open the Kudo https://docs.microsoft.com/en-us/azure/app-service/resources-kudu from Logic app site.

Then chose CMD

![image](https://user-images.githubusercontent.com/891607/180943465-1c8ae261-91ce-4b75-ac41-242d4284fcbf.png)


Execute the below command to clone the GitHub repository 

`git clone https://github.com/mbarqawi/HttpTraceForLogicApps.git`


the above commend will create a folder 

`HttpTraceForLogicApps`

Then you will have to publish the solution to be able to use the executable 


```
Cd HttpTraceForLogicApps
dotnet publish
Cd HttpTraceForLogicApps\bin\Debug\netcoreapp3.1\publish
```




You need to set the environment variable DOTNET_ADD_GLOBAL_TOOLS_TO_PATH to false to be able to run the dotnet publish  on the Kudu





# How to Run it
Make sure that the site is stop Run the command FlowHistoryCleaner.exe in the CMD to start the cleaning process

1. When prompted, enter a URL.
2. The application will send a GET request to the URL and display a message when the response is received.
3. If an error occurs (for example, if the URL is invalid), an error message will be displayed.
4. After each request, you will be asked if you want to exit. Enter `y` to exit, or `n` to send another request.

## License

This project is licensed under the MIT License.
