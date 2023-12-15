using System;
using System.Diagnostics.Tracing;
using System.Text;
using System.Text.Json;
using System.Xml;
namespace HttpsTrace
{
    internal sealed class HttpEventListener : EventListener
    {
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            // Allow internal HTTP logging
            if (eventSource.Name == "Private.InternalDiagnostics.System.Net.Http")
            {
                EnableEvents(eventSource, EventLevel.LogAlways);
            }
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            // Log whatever other properties you want, this is just an example
            var sb = new StringBuilder().Append($"{eventData.TimeStamp:HH:mm:ss.fffffff}[{eventData.EventName}] ");
            if (eventData.EventName == "EventCounters"|| eventData.EventName=="Info")
              return;

            JsonSerializerOptions opt=new JsonSerializerOptions();
            opt.WriteIndented=true;
            opt.IgnoreNullValues = true;

            string jsonString = JsonSerializer.Serialize(eventData,opt);
              sb.Append(", \n");
              sb.Append(jsonString);
            //for (int i = 0; i < eventData.Payload?.Count; i++)
            //{
              
              //  if (i > 0)
                //    sb.Append(", ");
                //b.Append(eventData.PayloadNames?[i]).Append(": ").Append(eventData.Payload[i]);
            //}
            try
            {
                Console.WriteLine(sb.ToString());
            }
            catch { }
        }
    }
}
