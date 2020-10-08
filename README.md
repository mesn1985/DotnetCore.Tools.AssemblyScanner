# DotnetCore.Tools.AssemblyScanner
**Under Refactoring**


**What is it and what can it do?**  
It is a library, that can preload all dll file assemblies within the root directory.
When a dependecy is  pulled from a nuget repository, or when another project is  referenced,
the dll and all the dll dependencies of for that  project is copied into the project folder.
when the project is then build, all dll's from the project folder is copied into the build output
folder regardless of wheter it is referenced or not in any of the source code file(.cs). (At least by default)
This mean that Assemblies can be picked up at runtime from the dll file's, and that the type can be extracted
from the dll  file.


**Examlple executable project**  
    - DonetCore.Tools.AssemblyScanner.Example.WorkerService

**Included example**  
In the included example with worker service i use the IServiceCollection exetention method "AddAllWorkerServicesFromTheRootLibraryAsBackgroundServices"(part of the example)
to add backgroundservices to the service worker.
The  workerservice have a refrence to the "DotnetCore.Tools.AssemblyScanner.Example.Worker" project which contains a worker implementation. the worker implementations
class is not referenced anywhere in the workerservice project. When the exentions method is invoked on the IServiceCollection in the workerService "Program.cs", the
extention method use the Assembly scanner to load all assemblies from all dll  files in the root directory. After that types from the current application domain is
filtered by the ones that is of type "BackgroundService", once those are found, they are added as workerservice through the IServiceCollection extention method "AddHostedService".
AddHostedService is part of the "using Microsoft.Extensions.Hosting" namespace and takes a generic parameter. Unfortantely you cannot add generic parameters dynamically during
runtime without using reflection so there is a caveat there. 


**An experience i made while Writting this**
I Manually tested this application from the IDE without any bugs.
Then i tested this application published as a Cross-platform application  without  any bugs.
Then i tested this application published as a win-x64 application, and i got an exception
when trying to obtain an AssemblyName from from a Path. It would appear that the windows
API is injected as a *.dll file into the root directory when publishing as a Win-x64 application.
The win-x64 API dll file does not contain valid Core CLR IL, and would therefor cause  an exception.
The issue is fixed, but  i figured that it might help others to know this:
**Different type of dll files can be injected during the build, depending on which runtime the application is publish for**  
