using AuthorizationServer.Contacts;
using MassTransit;
using SmileSLogin.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmileSLogin.Consumers
{
    public class ChangePasswordConsumer : IConsumer<ChangePasswordRequest>
    {
        public async Task Consume(ConsumeContext<ChangePasswordRequest> context)
        {
            ChangePasswordRequest message = context.Message;

            if (!AuthMethod.ChangePassword(message.UserName, message.NewPassword))
            {
                await context.RespondAsync(CreateFailResponse(message, "เกิดข้อผิดพลาด ไม่สามารถเปลี่ยนรหัสผ่าน SmileS ได้"));
                return;
            }

            //Reset Password SSS
            if (!AuthMethod.ChangePassword_SSS(message.UserName, message.NewPassword))
            {
                await context.RespondAsync(CreateFailResponse(message, "เกิดข้อผิดพลาด ไม่สามารถเปลี่ยนรหัสผ่าน SSS ได้"));
                return;
            }

            await context.RespondAsync(CreateSuccessResponse(message));
        }

        private ChangePasswordSuccess CreateSuccessResponse(ChangePasswordRequest request)
        {
            return new ChangePasswordSuccess
            {
                Id = request.Id
            };
        }

        private ChangePasswordFail CreateFailResponse(ChangePasswordRequest request, string error)
        {
            return new ChangePasswordFail
            {
                Id = request.Id,
                Error = error
            };
        }
    }

    public class ChangePasswordConsumerDefinition : ConsumerDefinition<ChangePasswordConsumer>
    {
        public ChangePasswordConsumerDefinition()
        {
            EndpointName = Properties.Settings.Default.SmileSLoginQueue;
            ConcurrentMessageLimit = 1;
        }
    }
}
