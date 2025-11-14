using BA.Utility.Folder;
using Newtonsoft.Json;

namespace BA.Utility.Content
{
    public static class ContentLoader
    {
        private static Dictionary<string, string> en_US = [];

        public static void LanguageLoader(string folderPath)
        {
            try
            {
                string languageContent, languageData;
                if (en_US == null || en_US.Count <= 0)
                {
                    languageContent = Path.Combine(folderPath + FolderLocation.EN_US);
                    languageData = File.ReadAllText(languageContent);
                    var _en_US = JsonConvert.DeserializeObject<Dictionary<string, string>>(languageData);
                    if (_en_US != null)
                        en_US = _en_US;
                }
            }
            catch
            {
                throw new Exception();
            }
        }

        //For returning language data based on key and language
        public static string ReturnLanguageData(string key, string language = "")
        {
            try
            {
                language = string.IsNullOrEmpty(language) ? "en-US" : language;

                return language switch
                {
                    "en-US" => en_US[key],
                    _ => en_US[key],
                };
            }
            catch{return key;}
            finally{}
        }

        //For replacing placeholders in the message
        public static string ReplacePlaceholders(string message, Dictionary<string, string>? values = null)
        {
            if (string.IsNullOrEmpty(message) || values == null || values.Count == 0)
                return message;

            foreach (var item in values)
            {
                string placeholder = $"@{item.Key}@";
                if (message.Contains(placeholder))
                    message = message.Replace(placeholder, item.Value ?? "");
            }

            return message;
        }

        //For returning final message with replaced placeholders
        public static string ReturnLanguageMessage(string key, Dictionary<string, string>? values = null, string language = "")
        {
            string message = ReturnLanguageData(key, language);
            return ReplacePlaceholders(message, values);
        }
    }
}
