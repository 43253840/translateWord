using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using System.Web;
using ChakraCore.NET;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace translateLib
{
    /// <summary>
    /// 翻译 助手
    /// </summary>
    public static class TranslationHelper
    {
        /// <summary>
        /// Chakra 上下文
        /// </summary>
        private static readonly ChakraContext _chakraContext;

        /// <summary>
        /// Cookie
        /// </summary>
        private static readonly CookieContainer _cookieContainer;


        /// <summary>
        /// 请求地址
        /// </summary>

        private static readonly string _baseUrl;

        /// <summary>
        /// tkk 
        /// </summary>
        private static string _googleTkk;

        private static readonly string _googleTransBaseUrl;

        /// <summary>
        /// 静态
        /// </summary>
        static TranslationHelper()
        {
            var runtime = ChakraRuntime.Create();

            _baseUrl = "http://translate.google.cn/translate_a/single";
            _cookieContainer = new CookieContainer();
            _chakraContext = runtime.CreateContext(true);

            _googleTransBaseUrl = "https://translate.google.com/";
            _googleTkk = "";

            var baseResultHtml = HttpHelper.GetRequest(_googleTransBaseUrl, _cookieContainer, null);

            int index = baseResultHtml.IndexOf("tkk:");
            int startIndex = baseResultHtml.IndexOf("'", index);
            int endIndex = baseResultHtml.IndexOf("'", startIndex + 1);
            _googleTkk = baseResultHtml.Substring(startIndex + 1, endIndex - startIndex);
            //Regex re = new Regex(@"(?<=tkk=')(.*?)(?=')");
            //Regex re = new Regex("(?<=(tkk:'))[.\\s\\S]*?(?=(';))"); 
            //_googleTkk = re.Match(baseResultHtml).ToString();

            var jsFileText = File.ReadAllText("../translateLib/gettk.js");

            _chakraContext.RunScript(jsFileText); //运行脚本
        }

        /// <summary>
        /// 获取翻译结果(需要翻译的文字默认使用中文)
        /// </summary>
        /// <param name="toLang">语言</param>
        /// <param name="originalText">待翻译的文本</param>
        /// <returns></returns>
        public static string GetTranslation(this string toLang, string originalText)
        {
            if (string.IsNullOrEmpty(toLang))
            {
                return toLang;
            }
            if (string.IsNullOrEmpty(originalText))
            {
                return originalText;
            }

            return GetTranslation("zh-cn", toLang, originalText);

        }

        /// <summary>
        /// 获取翻译结果
        /// </summary>
        /// <param name="fromLang">需要翻译的语言</param>
        /// <param name="toLang">翻译结果的语言</param>
        /// <param name="originalText">待翻译文本</param>
        /// <returns></returns>
        public static string GetTranslation(this string fromLang, string toLang, string originalText)
        {
            // var args = new Dictionary<string, dynamic>
            // {
            //     { "client", "t" },
            //     { "sl", fromLang },
            //     { "tl", toLang },
            //     { "dt", "t" },
            //     { "tk", GetTK(originalText) },
            //     { "q", HttpUtility.UrlEncode(originalText) }
            // };
            var args = new Dictionary<string, dynamic>
            {
                { "client", "gtx" },
                { "dt", "t" },
                { "ie", "UTF-8" },
                { "oe", "UTF-8" },
                { "sl", fromLang },
                { "tl", toLang },
                { "q", HttpUtility.UrlEncode(originalText) }
            };

            string tk = GetTK(originalText);

            //string googleTransUrl = "https://translate.google.com/translate_a/single?client=gtx&sl=" + fromLang + "&tl=" + toLang + "&hl=en&dt=at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t&ie=UTF-8&oe=UTF-8&otf=1&ssel=0&tsel=0&kc=1&tk=" + tk + "&q=" + HttpUtility.UrlEncode(originalText);
            //string googleTransUrl = "http://translate.google.cn/translate_a/single?client=gtx&dt=t&ie=UTF-8&oe=UTF-8&sl=auto&tl=en&q=" + HttpUtility.UrlEncode(originalText);

            //var ResultHtml = GetResultHtml(googleTransUrl, _cookieContainer, "https://translate.google.com/");
            var result = HttpHelper.GetRequest(_baseUrl, _cookieContainer, args);
            //dynamic TempResult = Newtonsoft.Json.JsonConvert.DeserializeObject(ResultHtml);

            //string ResultText = (TempResult[0][0][0]).ToString();

            //return ResultText;
            return result.FormattedJson();
        }

        /// <summary>
        /// 获取TK
        /// </summary>
        /// <param name="originalText"></param>
        /// <returns></returns>
        private static string GetTK(string originalText)
        {
            _chakraContext.GlobalObject.WriteProperty("originalText", originalText);
            return _chakraContext.RunScript("tk(\"" + originalText + "\",\"" + _googleTkk + "\")");
        }

        /// <summary>
        /// 格式化Json
        /// </summary>
        /// <param name="jsonStr">Json</param>
        /// <returns></returns>
        private static string FormattedJson(this string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
            {
                return string.Empty;
            }

            var array = JsonConvert.DeserializeObject<JArray>(jsonStr);

            var result = array[0][0][0].ToString();

            return result;
        }

        public static string GetResultHtml(string url, CookieContainer cookie, string referer)
        {
            var html = "";

            var webRequest = WebRequest.Create(url) as HttpWebRequest;

            webRequest.Method = "GET";

            webRequest.CookieContainer = cookie;

            webRequest.Referer = referer;

            webRequest.Timeout = 20000;

            webRequest.Headers.Add("X-Requested-With:XMLHttpRequest");

            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";

            webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";

            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                using (var reader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
                {

                    html = reader.ReadToEnd();
                    reader.Close();
                    webResponse.Close();
                }
            }
            return html;
        }

    }
}
