using CommandLineParser.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movieortv
{
    class Arguments
    {
        [ValueArgument(typeof(string), 'i', "input", Description = "Input torrent name", ValueOptional = false)]
        public string input;

        [ValueArgument(typeof(string), 'c', "category", Description = "Category for this torrent", ValueOptional = false)]
        public string category;

        [ValueArgument(typeof(string), 'r', "root-path", Description = "Root path of the torrent", ValueOptional = false)]
        public string rootPath;

        [ValueArgument(typeof(string), 's', "save-path", Description = "Save path tof the torrent", ValueOptional = false)]
        public string savePath;
    }
}
