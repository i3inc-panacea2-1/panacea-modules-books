using Panacea.Core;
using Panacea.Modularity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Panacea.Models;

namespace Panacea.Modules.Books
{
    class BooksPlugin : IPlugin //TODO: ,IHasFavorites in modularity?
    {
        readonly PanaceaServices _core;
        public BooksPlugin(PanaceaServices core)
        {
            _core = core;
        }
        public List<ServerItem> Favorites { get; set; }

        public Task BeginInit()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            return;
        }

        public Task EndInit()
        {
            return Task.CompletedTask;
        }

        public Task Shutdown()
        {
            return Task.CompletedTask;
        }
    }
}
