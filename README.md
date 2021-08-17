# Chapter de Microsserviços

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
verificar os endpoints da 'users-api', acesse: https://localhost:5005/swagger/index.html
