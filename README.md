# LoomiBanking - Microsserviços Bancários 🏦

Este projeto é a resolução do Desafio Técnico .NET para a avaliação de arquitetura, mensageria e resiliência no contexto
de microsserviços. A solução foi desenhada com foco central em alta disponibilidade, resiliência na comunicação entre
serviços, performance de leitura e segurança.

## 📋 Gestão e Planejamento

O gerenciamento das tarefas, estimativas e tempo gasto (Time Spent) foi realizado via GitHub Projects (Kanban).
👉 **[Acesse o Board Kanban aqui]([https://github.com/users/lu0x43/projects/2)**

## 🏗️ Arquitetura e Decisões Técnicas

O sistema foi desenhado seguindo os princípios da **Clean Architecture**, **Domain-Driven Design (DDD)** e do **SOLID**,
garantindo que o Domínio (regras de negócio) permaneça completamente isolado de detalhes de infraestrutura.

* **Microsserviço de Clientes:** Gerencia dados cadastrais, upload de fotos (Azurite Blob Storage) e dados bancários.
* **Microsserviço de Transações:** Efetiva e lista transferências financeiras.
* **Cache Distribuído (Redis):** Implementação do padrão *Cache-Aside* nas consultas de clientes para mitigar o I/O no
  banco de dados relacional e suportar alto volume de leituras. Invalidação baseada em eventos (ex: alteração de foto).
* **Segurança (Defesa em Profundidade):**
    * **Autenticação JWT:** Proteção de todos os endpoints transacionais com suporte integrado no Swagger.
    * **FluentValidation:** Validação estrita de *inputs* (Fail-Fast) na borda da API, blindando o domínio contra dados
      maliciosos ou malformados.
* **Qualidade e Confiabilidade:** Cobertura de testes unitários utilizando **xUnit**, **Moq** (para isolamento via
  dublês) e **FluentAssertions** para validação semântica de regras e fluxos de controllers.

## 🔄 Fluxo de Comunicação entre Microsserviços

O projeto utiliza uma estratégia híbrida, aplicando o padrão correto para cada cenário de negócio:

### 1. Comunicação Síncrona (HTTP com Polly)

Utilizada onde a resposta imediata é um requisito bloqueante.

* **Cenário:** O MS de Transações consulta o MS de Clientes para validar se as contas de origem e destino existem antes
  de autorizar a transferência.
* **Resiliência:** Implementação de políticas de *Retry* (tentativas exponenciais) para instabilidades de rede e
  *Circuit Breaker* para evitar sobrecarga em cascata caso o MS de Clientes sofra latência severa.

### 2. Comunicação Assíncrona (RabbitMQ / MassTransit)

Utilizada para delegação de tarefas e consistência eventual (Fire-and-Forget).

* **Cenário:** Após efetivar a transferência, o MS de Transações publica o evento `TransferCompletedEvent`. O MS de
  Clientes consome este evento para disparar notificações assíncronas.
* **Vantagem:** O usuário não aguarda o processamento da notificação. Se o serviço falhar, a mensagem persiste na fila
  do RabbitMQ e é processada na retomada, garantindo tolerância a falhas.

---

## 🚀 Como Executar Localmente

A infraestrutura foi conteinerizada via `docker-compose` para garantir consistência de ambiente.

### Pré-requisitos

* .NET 8 SDK
* Docker Desktop (com WSL2 ativado, se no Windows)

### Passo a Passo

1. **Subir a Infraestrutura Base:**
   Na raiz do projeto, inicie os contêineres de banco e mensageria:
   ```bash
   docker-compose up -d

(Serviços iniciados: SQL Server, Redis, RabbitMQ e Azurite)

2. Iniciar as APIs:
   Abra dois terminais na raiz do projeto e execute:

    ```bash
    # Terminal 1 - MS de Clientes
    dotnet run --project src/Loomi.Clients.Api/Loomi.Clients.Api.csproj

    # Terminal 2 - MS de Transações
    dotnet run --project src/Loomi.Transactions.Api/Loomi.Transactions.Api.csproj

### 📖 Uso da API (Guia Rápido)

Ao iniciar, as APIs expõem o Swagger. O fluxo principal de uso consiste em:

Autenticação:

Acesse POST /api/v1/Auth/login no MS de Clientes e copie o token.

No botão Authorize do Swagger, insira: Bearer {seu_token}.

Criação de Entidades:

Utilize POST /api/v1/Clients para registrar contas (Origem e Destino) e anote os IDs (Guid).

Operação Bancária:

No Swagger do MS de Transações, utilize POST /api/v1/Transactions com os IDs e o valor.

Acompanhe os logs no terminal do MS de Clientes para confirmar o processamento via RabbitMQ.


---

### Fim