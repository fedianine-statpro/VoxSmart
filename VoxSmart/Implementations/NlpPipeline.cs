using Catalyst.Models;
using Catalyst;
using Mosaik.Core;

namespace VoxSmart.Implementations
{
    public class NlpPipeline
    {
        private bool _isInitialized;
        public bool IsInitialized => _isInitialized;
        public Pipeline Pipeline { get; }

        public NlpPipeline()
        {
            English.Register();
            Storage.Current = new DiskStorage("catalyst-models");
            Pipeline = Pipeline.For(Language.English);
        }

        public async Task InitializeAsync()
        {
            if (!_isInitialized)
            {
                var process = await AveragePerceptronEntityRecognizer.FromStoreAsync(language: Language.English,
                version: Mosaik.Core.Version.Latest, tag: "WikiNER");
                Pipeline.Add(process);
                _isInitialized = true;
            }
        }
    }
}
