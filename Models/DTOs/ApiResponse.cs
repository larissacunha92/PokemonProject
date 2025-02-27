namespace PokemonProject.Models.DTOs
{
    public class ApiResponse
    {
        public class Result<T>
        {
            public T? Data { get; set; }
            public string? ErrorMessage { get; set; }
            public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
            public static Result<T> Success(T data) => new() { Data = data };
            public static Result<T> Fail(string errorMessage) => new() { ErrorMessage = errorMessage };
        }
    }
}
