using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System;

namespace JasperFx.Core;

public static class StringExtensions
{
    /// <summary>
    /// "Elid" a string longer than the designated length
    /// </summary>
    /// <param name="longString"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string Elid(this string longString, int length)
    {
        if (longString.Length > length)
        {
            return string.Concat(longString.AsSpan(0, length - 3), "...");
        }

        return longString;
    }
    
    /// <summary>
    /// If the path is rooted, just returns the path.  Otherwise,
    /// combines root & path
    /// </summary>
    /// <param name="path"></param>
    /// <param name="root"></param>
    /// <returns></returns>
    public static string CombineToPath(this string path, string root)
    {
        if (Path.IsPathRooted(path)) return path;

        return Path.Combine(root, path);
    }

    public static void IfNotNull(this string? target, Action<string> continuation)
    {
        if (target != null)
        {
            continuation(target);
        }
    }

    public static string ToFullPath(this string path)
    {
        return Path.GetFullPath(path);
    } 

    /// <summary>
    /// Retrieve the parent directory of a directory or file
    /// Shortcut to Path.GetDirectoryName(path)
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string? ParentDirectory(this string path)
    {
        return Path.GetDirectoryName(path.TrimEnd(Path.DirectorySeparatorChar));
    }

    /// <summary>
    /// Equivalent of FileSystem.Combine( [Union of path, parts] )
    /// </summary>
    /// <param name="path"></param>
    /// <param name="parts"></param>
    /// <returns></returns>
    public static string AppendPath(this string path, params string[] parts)
    {
        return Path.Combine(new string[]{path}.Concat(parts).ToArray());
    }

    public static string PathRelativeTo(this string path, string root)
    {
        var pathParts = path.getPathParts();
        var rootParts = root.getPathParts();

        var length = pathParts.Count > rootParts.Count ? rootParts.Count : pathParts.Count;
        for (int i = 0; i < length; i++)
        {
            if (pathParts.First() == rootParts.First())
            {
                pathParts.RemoveAt(0);
                rootParts.RemoveAt(0);
            }
            else
            {
                break;
            }
        }

        for (int i = 0; i < rootParts.Count; i++)
        {
            pathParts.Insert(0, "..");
        }            

        return pathParts.Count > 0 ? Path.Combine(pathParts.ToArray()) : string.Empty;
    }

    public static bool IsEmpty([NotNullWhen(false)] this string? stringValue)
    {
        return string.IsNullOrEmpty(stringValue);
    }

    public static bool IsNotEmpty([NotNullWhen(true)] this string? stringValue)
    {
        return !string.IsNullOrEmpty(stringValue);
    }

    public static void IsNotEmpty([NotNullWhen(true)] this string? stringValue, Action<string> action)
    {
        if (stringValue.IsNotEmpty())
            action(stringValue);
    }

    public static bool ToBool(this string stringValue)
    {
        if (string.IsNullOrEmpty(stringValue)) return false;

        return bool.Parse(stringValue);
    }

    public static string ToFormat(this string stringFormat, params object[] args)
    {
        return string.Format(stringFormat, args);
    }

    /// <summary>
    /// Performs a case-insensitive comparison of strings
    /// </summary>
    public static bool EqualsIgnoreCase(this string thisString, string otherString)
    {
        return thisString.Equals(otherString, StringComparison.CurrentCultureIgnoreCase);
    }

    /// <summary>
    /// Converts the string to Title Case
    /// </summary>
    public static string Capitalize(this string stringValue)
    {
        StringBuilder result = new StringBuilder(stringValue);
        result[0] = char.ToUpper(result[0]);
        for (int i = 1; i < result.Length; ++i)
        {
            if (char.IsWhiteSpace(result[i - 1]) && !char.IsWhiteSpace(result[i]))
                result[i] = char.ToUpper(result[i]);
        }
        return result.ToString();
    }

    /// <summary>
    /// Formats a multi-line string for display on the web
    /// </summary>
    /// <param name="plainText"></param>
    public static string ConvertCRLFToBreaks(this string plainText)
    {
        return new Regex("(\r\n|\n)").Replace(plainText, "<br/>");
    }

    /// <summary>
    /// Returns a DateTime value parsed from the <paramref name="dateTimeValue"/> parameter.
    /// </summary>
    /// <param name="dateTimeValue">A valid, parseable DateTime value</param>
    /// <returns>The parsed DateTime value</returns>
    public static DateTime ToDateTime(this string dateTimeValue)
    {
        return DateTime.Parse(dateTimeValue);
    }

    public static string ToGmtFormattedDate(this DateTime date)
    {
        return date.ToString("yyyy'-'MM'-'dd hh':'mm':'ss tt 'GMT'");
    }

