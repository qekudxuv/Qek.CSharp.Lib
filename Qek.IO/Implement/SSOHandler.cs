using System;
using System.IO;

namespace Qek.IO
{
    public class SSOHandler : HttpHandler
    {
        private bool _isLoginSSOPassed = false;
        private string _ssoLoginUrl;
        private string _account;
        private string _pwd;

        public SSOHandler(string ssoLoginUrl, string account, string pwd)
        {
            this._ssoLoginUrl = ssoLoginUrl;//https://sso.htc.com/sso/login
            this._account = account;
            this._pwd = pwd;
        }

        /// <summary>
        /// 協助通過SSO，並取得資源
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="pwd">The PWD.</param>
        /// <param name="resourceUrl">The resource URL.</param>
        /// <returns></returns>
        public byte[] PassSSOAndGetResource(string resourceUrl)
        {
            if (string.IsNullOrEmpty(this._account))
            {
                throw new Exception("_account is empty");
            }
            else if (string.IsNullOrEmpty(this._pwd))
            {
                throw new Exception("_pwd is empty");
            }

            byte[] remoteResource = null;

            if (_isLoginSSOPassed)
            {
                remoteResource = this.FetchResourceByHttpGet(resourceUrl);
            }
            else
            {
                string lt_hidden_value = this.GetSSOHiddenValue();
                if (lt_hidden_value != string.Empty)
                {
                    bool isPassSSO = this.PassSSO(this._account, this._pwd, lt_hidden_value);
                    if (isPassSSO)
                    {
                        remoteResource = this.FetchResourceByHttpGet(resourceUrl);
                    }
                    else
                    {
                        throw new Exception(
                            string.Format("SSO authentication fail. account = {0}",
                            this._account)
                            );
                    }
                }
                else
                {
                    throw new Exception("SSO lt_hidden_value is empty");
                }
            }
            return remoteResource;
        }

        private string GetSSOHiddenValue()
        {
            string url = this._ssoLoginUrl + "?service=http%3a%2f%2fmasd%2fLogin.aspx";
            string lt_hidden_value = string.Empty;
            byte[] remoteResource = this.FetchResourceByHttpGet(url);

            using (Stream ms = new MemoryStream(remoteResource))
            {
                using (StreamReader sr = new StreamReader(ms, System.Text.Encoding.Default))
                {
                    string ssoResponseString = sr.ReadToEnd();
                    string lt_preStr = "<input type=\"hidden\" name=\"lt\" value=\"";
                    int lt_preStrIndex = ssoResponseString.IndexOf(lt_preStr);
                    if (lt_preStrIndex == -1)
                    {
                        throw new Exception("could not find hidden value from SSO login page!");
                    }
                    int startPos = lt_preStrIndex + lt_preStr.Length;
                    int endPos = ssoResponseString.IndexOf("\"", startPos + 1);

                    lt_hidden_value = ssoResponseString.Substring(startPos, endPos - startPos);
                    //Console.WriteLine(lt_hidden_value);
                    //Console.WriteLine("--------------------------------------");
                    //Console.WriteLine(ssoResponseString.Substring(startPos));
                }

            }

            return lt_hidden_value;
        }

        private bool PassSSO(string account, string pwd, string lt_hidden_value)
        {
            bool isPassSSO = false;

            string param = string.Format("username={0}&password={1}&lt={2}&_eventId=submit&submit=LOGIN",
                account, pwd, lt_hidden_value);

            using (Stream ms = new MemoryStream(this.FetchResourceByHttpPost(this._ssoLoginUrl, param)))
            {
                using (StreamReader sr = new StreamReader(ms, System.Text.Encoding.Default))
                {
                    string ssoResponseString = sr.ReadToEnd();

                    isPassSSO = ssoResponseString.IndexOf("Authentication fail") == -1;
                    if (isPassSSO)
                    {
                        _isLoginSSOPassed = true;
                    }
                }
            }

            return isPassSSO;
        }
    }
}
