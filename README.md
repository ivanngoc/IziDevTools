# Easy switch
chmod +x easyswitch.sh

sudo ./easyswitch.sh

# Build and publish NuGet
1. забилдить проект **nuget/NuGetPackage/NuGetPackage.csproj**
2. пакет будет создан по пути в переменной среде $(MY_NUGET_REGISTRY)

# Инкрементное обновление пакета и публикация на сервере
1. в файле **nuget/NuGetPackage/DevTools.nuspec.template** указать <version/>
2. в вайле **nuget/NuGetPackage/NuGetPackage.csproj** указать такую же <Version/>
2. забилдить проект **nuget/NuGetPackage/NuGetPackage.csproj** в RELEASSE конфигурации

# ENV VARS
