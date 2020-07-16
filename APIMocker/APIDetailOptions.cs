namespace APIMocker
{
    public class APIDetailOptions
    {
        public string Path { get; set; }
        public string QueryString { get; set; }
        public string ResponseBody { get; set; }
        public int StatusCode { get; set; }
    }
}
