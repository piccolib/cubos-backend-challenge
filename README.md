
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

2. **Configure o banco de dados**:

Crie um banco chamado `cubos_db` no seu PostgreSQL local.

3. **Atualize o banco com as migrations**:

```bash
dotnet ef database update
```

> Certifique-se de estar com o projeto de API como projeto de inicialização.

4. **Rode a aplicação**:

Você pode usar o Visual Studio (F5) ou executar:

```bash
dotnet run --project CubosFinance.Api
```

---

## 🔐 Variáveis de ambiente / appsettings

A configuração está no arquivo `appsettings.Development.json`:

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

# cubos-backend-challenge

## 📝 Description

This is a RESTful API built with .NET 8 and PostgreSQL. It allows management of people, bank accounts, cards and transactions — including internal transfers and reversals.

Authentication is based on JWT and there's integration with an external compliance API for CPF/CNPJ validation.

---

## 🚀 Tech Stack

- ASP.NET Core 8
- Entity Framework Core
- PostgreSQL
- Refit (for external API consumption)
- JWT Authentication
- Swagger
- xUnit and Moq for unit tests
- AutoMapper

---

## 🧪 Running tests

To run all tests, use the following command in terminal:

```bash
dotnet test
```

> Or use **Test Explorer** in Visual Studio.

---

## ⚙️ How to run the project

1. **Requirements**:
   - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
   - [PostgreSQL](https://www.postgresql.org/)

2. **Create the database**:

Create a database named `cubos_db` in your local PostgreSQL server.

3. **Apply migrations**:

```bash
dotnet ef database update
```

4. **Run the app**:

You can use Visual Studio (F5) or run:

```bash
dotnet run --project CubosFinance.Api
```

---

## 🔐 Environment Variables / Settings

Set them in `appsettings.Development.json`:

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

## 🐳 Docker (Optional)

Docker support is not implemented yet but may be added later.
