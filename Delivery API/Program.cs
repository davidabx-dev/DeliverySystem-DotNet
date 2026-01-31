var builder = WebApplication.CreateBuilder(args);

// --- Adiciona os serviços (Configuração) ---
builder.Services.AddControllers(); // Ativa os Controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- Configura o Pipeline (Como a requisição passa) ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers(); // Diz para o App usar as rotas que criamos

app.Run();