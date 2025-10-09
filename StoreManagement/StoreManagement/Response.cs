namespace StoreManagement
{
    public class Response
    {
        public int Status { get; set; }       // HTTP status code
        public string Message { get; set; }   // Thông báo
        public object Data { get; set; }           // Dữ liệu trả về

        public Response() { }

        public Response(int status, string message, object data)
        {
            Status = status;
            Message = message;
            Data = data;
        }

        public static Response Success(object data, string message = "OK")
        {
            return new Response
            {
                Status = 200,
                Message = message,
                Data = data
            };
        }

        public static Response Fail(string message, int status = 400)
        {
            return new Response
            {
                Status = status,
                Message = message,
                Data = null
            };
        }
    }
}
