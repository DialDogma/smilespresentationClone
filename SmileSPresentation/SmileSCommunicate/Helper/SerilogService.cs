using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using SmileSCommunicate.Models;
using System;

namespace SmileSCommunicate.Helper
{
    public static class SerilogService
    {
        public static void WriteSerilog()
        {
            try
            {
                var db = new CommunicateV1Entities();
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .WriteTo.MSSqlServer(
                                        connectionString: db.Database.Connection.ConnectionString,
                                        sinkOptions: new MSSqlServerSinkOptions
                                        {
                                            SchemaName = "EventLogging",
                                            TableName = "Communicate",
                                            AutoCreateSqlTable = true,
                                            BatchPostingLimit = 50,
                                            BatchPeriod = new TimeSpan(0, 00, 00, 05, 00)
                                        },
                                        restrictedToMinimumLevel: LogEventLevel.Information)
                    .CreateLogger();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "[SmileSSMSSendList][WriteSerilog] Error starting");
            }
        }
    }
}