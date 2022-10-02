using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Repository_Pattern;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ignora o objeto quando o ciclo de refer�ncia � detectado durante a serializa��o - ex: Controller Categoria, Action GetCategoriasProdutos
builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });

// adicionando IUnitOfWork como servi�o, AddScoped - cada request cria um novo escopo de servi�o separado
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// injetando AutoMapper - para mapear DTOs automaticamente
var mappingConfig = new MapperConfiguration(x => {
    x.AddProfile(new MappingProfile());
});
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// adicionando servi�o Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

// validando JWT Token
// adiciona o manipulador de autentica��o
// define o esquema de autentica��o usado : Bearer
// valida o emissor, a audiencia e a chave
// usando a chave secreta e valida a assinatura
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime =  true,
    ValidAudience = builder.Configuration["TokenConfiguration:Audience"],
    ValidIssuer = builder.Configuration["TokenConfiguration:Issuer"],
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
});



// injetando DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// adicionando middleware de roteamento
app.UseRouting();

// adicionando middleware de autentica��o
app.UseAuthentication();

// adicionando middlewate que habilita a autoriza��o 
app.UseAuthorization();

app.MapControllers();

app.Run();
