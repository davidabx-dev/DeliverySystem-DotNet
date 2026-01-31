using Microsoft.AspNetCore.Mvc;
using DeliveryApi.Models;
using RabbitMQ.Client; // Biblioteca do Coelho
using System.Text;
using System.Text.Json;

namespace DeliveryApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private static readonly List<Order> _orders = new();

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        if (order.TotalPrice <= 0) return BadRequest("O preço deve ser maior que zero");

        // 1. Salva na memória (como fazíamos antes)
        order.Status = "Enviado para Cozinha"; // Atualizamos o status
        _orders.Add(order);

        // ============================================================
        // 2. MENSAGERIA: Avisa o RabbitMQ (A Cozinha)
        // ============================================================
        
        // Configura a conexão com o Docker (localhost)
        var factory = new ConnectionFactory { HostName = "localhost" };
        
        // Cria a conexão e o canal
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        // Cria a Fila "pedidos_delivery" (se não existir)
        await channel.QueueDeclareAsync(queue: "pedidos_delivery", durable: false, exclusive: false, autoDelete: false, arguments: null);

        // Transforma o Pedido em Texto JSON e depois em Bytes
        string message = JsonSerializer.Serialize(order);
        var body = Encoding.UTF8.GetBytes(message);

        // Publica a mensagem na fila
        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "pedidos_delivery", body: body);
        // ============================================================

        return CreatedAtAction(nameof(GetAll), new { id = order.Id }, order);
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_orders);
}