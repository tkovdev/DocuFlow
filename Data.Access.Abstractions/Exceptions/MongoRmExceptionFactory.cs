namespace Data.Access.Abstractions.Exceptions;

public abstract class MongoRmExceptionFactory
{
    public static string DataAccessWarningExceptionType = "DataAccessWarningException";
    public static string DataAccessCriticalExceptionType = "DataAccessCriticalException";
    public static Exception DataAccessWarningException(string message)
    {
        var ex = new Exception(message);
        ex.Source = DataAccessWarningExceptionType;
        return ex;
    }
    
    public static Exception DataAccessCriticalException(string message)
    {
        var ex = new Exception(message);
        ex.Source = DataAccessCriticalExceptionType;
        return ex;
    }
}