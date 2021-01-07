# API RESTful .Net Core

### Requisitos

* .Net Core 3.1 Instalado

* SqlServer Instalado

### Instruções

* Para consumir os Endpoints de Usuario é necessário o Token de autenticação, para gerar o Token, consumir o Endpoint "/api/autenticar/gerar-token" do controller Autenticar

* Informar a sua ConnectionStrings no arquivo appsettings.json dos projetos UserCRUDApi.Presentation.Api e UserCRUDApi.Infra.Data

### Arquitetura

* Arquitetura DDD

* Dependency Injection (DI)

* Unit of Work

### Pacotes

* AutoMapper

* FluentValidation

* JWT Security

### Persistência

Contexto configurado para criar o banco de dados automaticamente

* Entity Framework Core

* UseSqlServer

* Migrations

### Documentação

* Swagger
