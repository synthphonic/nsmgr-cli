namespace Nautilus.SolutionExplorerClient;

public static class ExceptionExtension
{
    public static Exception GetFirstException(this Exception ex)
    {
        if (ex.InnerException == null)
        {
            return ex;
        } // end case
        else
        {
            return GetFirstException(ex.InnerException);
        } // recurse
    }
}