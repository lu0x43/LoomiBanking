# LoomiBanking - Microsserviços Bancários 🏦

Este projeto é a resolução do Desafio Técnico .NET para a avaliação de arquitetura, mensageria e resiliência no contexto de microsserviços.

## 📋 Gestão e Planejamento
O gerenciamento das tarefas, estimativas e tempo gasto (Time Spent) foi realizado via GitHub Projects (Kanban).
👉 **[Acesse o Board Kanban aqui]([LINK_DO_SEU_KANBAN_AQUI])**

## 🏗 Arquitetura e Decisões Técnicas
A solução foi desenhada utilizando **Clean Architecture** e princípios de **Domain-Driven Design (DDD)**, dividida em dois microsserviços autônomos.

* **Microsserviço de Clientes:** Responsável por gerenciar os dados cadastrais, upload de fotos de perfil (Azure Blob Storage) e dados bancários (Value Objects). Utiliza **Redis** para cache de leitura.
* **Microsserviço de Transações:** Responsável por efetivar e listar transferências. 

### Padrões e Tecnologias
* **.NET 8 & C# 12**
* **Entity Framework Core (SQL Server)**
* **Comunicação Síncrona (HTTP com Polly):** O MS de Transações valida os dados da conta junto ao MS de Clientes. Foi implementado `Retry`, `Circuit Breaker` e `Timeout` para garantir resiliência caso o serviço de Clientes sofra latência.
* **Comunicação Assíncrona (RabbitMQ / MassTransit):** Após o sucesso de uma transferência, o evento `TransferCompletedEvent` é publicado na fila para desacoplar ações futuras (como notificações e envio de e-mails via Worker).
* **Segurança:** Autenticação via JWT com Role-Based Access Control (RBAC).
* **Tratamento de Erros:** Global Exception Handler Middleware retornando no padrão RFC 7807 (Problem Details).

## 🚀 Como Executar o Projeto localmente

### Pré-requisitos
* .NET 8 SDK
* Docker Desktop (opcional para rodar a infraestrutura completa localmente)

### Opção 1: Via Docker Compose (Recomendado)
A raiz do projeto contém um `docker-compose.yml` que orquestra o SQL Server, Redis, RabbitMQ e o Azurite (Emulador do Azure Storage).

```bash
# Subir a infraestrutura
docker compose up -d

# Executar as APIs (na raiz do projeto)
dotnet run --project src/Loomi.Clients.Api/Loomi.Clients.Api.csproj
dotnet run --project src/Loomi.Transactions.Api/Loomi.Transactions.Api.csproj
