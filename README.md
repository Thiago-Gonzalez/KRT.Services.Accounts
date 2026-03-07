# KRT Accounts Service

API responsável pelo gerenciamento de contas de clientes do banco **KRT**, permitindo a criação, atualização, consulta e exclusão de contas.

O serviço também publica eventos para que outros sistemas do banco possam reagir às alterações realizadas nas contas.

---

# 📌 Objetivo

Este projeto foi desenvolvido como parte de um **teste técnico para Desenvolvedor Backend**.

A API permite realizar operações **CRUD** sobre contas de clientes contendo as seguintes informações:

* **ID**
* **Nome do titular**
* **CPF**
* **Status da conta (Ativa / Inativa)**

Sempre que uma conta é criada, atualizada ou removida, um evento é publicado para que outros sistemas possam processar essas alterações.

Exemplos de sistemas que poderiam consumir esses eventos:

* Sistema de **Prevenção à Fraude**
* Sistema de **Cartões**
* Outros serviços internos do banco

---

# 🏗 Arquitetura

O projeto foi estruturado utilizando **Clean Architecture**, separando responsabilidades em diferentes camadas.

```
KRT.Services.Customers.API
KRT.Services.Customers.Application
KRT.Services.Customers.Core
KRT.Services.Customers.Infrastructure
KRT.Services.Customers.Tests
```

### Camadas

**API**

Responsável pela exposição dos endpoints HTTP e configuração da aplicação.

**Application**

Contém casos de uso e regras de aplicação.

**Core**

Contém entidades e regras de domínio.

**Infrastructure**

Responsável pela comunicação com banco de dados e mensageria.

**Tests**

Contém testes automatizados da aplicação.

---

# 🧰 Tecnologias Utilizadas

* **.NET 8**
* **ASP.NET Core Web API**
* **Entity Framework Core**
* **SQL Server**
* **RabbitMQ**
* **Docker**
* **Docker Compose**

---

# 🐳 Executando o projeto com Docker (Recomendado)

A forma mais simples de executar o projeto é utilizando **Docker Compose**, que sobe toda a infraestrutura necessária automaticamente.

## Pré-requisitos

Instale o **Docker Desktop**:

https://www.docker.com/products/docker-desktop/

Após instalar, verifique se o Docker está funcionando:

```
docker --version
docker compose version
```

---

## Subindo a aplicação

Na raiz do projeto execute:

```
docker compose up -d
```

Esse comando irá:

* Construir a imagem da API
* Criar os containers necessários
* Subir o SQL Server
* Subir o RabbitMQ
* Iniciar a API

---

## Verificando os containers

```
docker ps
```

---

## Acessando os serviços

### API

```
http://localhost:5000
```

### RabbitMQ Management

```
http://localhost:15672
```

Credenciais padrão:

```
User: guest
Password: guest
```

---

## Parando os containers

```
docker compose down
```

---

# 💻 Executando sem Docker (Opcional)

Também é possível executar a aplicação diretamente via **.NET CLI** ou **Visual Studio**.

## Pré-requisitos

* .NET 8 SDK
* SQL Server ou SQL Server Express

Download do .NET:

https://dotnet.microsoft.com/download

---

## Executar a API

Dentro do diretório da API execute:

```
dotnet run
```

---

# 📡 Integração entre serviços

A aplicação utiliza **RabbitMQ** para comunicação assíncrona entre serviços.

Sempre que uma conta sofre alteração, eventos são publicados para permitir que outros sistemas reajam a essas mudanças.

Eventos publicados:

* **AccountCreated**
* **AccountUpdated**
* **AccountDeleted**

Esse padrão permite que o sistema evolua para uma arquitetura **event-driven**.

---

# 💾 Banco de Dados

O projeto utiliza **SQL Server** para persistência de dados.

Quando executado via Docker, o banco é iniciado automaticamente através do `docker-compose`.