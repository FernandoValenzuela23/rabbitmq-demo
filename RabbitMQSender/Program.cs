/*
 * CLIENT (SENDER)
    START RABBITMQ CONTAINER
    docker run -d --hostname rmq --name rabbit-server -p 8080:15672 -p 5672:5672 rabbitmq:3-management
 
 */



using RabbitMQ.Client;
using System.Text;


ConnectionFactory factory = new();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbit Sender App";
IConnection cnn = factory.CreateConnection();
IModel channel = cnn.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-name";
string queueName = "DemoQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey, null);

// Sending process

// One message at time
//byte[] bytes = Encoding.UTF8.GetBytes("Hola gatos B...");
//channel.BasicPublish(exchangeName, routingKey, null, bytes);

// bucle of messages
for (int i = 0; i < 60; i++)
{
	byte[] bytes = Encoding.UTF8.GetBytes($"Hola gatos... Saludo {i}");
	channel.BasicPublish(exchangeName, routingKey, null, bytes);
    Thread.Sleep(500);
}


channel.Close();
cnn.Close();
