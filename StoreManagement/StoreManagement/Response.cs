
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
        public static Response<object> OnlyMessage(string message)
        {
            return new Response<object>
            {
                Message = message,
                Data = null
            };
        }
    }
}