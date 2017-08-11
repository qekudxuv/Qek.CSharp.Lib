using NHibernate.Cfg;
using Qek.Common;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Qek.NHibernate
{
    /// <summary>
    /// Reducing application startup time
    /// 
    /// NHibernate's Configuration class is serializable. Thoroughly validating the mappings and
    /// settings takes some effort and time. The very first time our application runs, we can't escape
    /// this, but if we serialize our Configuration object to disk, we can deserialize it the next time we
    /// run it, saving us all of this busy work.
    /// 
    /// The IsNHibernateCfgValid method ensures that the Configuration we've serialized
    /// is still fresh. If the executable or the App.config has been updated, we need to rebuild our
    /// configuration object from scratch.
    /// 
    /// We compare the last write time of the various files to decide if the serialized configuration
    /// is stale. We use a BinaryFormatter to serialize and deserialize the configuration.
    /// </summary>
    public sealed class NHibernateCfgBuilder
    {
        private static ProjectType _projectType = ConfigHelper.GetAppSetting<ProjectType>("ProjectType");
        private static ENV _env = ConfigHelper.GetAppSetting<ENV>("ENV");
        private static string _systemShortname = ConfigHelper.GetAppSetting("SystemShortName");
        private static string SERIALIZED_CFG = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            String.Format("NHibernateCfg.{0}.{1}.bin", _systemShortname, _env));

        public Configuration Build()
        {
            Configuration cfg = Load();
            if (cfg == null)
            {
                cfg = new Configuration();
                if (_projectType == ProjectType.WebSite)
                {
                    cfg.Configure();
                }
                else
                {
                    cfg.Configure(Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory,
                            _projectType == ProjectType.WebApplication || _projectType == ProjectType.WebAPI ? "bin" : string.Empty,
                            string.Format(@"ConfigFiles\NHibernate\{0}_hibernate.cfg.xml", _env)));
                }
                //如果我們不想每次都要將hibernate.cfg.xml輸出至bin呢？NHibernate也提供指定設定檔位置的方式讀取設定檔。
                //假設hibernate.cfg.xml置放在web ap根目錄下，我們可將程式碼修改為以下片段，即可讀取設定檔資訊
                //config.Configure(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hibernate.cfg.xml"));
                //config.Configure(@"D:\_MASDSystem\_WebSystem\FWRS\Console\bin\Debug\hibernate.cfg.xml");
                Save(cfg);
            }
            return cfg;
        }

        private Configuration Load()
        {
            if (!IsNHibernateCfgValid())
            {
                return null;
            }

            using (var file = File.Open(SERIALIZED_CFG, FileMode.Open))
            {
                var bf = new BinaryFormatter();
                return bf.Deserialize(file) as Configuration;
            }
        }

        private void Save(Configuration cfg)
        {
            using (var file = File.Open(SERIALIZED_CFG, FileMode.Create))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(file, cfg);
            }
        }

        private bool IsNHibernateCfgValid()
        {
            // If we don't have a cached config,
            // force a new one to be built
            if (!File.Exists(SERIALIZED_CFG))
                return false;

            var configInfo = new FileInfo(SERIALIZED_CFG);
            var asm = Assembly.GetExecutingAssembly();
            if (asm.Location == null)
                return false;

            // If the assembly is newer,
            // the serialized config is stale
            var asmInfo = new FileInfo(asm.Location);
            if (asmInfo.LastWriteTime > configInfo.LastWriteTime)
                return false;

            // If the app.config is newer,
            // the serialized config is stale
            var appDomain = AppDomain.CurrentDomain;
            var appConfigPath = appDomain.SetupInformation.ConfigurationFile;
            var appConfigInfo = new FileInfo(appConfigPath);
            if (appConfigInfo.LastWriteTime > configInfo.LastWriteTime)
                return false;

            // It's still fresh
            return true;
        }
    }
}
