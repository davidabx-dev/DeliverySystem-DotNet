using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// 1. Conecta no RabbitMQ (Docker)
var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

// 2. Garante que a fila "pedidos_delivery" existe
await channel.QueueDeclareAsync(queue: "pedidos_delivery", durable: false, exclusive: false, autoDelete: false, arguments: null);

Console.WriteLine(" [*] Aguardando pedidos na cozinha... (Aperte ENTER para sair)");

// 3. Cria o Consumidor
var consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    
    // Simula o trabalho da cozinha
    Console.WriteLine($" [x] PEDIDO RECEBIDO: {message}");
    Console.WriteLine(" [x] Preparando prato... 🍳");
    await Task.Delay(2000); // Espera 2 segundos
    Console.WriteLine(" [x] Pedido PRONTO para entrega! 🚀");
    Console.WriteLine(" ------------------------------------------------");
};

// 4. Liga o consumidor
await channel.BasicConsumeAsync(queue: "pedidos_delivery", autoAck: true, consumer: consumer);

// Mantém rodando
Console.ReadLine();