# Estapar

Sistema de gestão de estacionamentos desenvolvido em **.NET 10**. Recebe eventos de cancelas (entrada, estacionamento e saída de veículos) via webhook, controla a ocupação das vagas em tempo real e calcula a cobrança com **preço dinâmico** conforme a lotação do pátio.

## Estrutura da solução

| Projeto | Responsabilidade |
|---|---|
| `Estapar.Api` | API REST (controllers, Swagger, versionamento, Hub SignalR) |
| `Estapar.Application` | Regras de negócio (services), configuração de DI, middlewares |
| `Estapar.Domain` | Entidades, DTOs, contratos, validações e exceções |
| `Estapar.Infraestructure` | EF Core (PostgreSQL), repositórios, migrations e serviço em background |
| `SimuladorEstapar` | Worker que simula tráfego de veículos consumindo a API e o Hub |

## Como executar

### Pré-requisitos
- .NET 10 SDK
- PostgreSQL (local ou container)

### 1. Banco de dados
Suba um PostgreSQL local, por exemplo via Docker:

```bash
docker run --name estapar-postgres -e POSTGRES_PASSWORD=12099977 -p 5432:5432 -d postgres
```

A connection string padrão está em `src/Estapar.Api/appsettings.json` (`ConnectionStrings:DataBase`), podendo ser sobrescrita pela variável de ambiente `POSTGRES_DATABASE`. As migrations são aplicadas automaticamente na inicialização da API — não é necessário rodar `dotnet ef` manualmente.

### 2. Executar a API
```bash
dotnet run --project src/Estapar.Api/Estapar.Api.csproj
```
A API sobe em `http://localhost:5292`. O Swagger fica disponível em `/swagger` para explorar e testar os endpoints.

### 3. Executar o simulador (opcional)
Com a API em execução, o simulador gera entradas e saídas de veículos automaticamente:
```bash
dotnet run --project SimuladorEstapar/SimuladorEstapar.csproj
```
O endereço da API/Hub é configurado em `SimuladorEstapar/appsettings.json` (`Estapar:ApiBaseUrl` e `Estapar:HubBaseUrl`).

## Como funciona

1. **Cadastro**: um *Park* (pátio) é criado com suas *Lanes* (cancelas de entrada/saída), *Garages* (vagas) e uma tabela de preços (valor por hora + tempo de tolerância).
2. **Webhook** (`POST /webhook`) recebe os eventos das cancelas e sempre responde 200 — violações de regra (pátio lotado, veículo já dentro) geram apenas um registro de tráfego de erro, sem quebrar a integração:
   - `ENTRY` — valida vaga disponível, calcula o preço de entrada e registra o tráfego.
   - `PARKED` — confirma o veículo fisicamente estacionado e o vincula a uma vaga.
   - `EXIT` — registra a saída, gera a transação de cobrança e libera a vaga.
3. **Preço dinâmico**: o valor da hora é ajustado conforme a ocupação do pátio no momento da entrada:

   | Ocupação | Ajuste |
   |---|---|
   | < 25% | -10% |
   | 25% – 50% | preço normal |
   | 50% – 75% | +10% |
   | > 75% | +25% |
   | 100% | entrada bloqueada |

4. **Notificação em tempo real**: cada entrada é publicada em um canal dedicado à lane; um serviço em background consome esse canal e notifica os ouvintes conectados ao Hub SignalR (`/hubs/lane`), simulando a abertura da cancela.
5. **Consultas**: também é possível consultar as vagas ocupadas e o faturamento (`revenue`) por pátio e data através da API.
