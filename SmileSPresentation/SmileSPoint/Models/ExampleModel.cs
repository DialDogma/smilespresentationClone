using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSPoint.Models
{
    public class ExampleModel
    {
        public List<ExampleTimeline> GetExampleTimeLine()
        {
            var lst = new List<ExampleTimeline>();

            lst.Add(new ExampleTimeline()
            {
                Date = new DateTime(2018, 1, 1),
                TransType = "Pay",
                Amount = 1000,
                CashReceiveId = "TFHD001",
                QueueId = "",
                QueueClosedBy = ""
            });

            lst.Add(new ExampleTimeline()
            {
                Date = new DateTime(2018, 1, 2),
                TransType = "CreateQueue",
                Amount = 1000,
                CashReceiveId = "",
                QueueId = "QUE001",
                QueueClosedBy = ""
            });

            lst.Add(new ExampleTimeline()
            {
                Date = new DateTime(2018, 1, 3),
                TransType = "CloseQueue",
                Amount = 0,
                CashReceiveId = "",
                QueueId = "QUE001",
                QueueClosedBy = "Mark"
            });

            lst.Add(new ExampleTimeline()
            {
                Date = new DateTime(2018, 1, 4),
                TransType = "Pay",
                Amount = 2000,
                CashReceiveId = "TFHD002",
                QueueId = "",
                QueueClosedBy = ""
            });

            lst.Add(new ExampleTimeline()
            {
                Date = new DateTime(2018, 1, 5),
                TransType = "CreateQueue",
                Amount = 0,
                CashReceiveId = "",
                QueueId = "QUE002",
                QueueClosedBy = ""
            });

            lst.Add(new ExampleTimeline()
            {
                Date = new DateTime(2018, 1, 6),
                TransType = "Pay",
                Amount = 3000,
                CashReceiveId = "TFHD003",
                QueueId = "",
                QueueClosedBy = ""
            });

            lst.Add(new ExampleTimeline()
            {
                Date = new DateTime(2018, 1, 7),
                TransType = "Pay",
                Amount = 4000,
                CashReceiveId = "TFHD004",
                QueueId = "",
                QueueClosedBy = ""
            });

            lst.Add(new ExampleTimeline()
            {
                Date = new DateTime(2018, 1, 9),
                TransType = "CloseQueue",
                Amount = 0,
                CashReceiveId = "",
                QueueId = "QUE002",
                QueueClosedBy = "Bell"
            });

            return lst;
        }
    }

    public class ExampleTimeline
    {
        public DateTime Date { get; set; }
        public String TransType { get; set; }
        public int? Amount { get; set; }
        public string CashReceiveId { get; set; }
        public string QueueId { get; set; }
        public string QueueClosedBy { get; set; }
    }
}