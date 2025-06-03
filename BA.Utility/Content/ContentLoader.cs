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

        public static string ReturnLanguageData(string key, string language)
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
            catch
            {
                Console.WriteLine("Exception Multilingual Data:" + key.ToString());
                return key;
            }
            finally
            {

            }
        }
    }
}
