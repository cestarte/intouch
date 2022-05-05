
public static class CorrespondenceCalculations {
  public static string CalculateWhenText(DateTime when)
  {
    var dateText = when.ToShortDateString();
    var daysAgo = (DateTime.Now.Date - when.Date).Days;
    string message = "";

    if (daysAgo < 1)
      message = $"Today, {dateText}";
    else if (daysAgo < 2)
      message = $"Yesterday, {dateText}";
    else
      message = $"{daysAgo} days ago. {dateText}";

    return message;
  }
}
