# 🚚 Delivery System (.NET 8 + RabbitMQ)

Este projeto demonstra uma **Arquitetura Distribuída e Assíncrona** utilizando **C# .NET 8** e **RabbitMQ**. O objetivo é simular um sistema de delivery real, onde o recebimento do pedido é desacoplado do seu processamento para garantir alta escalabilidade.

## 🏛️ Arquitetura do Projeto

O sistema é dividido em dois microsserviços principais que se comunicam via mensageria:

1.  **🍽️ DeliveryAPI (Producer/Garçom):**
    * Recebe o pedido do cliente via HTTP POST.
    * Valida os dados.
    * Publica o pedido na fila `pedidos_delivery` do RabbitMQ.
    * Responde imediatamente ao cliente ("Pedido Recebido"), liberando a conexão.

2.  **👨‍🍳 DeliveryWorker (Consumer/Cozinha):**
    * Aplicação Console rodando em background.
    * Escuta a fila `pedidos_delivery`.
    * Processa os pedidos assim que chegam (simula o preparo).
    * Garante que nenhum pedido seja perdido, mesmo se o serviço reiniciar.

---

## 🚀 Tecnologias Utilizadas

* **[C#](https://docs.microsoft.com/en-us/dotnet/csharp/)** - Linguagem principal.
* **[.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)** - Framework de alta performance.
* **[RabbitMQ](https://www.rabbitmq.com/)** - Message Broker para comunicação assíncrona.
* **[Docker](https://www.docker.com/)** - Containerização do RabbitMQ.
* **Swagger** - Documentação automática da API.

---

## ⚙️ Como Rodar o Projeto

### Pré-requisitos
* [.NET 8 SDK](https://dotnet.microsoft.com/download) instalado.
* [Docker](https://www.docker.com/products/docker-desktop) rodando.

### 1. Subir o RabbitMQ (Docker)
Execute o comando abaixo para iniciar o servidor de mensageria:
```bash
docker run -d --hostname meu-rabbit --name rabbitmq -p 15672:15672 -p 5672:5672 rabbitmq:3-management
