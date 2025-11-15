
namespace StoreManagement
{
    public class Response<TModel> where TModel : class
    {
        public string? Message { get; set; }   // Thông báo
        public TModel? Data { get; set; }   // Dữ liệu trả về

        public Response() { }

        public Response(string message, TModel data)
        {
            Message = message;
            Data = data;
        }
    }
    public static class Response
    {
        public static Response<TModel> Create<TModel>(TModel data, string message = "OK")
            where TModel : class
        {
            return new Response<TModel>
            {
                Message = message,
                Data = data
            };
        }

        public static Response<object> OnlyMessage(string message)
        {
            return new Response<object>
            {
                Message = message,
                Data = null
            };
        }

        public static Response<TModel> Fail<TModel>(string message, TModel? data = null)
            where TModel : class
        {
            return new Response<TModel>
            {
                Message = message,
                Data = data
            };
        }
    }
}