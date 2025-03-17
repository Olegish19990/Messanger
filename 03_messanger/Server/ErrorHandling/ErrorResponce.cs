
namespace Server.ErrorHandling;

    public enum ErrorCode
    {
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 500,
        InvalidRequest = 400,
        Conflict = 409,
        AuthorizedError = 410,
}

    public class ErrorResponse
    {
        public ErrorCode Code { get; }
        public string Message { get; }

        private static readonly Dictionary<ErrorCode, string> ErrorMessages = new()
    {
        { ErrorCode.Unauthorized, "You are not authorized" },
        { ErrorCode.Forbidden, "Access denied" },
        { ErrorCode.NotFound, "Recipient not found" },
        { ErrorCode.InternalServerError, "Internal error" },
        { ErrorCode.InvalidRequest, "Incorrect request" },
        { ErrorCode.Conflict, "Data conflict" },
        { ErrorCode.AuthorizedError, "Wrong login or password" }
    };

        public ErrorResponse(ErrorCode code)
        {
            Code = code;
            Message = ErrorMessages.TryGetValue(code, out var message) ? message : "Unknown error";
        }
    }
