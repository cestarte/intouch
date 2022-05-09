using System.Text;

namespace application.Extensions;
public static class ExceptionExtensions
{
  public static string FriendlyMessage(this Exception self)
  {
    var sb = new StringBuilder();
    var ex = self;
    do
    {
      ex = ex.InnerException;
      sb.Append(ex?.Message);
      sb.Append(" ");
    } while (ex?.InnerException != null);
    return sb.ToString();
  }
}