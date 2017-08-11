using Qek.Common.Dto.Git;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Qek.Git
{
    /// <summary>
    /// Git Bash
    /// </summary>
    public class BaseGitBash
    {
        private string _gitBashFile;
        private string _rootWorkingDirectory;
        private string _currentRepoName;
        private GitCommandLog _gitCommandLog = null;
        private Process _bashProcess = null;

        #region constructors
        public BaseGitBash(string gitBashFile, string rootWorkingDirectory)
        {
            if (string.IsNullOrEmpty(gitBashFile))
            {
                throw new ArgumentNullException("Git bash file is empty.");
            }
            else if (!File.Exists(gitBashFile))
            {
                throw new FileNotFoundException(gitBashFile + " not found.");
            }
            else if (string.IsNullOrEmpty(rootWorkingDirectory))
            {
                throw new ArgumentNullException("WorkingDirectory is empty.");
            }

            this._gitBashFile = gitBashFile;
            this._rootWorkingDirectory = rootWorkingDirectory.EndsWith("\\") ? rootWorkingDirectory : rootWorkingDirectory + "\\";
            this.CreateBash();
        }

        #endregion
        public string GitBashFile
        {
            get { return _gitBashFile; }
            private set { _gitBashFile = value; }
        }

        public string RootWorkingDirectory
        {
            get { return this._rootWorkingDirectory; }
        }

        public string CurrentWorkingDirectory
        {
            get { return string.Format("{0}{1}", this._rootWorkingDirectory, _currentRepoName); }
            //set { _workingDirectory = value; }
        }

        public void SwitchToRoot()
        {
            this._currentRepoName = "";
        }

        public void SwitchRepo(string repoName)
        {
            this._currentRepoName = repoName;
        }
        /// <summary>
        /// 執行一組 Git Script
        /// 並且執行每一行 Command 後，都暫停所設定毫秒
        /// </summary>
        /// <param name="gitCommands">The git commands.</param>
        /// <param name="sleepMilliSecond">The sleep milli second.</param>
        /// <returns></returns>
        public GitCommandLog RunScript(IEnumerable<string> gitCommands, int sleepMilliSecond = 0)
        {
            var gitCommandList = new List<GitCommandDto>();
            foreach (string gitCommand in gitCommands)
            {
                gitCommandList.Add(new GitCommandDto(gitCommand, sleepMilliSecond, true));
            }
            return this.RunScript(gitCommandList);
        }

        /// <summary>
        /// 執行 Command 後，暫停所設定毫秒
        /// </summary>
        /// <param name="gitCommand">The git command.</param>
        /// <param name="sleepMilliSecond">The sleep milli second.</param>
        /// <returns></returns>
        public GitCommandLog RunScript(string gitCommand, int sleepMilliSecond = 0, bool isReturnZeroAfterExecute = true)
        {
            var gitCommandList = new List<GitCommandDto>();
            gitCommandList.Add(new GitCommandDto(gitCommand, sleepMilliSecond, isReturnZeroAfterExecute));
            return this.RunScript(gitCommandList);
        }

        /// <summary>
        /// 執行每一 Command 後，依據設定暫停若干毫秒
        /// </summary>
        /// <param name="gitCommandList">The git command list.</param>
        /// <returns></returns>
        public GitCommandLog RunScript(IEnumerable<GitCommandDto> gitCommandList)
        {
            _gitCommandLog = new GitCommandLog();
            _bashProcess.StartInfo.WorkingDirectory = string.Format(@"{0}", CurrentWorkingDirectory);
            _bashProcess.Start();
            _bashProcess.BeginErrorReadLine();
            _bashProcess.BeginOutputReadLine();

            foreach (GitCommandDto dto in gitCommandList)
            {
                _gitCommandLog.InputCommand.AppendLine(dto.GitCommand);
                _bashProcess.StandardInput.WriteLine(dto.GitCommand);
                _bashProcess.StandardInput.WriteLine("echo $?");
                if (dto.SleepMilliSecond > 0)
                {
                    System.Threading.Thread.Sleep(dto.SleepMilliSecond);
                }
            }

            _bashProcess.StandardInput.WriteLine("exit");
            _bashProcess.StandardInput.Close();

            _bashProcess.WaitForExit();

            _gitCommandLog.TotalInputCommand = gitCommandList.Where(c => c.IsReturnZeroAfterExecute).Count();
            _gitCommandLog.TotalSucceedCommand = this.GetTotalReturnZeroCommand();

            _bashProcess.CancelErrorRead();
            _bashProcess.CancelOutputRead();

            return _gitCommandLog;
        }


        /// <summary>
        /// Gets the total return zero command.
        /// </summary>
        /// <returns></returns>
        private int GetTotalReturnZeroCommand()
        {
            int totalReturnZeroCommand = 0;
            using (StringReader reader = new StringReader(_gitCommandLog.DataMessage.ToString()))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Trim().Equals("0"))
                    {
                        totalReturnZeroCommand++;
                    }
                }
            }
            return totalReturnZeroCommand;
        }

        /// <summary>
        /// Creates the bash.
        /// </summary>
        private void CreateBash()
        {
            _bashProcess = new Process();

            _bashProcess.StartInfo.FileName = string.Format(@"{0}", _gitBashFile);
            _bashProcess.StartInfo.Arguments = "--login -i ";
            //bashProcess.StartInfo.EnvironmentVariables["CYGWIN"] = "tty";
            _bashProcess.StartInfo.EnvironmentVariables["MINGW32"] = "tty";

            _bashProcess.StartInfo.RedirectStandardError = true;
            _bashProcess.StartInfo.RedirectStandardInput = true;
            _bashProcess.StartInfo.RedirectStandardOutput = true;
            _bashProcess.StartInfo.CreateNoWindow = true;
            _bashProcess.StartInfo.UseShellExecute = false;
            _bashProcess.StartInfo.ErrorDialog = false;

            DataReceivedEventHandler errorEventHandler = new DataReceivedEventHandler(ErrorDataReceived);
            DataReceivedEventHandler outEventHandler = new DataReceivedEventHandler(OutDataReceived);
            _bashProcess.OutputDataReceived += outEventHandler;
            _bashProcess.ErrorDataReceived += errorEventHandler;

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
                _gitCommandLog.ErrMessage.AppendLine(dataReceivedEventArgs.Data.ToString());
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
                _gitCommandLog.DataMessage.AppendLine(dataReceivedEventArgs.Data.ToString());
            }
        }
    }
}
