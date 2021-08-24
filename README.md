If you are here because of the articles at [dev.to](https://dev.to/), you can jump
to the English section of the README by clicking [here](#introduction-to-microservices)

# Introdução aos Microsserviços

Esse repositório contém o código para a aplicação de exemplo básico de uma arquitetura
de microsserviços para apresentação no Chapter Back-End (18/08/2021).

O exemplo contido aqui engloba os seguintes padrões da arquitetura:
- API Gateway;
- Service Registry;
- Load Balancer;
- Comunicação entre serviços (Message Broker);
- Circuit Breaker
- Observabilidade - Health Checks

O intuito deste projeto foi/é realizar um estudo de alguns dos padrões básicos da arquitetura,
podendo ser evoluído com o passar do tempo para aprofundar os conceitos. Sugestões de evolução,
ideias e contribuições são bem-vindas.

## Desenvolvimento

Para desenvolver código novo para aplicação, são necessários alguns softwares:
### Consul

É o Service Registry utilizado na aplicação. Pode ser baixado [aqui](https://www.consul.io/downloads).
Após o download, extraia o executável para pasta de sua preferência, e adicione ao PATH do sistema.
O software possui versões para Windows, MAC e Linux.

### Docker (opcional)

Para disponibilizar o banco de dados e o message broker utilizados na aplicação, foi utilizado o Docker.
Essa parte é opcional, já que é possível instalar o PostgreSQL e RabbitMQ diretamente na máquina do desenvolvedor. Porém, a utilização do Docker simplifica o processo.

Ainda, o banco de dados PostgreSQL foi utilizado por uma preferência pessoal. Fique a vontade para utilizar o banco de dados de sua preferência.

### Primeiros Passos

Após a instalação do Consul, banco de dados e message broker, execute um restore da solução para instalar os pacotes necessários:

```bash
dotnet restore
```

Após a instalação dos pacotes, certifique-se de que o banco de dados e o message broker estejam
funcionando. Caso opte por utilizar o docker, basta executar:

```bash
docker compose up       # Na primeira execução ou
docker compose start    # Nas demais execuções
```

Inicie a execução do Service Registry. Para isso, execute:

```bash
consul agent -dev
```

Para verificar que o Service Registry iniciou, acesse o endereço: http://localhost:8500/

Com o nosso Service Registry executando, iremos executar as migrations do projeto 'users-api'.
Para isso, iremos executar:

```bash
dotnet ef -p Services/users-api/Users.API database update --context UsersDbContext
```

Por fim, iremos iniciar todos os projetos:

```bash
dotnet run -p ApiGateways

dotnet run -p Services/users-api/Users.API

dotnet run -p Services/notifications-api/Notifications.API
```

Pronto! A aplicação já está executando e você pode testar os endpoints a vontade. Para
verificar os endpoints da 'users-api', acesse: https://localhost:5005/swagger/index.html.
Para acessar a 'users-api' pelo API Gateway, você pode usar o endereço base:
https://localhost:44340/api/gateway/{users-api-endpoints}, onde 'users-api-endpoints'
estão localizados no Swagger indicado (menos a parte de '/api/v1').

# Introduction to Microservices
This repository contains code for a basic application example that uses the microservices
architecture.

This example covers the following architectural patterns:
- API Gateway;
- Service Registry;
- Load Balancer;
- Service communication (messaging);
- Circuit Breaker;
- Observability - Health Checks.

This repository idea was/is to study some of the basic architectural patters of microservices,
it can be evolved over time to deepen the concepts. Ideas, suggestions and contributions are
very welcome.

## Development

To develop new code to the application, you'll need some softwares:
### Consul

It's the Service Registry used in the application. It can be downloaded [here](https://www.consul.io/downloads). After the download, extract the executable to the directory of your preference, and add it
to the system PATH. This software works on Windows, MAC and Linux.

### Docker (optional)

To provide a database and the application message broker, I used Docker. This part is optional,
since it is possible to install PostgreSQL and RabbitMQ directly on the developer's computer. But
using docker simplifies the process.

Also, PostgreSQL was used because of a personal preference. Feel free to use another database.

### First Steps
After installing Consul, the database and the message broker, execute a solution restore to install
all the necessary packages:

```bash
dotnet restore
```

After installing the packages, make sure that the database and the message broker are working. Should
you choose to use docker, just execute:

```bash
docker compose up       # On the first execution; or
docker compose start    # On the following executions
```

Start the Service Registry. For that, type:

```bash
consul agent -dev
```

To verify that the Service Registry is running, access the address: http://localhost:8500/

With our Service Registry running, we'll run the 'users-api' project migrations. To do that, run:

```bash
dotnet ef -p Services/users-api/Users.API database update --context UsersDbContext
```

Finally, we'll start all of our projects:

```bash
dotnet run -p ApiGateways

dotnet run -p Services/users-api/Users.API

dotnet run -p Services/notifications-api/Notifications.API
```

Done! The application is running and you can test the endpoints at will. To verify
the 'users-api' endpoints, access: https://localhost:5005/swagger/index.html.
To access the 'users-api' through the API Gateway, you can use the base address
https://localhost:44340/api/gateway/{users-api-endpoints}, where 'users-api-endpoints'
are the ones located at the Swagger address (minus the '/api/v1' stuff!).
