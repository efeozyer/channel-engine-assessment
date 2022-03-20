using System.Collections.Generic;

namespace ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine
{
    public abstract class ApiResponse
    {
        public int StatusCode { get; set; }
        public int? LogId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public ValidationErrors ValidationErrors { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Content { get; set; }
    }

    public class ValidationErrors
    {
        public string[] AdditionalProp1 { get; set; }
        public string[] AdditionalProp2 { get; set; }
        public string[] AdditionalProp3 { get; set; }

        public override string ToString()
        {
            var validationErrors = new List<string>();

            validationErrors.AddRange(AdditionalProp1);
            validationErrors.AddRange(AdditionalProp2);
            validationErrors.AddRange(AdditionalProp3);

            return string.Join(",", validationErrors);
        }

        public bool Any() 
        {
            return AdditionalProp1?.Length > 0 || AdditionalProp2?.Length > 0 || AdditionalProp3?.Length > 0;
        }
    }
}