    public static string[] ToDelimitedArray(this string content)
    {
        return content.ToDelimitedArray(',');
    }

    public static string[] ToDelimitedArray(this string content, char delimiter)
    {
        string[] array = content.Split(delimiter);
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = array[i].Trim();
        }

        return array;
    }

    public static bool IsValidNumber(this string number)
    {
        return IsValidNumber(number, CultureInfo.CurrentCulture);
    }

    public static bool IsValidNumber(this string number, CultureInfo culture)
    {
        string _validNumberPattern =
            @"^-?(?:\d+|\d{1,3}(?:" 
            + culture.NumberFormat.NumberGroupSeparator + 
            @"\d{3})+)?(?:\" 
            + culture.NumberFormat.NumberDecimalSeparator + 
            @"\d+)?$";

        return new Regex(_validNumberPattern, RegexOptions.ECMAScript).IsMatch(number);
    }

    public static IList<string> getPathParts(this string path)
    {
        return path.Split(new[] {Path.DirectorySeparatorChar}, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
		
    public static string? DirectoryPath(this string path)
    {
        return Path.GetDirectoryName(path);
    }

    /// <summary>
    /// Reads text and returns an enumerable of strings for each line
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static IEnumerable<string> ReadLines(this string text)
    {
        var reader = new StringReader(text);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return line;
        }
    }

    /// <summary>
    /// Reads text and calls back for each line of text
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static void ReadLines(this string text, Action<string> callback)
    {
        var reader = new StringReader(text);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            callback(line);
        }
    }

    /// <summary>
    /// Just uses MD5 to create a repeatable hash
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string ToHash(this string text)
    {
        var parts = MD5.HashData(Encoding.UTF8.GetBytes(text));
        return Convert.ToHexString(parts).ToLowerInvariant();
    }

    /// <summary>
    /// Splits a camel cased string into seperate words delimitted by a space
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string SplitCamelCase(this string str)
    {
        return Regex.Replace(Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
    }

    /// <summary>
    /// Splits a pascal cased string into seperate words delimitted by a space
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string SplitPascalCase(this string str)
    {
        return SplitCamelCase(str);
    }
        
    public static string ToCamelCase(this string s)
    {
        if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
        {
            return s;
        }

        return string.Create(s.Length, s, (span, str) =>
        {
            str.CopyTo(span);

            for (int i = 0; i < span.Length; i++)
            {
                if (i == 1 && !char.IsUpper(span[i]))
                {
                    break;
                }

                bool hasNext = (i + 1 < span.Length);
                if (i > 0 && hasNext && !char.IsUpper(span[i + 1]))
                {
                    break;
                }

                span[i] = char.ToLowerInvariant(span[i]);
            }
        });
    }

    public static TEnum ToEnum<TEnum>(this string text) where TEnum : struct
    {
        var enumType = typeof (TEnum);
        if(!enumType.GetTypeInfo().IsEnum) throw new ArgumentException("{0} is not an Enum".ToFormat(enumType.Name));
        return Enum.Parse<TEnum>(text, true);
    }

    /// <summary>
    /// Wraps a string with parantheses.  Originally used to file escape file names when making command line calls
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static string FileEscape(this string file)
    {
        return "\"{0}\"".ToFormat(file);
    }
        
    /// <summary>
    /// Replace only the first instance of the "search" string with the value
    /// of "replace"
    /// </summary>
    /// <param name="text"></param>
    /// <param name="search"></param>
    /// <param name="replace"></param>
    /// <returns></returns>
    public static string ReplaceFirst(this string text, string search, string replace)
    {
        int pos = text.IndexOf(search);
        if (pos < 0)
        {
            return text;
        }

        return string.Concat(text.AsSpan(0, pos), replace, text.AsSpan(pos + search.Length));
    }


    /// <summary>
    /// string.Contains() with OrdinalIgnoreCase semantics
    /// </summary>
    /// <param name="source"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool ContainsIgnoreCase(this string source, string value)
    {
        return source.Contains(value, StringComparison.OrdinalIgnoreCase);
    }
    
    /// <summary>
    /// Concatenates a string between each item in a list of strings
    /// </summary>
    /// <param name="values">The array of strings to join</param>
    /// <param name="separator">The value to concatenate between items</param>
    /// <returns></returns>
    public static string Join(this string[] values, string separator)
    {
        return string.Join(separator, values);
    }

    /// <summary>
    /// Concatenates a string between each item in a sequence of strings
    /// </summary>
    /// <param name="values"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string Join(this IEnumerable<string> values, string separator)
    {
        return Join(values.ToArray(), separator);
    }
}