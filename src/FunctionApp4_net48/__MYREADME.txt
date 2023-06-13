* package ppur les durable functions:
ajouter package 
Microsoft.Azure.WebJobs (version qui semble fonctionner en .NET 4.8: 2.1.0; les versions supérieures génèrent des erreurs)
Microsoft.Azure.WebJobs.Extensions.DurableTask (version qui semble fonctionner en .NET 4.8: 2.1.0; les versions supérieures génèrent des erreurs)

les erreurs pour Microsoft.Azure.WebJobs.Extensions.DurableTask > 2.1 dans
C:\Users\ggo98\.nuget\packages\microsoft.net.sdk.functions\1.0.24\build\netstandard1.0\Microsoft.NET.Sdk.Functions.Build.targets
    <GenerateFunctions
      TargetPath="$(TargetPath)"
      OutputPath="$(TargetDir)"
      UseNETCoreGenerator="$(UseNETCoreGenerator)"
      UseNETFrameworkGenerator="$(UseNETFrameworkGenerator)"
      UserProvidedFunctionJsonFiles="@(UserProvidedFunctionJsonFiles)" />
  </Target>
:

System.IO.FileNotFoundException: Could not load file or assembly 'Microsoft.Azure.WebJobs, Version=2.2.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35' or one of its dependencies. The system cannot find the file specified.

* erreur au run:
=> probablement démarrer le node local.

* Azure Storage Emulator:
en .NET 4.8, Ca ne fonctionne pas avec Azurite.
Il faut utiliser Azure Storage Emulator (à installer:
https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409
https://learn.microsoft.com/en-us/azure/storage/common/storage-use-emulator
C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator>AzureStorageEmulator.exe start
)
Pour initialiser (la première fois):
. C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator>AzureStorageEmulator.exe init
Pour démarrer l'emulator:
. C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator>AzureStorageEmulator.exe start
