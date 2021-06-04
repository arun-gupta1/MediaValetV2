using System.Text.Json;

namespace SupervisorAPI.Model
{
    public class ConfirmationResponse
    {
        public string OrderID { get; set; }
        public string AgentId { get; set; }
        public string OrderStatus { get; set; }
        public int StatusCode { get; set; }
        public string FaultMessage { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
