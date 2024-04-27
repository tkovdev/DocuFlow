namespace DataAccess.Exceptions;

/**
 * <summary>
 * Exception for Warnings. This is used when a known error occurs in the system. This inherits from the Exception class and provides a cleaner exception event.
 * For example, this exception will be thrown when search results in no results. A message indicating no results will be returned.
 * </summary>
 * <seealso cref="System.Exception"/>
 */
public class DataAccessWarningException : Exception
{
    public DataAccessWarningException(string message)
        : base(message)
    {
    }
}

/**
 * <summary>
 * Exception for Critical Errors. This is used when an unknown error occurs in the system. This inherits from the Exception class and provides a cleaner exception event.
 * For example, this exception will be thrown when a save fails unexpectedly. A message indicating the failure will be returned.
 * </summary>
 * <seealso cref="System.Exception"/>
 */
public class DataAccessCriticalException : Exception
{
    public DataAccessCriticalException(string message)
        : base(message)
    {
    }
}