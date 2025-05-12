using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System.Text;

namespace edusync.Models
{
    public class EventHubProducer
    {
        private readonly EventHubProducerClient _client;

        public EventHubProducer(IConfiguration configuration)
        {
            var connectionString = configuration["EventHub:ConnectionString"];
            var eventHubName = configuration["EventHub:HubName"];
            _client = new EventHubProducerClient(connectionString, eventHubName);
        }

        public async Task SendAsync(string json)
        {
            using EventDataBatch eventBatch = await _client.CreateBatchAsync();
            eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(json)));
            await _client.SendAsync(eventBatch);
        }
    }
}
