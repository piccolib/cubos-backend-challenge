# Pessoa Backend Pleno

## Introdução
A Cubos pode ser definida como um hub de conhecimento e inovação, capaz de criar empresas digitais próprias e apoiar negócios distintos na tomada de decisões, além do desenvolvimento e resolução de desafios técnicos complexos.

Veja o que já construímos ao longo de 12 anos [clicando aqui](https://cubos.io/cases).


## Contexto

A Cubos é especialista em tecnologia financeira, implementamos ideias que têm o desejo de inovar e otimizar os serviços do sistema financeiro. e para este desafio técnico vamos construir uma API com as principais funcionalidades de uma aplicação financeira.


## Sobre a aplicação
- A aplicação deve ser construída utilizando .NET Core 8 ou maior; C# 12 ou maior e os seguintes requisitos:
    - Uso da Lib [Refit](https://www.nuget.org/packages/refit/) para consumos de endpoints REST
    - É necessário ter um arquivo de Solution `.sln`
- O banco de dados deve ser o [PostgreSQL](https://www.postgresql.org/), migrations devem ser utilizadas. Utilize o ORM de sua preferência.
    - Uso de `Docker Compose` não é obrigatório, mas é um diferencial
- Testes de integração e/ou unitários são obrigatórios
- As rotas devem ser protegidas no padrão [Bearer Authentication](https://swagger.io/docs/specification/authentication/bearer-authentication).
- Mantenha um código limpo e padronizado.

## Funcionalidades
- Criar uma pessoa.
- Autenticar uma pessoa.
- Adicionar e listar cartões de uma conta.
- Adicionar e listar contas da pessoa.
- Realizar e listar transações em uma conta.
- Consultar o saldo de uma conta.
- Reverter uma transação.


## Endpoints
Os endpoints necessários estão descritos [aqui](endpoints/endpoints.md) com os respectivos requisitos.

Siga exatamente os padrões de request e response, o cumprimento do contrato das rotas é caráter obrigatório.
Antes de realizar a entrega, teste todos os endpoints com os exemplos de request body e verifique se o response body possui exatamente as propriedades esperadas.

## Entrega
Utilizando o github, nos envie o seu repositório. Nele deve conter as instruções de como executar a aplicação.

Observação: Caso configure o repositório como privado, nos solicite as contas que devem ter acesso para realizar a correção.


Aguardamos você! :)
