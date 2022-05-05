namespace application.Extensions;
public static class ExceptionExtensions {
  public static string FriendlyMessage(this Exception self)
  {
    return self.ToString(); // TODO
  }
}