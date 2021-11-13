using System;
using System.Collections.Generic;
using System.Linq;

namespace BragaPets.Domain.DTOs.Responses
{
    public class BaseResponse
    {
        public List<DataResponse> Data  { get; }

        public BaseResponse(object attributes, string type)
        {
            Data = new List<DataResponse>();
            if (attributes != null)
                Data.Add(new DataResponse(attributes, type));    
        }
        
        public BaseResponse(IEnumerable<object> attributes, string type)
        {
            Data = new List<DataResponse>();
            if (attributes != null)
                attributes.ToList().ForEach(a => Data.Add(new DataResponse(a, type)));    
        }
    }

    public class DataResponse
    {
        public string Type { get; set; }
        public Guid Id { get; set; }
        public object Attributes { get; set; }

        public DataResponse(object attributes,string type)
        {
            Attributes = attributes;
            Type = type;
            Id = (Guid) attributes.GetType().GetProperty("Uid")?.GetValue(attributes);
        }
    }
}