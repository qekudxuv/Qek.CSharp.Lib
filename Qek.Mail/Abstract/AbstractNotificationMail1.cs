using System.IO;
using System.Reflection;
using System.Text;

namespace Qek.Mail
{
    public abstract class AbstractNotificationMail1 : AbstractMailTemplate
    {
        private static readonly string _mailTemplate;
        private readonly string _thbackgroundcolor = "#72C530";//草綠色

        protected enum ColumnHtmlTag
        {
            Alternate,
            TH,
            TD,
        }

        static AbstractNotificationMail1()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string assemblyFilename = assembly.GetName().Name;

            using
            (
                StreamReader sr = new StreamReader(
                    assembly.GetManifestResourceStream(assemblyFilename + ".Mail.EmailTemplate.NotificationMail1.htm"))
            )
            {
                _mailTemplate = sr.ReadToEnd();
            }
        }

        public AbstractNotificationMail1()
        {

        }

        public AbstractNotificationMail1(string thbackgroundcolor)
        {
            this._thbackgroundcolor = thbackgroundcolor;
        }

        /// <summary>
        /// 取得內容前言
        /// </summary>
        /// <returns></returns>
        protected abstract string GetMailPrefix();

        /// <summary>
        /// 取得內容主體
        /// </summary>
        /// <returns></returns>
        protected abstract string GetMailContext();

        /// <summary>
        /// 取得內容後序
        /// </summary>
        /// <returns></returns>
        protected abstract string GetMailSuffix();

        /// <summary>
        /// Adds the HTML table field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldContent">Content of the field.</param>
        /// <returns></returns>
        protected string AddHtmlTableField(string fieldName, object fieldContent)
        {
            return this.AddHtmlTableField(ColumnHtmlTag.Alternate, fieldName, fieldContent);
        }

        /// <summary>
        /// Adds the HTML table field.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="cols">The cols.</param>
        /// <returns></returns>
        protected string AddHtmlTableField(ColumnHtmlTag tag, params object[] cols)
        {
            var row = new StringBuilder("<tr>");
            for (int i = 0; i < cols.Length; i++)
            {
                switch (tag)
                {
                    case ColumnHtmlTag.Alternate:
                        if ((i & 1) == 0)
                        {
                            row.Append("<th>").Append(cols[i] ?? "").Append("</th>");
                        }
                        else
                        {
                            row.Append("<td>").Append(cols[i] ?? "").Append("</td>");
                        }
                        break;
                    case ColumnHtmlTag.TH:
                        row.Append("<th>").Append(cols[i] ?? "").Append("</th>");
                        break;
                    case ColumnHtmlTag.TD:
                        row.Append("<td>").Append(cols[i] ?? "").Append("</td>");
                        break;
                }
            }
            row.Append("</tr>");
            return row.ToString();
        }

        /// <summary>
        /// 取得信件內容
        /// </summary>
        /// <returns></returns>
        protected override sealed string GetMailContent()
        {
            StringBuilder mailContent = new StringBuilder(_mailTemplate);
            mailContent.Replace("#thbackgroundcolor", this._thbackgroundcolor);
            mailContent.Replace("${Content}",
                new StringBuilder()
                .Append(this.GetMailPrefix())
                .Append(this.GetMailContext())
                .Append(this.GetMailSuffix())
                .ToString());

            return mailContent.ToString();
        }
    }
}
