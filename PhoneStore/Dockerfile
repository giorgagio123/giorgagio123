#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.2-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY ["PhoneStore/PhoneStore.csproj", "PhoneStore/"]
RUN dotnet restore "PhoneStore/PhoneStore.csproj"
COPY . .
WORKDIR "/src/PhoneStore"
RUN dotnet build "PhoneStore.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "PhoneStore.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PhoneStore.dll"]