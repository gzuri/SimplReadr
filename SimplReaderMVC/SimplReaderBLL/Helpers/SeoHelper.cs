using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SimplReaderBLL.Helpers {
	public class SeoHelper {
		/// <summary>
		/// Converts a content into a string that can be appended to a Uri.
		/// TODO should have max size option
		/// </summary>
		public static string GenerateSEOName(string content) {
			if (!string.IsNullOrEmpty(content)) {
				content = NormalizeString(content);

				// Replaces all non-alphanumeric character by a space
				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < content.Length; i++) {
					builder.Append(char.IsLetterOrDigit(content[i]) ? content[i] : ' ');
				}

				content = builder.ToString();

				// Replace multiple spaces into a single dash
				content = Regex.Replace(content, @"[ ]{1,}", @"-", RegexOptions.None);
			}

			return content;
		}

		/// <summary>
		/// Strips the value from any non english character by replacing thoses with their english equivalent.
		/// </summary>
		/// <param name="value">The string to normalize.</param>
		/// <returns>A string where all characters are part of the basic english ANSI encoding.</returns>
		/// <seealso cref="http://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net"/>
		private static string NormalizeString(string value) {
			string normalizedFormD = value.Normalize(NormalizationForm.FormD);
			StringBuilder builder = new StringBuilder();

			for (int i = 0; i < normalizedFormD.Length; i++) {
				UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(normalizedFormD[i]);
				if (uc != UnicodeCategory.NonSpacingMark) {
					builder.Append(normalizedFormD[i]);
				}
			}

			return builder.ToString().Normalize(NormalizationForm.FormC);
		}
	}
}
