﻿using System;
using System.Text;
using System.Text.RegularExpressions;

namespace i18n
{
    internal static class Extensions
    {
        /// <summary>
        /// String extension method to simplify testing for non-null/non-empty values.
        /// </summary>
        public static bool IsSet(
            this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Efficiently returns the number of time the specified char appears in the string.
        /// </summary>
        public static int CountOfChar(
            this string str,
            char ch)
        {
            int n = 0;
            foreach (char ch1 in str) {
                if (ch1 == ch) {
                    ++n; }
            }
            return n;
        }

        /// <summary>
        /// Isolates and returns the character sequence between any first and last quote chars.
        /// </summary>
        /// <param name="lhs">Subject string possibly containing a quoted sequence.</param>
        /// <param name="quotechar">Quote char, defaults to double quotes. May be a string of more than one character.</param>
        /// <returns>
        /// Any character sequence contained within the first and last occurence of quotechar.
        /// Empty string if the first and last occurrence of quotechar are adjacent chars.
        /// Null if no welformed quoted sequence found.
        /// </returns>
        public static string Unquote(this string lhs, string quotechar = "\"")
        {
            int begin = lhs.IndexOf(quotechar);
            if (begin == -1) {
                return null; }
            int end = lhs.LastIndexOf(quotechar);
            if (end <= begin) {
                return null; }
            return lhs.Substring(begin +1, end -begin -1);
        }

        /// <summary>
        /// Looks up in the subject string standard C escape sequences and converts them
        /// to their actual character counterparts.
        /// </summary>
        /// <seealso href="http://stackoverflow.com/questions/6629020/evaluate-escaped-string/8854626#8854626"/>
        public static string Unescape(this string s)
        {
            StringBuilder sb = new StringBuilder();
            Regex r = new Regex("\\\\[abfnrtv?\"'\\\\]|\\\\[0-3]?[0-7]{1,2}|\\\\u[0-9a-fA-F]{4}|.");
            MatchCollection mc = r.Matches(s, 0);

            foreach (Match m in mc) {
                if (m.Length == 1) {
                    sb.Append(m.Value);
                } else {
                    if (m.Value[1] >= '0' && m.Value[1] <= '7') {
                        int i = 0;

                        for (int j = 1; j < m.Length; j++) {
                            i *= 8;
                            i += m.Value[j] - '0';
                        }

                        sb.Append((char)i);
                    } else if (m.Value[1] == 'u') {
                        int i = 0;

                        for (int j = 2; j < m.Length; j++) {
                            i *= 16;

                            if (m.Value[j] >= '0' && m.Value[j] <= '9') {
                                i += m.Value[j] - '0';
                            } else if (m.Value[j] >= 'A' && m.Value[j] <= 'F') {
                                i += m.Value[j] - 'A' + 10;
                            } else if (m.Value[j] >= 'a' && m.Value[j] <= 'a') {
                                i += m.Value[j] - 'a' + 10;
                            }
                        }

                        sb.Append((char)i);
                    } else {
                        switch (m.Value[1]) {
                            case 'a':
                                sb.Append('\a');
                                break;
                            case 'b':
                                sb.Append('\b');
                                break;
                            case 'f':
                                sb.Append('\f');
                                break;
                            case 'n':
                                sb.Append('\n');
                                break;
                            case 'r':
                                sb.Append('\r');
                                break;
                            case 't':
                                sb.Append('\t');
                                break;
                            case 'v':
                                sb.Append('\v');
                                break;
                            default:
                                sb.Append(m.Value[1]);
                                break;
                        }
                    }
                }
            }

            return sb.ToString();
        }

        public static void PrependPath(this UriBuilder uriBuilder, string folder)
        {
            string s1 = uriBuilder.Path;
            uriBuilder.Path = "/" + folder;
            if (s1.IsSet() && s1 != "/") {
                uriBuilder.Path += s1; }
        }

        public static string UrlPrependPath(this string path, string folder)
        {
            StringBuilder sb = new StringBuilder(path.Length + folder.Length + 10);

            sb.Append("/");
            sb.Append(folder);
            if (path.IsSet() && path != "/") {
                if (path[0] != '/') {
                    sb.Append("/"); }
                sb.Append(path);
            }
            path = sb.ToString();
            return path;
        }
    }
}
