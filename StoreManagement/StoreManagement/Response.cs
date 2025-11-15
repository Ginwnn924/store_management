
namespace StoreManagement
{
    public class Response<TModel> where TModel : class
    {
        public int Status { get; set; }       // HTTP status code
        public string? Message { get; set; }   // Thông báo
        public TModel? Data { get; set; }         // Dữ liệu trả về

        public Response() { }

        public Response(int status, string message, TModel data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
    public static class Response
    {
        public static Response<TModel> Success<TModel>(TModel data, string message = "OK")
            where TModel : class
        {
            return new Response<TModel>
            {
                Status = 200,
                Message = message,
                Data = data
            };
        }

        public static Response<object> Fail(string message, int status = 400)
        {
            return new Response<object>
            {
                Status = status,
                Message = message,
                Data = null
            };
        }

        public static Response<TModel> Fail<TModel>(string message, int status = 400, TModel? data = null)
            where TModel : class
        {
            return new Response<TModel>
            {
                Status = status,
                Message = message,
                Data = null
            };
        }
    }
}