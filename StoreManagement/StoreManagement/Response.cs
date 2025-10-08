namespace StoreManagement
{
    public class Response<T>
    {
        public int Status { get; set; }       // HTTP status code
        public string Message { get; set; }   // Thông báo
        public T Data { get; set; }           // Dữ liệu trả về

        public Response() { }

        public Response(int status, string message, T data)
        {
            Status = status;
            Message = message;
            Data = data;
        }

        public static Response<T> Success(T data, string message = "OK")
        {
            return new Response<T>(200, message, data);
        }

        public static Response<T> Fail(string message, int status = 400)
        {
            return new Response<T>(status, message, default(T));
        }
    }
}
