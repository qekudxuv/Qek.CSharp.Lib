using Qek.Common.Dto.Tool;
using System;
using System.Diagnostics;
using System.IO;

namespace Qek.Common.Tool
{
    /// <summary>
    /// Sign Apk(s) Key Tool
    /// </summary>
    public class SignApkKeyTool
    {
        private string _signApkToolPath = string.Empty;
        private SignApkKeyLog _log = null;

        public SignApkKeyTool(string signApkToolPath)
        {
            if (string.IsNullOrEmpty(signApkToolPath))
            {
                throw new ArgumentNullException("Sign Apk Tool Path is empty.");
            }
            else if (!File.Exists(signApkToolPath))
            {
                throw new FileNotFoundException("Sign Apk Tool not found.");
            }

            this._signApkToolPath = signApkToolPath;
        }

        public SignApkKeyLog SignApks(string project, int keyAlias, string sourceApkPath)
        {
            _log = new SignApkKeyLog();
            string inputCommand = string.Format("{0} APK {1} {2}", project, keyAlias, sourceApkPath);
            _log.InputCommand.AppendLine(inputCommand);

            Process _bashProcess = new Process();

            _bashProcess.StartInfo.FileName = string.Format(@"{0}", _signApkToolPath);
            _bashProcess.StartInfo.Arguments = inputCommand;
            //__bashProcess.StartInfo.EnvironmentVariables["CYGWIN"] = "tty";
            _bashProcess.StartInfo.EnvironmentVariables["MINGW32"] = "tty";
            //_bashProcess.StartInfo.WorkingDirectory = string.Format(@"{0}", _WorkingDirectory);
            _bashProcess.StartInfo.RedirectStandardError = true;
            _bashProcess.StartInfo.RedirectStandardInput = true;
            _bashProcess.StartInfo.RedirectStandardOutput = true;
            _bashProcess.StartInfo.CreateNoWindow = false;
            _bashProcess.StartInfo.UseShellExecute = false;
            _bashProcess.StartInfo.ErrorDialog = false;
            _bashProcess.Start();

            DataReceivedEventHandler errorEventHandler = new DataReceivedEventHandler(ErrorDataReceived);
            DataReceivedEventHandler outEventHandler = new DataReceivedEventHandler(OutDataReceived);
            _bashProcess.OutputDataReceived += outEventHandler;
            _bashProcess.ErrorDataReceived += errorEventHandler;
            _bashProcess.BeginErrorReadLine();
            _bashProcess.BeginOutputReadLine();

            _bashProcess.WaitForExit();

            bool flag = false;
            using (StringReader reader = new StringReader(_log.DataMessage.ToString()))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Trim().IndexOf("<FAIL>") > -1)
                    {
                        flag = false;
                        break;
                    }
                    else if (line.Trim().IndexOf("< OK >") > -1)
                    {
                        flag = true;
                    }
                }
            }

            _log.OverallResult = flag;

            return _log;
        }

        /// <summary>
        /// Deprecated
        /// Error data received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="dataReceivedEventArgs">The <see cref="System.Diagnostics.DataReceivedEventArgs"/> instance containing the event data.</param>
        private void ErrorDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            if (dataReceivedEventArgs.Data != null)
            {
                _log.ErrMessage.AppendLine(dataReceivedEventArgs.Data.ToString());
            }
        }

        /// <summary>
        /// Output data received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="dataReceivedEventArgs">The <see cref="System.Diagnostics.DataReceivedEventArgs"/> instance containing the event data.</param>
        private void OutDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            if (dataReceivedEventArgs.Data != null)
            {
                _log.DataMessage.AppendLine(dataReceivedEventArgs.Data.ToString());
            }
        }
    }
}

