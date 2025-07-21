
# cubos-backend-challenge

## 🇧🇷 Descrição

Este projeto é uma API RESTful desenvolvida em .NET 8 com PostgreSQL como banco de dados. Ele permite o gerenciamento de pessoas, contas bancárias, cartões e transações, incluindo transferências internas e reversões.

A autenticação é baseada em JWT e há uma integração com uma API externa de compliance para validação de CPF/CNPJ.

Consulte também o [CHALLENGE_INSTRUCTIONS.md](./CHALLENGE_INSTRUCTIONS.md) para ver o enunciado original do desafio técnico.

---

## 🚀 Tecnologias Utilizadas

- ASP.NET Core 8
- Entity Framework Core
- PostgreSQL
- Refit (para consumo da API de Compliance)
- JWT (autenticação)
- Swagger (documentação)
- xUnit e Moq (testes unitários)

---

## 🧪 Como executar os testes

No terminal, dentro da raiz do projeto de testes, execute:

```bash
dotnet test
```

> Você também pode rodar os testes diretamente pelo **Test Explorer** do Visual Studio.

---

## ⚙️ Como rodar o projeto

1. **Pré-requisitos**:
   - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
   - [PostgreSQL](https://www.postgresql.org/)

2. *Clone o repositório:

   - git clone https://github.com/seu-usuario/cubos-backend-challenge.git
   - cd cubos-backend-challenge

3. **Rode a aplicação**:

Você pode usar o Visual Studio (F5) ou executar:

```bash
dotnet run --project CubosFinance.Api
```

## 🔐 Variáveis de ambiente / appsettings

A configuração está no arquivo `appsettings.Development.json`:
As principais variáveis a serem trocadas aqui são Email e Password do ComplianceAuth, é preciso utilizar credenciais válidas da Cubos para funcionar.

```json
"ConnectionStrings": {
  "Default": "Host=localhost;Port=5432;Database=cubos_db;Username=postgres;Password=postgres"
},
"ComplianceAuth": {
  "Email": "seu@email.com",
  "Password": "suaSenha",
  "BaseUrl": "https://compliance-api.cubos.io/"
},
"Jwt": {
  "Key": "minhachavedesegurancaprecisaterumtamanhominimo",
  "Issuer": "ApiFinance",
  "Audience": "ApiFinanceUsers",
  "ExpiresInMinutes": 30
}
```

---

## 📄 Documentação

Acesse o Swagger em:

```
https://localhost:7065/swagger
```

---

## 🐳 Docker (opcional futuramente)

O projeto ainda não possui configuração Docker. Isso poderá ser adicionado posteriormente com Dockerfile e docker-compose.

---

## 🇬🇧 English Version

## 📝 Description

This is a RESTful API built with .NET 8 and PostgreSQL as the database. It allows the management of people, bank accounts, cards, and transactions — including internal transfers and reversals.

Authentication is based on JWT, and there is integration with an external compliance API for CPF/CNPJ validation.

See the original challenge statement at [CHALLENGE_INSTRUCTIONS.md](./CHALLENGE_INSTRUCTIONS.md).

---

## 🚀 Tech Stack

- ASP.NET Core 8
- Entity Framework Core
- PostgreSQL
- Refit (for consuming the Compliance API)
- JWT Authentication
- Swagger (documentation)
- xUnit and Moq (unit testing)

---

## 🧪 Running the tests

In the terminal, from the test project root, run:

```bash
dotnet test
```

> You can also run the tests directly via **Test Explorer** in Visual Studio.

---

## ⚙️ How to run the project

1. **Requirements**:
   - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
   - [PostgreSQL](https://www.postgresql.org/)

2. **Clone the repository**:

```bash
git clone https://github.com/your-username/cubos-backend-challenge.git
cd cubos-backend-challenge
```

3. **Run the application**:

You can use Visual Studio (F5) or run:

```bash
dotnet run --project CubosFinance.Api
```

> Migrations are applied automatically at startup.

---

## 🔐 Environment Variables / appsettings

Configuration is located in the `appsettings.Development.json` file.  
The main variables to be changed are the `Email` and `Password` under `ComplianceAuth`. You must use valid Cubos credentials.

```json
"ConnectionStrings": {
  "Default": "Host=localhost;Port=5432;Database=cubos_db;Username=postgres;Password=postgres"
},
"ComplianceAuth": {
  "Email": "your@email.com",
  "Password": "yourPassword",
  "BaseUrl": "https://compliance-api.cubos.io/"
},
"Jwt": {
  "Key": "minhachavedesegurancaprecisaterumtamanhominimo",
  "Issuer": "ApiFinance",
  "Audience": "ApiFinanceUsers",
  "ExpiresInMinutes": 30
}
```

---

## 📄 Documentation

Access Swagger at:

```
https://localhost:7065/swagger
```

---

## 🐳 Docker (optional in the future)

This project currently has no Docker configuration. It may be added later using a Dockerfile and docker-compose.
