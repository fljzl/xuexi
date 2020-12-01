using System.Text.RegularExpressions;


namespace Cheng.Comon.Regex
{
    public class RegexHelper
    {
        public static string GetImgSrc(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return "";
            }

            var rg = new System.Text.RegularExpressions.Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection mc = rg.Matches(content);
            if (mc.Count > 0)
            {
                return mc[0].Groups["imgUrl"].Value;
            }
            return "";
        }

        public static string PUGetImgSrc(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return "";
            }
            var rg = new System.Text.RegularExpressions.Regex(@"<img\s+[^>]*\s*src\s*=\s*([']?)(?<url>\S+)'?[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection mc = rg.Matches(content);
            if (mc.Count > 0)
            {
                return mc[0].Result("${url}").Replace("\"", "");
            }
            return "";
        }
    }

    public class HtmlHelper
    {
        /// <summary>
        /// 去除HTML标签
        /// </summary>
        /// <param name="strhtml">字符串</param>
        /// <returns></returns>
        public static string Clearhtml2(string strhtml)
        {
            string stroutput = strhtml;
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"<[^>]+>|</[^>]+>");
            stroutput = regex.Replace(stroutput, "");
            return stroutput;
        }
        public static string Clearhtml(string Htmlstring)
        {
            //删除脚本   
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"([/r/n])[/s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(quot|#34);", "/", RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(iexcl|#161);", "/xa1", RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(cent|#162);", "/xa2", RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(pound|#163);", "/xa3", RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(copy|#169);", "/xa9", RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&#(/d+);", "", RegexOptions.IgnoreCase);
            //替换掉 < 和 > 标记
            Htmlstring = Htmlstring.Replace("<", "");
            Htmlstring = Htmlstring.Replace(">", "");
            Htmlstring = Htmlstring.Replace("/r/n", "");
            Htmlstring = Htmlstring.Replace("\r\n", "");
            //返回去掉html标记的字符串
            return Htmlstring.Trim();
        }
    }
}
