using System.Text;

public static class Identifier
{
    public static string Clean(string identifier)
    {
        if (identifier.Length == 0)
        {
            return string.Empty;
        }

        StringBuilder sb = new StringBuilder(identifier);

        // Replace ' ' with '_'
        sb.Replace(' ', '_');

        // Replace kebab-case with camelCase
        int indexOfHyphen = sb.ToString().IndexOf('-');
        sb.Replace("-" + sb[indexOfHyphen + 1], sb[indexOfHyphen + 1].ToString().ToUpper());

        // Replace control characters with "CTRL";
        // Omit characters that are not letters
        for (int i = 0; i < sb.Length; i++)
        {
            if (char.IsControl(sb[i]))
            {
                sb.Replace(sb[i].ToString(), "CTRL");
            }
            else if ((!char.IsLetter(sb[i]) && sb[i] != '_') || ((int)sb[i] >= 945 && (int)sb[i] <= 969))
            {
                sb.Remove(i, 1);
                i--;
            }
        }

        return sb.ToString();
    }
}
