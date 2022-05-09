
public static class CorrespondenceCalculations
{
  private static int MAX_STRING_LENGTH = 75;

  /// <summary>
  /// Formats date and time with a few helper words.
  /// </summary>
  public static string CalculateWhenText(DateTime when)
  {
    var dateText = when.ToShortDateString();
    var daysAgo = (DateTime.Now.Date - when.Date).Days;
    string message = "";

    if (daysAgo < 1)
      message = $"Today, {dateText}";
    else if (daysAgo < 2)
      message = $"Yesterday, {dateText}";
    // TODO add last week, last month, "2 months ago", ... "a year ago"
    else
      message = $"{daysAgo} days ago. {dateText}";

    return message;
  }

  /// <summary>
  /// Truncates long strings to fit in a table cell. If the string is truncated,
  /// ellipsis marks will be appended.
  /// </summary>
  public static string? FormatDescriptionText(string? text)
  {
    if (text == null)
      return text;
    else if (text.Length >= MAX_STRING_LENGTH)
      text += "...";

    return text;
  }
}
