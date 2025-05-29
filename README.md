# Desafio Técnico Objective

Este projeto é uma solução para o desafio técnico proposto pela Objective, desenvolvido em .NET 9. O objetivo é criar uma API para gestão bancária que permita criar contas e realizar transações financeiras, seguindo regras específicas de negócio.

## Tecnologias Utilizadas

- C#
- .NET 9
- Entity Framework Core
- xUnit 
- Visual Studio 2022 
- SQL Server Management Studio


## Pré Requisitos

1. .NET 9 SDK instalado.
2. SQL Server LocalDB (instalado junto com o Visual Studio)
3. Ferramenta Entity Framework Core CLI instalada globalmente

## Como Executar

1. Clone o repositório e acesse a pasta do projeto
   ```sh
   git clone https://github.com/guilhermearantes/DesafioTecnicoObjective.git
   ```
   ```sh
   cd DesafioTecnicoObjective
   ```

2. Restaure os pacotes (opcional)

   ```sh
   dotnet restore
   ```

3. Crie o banco de dados e aplique as migrations.

   ```sh
   dotnet ef database update --context ContaDbContext
   ```

4. Execute o projeto pelo comando:
   ```sh
   dotnet run --project DesafioTecnicoObjective
   ```
   Ou pressione `F5` no Visual Studio.

## Como Rodar os Testes

- Via Visual Studio: utilize o Test Explorer.
- Via terminal:
   ```sh
   dotnet test
   ```

## Endpoints da API

### Criar Conta

`POST /conta`

**Exemplo de Request:**
```json
{ "numero_conta": 234, "saldo": 180.37 }
```
**Response 201:**
```json
{ "numero_conta": 234, "saldo": 180.37 }
```
**Se a conta já existe:** HTTP 409

---

### Realizar Transação

`POST /transacao`

**Exemplo de Request:**
```json
{ "forma_pagamento": "D", "numero_conta": 234, "valor": 10 }
```
**Response 201:**
```json
{ "numero_conta": 234, "saldo": 170.07 }
```
- **Saldo insuficiente:** HTTP 404, mensagem `"Saldo insuficiente."`
- **Conta não encontrada:** HTTP 404, mensagem `"Conta não encontrada."`

---

### Consultar Conta

`GET /conta?numero_conta=234`

**Response 200:**
```json
{ "numero_conta": 234, "saldo": 170.07 }
```
- **Conta não encontrada:** HTTP 404, mensagem `"Conta não encontrada."`

---

## Regras de Negócio

- Cartão de Débito: taxa de 3%
- Cartão de Crédito: taxa de 5%
- Pix: sem taxa
- Não é permitido saldo negativo
- Não existe cheque especial
- Não é possível criar conta já existente

## Decisões Técnicas e Boas Práticas

- **Injeção de dependência** para serviços.
- **Separação de camadas** (Controller, Service, Repository).
- **DTOs** para entrada e saída dos endpoints.
- **Testes unitários e de integração** cobrindo os principais fluxos e regras de negócio.

  
 ## Patterns aplicados:
 
- Strategy: Cálculo de taxa por forma de pagamento.
- Factory: Instancia a strategy correta.
- Decorator: Logging em estratégias de taxa.
- DTO: Separação de transporte de dados e domínio.
- Service Layer: Centraliza regras de negócio.
- Repository: Abstração do acesso a dados.


## Observação sobre o uso de float nos valores monetários 

O enunciado do desafio solicita explicitamente o uso do tipo float para representar o saldo inicial das contas.

Embora tenha seguido essa instrução fielmente, é importante destacar que o tipo float não é recomendado para operações financeiras em aplicações reais, 
devido à sua natureza binária de representação e à consequente imprecisão em cálculos decimais, o que pode causar erros de arredondamento acumulativos.

Em ambientes de produção, o tipo mais apropriado para representar valores monetários em C# é o decimal, que oferece maior precisão e segurança para cálculos financeiros.

Outra abordagem recomendada é usar int para armazenar valores em centavos (por exemplo, 12345 para representar R$123,45), principalmente em sistemas de alto desempenho, 
como gateways de pagamento ou bancos. Essa técnica elimina completamente problemas de precisão, já que não há frações, e pode ser mais eficiente para o processador, 
pois operações com inteiros são mais rápidas que com decimais. Mas exige conversão manual para exibição.

Apesar disso, optei por manter o uso de float para garantir conformidade com o enunciado do desafio, considerando também a possibilidade de existirem validadores automáticos que esperam esse tipo específico.
