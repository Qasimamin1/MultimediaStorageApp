namespace Shared
{
    //The Message class in the Shared namespace appears to be a utility class
    //for storing common or standard messages that the application can use across
    //various layers, particularly in responses and error handling.
    public class Message
    {
        public static readonly string Success = "Request processed successfully";

        public static readonly string Exists = "Already Exists";

        public static readonly string Error = "Something went wrong";
        public static string KeyNotFound(string key) => $"{key} not found";
    }
}
