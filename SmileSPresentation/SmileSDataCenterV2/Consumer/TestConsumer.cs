using MassTransit;
using System;
using System.Threading.Tasks;
using static SmileSDataCenterV2.DTOs.Contracts;

namespace SmileSDataCenterV2.Consumer
{
    public class TestConsumer : IConsumer<OrderCreated>
    {
        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            try
            {
                var msg = context.Message;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}