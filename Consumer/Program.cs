using Confluent.Kafka;

namespace Consumer;

class Consumer
{
    static void Main(string[] args)
    {
        const string topic = "DriverCreated";
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "consumer-group-2",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };


        CancellationTokenSource cts = new CancellationTokenSource();

        while (true)
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(
                consumerConfig).Build();
            try
            {
                consumer.Subscribe(topic);
                while (true)
                {
                    var cr = consumer.Consume(cts.Token);
                    Console.WriteLine(
                        $"Consumed event from topic {topic}: key = {cr.Message.Key} value = {cr.Message.Value}");
                }
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Error occured: {e.Error.Reason}");
                Thread.Sleep(10000);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}
