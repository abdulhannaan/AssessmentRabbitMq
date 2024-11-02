namespace InboundApi.Models
{
    public class MyRequestModel
    {
        public string Originator { get; set; }
        public string FileName { get; set; }
        public List<string> FileContentLines { get; set; }
    }

}
